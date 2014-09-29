#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NF_InputController : MonoBehaviour{
	
public AudioClip ropeCutSFx;
public GameObject _tickSign;
public GameObject _crossSign;
private GameObject qTickObj;
private GameObject qCrossObj;
private GameObject selectedFruit;
private NF_IckyAnimations ickyAnim;
private GameObject rope;
private NF_BreakRopeJoints breakJoints;
private string questionTitle;
private NF_AnimationController animationCtrl;
public int _totalAnswersCount;
private int qAttempts;
private bool isWrongAnswer=false;
private NF_GameController gameController;
public bool isTouchEnabled = true;
GameObject fruitObj=null;
GameObject questionLoaderObj;
public int questionPlayedCount=0;	
public GameObject EmptyFruit;
public GameObject particleObj;
public GameObject ropeCutDownPrefab;
public int wrongAttempts =0;
public bool isRestarting=false;	
public GameObject dragTrailObj;
NF_DragTrail dragTrail;
string pathForObjectResource;	
public int randomJumpIndex;
string resourceForParticles;
int categoryIDForSkill;
bool isTouchesEnable_2=false;
	
void Awake(){
	
	dragTrail = dragTrailObj.GetComponent<NF_DragTrail>();	
	gameController = (NF_GameController) Camera.mainCamera.GetComponentInChildren<NF_GameController>();
	addFingureGestureObjects();
	}
	
 void addFingureGestureObjects(){
			// fingure gesture	 add
	FingerGestures.OnTap += FingureGestures_OnTap;
	FingerGestures.OnFingerDragBegin += FingureGestures_OnDragBegin;
	FingerGestures.OnFingerDragMove += FingureGestures_OnDragMove;
	FingerGestures.OnFingerDragEnd += FingureGestures_OnDragEnd;
		
	}
	
 void removeFingureGestureObjects(){
		
			// fingure gesture	 remove
	FingerGestures.OnTap -= FingureGestures_OnTap;
	FingerGestures.OnFingerDragBegin -= FingureGestures_OnDragBegin;
	FingerGestures.OnFingerDragMove -= FingureGestures_OnDragMove;
	FingerGestures.OnFingerDragEnd -= FingureGestures_OnDragEnd;
		
	}

public void disableTouches(){
		isTouchEnabled = false;
		//removeFingureGestureObjects();
		dragTrail.cancelDragTrail();
	}

public void enableTouches(){
		isTouchEnabled = true;
		//addFingureGestureObjects();
		dragTrail.enableDragTrail();
}
	
	
public void inputControllerInitializer(string qtitle,string resourceQTitle, GameObject correctRopeRef,int answerCount, int categoryID){
		
	isRestarting=false;
	categoryIDForSkill = categoryID;
	Debug.Log("Answer: "+ qtitle);
	if(AGGameState.currentGameIndex == (int) AGGameIndex.k_NamingFruits && categoryID==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
		questionTitle = resourceQTitle;
		}
	else{
		questionTitle=qtitle;
		}
		resourceForParticles = resourceQTitle;
	_totalAnswersCount = answerCount;	
	GameObject ickyModel = GameObject.Find("Icky_Model");
	ickyAnim = (NF_IckyAnimations)ickyModel.GetComponent<NF_IckyAnimations>();
	getPathOfTheCategory(categoryID);
}

void getPathOfTheCategory(int currentCategoryID){
	
		if(AGGameState.currentGameIndex == (int) AGGameIndex.k_NamingFruits){
				if(currentCategoryID==(int)NamingFruits_CategoryID.id_Food || currentCategoryID==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
						pathForObjectResource = NF_Constants._fruitsPath;
					}
				}
		else if(AGGameState.currentGameIndex == (int) AGGameIndex.k_2DShapesColors){
			
			 if(currentCategoryID==(int)SimpleColors_CategoryID.id_2DShapes){
							pathForObjectResource = NF_Constants._2DShapesPath;
						}
			else if(currentCategoryID==(int)SimpleColors_CategoryID.id_Colors){
							pathForObjectResource = NF_Constants._ColorsPath;
						}
			}
		
}
void playInstructionalVO(){
		NF_QuestionLoader ql = GameObject.Find("Question").GetComponent<NF_QuestionLoader>();
		ql.playQuestionVO();
}
	
void DestroyParticlesEmitted(Object particles){
		
	Destroy((GameObject)particles);
}
	
void createParticles(Object food){
	
		GameObject ickyModel = GameObject.Find("Icky_Model");
		GameObject particle_1 = Instantiate(particleObj) as GameObject;
		GameObject particle_2 = Instantiate(particleObj) as GameObject;
		
		particle_1.renderer.material.mainTexture = Resources.Load(pathForObjectResource+resourceForParticles) as Texture;
		particle_2.renderer.material.mainTexture = Resources.Load(pathForObjectResource+resourceForParticles) as Texture;
	
		particle_1.transform.localPosition = new Vector3(ickyModel.transform.position.x,ickyModel.transform.position.y+70,ickyModel.transform.position.z);
		particle_1.transform.localScale = new Vector3(20,20,1);
		particle_2.transform.localPosition = new Vector3(ickyModel.transform.position.x,ickyModel.transform.position.y+70,ickyModel.transform.position.z);
		particle_2.transform.localScale = new Vector3(20,20,1);
		
		Destroy((GameObject) food);
		
		iTween.MoveTo(particle_1, iTween.Hash("position", new Vector3(ickyModel.transform.position.x-70,ickyModel.transform.position.y-30,ickyModel.transform.position.z-10),
			"time", 0.5f, "onComplete", "DestroyParticlesEmitted", "oncompletetarget", gameObject, "oncompleteparams", particle_1, "easetype", iTween.EaseType.linear));
		iTween.MoveTo(particle_2, iTween.Hash("position", new Vector3(ickyModel.transform.position.x+70,ickyModel.transform.position.y-30,ickyModel.transform.position.z-10),
		"time", 0.5f, "onComplete", "DestroyParticlesEmitted", "oncompletetarget", gameObject, "oncompleteparams", particle_2, "easetype", iTween.EaseType.linear));
		iTween.FadeTo(particle_1,0.0f,0.55f);
		iTween.FadeTo(particle_2,0.0f,0.55f);	
	}
	
void onBouncingFruit(Object food){
		
	GameObject foodObj = (GameObject) food;
	iTween.MoveTo(foodObj,iTween.Hash("position",new Vector3(foodObj.transform.position.x,foodObj.transform.position.y-90,foodObj.transform.position.z),
	"time", 9.0f/50.0f, "onComplete", "createParticles", "oncompletetarget", gameObject, "oncompleteparams", foodObj, "easetype", iTween.EaseType.easeInSine));
	}
	
void bounceFruitOnIcky(Object food){
		
		GameObject foodObj = (GameObject) food;
		// delete attached rope
		Destroy(foodObj.transform.FindChild("fruit_RopeCutDown").gameObject);
		
		iTween.MoveTo(foodObj,iTween.Hash("position",new Vector3(foodObj.transform.position.x,foodObj.transform.position.y+70,foodObj.transform.position.z),
		"time", 9.0f/50.0f,"onComplete", "onBouncingFruit", "oncompletetarget", gameObject, "oncompleteparams", foodObj, "easetype", iTween.EaseType.easeOutSine));
	}

void ickyCorrentAnswer(GameObject destroyedFood){
	
	isTouchesEnable_2=false;
	// create rope cut animation here 
	GameObject ropeCutDown = Instantiate(ropeCutDownPrefab) as GameObject;
	ropeCutDown.name = "fruit_RopeCutDown";
	ropeCutDown.transform.parent = destroyedFood.transform;	
		
	destroyedFood.transform.position = new Vector3(destroyedFood.transform.position.x,destroyedFood.transform.position.y,300);
	ropeCutDown.transform.position = new Vector3(destroyedFood.transform.position.x,destroyedFood.transform.position.y+70,destroyedFood.transform.position.z-5);
	
	randomJumpIndex = Random.Range(0,3);
	if(randomJumpIndex==0){
			iTween.MoveTo(destroyedFood, iTween.Hash("position", 
			new Vector3(destroyedFood.transform.position.x,-240,300.0f), "time",18.0f/50.0f + 9.0f/30.0f, "onComplete",
				"createParticles", "oncompletetarget", gameObject, "oncompleteparams", destroyedFood, "easetype", iTween.EaseType.linear));
		ickyAnim.playEatingAnimation(destroyedFood.transform.position.x,NF_IckyAnimationStates.icky_jumping);
		}
	else if(randomJumpIndex==1){
			iTween.MoveTo(destroyedFood, iTween.Hash("position", new Vector3(destroyedFood.transform.position.x,-240+177.5f,300.0f),
				"time",21.0f/50.0f, "onComplete",
				"createParticles", "oncompletetarget", gameObject, "oncompleteparams", destroyedFood, "easetype", iTween.EaseType.linear));
			ickyAnim.playEatingAnimation(destroyedFood.transform.position.x,NF_IckyAnimationStates.icky_angleJumpNEat);
		}
	else if(randomJumpIndex==2){
			iTween.MoveTo(destroyedFood, iTween.Hash("position", new Vector3(destroyedFood.transform.position.x,-180,300.0f),
				"time",27.0f/50.0f, "onComplete",
				"bounceFruitOnIcky", "oncompletetarget", gameObject, "oncompleteparams", destroyedFood, "easetype", iTween.EaseType.linear));
				ickyAnim.ickyStates = NF_IckyAnimationStates.icky_bounceNEat;
				ickyAnim.setIckyPosition_HopAnimation(destroyedFood.transform.position.x,NF_IckyAnimationStates.icky_bounceNEat);
		}
	// fade out bubble
	NF_QuestionLoader questionLoader = GameObject.Find("Question").GetComponent<NF_QuestionLoader>();
	questionLoader.fadeOutAllObjects();
}
	
void ickyWrongAnswer(){
	ickyAnim.ickyStates =NF_IckyAnimationStates.icky_wrongAns;
	ickyAnim.playSoundClip();
}
	
void ickyGiggleAnimation () {
	ickyAnim.ickyStates =NF_IckyAnimationStates.icky_giggle;
	ickyAnim.playSoundClip();
}

public IEnumerator callNextGame(float delay){
		yield return new WaitForSeconds(delay);
		AGGameState.loadNextGameInLoop(AGGameIndex.k_NamingFruits); // to play in loop
	}

public void setCelebrationStatus(bool status){
		ickyAnim.shouldPlayCelebration=status;
	}
public void switchToNextQuestion(){	
		
		questionPlayedCount++;
		Debug.Log("Total Q.Played Count:"+questionPlayedCount);
		if(questionPlayedCount>=5){
			setCelebrationStatus(true);
			StartCoroutine(callNextGame(2.00f));
			}
		else{
			isTouchesEnable_2=true;
			gameController.loadNextQuestion(0);
			}
}
	
public int qAttemptsCount(){
	return qAttempts;	
	}
	
public void resetInputController(){
	qAttempts=0;
	wrongAttempts=0;
	breakJoints = null;
	ickyAnim.ickyStates = NF_IckyAnimationStates.icky_idle;
	ickyAnim.setIckyPosition();
	}
	
string returnNameOfParent(GameObject obj){	
	string parentName= obj.transform.parent.name;
	return parentName;
}

IEnumerator destroyCreatedObject(GameObject obj, float delay){
	yield return new WaitForSeconds(delay);
	DestroyObject(obj);
}
	
void createTickSign(){
	Vector3 newPos;
	questionLoaderObj = GameObject.Find("Question");
	if(AGGameState.currentGameIndex ==(int)AGGameIndex.k_NamingFruits && categoryIDForSkill==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
		 newPos = new Vector3(questionLoaderObj.transform.position.x,questionLoaderObj.transform.position.y,-10);	
			}
	else{
	 	newPos = new Vector3(questionLoaderObj.transform.position.x,questionLoaderObj.transform.position.y+20,-10);
		}
	qTickObj= Instantiate(_tickSign,newPos,Quaternion.identity) as GameObject;
	qTickObj.transform.parent = this.gameObject.transform;
}

void createCrossSign(){
	Vector3 newPos;
	questionLoaderObj = GameObject.Find("Question");
	if(AGGameState.currentGameIndex == (int)AGGameIndex.k_NamingFruits && categoryIDForSkill==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
		 newPos = new Vector3(questionLoaderObj.transform.position.x,questionLoaderObj.transform.position.y,-10);	
			}
	else{
	 	newPos = new Vector3(questionLoaderObj.transform.position.x,questionLoaderObj.transform.position.y+20,-10);
		}
	qCrossObj= Instantiate(_crossSign,newPos,Quaternion.identity) as GameObject;
	qCrossObj.transform.parent = this.gameObject.transform;
}
	
#region FingerGestures events		
void FingureGestures_OnTap (Vector2 fingurePos, int tapCount){
		
	if(isTouchEnabled==false && isTouchesEnable_2==false){
			return;
		}
	RaycastHit hit;
	Ray ray;
	ray = Camera.main.ScreenPointToRay(fingurePos);
   	if(Physics.Raycast(ray, out hit,10000)){
		CancelInvoke("playInstructionalVO");
			
	if(hit.collider.tag=="tag_Fruits"){
				qAttempts++;
				Debug.Log("QuestionAttempts:" +qAttempts);
				if(hit.collider.name == questionTitle){
					
							NF_SoundManager.playSFX(ropeCutSFx);
							isWrongAnswer=false;
							selectedFruit = hit.transform.gameObject;
							createTickSign();
							StartCoroutine(destroyCreatedObject(qTickObj,0.2f));
							Transform rope = selectedFruit.transform.parent;
							breakJoints = rope.GetComponent<NF_BreakRopeJoints>();
							GameObject destroyedFood = Instantiate(EmptyFruit) as GameObject;
							destroyedFood.transform.localScale = breakJoints._hangingObject.transform.localScale;
							destroyedFood.renderer.material.mainTexture = breakJoints._hangingObject.renderer.material.mainTexture;
							destroyedFood.transform.position = breakJoints._hangingObject.transform.position;
							breakJoints.breakRopeNow();
							ickyCorrentAnswer(destroyedFood);
							StartCoroutine(breakJoints.deleteRopeCutUp(1.0f));
							_totalAnswersCount--;
							if(_totalAnswersCount<=0){
								isTouchEnabled=false;
								switchToNextQuestion();
									}
								}
						else if(hit.collider.name != questionTitle){
								isWrongAnswer=true;
								wrongAttempts++;
								createCrossSign();
								ickyWrongAnswer();
								StartCoroutine(destroyCreatedObject(qCrossObj,0.2f));
									if(wrongAttempts==5){
										isTouchEnabled=false;
										gameController.loadNextQuestion(0);
										}
							}
	
						}
			else if (hit.collider.gameObject.name.Equals("Icky_Model")) {
				ickyGiggleAnimation();
			}
		}
		Invoke("playInstructionalVO",10.0f);
}
	
void FingureGestures_OnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 delta){
	if(isTouchEnabled==false && isTouchesEnable_2==false){
			return;
		}
		CancelInvoke("playInstructionalVO");
}

void checkHitOnObject(Vector2 fingerPos){
		
		RaycastHit hit;
		Ray ray;
		ray = Camera.main.ScreenPointToRay(fingerPos);
		if(Physics.Raycast(ray, out hit,10000)){
			if(hit.collider.tag=="tag_Fruits"){
					if(fruitObj != hit.collider.transform.gameObject){
							fruitObj = hit.collider.transform.gameObject;
							qAttempts++;
							}
					if(hit.collider.name == questionTitle){
								NF_SoundManager.playSFX(ropeCutSFx);
								isWrongAnswer =false;
								selectedFruit = hit.transform.gameObject;
								createTickSign();
								StartCoroutine(destroyCreatedObject(qTickObj,0.2f));
							
								Transform rope = selectedFruit.transform.parent;
								breakJoints = rope.GetComponent<NF_BreakRopeJoints>();
							GameObject destroyedFood = Instantiate(EmptyFruit) as GameObject;
							destroyedFood.transform.localScale = breakJoints._hangingObject.transform.localScale;
							destroyedFood.renderer.material.mainTexture =breakJoints._hangingObject.renderer.material.mainTexture;
							destroyedFood.transform.position = breakJoints._hangingObject.transform.position;
						
							breakJoints.breakRopeNow();
							ickyCorrentAnswer(destroyedFood);
							StartCoroutine(breakJoints.deleteRopeCutUp(1.0f));
							_totalAnswersCount--;
							Debug.Log("answerCount:"+_totalAnswersCount);
								}
						else if(hit.collider.name != questionTitle){
							isTouchEnabled=false;
							isWrongAnswer =true;
							createCrossSign();
							ickyWrongAnswer();
							StartCoroutine(destroyCreatedObject(qCrossObj,0.2f));
							}
				}
			else if (hit.collider.tag == "RopeCollider"){
				
				GameObject ropeSwiped = hit.collider.gameObject.transform.parent.gameObject;
				NF_BreakRopeJoints ropeObj = ropeSwiped.GetComponent<NF_BreakRopeJoints>();
				
				if(fruitObj != ropeObj._hangingObject){
							fruitObj = hit.collider.transform.gameObject;
							qAttempts++;
							}
				if(ropeObj._hangingObject.name == questionTitle){
								NF_SoundManager.playSFX(ropeCutSFx);
								isWrongAnswer=false;
								selectedFruit = hit.transform.gameObject;
								createTickSign();
								StartCoroutine(destroyCreatedObject(qTickObj,0.2f));
						
								Transform rope = selectedFruit.transform.parent;
								breakJoints = rope.GetComponent<NF_BreakRopeJoints>();
							GameObject destroyedFood = Instantiate(EmptyFruit) as GameObject;
							destroyedFood.transform.localScale = breakJoints._hangingObject.transform.localScale;
							destroyedFood.renderer.material.mainTexture =breakJoints._hangingObject.renderer.material.mainTexture;
							destroyedFood.transform.position = breakJoints._hangingObject.transform.position;
						
							breakJoints.breakRopeNow();
							ickyCorrentAnswer(destroyedFood);
							StartCoroutine(breakJoints.deleteRopeCutUp(1.0f));
							_totalAnswersCount--;
							Debug.Log("answerCount:"+_totalAnswersCount);
								}
					else if(hit.collider.name != questionTitle){
							isTouchEnabled=false;
							isWrongAnswer =true;
							createCrossSign();
							ickyWrongAnswer();
							StartCoroutine(destroyCreatedObject(qCrossObj,0.2f));
					}
				}
		}
}
	
void FingureGestures_OnDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta){
		
		if(!isTouchEnabled) 
			return;
		checkHitOnObject(fingerPos);
}
	
void FingureGestures_OnDragEnd( int fingerIndex, Vector2 fingerPos){
		
		if(isWrongAnswer){
			wrongAttempts++;
			Debug.Log("Wrong Attempts:" + wrongAttempts);
			isWrongAnswer=false;

			if(wrongAttempts==5){
					isTouchEnabled=false;
					gameController.loadNextQuestion(0);
					}
		}
		else{
				if(_totalAnswersCount<=0){
						isTouchEnabled=false;
						switchToNextQuestion(); // switch question
						}
			
		}
		
		Invoke("playInstructionalVO",10.0f);
}
#endregion
	
void OnDisable () {
		removeFingureGestureObjects();
	}

} // class end
