#region AGNITUS 2013
/* JungleWorld- DataManager
* Unity3D*/
#endregion

//#define MAC_PLATFORM

using UnityEngine;
using System.Collections;

enum GameModes{
	mode_same=0,
	mode_similiar=1,
	mode_counting=2,
	mode_different=3
};
public class JW_DataManager {
		
private ArrayList finalAvailableShapes;
private ArrayList shapesToPlay = null;
public ArrayList availableShapes=null;  // all available shapes from plist

private CoreDataManager coreManager = null;
private Hashtable allLevels = null;
private Hashtable allCategoriesCoreData = null;
private Hashtable resourcesData =null;
int correctAnswerIndex;
int wrongAnswerIndex;
float easyThreshold;
float hardThreshold;
int failEasyGoto;
int succeedHardGoto;
int minQuestions;
int maxQuestions;

int questionsAsked;
int totalQuestions;

public int	questionSpriteCount;
public int	answerSpriteCount;

Hashtable questionData=null;    //  question data, having modes (counting,different,same,similiar)

// Modes of the Game
Hashtable countingMode=null;
Hashtable differentMode=null;
Hashtable sameSimilarMode=null;

Hashtable categoryDataResources = null;
// Same mode data
bool HardMode;
int  HardModeTime=0;
int  minASprites;
int  maxASprites;
int  minQSprites;
int  maxQSprites;
int  optionsShown;
bool showReferent;

public int currentLevel;
public string correctAnswerSpriteTitle;
public string _skillName;
private int currentGameIndex;
int gameMode;
int _CategoryIndex;
int gameIndex;
public int currentSkillID; // Jungle World Games
public string catergoryName;	
private string finalcorrectATitle;	
string shapeResourceName;	
private void getSkillIDForGame(AGGameIndex gindex){
			
		switch(gindex){
			case AGGameIndex.k_NamingFruits:{
					if(_CategoryIndex==(int)NamingFruits_CategoryID.id_Food){
						currentSkillID=32;
						catergoryName = "Food";
						shapeResourceName = "FruitGame";
							}
					else if(_CategoryIndex==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
							currentSkillID=131;
							catergoryName="Alphabetic Order";
							shapeResourceName = "FruitGame";
							}
						}break;
		case AGGameIndex.k_2DShapesColors:{	
				 if(_CategoryIndex==(int) SimpleColors_CategoryID.id_Colors){
							currentSkillID=132; //temp
							catergoryName="Colors";
							shapeResourceName = "SimpleColors";
							}
			else if(_CategoryIndex==(int) SimpleColors_CategoryID.id_2DShapes){
							currentSkillID=51; // temp
							catergoryName="2-D Shapes";
							shapeResourceName = "SimpleColors";
							}
						}break;
			case AGGameIndex.k_ShapePond:
					currentSkillID= 51;break;
			default: Debug.Log("Wrong Game Index");break;
			}
}

public void loadCategoryResources(string _resourcesData){
		
		Hashtable localData = null;
		bool noError = PListManager.ParsePListFile(_resourcesData, ref localData);
		if(noError) {
			resourcesData =(Hashtable)localData["Resources"];
		} else {
			Debug.Log("cannot access progression data");
		}
			categoryDataResources = (Hashtable)resourcesData[catergoryName];
}
	
public string getShapeResources(string shapeName){
		
		Hashtable shapeRes = (Hashtable)categoryDataResources[shapeName];
		ArrayList _shapesArray = (ArrayList)shapeRes[shapeResourceName];
		return (string)_shapesArray[Random.Range(0,_shapesArray.Count)];
}
	
public JW_DataManager (AGGameIndex gIndex, int categoryIndex, string file) {
		
		_CategoryIndex=categoryIndex;
		gameIndex=(int)gIndex;
		
		getSkillIDForGame(gIndex);
		CategoriesData data = CategoriesData.getSharedData(file);
		Hashtable shapesData = data.getGameDataForIndex(gIndex);
		ArrayList availableCat = (ArrayList)shapesData["skills"];
		allLevels = (Hashtable)availableCat[categoryIndex];

   //----------- Core Communication	
	#if (!MAC_PLATFORM)
				coreManager = new CoreDataManager(currentSkillID);
				allCategoriesCoreData = coreManager.getResultDict();
				currentLevel = coreManager.getCurrentLevel();
	#endif
}

public void fetchLevelData (){
		
		if(allLevels == null){
			Debug.Log("Exception: level data not available ... something wrong with plist");
			return;
		}
		
	#if (!MAC_PLATFORM)
			currentLevel = coreManager.getCurrentLevel();
	#else
		if(gameIndex==(int)AGGameIndex.k_NamingFruits || gameIndex==(int)AGGameIndex.k_2DShapesColors)
			currentLevel=currentLevel;
		else
			currentLevel=0;
	#endif	
		if (currentLevel ==0) 
			currentLevel =1;          //-------if the game starts from level other than 0 than you dont need this check
		
		Debug.Log("currentLevel: "+currentLevel);
		
		string currentLevelString = string.Format("level{0}", currentLevel);
	    Hashtable currentLevelData = (Hashtable)allLevels[currentLevelString];
		
	    if (currentLevelData == null){
			Debug.Log("Exception: not data for current level available ... trying level1 ...");
	        currentLevelData = (Hashtable)allLevels["level1"];
	        if (currentLevelData == null) {
				Debug.Log("Exception: not data for any level available ... terminate");
	            return;
	        }
	    }
		easyThreshold 		= GetAsFloat(currentLevelData["easyThreshold"]);
		hardThreshold	    = GetAsFloat(currentLevelData["hardThreshold"]);
	    failEasyGoto        = GetAsInt(currentLevelData["failEasyGotoLevel"]);
	    succeedHardGoto     = GetAsInt(currentLevelData["succeedHardGotoLevel"]);
	    minQuestions        = GetAsInt(currentLevelData["minQuestionsOnLevel"]);
	    maxQuestions        = GetAsInt(currentLevelData["maxQuestionsOnLevel"]);
	    availableShapes     = (ArrayList)currentLevelData["available"];
		questionData 		= (Hashtable)currentLevelData["questionData"]; 
		
		sameSimilarMode=(Hashtable)questionData["same"];
		differentMode=(Hashtable)questionData["different"];
		countingMode=(Hashtable)questionData["counting"];
		

#if (MAC_PLATFORM)
		shapesToPlay = new ArrayList();
		for(int i=0;i<availableShapes.Count;i++){
			shapesToPlay.Add(availableShapes[i]);
		}
#else
		shapesToPlay = new ArrayList();
		foreach(string shape in availableShapes)
		{
			Hashtable shapeDict = (Hashtable)allCategoriesCoreData[shape];
			bool hasMastered = (bool)shapeDict["hasMastered"];
			if(!hasMastered) 
			{
				shapesToPlay.Add(shape);
				Debug.Log("Shapes not mastered: " + shape);
			}
			Debug.Log("All available shapes: "+shape);
	   }
#endif
		
}
	
private  ArrayList ShuffleList(ArrayList _list){
		
        ArrayList randomizedList = new ArrayList();
        while (_list.Count > 0)
        {
            int index = Random.Range(0, _list.Count); //pick a random item from the master list
            randomizedList.Add(_list[index]); //place it at the end of the randomized list
            _list.RemoveAt(index);
        }
        return randomizedList;
   }

public void PG_generateQuestion(){
		
			gameMode=loadGameMode();
			int optionsIndex;
			correctAnswerIndex =Random.Range(0,shapesToPlay.Count);
			finalAvailableShapes = new ArrayList();
			ArrayList optionsShownList=new ArrayList(optionsShown);
			correctAnswerSpriteTitle =(string) shapesToPlay[correctAnswerIndex];
		
			// ----------------- Same/Similiar Mode
		if(gameMode==(int)GameModes.mode_same || gameMode==(int)GameModes.mode_similiar){
			
			HardMode=(bool)sameSimilarMode["HardMode"];
			if(HardMode) HardModeTime=GetAsInt(sameSimilarMode["HardModeTime"]);
			else HardModeTime=0;
			minASprites=GetAsInt(sameSimilarMode["minASprites"]);
		    maxASprites=GetAsInt(sameSimilarMode["maxASprites"]);
		    minQSprites=GetAsInt(sameSimilarMode["minQSprites"]);
		    maxQSprites=GetAsInt(sameSimilarMode["maxQSprites"]);
		    optionsShown=GetAsInt(sameSimilarMode["optionsShown"]);
		    showReferent=(bool)sameSimilarMode["showReferent"]; 
			
			questionSpriteCount =Random.Range(minQSprites,maxQSprites+1);
			answerSpriteCount	=Random.Range(minASprites,maxASprites+1);

			if(correctAnswerSpriteTitle!=null)
				
				for(int i=0;i<optionsShown-1;i++){
						do{
							optionsIndex=Random.Range(0,availableShapes.Count);
						
						}while((availableShapes[optionsIndex].ToString()==correctAnswerSpriteTitle) || optionsShownList.Contains(availableShapes[optionsIndex]));
				
						optionsShownList.Add(availableShapes[optionsIndex]);
					}
				for(int j=0;j<answerSpriteCount;j++){   
							finalAvailableShapes.Add(correctAnswerSpriteTitle);
					 }
				for(int k=finalAvailableShapes.Count;k<questionSpriteCount;k++){
						int index = k % optionsShownList.Count;
						finalAvailableShapes.Add(optionsShownList[index]);
						}
			finalAvailableShapes= ShuffleList(finalAvailableShapes);
		}
			// ----------------- Counting Mode
		else if(gameMode==(int)GameModes.mode_counting){
			
			HardMode=(bool)countingMode["HardMode"];
			if(HardMode) HardModeTime=GetAsInt(countingMode["HardModeTime"]);
			else HardModeTime=0;
			minASprites=0;
		    maxASprites=0;
		    minQSprites=GetAsInt(countingMode["minQSprites"]);
		    maxQSprites=GetAsInt(countingMode["maxQSprites"]);
		    optionsShown=0;
		    showReferent=(bool)countingMode["showReferent"];
			questionSpriteCount =Random.Range(minQSprites,maxQSprites+1);
			answerSpriteCount	=questionSpriteCount;
			//Debug.Log("Counting Mode...");	
			for(int i=0;i<questionSpriteCount;i++){ 
				finalAvailableShapes.Add(correctAnswerSpriteTitle);
			}
		}
			// ----------------- Different Mode
		else if(gameMode==(int)GameModes.mode_different){
			
			HardMode=(bool)differentMode["HardMode"];
			if(HardMode) HardModeTime=GetAsInt(differentMode["HardModeTime"]);
			else HardModeTime=0;
			minASprites=0;
		    maxASprites=0;
		    maxQSprites=GetAsInt(differentMode["maxQSprites"]);
			minQSprites=maxQSprites;
		    optionsShown=0;
		    showReferent=(bool)differentMode["showReferent"]; 	
			questionSpriteCount =Random.Range(minQSprites,maxQSprites+1);
			answerSpriteCount=1;
			//Debug.Log("Different Mode...");
			int remainingOptionsCount = questionSpriteCount-1;
			do{
				wrongAnswerIndex=Random.Range(0,availableShapes.Count);
	
			}while(availableShapes[wrongAnswerIndex].ToString()==correctAnswerSpriteTitle);
		
		string wrongQTitle = (string)availableShapes[wrongAnswerIndex];
		// add all correct and wrong answers in a availableshapesToDisplay to generate questions 
		for(int i=0;i<remainingOptionsCount;i++){
				finalAvailableShapes.Add(wrongQTitle);
				}
		finalAvailableShapes.Add(correctAnswerSpriteTitle);	
		// shuffle available shapes array and passed to question generator to create ropes
	 	finalAvailableShapes= ShuffleList(finalAvailableShapes);
		}
}
	
public void NF_generateQuestion(){
		
		gameMode=loadGameMode();
		
			int optionsIndex;
			correctAnswerIndex =Random.Range(0,shapesToPlay.Count);
			finalAvailableShapes = new ArrayList();
			ArrayList optionsShownList=new ArrayList(optionsShown);
			correctAnswerSpriteTitle =(string) shapesToPlay[correctAnswerIndex];
		
			// ----------------- Same/Similiar Mode
		if(gameMode==(int)GameModes.mode_same || gameMode==(int)GameModes.mode_similiar){
			
			Debug.Log("Same/Similar Mode");
			
			HardMode=(bool)sameSimilarMode["HardMode"];
			if(HardMode) HardModeTime=GetAsInt(sameSimilarMode["HardModeTime"]);
			minASprites=GetAsInt(sameSimilarMode["minASprites"]);
		    maxASprites=GetAsInt(sameSimilarMode["maxASprites"]);
		    minQSprites=GetAsInt(sameSimilarMode["minQSprites"]);
		    maxQSprites=GetAsInt(sameSimilarMode["maxQSprites"]);
		    optionsShown=GetAsInt(sameSimilarMode["optionsShown"]);
		    showReferent=(bool)sameSimilarMode["showReferent"]; 
			
			questionSpriteCount =Random.Range(minQSprites,maxQSprites+1);
			answerSpriteCount	=Random.Range(minASprites,maxASprites+1);
			
			if(correctAnswerSpriteTitle!=null)
				
				for(int i=0;i<optionsShown-1;i++){
						do{
							optionsIndex=Random.Range(0,availableShapes.Count);
						
						}while((availableShapes[optionsIndex].ToString()==correctAnswerSpriteTitle) || optionsShownList.Contains(availableShapes[optionsIndex]));
				
						optionsShownList.Add(availableShapes[optionsIndex]);
					}
				for(int j=0;j<answerSpriteCount;j++){   
							finalcorrectATitle = getShapeResources(correctAnswerSpriteTitle);
							finalAvailableShapes.Add(finalcorrectATitle);
					 }
				for(int k=finalAvailableShapes.Count;k<questionSpriteCount;k++){
						int index = k % optionsShownList.Count;
						finalAvailableShapes.Add(getShapeResources((string)optionsShownList[index]));
						}
			finalAvailableShapes= ShuffleList(finalAvailableShapes);
		}
			// ----------------- Counting Mode
		else if(gameMode==(int)GameModes.mode_counting){
			
			HardMode=(bool)countingMode["HardMode"];
			if(HardMode) HardModeTime=GetAsInt(countingMode["HardModeTime"]);
			minASprites=0;
		    maxASprites=0;
		    minQSprites=GetAsInt(countingMode["minQSprites"]);
		    maxQSprites=GetAsInt(countingMode["maxQSprites"]);
		    optionsShown=0;
		    showReferent=(bool)countingMode["showReferent"];
			questionSpriteCount =Random.Range(minQSprites,maxQSprites+1);
			answerSpriteCount	=questionSpriteCount;
			Debug.Log("Counting Mode...");	
			for(int i=0;i<questionSpriteCount;i++){ 
				finalcorrectATitle = getShapeResources(correctAnswerSpriteTitle);
				finalAvailableShapes.Add(finalcorrectATitle);
			}
		}
			// ----------------- Different Mode
		else if(gameMode==(int)GameModes.mode_different){
			
			Debug.Log("Different Mode");
			
			HardMode=(bool)differentMode["HardMode"];
			if(HardMode) HardModeTime=GetAsInt(differentMode["HardModeTime"]);
			minASprites=0;
		    maxASprites=0;
		    maxQSprites=GetAsInt(differentMode["maxQSprites"]);
			minQSprites=maxQSprites;
		    optionsShown=0;
		    showReferent=(bool)differentMode["showReferent"]; 	
			questionSpriteCount =Random.Range(minQSprites,maxQSprites+1);
			answerSpriteCount=1;
			int remainingOptionsCount = questionSpriteCount-1;
			do{
				wrongAnswerIndex=Random.Range(0,availableShapes.Count);
	
			}while(availableShapes[wrongAnswerIndex].ToString()==correctAnswerSpriteTitle);
		
		string wrongQTitle = (string)availableShapes[wrongAnswerIndex];
		// add all correct and wrong answers in a availableshapesToDisplay to generate questions 
		for(int i=0;i<remainingOptionsCount;i++){
				finalAvailableShapes.Add(getShapeResources(wrongQTitle));
				}
			finalcorrectATitle = getShapeResources(correctAnswerSpriteTitle);
			finalAvailableShapes.Add(finalcorrectATitle);
		// shuffle available shapes array and passed to question generator to create ropes
	 	finalAvailableShapes= ShuffleList(finalAvailableShapes);
		}
}
	
public bool calculateResult (int attempts, int corrects) {	
		
	if(coreManager == null) return false;
	
	string objectKey = correctAnswerSpriteTitle;
	Debug.Log("CR-Object Key:"+objectKey);
	Hashtable resultDict = (Hashtable)allCategoriesCoreData[objectKey];
	
	if(resultDict == null) return false;
	
	Debug.Log("Reading values from dict");
	
	int questionAttempted = GetAsInt(resultDict["questionsPlayed"]);
	int levelAttempts = GetAsInt(resultDict["levelAttempts"]);
	int levelCorrects = GetAsInt(resultDict["levelCorrects"]);
	
	Debug.Log("Question Played: "+questionAttempted);
	Debug.Log("Level Attempts: "+levelAttempts);
	Debug.Log("Level Corrects: "+levelCorrects);
		
		
	int total_questionAttempted = GetAsInt(resultDict["totalQuestionsPlayed"]);
	int total_levelAttempts = GetAsInt(resultDict["attempted"]);
	int total_levelCorrects = GetAsInt(resultDict["solved"]);
	
	Debug.Log("T.Question Played: "+total_questionAttempted);
	Debug.Log("T.Attempted: "+total_levelAttempts);
	Debug.Log("T.Solved: "+total_levelCorrects);	
		
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
	
		
	Debug.Log("obtainedThreshold: "+obtainedThreshold);
	Debug.Log("minQuestions: "+minQuestions);
		
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
	Debug.Log("hasMastered: "+hasMastered);
	resultDict["totalQuestionsPlayed"] = total_questionAttempted;
	resultDict["attempted"] = total_levelAttempts;
	resultDict["solved"] = total_levelCorrects;
	
	resultDict["questionsPlayed"] = questionAttempted;
	resultDict["levelAttempts"] = levelAttempts;
	resultDict["levelCorrects"] = levelCorrects;
	resultDict["hasMastered"] = hasMastered;
	
	if(hasMastered){
		foreach (string shape in shapesToPlay) {
			if(shape==correctAnswerSpriteTitle) {
				Debug.Log("Removed Shape:"+shape);
				shapesToPlay.Remove(shape);
				break;
			}
		}
	}
	bool levelDone = false;
	if (shapesToPlay.Count == 0) {
	    completeCurrentLevel();
	    levelDone = true;
		Debug.Log("Level Completed");	
			
	}
	saveData();
	return levelDone;
}

private void completeCurrentLevel () {

	string currentLevelString = string.Format("level{0}", currentLevel);
	Hashtable currentLevelData = (Hashtable)allLevels[currentLevelString];
	
	ArrayList shapesArray = (ArrayList) currentLevelData["available"];
	
	int levelAttempts = 0;
	int levelCorrects = 0;
	
	foreach (string shape in shapesArray){
		Hashtable shapesDict = (Hashtable)allCategoriesCoreData[shape];
		levelAttempts += GetAsInt(shapesDict["levelAttempts"]);
		levelCorrects += GetAsInt(shapesDict["levelCorrects"]);
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
			Debug.Log("Current Level:"+currentLevel);
	    }
	}
	else if (obtainedThreshold >= hardThreshold) {
	    //succeed hard
	    currentLevel = succeedHardGoto;
		}

	coreManager.setCurrentLevel(currentLevel);
		
	Debug.Log("In levelCompleteWithThreshold: "+currentLevel);	
		
	//if user has completed the last level and category status is not set then set it.
	if(prevLevel == coreManager.getTotalLevels() && coreManager.getCategoryStatus() == 0) {
		coreManager.setCategoryStatus(1);
		}

	Debug.Log("Level changed");
	saveData();
	resetLevelSpecificData();
}

private void saveData (){
	coreManager.setCurrentLevel(currentLevel);
	coreManager.setResultDict(allCategoriesCoreData);
	coreManager.saveManager();
}

private void resetLevelSpecificData (){

foreach(string key in allCategoriesCoreData.Keys){
			
	Hashtable numDict = (Hashtable)allCategoriesCoreData[key];
	numDict["questionsPlayed"] = 0;
	numDict["levelAttempts"] = 0;
	numDict["levelCorrects"] = 0;
	numDict["hasMastered"] = false;
	}
}

#region Decide for Game mode
public int loadGameMode()
{
	gameMode=Random.Range((int)GameModes.mode_same,(int)GameModes.mode_different+1);
	if(gameMode==(int)GameModes.mode_counting && gameIndex==(int)AGGameIndex.k_NamingFruits && _CategoryIndex==(int)NamingFruits_CategoryID.id_Food){
				Debug.Log("Changed gameMode to similar");	
				gameMode = (int)GameModes.mode_similiar;
			}
		// check for alphabetic order	
	else if(gameMode==(int)GameModes.mode_counting && gameIndex==(int)AGGameIndex.k_NamingFruits && _CategoryIndex==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
				Debug.Log("Changed gameMode to similar");
				gameMode = (int)GameModes.mode_similiar;
		}
	else if(gameMode==(int)GameModes.mode_different && gameIndex==(int)AGGameIndex.k_NamingFruits && _CategoryIndex==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
							Debug.Log("Changed gameMode to same");	
							gameMode = (int)GameModes.mode_same;
		}
		
	// check for 2D Shapes
	else if(gameMode==(int)GameModes.mode_counting && gameIndex==(int)AGGameIndex.k_2DShapesColors && _CategoryIndex==(int)SimpleColors_CategoryID.id_2DShapes){
				Debug.Log("Changed gameMode to similar");
				gameMode = (int) GameModes.mode_similiar;
		}
		// check for Colors 
	else if(gameMode==(int)GameModes.mode_counting && gameIndex==(int)AGGameIndex.k_2DShapesColors && _CategoryIndex==(int)SimpleColors_CategoryID.id_Colors){
				Debug.Log("Changed gameMode to same");
				gameMode = (int)GameModes.mode_same;
		}
	else if(gameMode==(int)GameModes.mode_similiar && gameIndex==(int)AGGameIndex.k_2DShapesColors && _CategoryIndex==(int)SimpleColors_CategoryID.id_Colors){
				Debug.Log("Changed gameMode to same");
				gameMode = (int)GameModes.mode_same;
		}
		
		
	return gameMode;

}
#endregion	

#region Dummy Progression with all modes
public ArrayList getAvailableShapes(){
		return finalAvailableShapes;
 }
public int getIndexOfCurrentPlayingQuestion(){
			return correctAnswerIndex;
 }
public int gameModeSwitching(){
	return gameMode;
}
public int getAnswersListCount(){
	return answerSpriteCount;
}
public string getQuestionTitle(){ 
	return correctAnswerSpriteTitle;
}
public string getCorrectQuestionTitle(){
	return finalcorrectATitle;
}
public int numberOfQuestionPresented(){
	return questionSpriteCount;
}	
public bool getShowReferent(){
	return showReferent;
	}
	
public int getHardModeTime(){
//		
//		if(gameIndex==(int)AGGameIndex.k_NamingFruits && _CategoryIndex==(int)NamingFruits_CategoryID.id_Food){
//			if(currentLevel>5 && currentLevel<=17){
//				HardMode=true;
//				HardModeTime=6;
//				}
//			}
		return HardModeTime;
	}	
	
public int getMinQuestionPlayReqForResultCal(){
	return minQuestions;	
}
	
	
	
#endregion	


//  ==========================================
	public float GetAsFloat(object theObject)
{
    if(theObject == null) return 0.0f;
    if(theObject.GetType() == typeof(int)) 
	{
		return (float)(int)theObject;
	} 
	else if(theObject.GetType() == typeof(long)) 
	{
		return (float)(long)theObject;
	} 
	else if(theObject.GetType() == typeof(float))
	{
		return (float)theObject;
	} 
	else if(theObject.GetType() == typeof(double)) 
	{
		return (float)(double)theObject;
	}
	
	return (float)theObject;
}

public int GetAsInt(object theObject)
{
   
	if(theObject == null) return 0;
    if(theObject.GetType() == typeof(int))
	{
		return (int)theObject;
	} 
	else if(theObject.GetType() == typeof(long))
	{
		return (int)(long)theObject;
	} 
	else if(theObject.GetType() == typeof(float))
	{
		return (int)(float)theObject;
	} 
	else if(theObject.GetType() == typeof(double))
	{
		return (int)(double)theObject;
	}
	
    return (int)theObject;
}


}
