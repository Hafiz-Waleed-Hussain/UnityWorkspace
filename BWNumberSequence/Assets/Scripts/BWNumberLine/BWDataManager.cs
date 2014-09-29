#define MAC_PLATFORM

using UnityEngine;
using System.Collections;

public class BWDataManager {
	
	private Hashtable allLevels = null;
	private Hashtable allCategoriesCoreData = null;
	
	private CoreDataManager coreManager = null;
	
	float easyThreshold;
    float hardThreshold;
    int failEasyGoto;
    int succeedHardGoto;
    int minQuestions;
    int maxQuestions;
    
    int questionsAsked;
    int totalQuestions;
	BWNumberSequence numberSequence;
	
	ArrayList numbersForLevel = null;
	
	public int currentLevel = 0;
	public int numberToFind = 0;
	public int numberLineMin = 0;
	public int numberLineMax = 20;
	public int initialNumber = 0;
	public int beeStartingIndex = 0;
	public bool showNumbers = true;
	public bool shouldFlipBee = false;
	
	public int currentCategoryID = 179;
	public string currentCategory = "Number Sequence";
	
	public BWDataManager (AGGameIndex gameIndex, int categoryIndex, string file) {
		
		if(gameIndex == AGGameIndex.k_Beeworld_NumberSequence) {
			currentCategoryID = 179;
			currentCategory = "Number Sequence";
		} else {
			if(categoryIndex == 0) {
				currentCategoryID = 178;
				currentCategory = "Counting Up From";
			} else {
				currentCategoryID = 180;
				currentCategory = "Counting Down From";
			}
		}
		
		CategoriesData data = CategoriesData.getSharedData(file);
		Hashtable numberSequenceData = data.getGameDataForIndex(gameIndex);
		ArrayList availableCat = (ArrayList)numberSequenceData["availableCategories"];
		allLevels = (Hashtable)availableCat[categoryIndex];
		
#if (!MAC_PLATFORM)
		coreManager = new CoreDataManager(currentCategoryID);
		allCategoriesCoreData = coreManager.getResultDict();
		currentLevel = coreManager.getCurrentLevel();
#endif
		fetchLevelData();
	}
	
	public void fetchLevelData () {
		
		if(allLevels == null) {
			Debug.Log("Exception: level data not available ... something wrong with plist");
			return;
		}
#if (!MAC_PLATFORM)
		currentLevel = coreManager.getCurrentLevel();
#endif
		
		
		//if the game starts from level0 you dont need this check
		if (currentLevel == 0 && currentCategoryID != 178) currentLevel = 1;
/*	
#if (MAC_PLATFORM)
		currentLevel = 14;
#endif
*/
		string currentLevelString = string.Format("level{0}", currentLevel);
	    Hashtable currentLevelData = (Hashtable)allLevels[currentLevelString];
	    
	    if (currentLevelData == null) {
			Debug.Log("Exception: not data for current level available ... trying level1 ...");
	        currentLevelData = (Hashtable)allLevels["level1"];
	        if (currentLevelData == null) {
				Debug.Log("Exception: not data for any level available ... terminate");
	            return;
	        }
	    }
	    
	    easyThreshold = GetAsFloat(currentLevelData["easyThreshold"]);
		hardThreshold = GetAsFloat(currentLevelData["hardThreshold"]);
		
	    failEasyGoto        = GetAsInt(currentLevelData["failEasyGoto"]);
	    succeedHardGoto     = GetAsInt(currentLevelData["succeedHardGoto"]);
	    minQuestions        = GetAsInt(currentLevelData["minQuestions"]);
	    maxQuestions        = GetAsInt(currentLevelData["maxQuestions"]);
	    numberLineMin       = GetAsInt(currentLevelData["numberLineMin"]);
	    numberLineMax       = GetAsInt(currentLevelData["numberLineMax"]);
		
		showNumbers         = (bool)currentLevelData["showNumbers"];
	    
	    numberSequence = getNumberSequenceFromString((string)currentLevelData["numberSequence"]);
		
		ArrayList levelNumbers = (ArrayList)currentLevelData["numbers"];
#if (MAC_PLATFORM)
		numbersForLevel = levelNumbers;
#else
		numbersForLevel = new ArrayList();
		
		questionsAsked = 0;
		totalQuestions = 0;
		int questionsMastered = 0;
		
	    foreach(string num in levelNumbers) {
			
			Hashtable numDict = (Hashtable)allCategoriesCoreData[num];
				
			int qPlayed = GetAsInt(numDict["questionsPlayed"]);
			
			bool hasMastered = (bool)numDict["hasMastered"];
			if(!hasMastered) {
				numbersForLevel.Add(num);
				questionsAsked += qPlayed;
				Debug.Log("number for level " + num);
			} else {
				questionsMastered += qPlayed;
			}	
		}
		totalQuestions = questionsMastered + questionsAsked;
#endif	
	}
	
	public void fetchNextQuestionData () {
		
		if (numbersForLevel.Count == 0) {
			completeCurrentLevel();
			fetchLevelData();
    	}
		
		int index = 0;
	    switch (numberSequence) {
			case BWNumberSequence.BWNumberSequenceInOrder:
	            index = questionsAsked % numbersForLevel.Count;
	            break;
	        case BWNumberSequence.BWNumberSequenceRandom:
	            index = (int)Random.Range(0, numbersForLevel.Count);
	            break;
	        case BWNumberSequence.BWNumberSequenceReverse:
	            index = numbersForLevel.Count - 1 - (questionsAsked % numbersForLevel.Count);
	            break;
	        default:
	            index = 0;
	            break;
	    }
		
		if(index < numbersForLevel.Count) {
			numberToFind = System.Convert.ToInt32((string)numbersForLevel[index]);
		} else {
			fetchNextQuestionData();
			return;
		}
		
		string currentLevelString = string.Format("level{0}", currentLevel);
	    Hashtable currentLevelData = (Hashtable)allLevels[currentLevelString];
		string startingPosition = (string) currentLevelData["beeStartingPosition"];
		//bee starting position for find the number its always sky
	    if (startingPosition.Equals("sky")) {
	        beeStartingIndex = -1000; //set a number where no flower goes
	    } else {
	        string startingPointCondition = (string)currentLevelData["beeStartingPointCondition"];
	        beeStartingIndex = beeStartingPointFromString(startingPosition, startingPointCondition);
	    }
		
		string initialNumberString = (string)currentLevelData["initialNumber"];
		initialNumber = initialNumberFromString(initialNumberString);
			
		if (initialNumber > numberLineMax || initialNumber < numberLineMin) {
        	initialNumber = numberLineMin;
    	}
    	
		if ((beeStartingIndex > numberLineMax || beeStartingIndex < numberLineMin) && beeStartingIndex != -1000) {
        	beeStartingIndex = numberLineMin;
    	}
		
		//numberToFind = 0;
		
		if(numberToFind >= 0)
			shouldFlipBee = false;
		else 
			shouldFlipBee = true;
	}
	
	public bool calculateResult (int attempts, int corrects) {	
		
		//return true;
		
		if(coreManager == null) return false;
		
		string objectKey = string.Format("{0}", numberToFind);		
		Hashtable resultDict = (Hashtable)allCategoriesCoreData[objectKey];
		
		if(resultDict == null) return false;
		
		Debug.Log("reading values from dict");
		
		int questionAttempted = GetAsInt(resultDict["questionsPlayed"]);
	    int levelAttempts = GetAsInt(resultDict["levelAttempts"]);
	    int levelCorrects = GetAsInt(resultDict["levelCorrects"]);
	    
	    int total_questionAttempted = GetAsInt(resultDict["totalQuestionsPlayed"]);
	    int total_levelAttempts = GetAsInt(resultDict["attempted"]);
	    int total_levelCorrects = GetAsInt(resultDict["solved"]);
	    
	    bool hasMastered = (bool)resultDict["hasMastered"];
	    
		Debug.Log("reading end");
		
	    questionAttempted++;
	    levelAttempts += attempts;
	    levelCorrects += corrects;
	    
	    total_questionAttempted++;
	    total_levelAttempts += attempts;
	    total_levelCorrects += corrects;
	    
		Debug.Log("calculating threshold");
	    float obtainedThreshold = (float)(levelCorrects/levelAttempts);
	    
		if (questionAttempted >= minQuestions) {
	        if (obtainedThreshold < easyThreshold) {
	            hasMastered = false;
	        } else {
	            hasMastered = true;
	        }
	    }
	    if (questionAttempted >= maxQuestions) {
	        hasMastered = true;
	    }
		
		totalQuestions++;
		questionsAsked++;
		
		if(hasMastered) {
			questionsAsked -= questionAttempted;
		}
		
		resultDict["totalQuestionsPlayed"] = total_questionAttempted;
		resultDict["attempted"] = total_levelAttempts;
		resultDict["solved"] = total_levelCorrects;
	    
		resultDict["questionsPlayed"] = questionAttempted;
		resultDict["levelAttempts"] = levelAttempts;
		resultDict["levelCorrects"] = levelCorrects;
		resultDict["hasMastered"] = hasMastered;
		
		if(hasMastered) {
			foreach (string num in numbersForLevel) {
				if(System.Convert.ToInt32(num) == numberToFind) {
					numbersForLevel.Remove(num);
					break;
				}
			}
		}
	    
	    bool levelDone = false;
	    if (numbersForLevel.Count == 0) {
	        completeCurrentLevel();
	        levelDone = true;
	    }
		
	    saveData();
	    
	    return levelDone;
	}
	
	private void completeCurrentLevel () {
		
		string currentLevelString = string.Format("level{0}", currentLevel);
		Hashtable currentLevelData = (Hashtable)allLevels[currentLevelString];
		
		ArrayList numbers = (ArrayList) currentLevelData["numbers"];
		
		int levelAttempts = 0;
	    int levelCorrects = 0;
		
		foreach (string str in numbers) {
			Hashtable numberDict = (Hashtable)allCategoriesCoreData[str];
			levelAttempts += GetAsInt(numberDict["levelAttempts"]);
			levelCorrects += GetAsInt(numberDict["levelCorrects"]);
			
		}
    
		Debug.Log("Calculating level threshold");
	    float obtainedThreshold = (float)(levelCorrects/levelAttempts);
		levelCompleteWithThreshold(obtainedThreshold);
	}
	
	public void levelCompleteWithThreshold(float obtainedThreshold) {
		
		int prevLevel = currentLevel;
	    if (obtainedThreshold < easyThreshold) {
	        //fail - below easy
	        currentLevel = failEasyGoto;
	    }
	    else if (obtainedThreshold >= easyThreshold && obtainedThreshold < hardThreshold) {
	        //succeed easy
	        if (currentLevel < (coreManager.getTotalLevels())) {
	            currentLevel++;
	        }
	    }
	    else if (obtainedThreshold >= hardThreshold) {
	        //succeed hard
	        currentLevel = succeedHardGoto;
	    }
		
	    coreManager.setCurrentLevel(currentLevel);
		
		//if user has completed the last level and category status is not set then set it.
		if(prevLevel == coreManager.getTotalLevels() && coreManager.getCategoryStatus() == 0) {
			coreManager.setCategoryStatus(1);
		}
		
		Debug.Log("Level changed");
		saveData();
	    resetLevelSpecificData();
	}
	
	public void saveData (){
		coreManager.setCurrentLevel(currentLevel);
		coreManager.setResultDict(allCategoriesCoreData);
		coreManager.saveManager();
	}
	
	private void resetLevelSpecificData (){
		
		foreach(string key in allCategoriesCoreData.Keys) {
			Hashtable numDict = (Hashtable)allCategoriesCoreData[key];
			
			numDict["questionsPlayed"] = 0;
			numDict["levelAttempts"] = 0;
			numDict["levelCorrects"] = 0;
			numDict["hasMastered"] = false;
		}
	}
	
	public float GetAsFloat(object theObject){
        if(theObject == null){
            return 0.0f;
        }
		
		if(theObject.GetType() == typeof(int)) {
			return (float)(int)theObject;
		} else if(theObject.GetType() == typeof(long)) {
			return (float)(long)theObject;
		} else if(theObject.GetType() == typeof(float)) {
			return (float)theObject;
		} else if(theObject.GetType() == typeof(double)) {
			return (float)(double)theObject;
		}
		
        return (float)theObject;
    }
	
	public int GetAsInt(object theObject){
        if(theObject == null){
            return 0;
        }
        
		if(theObject.GetType() == typeof(int)) {
			return (int)theObject;
		} else if(theObject.GetType() == typeof(long)) {
			return (int)(long)theObject;
		} else if(theObject.GetType() == typeof(float)) {
			return (int)(float)theObject;
		} else if(theObject.GetType() == typeof(double)) {
			return (int)(double)theObject;
		}
		
        return (int)theObject;
    }
	
	//Utilities
	
	public BWNumberSequence getNumberSequenceFromString (string str) {
		
		if(str == null || str.Equals("")) {
			return BWNumberSequence.BWNumberSequenceRandom;
		} else if (str.Equals("inorder") || str.Equals("sequence")) {
			return BWNumberSequence.BWNumberSequenceInOrder;
		} else if (str.Equals("random")) {
			return BWNumberSequence.BWNumberSequenceRandom;
		} else if (str.Equals("reverse")) {
			return BWNumberSequence.BWNumberSequenceReverse;
		} else if (str.Equals("numberAsked")) {
			return BWNumberSequence.BWNumberSequenceNumberAsked;
		} else if (str.Equals("startingPoint")) {
			return BWNumberSequence.BWNumberSequenceStartingPoint;
		}
		
		return BWNumberSequence.BWNumberSequenceInOrder;
	}
	
	private void setBeeFlipState () {
    	if (numberToFind < 0) {
        	shouldFlipBee = true;
	    } else {
	        shouldFlipBee = false;
	    }
	}
	
	private int beeStartingPointFromString(string startingPosition, string condition) {
		
		int defaultNumber = numberLineMin;
		if(currentCategoryID.Equals("180")) defaultNumber = numberLineMax;
		
		int number = defaultNumber;
		
		string [] posArray = startingPosition.Split(new char [] {','});
		if (posArray.Length == 0) {
            number = initialNumber;
        } else if (posArray.Length == 1) {
            number = System.Convert.ToInt32(posArray[0]);
        } else if (posArray.Length == 2) {
            number = (int) Random.Range(System.Convert.ToInt32(posArray[0]), System.Convert.ToInt32(posArray[1])+1);
        } else {
            int index = totalQuestions % posArray.Length;
            number = System.Convert.ToInt32(posArray[index]);
        }
		
		BWNumberSequence sequence = getNumberSequenceFromString(condition);
		
		if (sequence == BWNumberSequence.BWNumberSequenceNumberAsked) {
            if (number < 0) {
                number = -numberToFind;
            } else {
                number = AGGameState.modInt(numberToFind);
            }
        }
		
		if (number + numberToFind > numberLineMax) {
            number = anySmallerNumberAvailable(number);
            if (number + numberToFind > numberLineMax) {
                number = numberLineMax - numberToFind;
            } else {
                numberToFind = number;
            }
            
        } else if (number + numberToFind < numberLineMin) {
            number = anyGreaterNumberAvailable(number);
            if (number + numberToFind < numberLineMin) {
                number = numberLineMin - numberToFind;
            } else {
                numberToFind = number;
            }
            
        }
		
		return number;
	}
	
	private int initialNumberFromString (string initialNumber) {
		
		int number = numberLineMin;
		int offNumber = -1000;
		
		if(initialNumber == null) return number;
		
		
		string [] initialArray = initialNumber.Split(new char [] {':'});
		
		string numberString = initialArray[0];
		string [] numberArray = numberString.Split(new char [] {','});
		
		if(numberArray == null || numberArray.Length == 0) {
			number = numberLineMin;
		} else if (numberArray.Length == 1) {
			number = System.Convert.ToInt32(numberArray[0]);
		} else if (numberArray.Length == 2) {
			number = (int) Random.Range(System.Convert.ToInt32(numberArray[0]), System.Convert.ToInt32(numberArray[1])+1);
		} else {
			number = numberLineMin;
		}
		
		if(initialArray.Length == 2) {
			BWNumberSequence condition = getNumberSequenceFromString(initialArray[1]);
			switch(condition) {
			case BWNumberSequence.BWNumberSequenceNumberAsked:
				number = numberToFind + number;
				break;
			case BWNumberSequence.BWNumberSequenceStartingPoint:
				offNumber = number;
				number = beeStartingIndex + number;
				break;
			default:
				number = numberLineMin;
				break;
			}
		}
		
		if (number < numberLineMin || number > numberLineMax) {
            number = numberLineMin;
        }
		
		//finally
		if (beeStartingIndex == -1000) {
            return number;
        }
		
		if (currentCategoryID.Equals("180")) {
            if (offNumber == -5 && beeStartingIndex == 0) {
                number = -3;
            }
            
            if (currentLevel == 49 || currentLevel == 50) {
                if (beeStartingIndex <= 0) {
                    number = -3;
                } else {
                    number = -2;
                }
            }
        }
        
        if (number + 6 >= numberLineMax) {
            number = numberLineMax - 7;
        }
		
		//Debug.Log("initial number" + number);
		
		return number;
	}
	
	
	private int anySmallerNumberAvailable(int _number) {
		
	    if (numberSequence == BWNumberSequence.BWNumberSequenceInOrder || numberSequence == BWNumberSequence.BWNumberSequenceReverse || numbersForLevel.Count > 1) {
	        return _number;
	    } else {
	        
	        int index = Random.Range(0, numbersForLevel.Count);
	        int num = System.Convert.ToInt32(numbersForLevel[index]);
	        int counter = 0;
	        do {
	            counter++;
	            index = Random.Range(0, numbersForLevel.Count);
	            num = System.Convert.ToInt32(numbersForLevel[index]);
	            
	        } while (num < _number && counter < 30);
	        
	        return _number;
	    }
	}
	
	private int anyGreaterNumberAvailable(int _number) {
	    if (numberSequence == BWNumberSequence.BWNumberSequenceInOrder || numberSequence == BWNumberSequence.BWNumberSequenceReverse || numbersForLevel.Count > 1) {
	        return _number;
	    } else {
	        
	        int index = Random.Range(0, numbersForLevel.Count);
	        int num = System.Convert.ToInt32(numbersForLevel[index]);
	        int counter = 0;
	        do {
	            counter++;
	            index = Random.Range(0, numbersForLevel.Count);
	            num = System.Convert.ToInt32(numbersForLevel[index]);
	            
	        } while (num > _number && counter < 30);
	        
	        return _number;
	    }
	}

}
