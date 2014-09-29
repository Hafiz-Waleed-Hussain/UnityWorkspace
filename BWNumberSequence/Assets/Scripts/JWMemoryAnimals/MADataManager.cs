//#define MAC_PLATFORM

using UnityEngine;
using System.Collections;

public class MADataManager {

	private Hashtable allLevels = null;
	private Hashtable allCategoriesCoreData = null;
	
	private CoreDataManager coreManager = null;
	
	float easyThreshold;
    float hardThreshold;
    int failEasyGoto;
    int succeedHardGoto;
    int minQuestions;
    int maxQuestions;
	ArrayList slotsForLevel = null;
	
	public int currentLevel = 0;
	public int currentSlots = 4;
	
	public int currentCategoryID = 169;
	public string currentCategory = "MemorySkill1";
	
	public MADataManager (AGGameIndex gameIndex, int categoryIndex, string file) {
		
		CategoriesData data = CategoriesData.getSharedData(file);
		Hashtable maData = data.getGameDataForIndex(gameIndex);
		allLevels = (Hashtable)maData["allLevels"];
		
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
		if (currentLevel == 0) currentLevel = 1;

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
		
		slotsForLevel = (ArrayList)currentLevelData["slots"];	
	}
	
	public void fetchNextQuestionData () {
		
		int rand = UnityEngine.Random.Range(0, slotsForLevel.Count);
		currentSlots = GetAsInt(slotsForLevel[rand]);
	}
	
	public bool calculateResult (int attempts, int corrects) {	
		
		//return true;
		
		if(coreManager == null) return false;
		
		string objectKey = string.Format("Animals");		
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
		
		resultDict["totalQuestionsPlayed"] = total_questionAttempted;
		resultDict["attempted"] = total_levelAttempts;
		resultDict["solved"] = total_levelCorrects;
	    
		resultDict["questionsPlayed"] = questionAttempted;
		resultDict["levelAttempts"] = levelAttempts;
		resultDict["levelCorrects"] = levelCorrects;
		resultDict["hasMastered"] = hasMastered;
		
		bool levelDone = false;
		if(hasMastered) {
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
		
		string str = string.Format("Animals");
		Hashtable numberDict = (Hashtable)allCategoriesCoreData[str];
		levelAttempts += GetAsInt(numberDict["levelAttempts"]);
		levelCorrects += GetAsInt(numberDict["levelCorrects"]);
	
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
}