#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

//#define MAC_PLATFORM

using UnityEngine;
using System.Collections;


public enum NamingFruits_CategoryID{
	id_Food=0,
	id_AlplabeticOrder=1,
};

public enum SimpleColors_CategoryID{
	id_Colors=0,
	id_2DShapes=1
};

public class NF_GameController : MonoBehaviour {

public AudioClip[] winningSounds;
public GameObject vineModel;	
public GameObject ollyModel;
public GameObject background;
public GameObject foreground;
public GameObject frontLayer;

private string[] backgroundArray;
private string[] foregroundArray;
private string[] frontLayerArray;
private string backgroundImage;
private string foregroundImage;
private string frontLayerImage;
private int questionIndexDM;
private int totalQuestionsDM;
private int gameModeDM;	
private string questionTitle;
private string questionTitleToShow;
private ArrayList availableShapes;
public int answersListCount;	
	
// objects of all Classes
public JW_DataManager dataManager;
public NF_QuestionLoader questionLoader;
public NF_RopesGenerator ropesGenerator;
public NF_InputController inputController;
public NF_DragTrail dragTrail;
public TextAsset _progressionList;
public TextAsset _resourcesData;
public int currentCategoryID;
private bool showReferent;
private float ollyCreationTime = 5.0f;	
public bool isResetInProgress = false;
float hardModeTime=0;	
public bool isIntro_AnimationDone=false;
bool changeBackground=false;
int backgroundsCount=0;	

void Start(){
		
		//playIntroAnimation();
		if(PlayerPrefs.GetInt("isIntro_AnimationDone") !=1){
			changeBackground=false;
			playIntroAnimation();
		}
		else{
			changeBackground=true;
			GameObject vine = GameObject.Find("Vine");
			NF_VineAnimations vineAnim = (NF_VineAnimations) vine.GetComponent<NF_VineAnimations>();
			vineAnim.destroyVine();			
			
			GameObject icky = GameObject.Find("Icky_Model");
			NF_IckyAnimations ickyAnim = (NF_IckyAnimations) icky.GetComponent<NF_IckyAnimations>();
			
			ickyAnim.setIckyPosition();
			ickyAnim.setInitialStateOfIcky();
			
			//load question now
			getAllComponentsRef();
			getInitialSetupFromDM();
		}
}
	
void createOllyModel(){
		
		GameObject olly = Instantiate(ollyModel,ollyModel.transform.position,Quaternion.identity) as GameObject;
		NF_OllyAnimations ollyAnim = (NF_OllyAnimations) olly.GetComponent<NF_OllyAnimations>();
		ollyAnim.ollyStates = NF_OllyStates.olly_idle;
		ollyAnim.ollyPeakingOut();
		CancelInvoke("createOllyModel");
		Invoke("createOllyModel",ollyCreationTime);
}
	
void getAllComponentsRef(){
	
	GameObject question = GameObject.Find("Question");
	questionLoader = (NF_QuestionLoader) question.GetComponent<NF_QuestionLoader>();
	
	GameObject trail = GameObject.Find("Drag_Trail");
	dragTrail = (NF_DragTrail) trail.GetComponent<NF_DragTrail>();
	ropesGenerator = (NF_RopesGenerator) this.GetComponent<NF_RopesGenerator>();
	inputController = (NF_InputController) this.GetComponent<NF_InputController>();
		
	// select skill id On random
	if(AGGameState.currentGameIndex == (int) AGGameIndex.k_NamingFruits){
			currentCategoryID=(int)Random.Range((int)NamingFruits_CategoryID.id_Food,(int)NamingFruits_CategoryID.id_AlplabeticOrder+1);
			backgroundsCount=4;
			backgroundArray = new string[] {"Green","Orange","Blue","Sunrise"};
			foregroundArray = new string[] {"Top-Green-Back","Top-Orange-Back","Top-Blue-Back","Top-Sunrise-Back"};
			frontLayerArray = new string[] {"Top-Green-Front","Top-Orange-Front","Top-Blue-Front","Top-Sunrise-Front"};
			
			dataManager = new JW_DataManager(AGGameIndex.k_NamingFruits,currentCategoryID,_progressionList.text);
			dataManager.loadCategoryResources(_resourcesData.text);
			dataManager.currentLevel=1;
			dataManager.fetchLevelData();
			
			}
	else if(AGGameState.currentGameIndex == (int) AGGameIndex.k_2DShapesColors){
			currentCategoryID= (int)Random.Range((int)SimpleColors_CategoryID.id_Colors,(int)SimpleColors_CategoryID.id_2DShapes+1);
			backgroundsCount=2;
			backgroundArray = new string[] {"Pink","Purple"};
			foregroundArray = new string[] {"Pink-Back-Frill","Purple-Back-Frill"};
			frontLayerArray = new string[] {"Pink-Front-Frill","Purple-Front-Frill"}; 
			dataManager = new JW_DataManager(AGGameIndex.k_2DShapesColors,currentCategoryID,_progressionList.text);
			dataManager.loadCategoryResources(_resourcesData.text);
			dataManager.currentLevel=1;
			dataManager.fetchLevelData();	
		}
	Debug.Log("NF_Skill ID:" + currentCategoryID);	
}

public void thoughtBubbleFadeIn(){
	//if(!isResetInProgress && isIntro_AnimationDone==true){
	if(!isResetInProgress){ 
		questionLoader.fadeInAllObjects();
		}
}
	
void getResourcesFromPlist(){
		
	availableShapes = new ArrayList();
	if(currentCategoryID==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
			
		ArrayList finalAvailableShapes =new ArrayList();
		finalAvailableShapes = dataManager.getAvailableShapes();
			
			for(int k=0;k<finalAvailableShapes.Count;k++){
				Debug.Log("Shape: " + finalAvailableShapes[k] +" _at Index:" + k);
					}
		availableShapes = dataManager.getAvailableShapes();
		for(int i=0;i<finalAvailableShapes.Count;i++){
			string shapeRes= dataManager.getShapeResources((string)finalAvailableShapes[i]);
			availableShapes.Add(shapeRes);
			Debug.Log("Resources:" + shapeRes);
			}
		}
		else
			availableShapes = dataManager.getAvailableShapes();
	}
	
	
public void initializeGameController(){
		
	Debug.Log("Initializer GameController");	
	isIntro_AnimationDone=true;
	changeBackground=false;
	PlayerPrefs.SetInt("isIntro_AnimationDone",1);
	getAllComponentsRef();
	getInitialSetupFromDM();
	}
void playIntroAnimation(){
	
		GameObject icky = GameObject.Find("Icky_Model");
		NF_IckyAnimations ickyAnim = (NF_IckyAnimations) icky.GetComponent<NF_IckyAnimations>();
		ickyAnim.IckyIntroAnimation();
		
		GameObject olly = Instantiate(ollyModel,ollyModel.transform.position,Quaternion.identity) as GameObject;
		olly.transform.position = new Vector3(icky.transform.position.x -200 ,icky.transform.position.y+50,750);
		NF_OllyAnimations ollyAnim = (NF_OllyAnimations) olly.GetComponent<NF_OllyAnimations>();
		ollyAnim.ollyTweenSequence(-650.0f,0.0f,NF_OllyStates.olly_walking);
	}
void getInitialSetupFromDM(){
	
	dataManager.NF_generateQuestion();
	availableShapes = dataManager.getAvailableShapes();
	answersListCount = dataManager.getAnswersListCount();
	questionTitle = dataManager.getCorrectQuestionTitle();
	showReferent= dataManager.getShowReferent();	
	questionTitleToShow = dataManager.getQuestionTitle();
	gameModeDM = dataManager.gameModeSwitching();
	questionIndexDM = dataManager.getIndexOfCurrentPlayingQuestion();
	totalQuestionsDM = dataManager.numberOfQuestionPresented();
	hardModeTime = dataManager.getHardModeTime();
	
	if(changeBackground==true){
			
		int backgroundIndex = Random.Range(0,backgroundsCount);
		backgroundImage = backgroundArray[backgroundIndex];
		foregroundImage = foregroundArray[backgroundIndex];
		frontLayerImage = frontLayerArray[backgroundIndex];
		
		// set background at random whenever new level is loaded
		background.renderer.material.mainTexture= Resources.Load(NF_Constants._backgroundsPath+backgroundImage) as Texture;
		foreground.renderer.material.mainTexture= Resources.Load(NF_Constants._backgroundsPath+foregroundImage) as Texture;
		frontLayer.renderer.material.mainTexture= Resources.Load(NF_Constants._backgroundsPath+frontLayerImage) as Texture;
	}
	Debug.Log("GC_QuestionLoaded:  "+questionTitle);
	questionLoader.questionInitializer(questionIndexDM,questionTitleToShow,currentCategoryID,gameModeDM,answersListCount,showReferent,questionTitle); // setup question elements
	ropesGenerator.generateRopeForQuestion(totalQuestionsDM,questionTitle,availableShapes,currentCategoryID,gameModeDM,questionTitleToShow);// generate Ropes
	GameObject correctRope = ropesGenerator.getCorrectRope();
	inputController.inputControllerInitializer(questionTitleToShow,questionTitle,correctRope,answersListCount,currentCategoryID);	
		
	// check hardMode here
//	if(hardModeTime>0){
//		Invoke("createMonkeys",hardModeTime);
//		}
		
	// call olly creation	
	Invoke("createOllyModel",ollyCreationTime);
}

void createMonkeys(){
		ropesGenerator.createMonkeyForHardMode();
		CancelInvoke("createMonkeys");
	}
void resetAllLevelState(){
		
	//yield return new WaitForSeconds(delay);
	inputController.resetInputController();
	ropesGenerator.resetRopeGenerator();
	questionLoader.resetQuestionLoader();
	NF_IckyAnimations ickyAnim = (NF_IckyAnimations) GameObject.Find("Icky_Model").GetComponent<NF_IckyAnimations>();
	ickyAnim.setInitialStateOfIcky();
		
	getInitialSetupFromDM();
	isResetInProgress = false;	
}	

public void loadNextQuestion(float timeToLoad){
		
		float restartLevelTime =0;
		//restartLevelTime= timeToLoad;
		Debug.Log("Load Next Question");
	 	int questionAttempts = 0;
		int wrongAttempts = 0;
		
		
		if(isResetInProgress){
			inputController.isTouchEnabled = false;
			return;
		}
		isResetInProgress = true;
		questionAttempts = inputController.qAttemptsCount();
		wrongAttempts = inputController.wrongAttempts;
		if(questionAttempts > 0) {
#if(MAC_PLATFORM)
				AGGameState.incrementStarCount();
			if(AGGameState.currentGameIndex==(int) AGGameIndex.k_NamingFruits){
				if(dataManager.currentLevel==17 || dataManager.currentLevel ==8)
					dataManager.currentLevel=1;
				else
					dataManager.currentLevel++;
				}
		else if(AGGameState.currentGameIndex==(int) AGGameIndex.k_2DShapesColors){
				if(dataManager.currentLevel==17 || dataManager.currentLevel ==20)
					dataManager.currentLevel=1;
				else
					dataManager.currentLevel++;
				}
		
				if(wrongAttempts>=5){
						restartLevelTime=1.5f;
					}
					else if(wrongAttempts<5){
						inputController.setCelebrationStatus(true);
						restartLevelTime=3.0f;
					}
			dataManager.fetchLevelData();
			questionAttempts = 0;
			wrongAttempts=0;
			CancelInvoke("createOllyModel");
			Invoke("resetAllLevelState",restartLevelTime);
#else
		AGGameState.incrementStarCount();
		bool isLevelDone = dataManager.calculateResult(questionAttempts,answersListCount);
		if (isLevelDone){
				restartLevelTime=3.0f;
				inputController.setCelebrationStatus(true);
				dataManager.fetchLevelData();
				}
		else if(!isLevelDone){
					if(wrongAttempts==5){
						restartLevelTime=1.25f;
					}
					else
						restartLevelTime=2.0f;
				}
			
			wrongAttempts=0;
			questionAttempts=0;
			CancelInvoke("createOllyModel");
			Invoke("resetAllLevelState",restartLevelTime);
#endif
		}
	}	
}
