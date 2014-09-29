using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NF_QuestionLoader : MonoBehaviour {

public AudioClip[] instructionalVO;
// prefabs
public GameObject _textMesh;
public GameObject _questionShape;
public GameObject _thoughtBubble;
// private 
GameObject thoughtBubble;
private GameObject qShapeObj;
private GameObject qTextObj;
public string selectedTitle;
private int currentCategoryID;	
public int currentGameMode;
private int answerCount;
private bool showReferent;	
AudioClip newQuestionClip;
List<AudioClip> listOfClips = null;
string questionTitleFor2DShapes;
AudioClip _balloonVO;
AudioClip _balloonsVO;
	
enum Instructional_VO{
		
	k_different_1 =0,
	k_different_2 =1,
	k_same_one_1=2,
	k_same_one_2=3,
	k_same_one_3=4,
	k_same_many_1=5,
	k_same_many_2=6,
	k_letters_todo_1=7,
	k_letters_todo_2=8,
	k_fruitLetters_todo_1=9,
	k_bubblePop=10
		
	};
	
void Awake(){
	// fingure gesture	
	FingerGestures.OnTap += FingureGestures_OnTap;
	 _balloonVO = Resources.Load(NF_Constants._balloonVOPath+"word_balloon") as AudioClip;
	 _balloonsVO = Resources.Load(NF_Constants._balloonVOPath+"word_balloons") as AudioClip;
	}
	
public void questionInitializer(int index, string selectedQTitle, int catID, int _gameMode, int aCount, bool referent, string originalTitle){
	
	questionTitleFor2DShapes = originalTitle;
	currentCategoryID=catID;
	currentGameMode = _gameMode;
	answerCount = aCount;
	showReferent=referent;
	selectedTitle=selectedQTitle;
	createObjectsForQuestion();
	changeMaterialPresentedQuestion();
	bubblePlayAnimation();
	moveToIckyPosition();
	startingSFX();
}

public void playQuestionVO(){
		
	AudioClip _instructionalVO = getInstructionalVO();
	AudioClip _nameOfClip = hangingObject_VO(selectedTitle);
		
	if(currentGameMode==(int) GameModes.mode_different){
		listOfClips = new List<AudioClip>();
		listOfClips.Add(_instructionalVO);
		NF_SoundManager.playAudioList(listOfClips);
		}
	else{
		listOfClips = new List<AudioClip>();
		listOfClips.Add(_instructionalVO);
		listOfClips.Add(_nameOfClip);
			if(AGGameState.currentGameIndex==(int) AGGameIndex.k_2DShapesColors && currentCategoryID ==(int) SimpleColors_CategoryID.id_Colors){
				if(answerCount>1)
						listOfClips.Add(_balloonsVO);
				else
					listOfClips.Add(_balloonVO);
			}
		NF_SoundManager.playAudioList(listOfClips);
		}
	}
	
public void startingSFX(){	
	NF_SoundManager.playSFX(instructionalVO[(int)Instructional_VO.k_bubblePop]);
	playQuestionVO();
}
	
AudioClip getInstructionalVO(){

	AudioClip selInstructionalVO = new AudioClip();
		
	if(AGGameState.currentGameIndex==(int) AGGameIndex.k_NamingFruits && currentCategoryID==(int) NamingFruits_CategoryID.id_Food){
		if(currentGameMode==(int)GameModes.mode_different){
			selInstructionalVO = instructionalVO[Random.Range((int)Instructional_VO.k_different_1,(int)Instructional_VO.k_different_2+1)];  // all diff_todo_4
			}
		else if(currentGameMode==(int)GameModes.mode_same || currentGameMode==(int)GameModes.mode_similiar){
				if(answerCount==1)
					selInstructionalVO = instructionalVO[Random.Range((int)Instructional_VO.k_same_one_1,(int)Instructional_VO.k_same_one_2+1)]; // all same todo one_4
				else
					selInstructionalVO = instructionalVO[(int) Instructional_VO.k_same_many_1]; // all same todo many_1
			}
		}
	else if(AGGameState.currentGameIndex==(int) AGGameIndex.k_NamingFruits && currentCategoryID==(int) NamingFruits_CategoryID.id_AlplabeticOrder){
		 if(currentGameMode==(int)GameModes.mode_same || currentGameMode==(int)GameModes.mode_similiar){
				int randomIndex = Random.Range((int) Instructional_VO.k_letters_todo_1,(int) Instructional_VO.k_fruitLetters_todo_1+1);
				selInstructionalVO =instructionalVO[randomIndex]; // all letters VO for alphabetic game
			}
		}
	else if(AGGameState.currentGameIndex==(int) AGGameIndex.k_2DShapesColors && currentCategoryID==(int) SimpleColors_CategoryID.id_Colors){
		if(currentGameMode==(int)GameModes.mode_different){
			selInstructionalVO = instructionalVO[Random.Range((int)Instructional_VO.k_different_1,(int)Instructional_VO.k_different_2+1)];
			}
		else if(currentGameMode==(int)GameModes.mode_same || currentGameMode==(int)GameModes.mode_similiar){
				if(answerCount==1)
					selInstructionalVO = instructionalVO[Random.Range((int)Instructional_VO.k_same_one_1,(int)Instructional_VO.k_same_one_2+1)];
				else
					selInstructionalVO = instructionalVO[(int) Instructional_VO.k_same_many_2];
			}
		}
	else if(AGGameState.currentGameIndex==(int) AGGameIndex.k_2DShapesColors && currentCategoryID==(int) SimpleColors_CategoryID.id_2DShapes){
		if(currentGameMode==(int)GameModes.mode_different){
			selInstructionalVO = instructionalVO[Random.Range((int)Instructional_VO.k_different_1,(int)Instructional_VO.k_different_2+1)];
			}
		else if(currentGameMode==(int)GameModes.mode_same || currentGameMode==(int)GameModes.mode_similiar){
				if(answerCount==1)
					selInstructionalVO = instructionalVO[Random.Range((int)Instructional_VO.k_same_one_1,(int)Instructional_VO.k_same_one_2+1)];
				else
					selInstructionalVO = instructionalVO[(int) Instructional_VO.k_same_many_1];
			}
		}
		
		
	return selInstructionalVO;
}
	
public AudioClip hangingObject_VO(string qtitle){
		
		string voiceOver;
		switch(qtitle){
			// for food game
			case "Apple":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_apples";
				else
				voiceOver = NF_Constants._fruitVOPath+"word_apple";}break;
			case "Orange":{
			if(AGGameState.currentGameIndex==(int) AGGameIndex.k_NamingFruits){
					if(answerCount>1)
						voiceOver = NF_Constants._fruitVOPath+"word_oranges";
					else
					voiceOver= NF_Constants._fruitVOPath+"word_orange";
						}
				else{
					voiceOver = NF_Constants._colorsVO+"word_orange";
						}
					}break;
			case "Banana":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_bananas";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_banana";}break;
			case "Coconut":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_coconuts";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_coconut";}break;
			case "Blueberry":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_blueberries";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_blueberry";}break;
			case "Grapes":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_grapes";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_grape";}break;
			case "Lemon":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_lemons";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_lemon";}break;
			case "Pear":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_pears";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_pear";}break;
			case "Strawberry":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_strawberries";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_strawberry";}break;
			case "Watermelon":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_watermelons";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_watermelon";}break;
			case "Cherry":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_cherries";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_cherry";}break;
			
			case "Plum":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_plums";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_plum";}break;
			
			case "Lime":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_limes";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_lime";}break;
			
			case "Blackberry":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_blackberries";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_blackberry";}break;
			
			case "Canteloupe":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_canteloupes";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_canteloupe";}break;
			
			case "Dragonfruit":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_dragonfruits";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_dragonfruit";}break;
			
			case "Kiwi":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_kiwis";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_kiwi";}break;
				
				
			case "Lychee":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_lychees";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_lychee";}break;
				
			case "Mango":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_mangos";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_mango";}break;
				
			case "Pineapple":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_pineapples";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_pineapple";}break;
				
			case "Raddish":{
				if(answerCount>1)
				voiceOver = NF_Constants._fruitVOPath+"word_raddishs";
				else
				voiceOver= NF_Constants._fruitVOPath+"word_raddish";}break;
			// for alphabetic order
			case "B":
				voiceOver= NF_Constants._lettersVOPath+"letter_b";break;
			case "L":
				voiceOver= NF_Constants._lettersVOPath+"letter_l";break;
			case "O":
				voiceOver= NF_Constants._lettersVOPath+"letter_o";break;
			case "P":
				voiceOver= NF_Constants._lettersVOPath+"letter_p";break;
			case "G":
				voiceOver= NF_Constants._lettersVOPath+"letter_g";break;
			case "W":
				voiceOver= NF_Constants._lettersVOPath+"letter_w";break;
			case "S":
				voiceOver= NF_Constants._lettersVOPath+"letter_s";break;
			case "C":
				voiceOver= NF_Constants._lettersVOPath+"letter_c";break;
			
			// for all 2D shapes
			
			case "Circle":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_circles";
				else
				voiceOver= NF_Constants._shapesVO+"word_circle";}break;
			case "Diamond":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_diamonds";
				else
				voiceOver= NF_Constants._shapesVO+"word_diamond";}break;
			case "Heart":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_hearts";
				else
				voiceOver= NF_Constants._shapesVO+"word_heart";}break;
			case "Hexagon":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_hexagon";
				else
				voiceOver= NF_Constants._shapesVO+"word_hexagons";}break;
			case "Octagon":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_octagons";
				else
				voiceOver= NF_Constants._shapesVO+"word_octagon";}break;
			case "Oval":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_ovals";
				else
				voiceOver= NF_Constants._shapesVO+"word_oval";}break;
			case "Pentagon":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_pentagons";
				else
				voiceOver= NF_Constants._shapesVO+"word_pentagon";}break;
			case "Rectangle":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_rectangles";
				else
				voiceOver= NF_Constants._shapesVO+"word_rectangle";}break;
			case "Square":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_squares";
				else
				voiceOver= NF_Constants._shapesVO+"word_square";}break;
			case "Star":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_stars";
				else
				voiceOver= NF_Constants._shapesVO+"word_star";}break;
			case "Triangle":{
				if(answerCount>1)
				voiceOver = NF_Constants._shapesVO+"word_triangles";
				else
				voiceOver= NF_Constants._shapesVO+"word_triangle";}break;
			// for all colors
			case "Black":
				voiceOver = NF_Constants._colorsVO+"word_black";break;
			case "Blue":
				voiceOver = NF_Constants._colorsVO+"word_blue";break;
			case "White":
				voiceOver = NF_Constants._colorsVO+"word_white";break;
			case "Brown":
				voiceOver = NF_Constants._colorsVO+"word_brown";break;
			case "Yellow":
				voiceOver = NF_Constants._colorsVO+"word_yellow";break;
			case "Red":
				voiceOver = NF_Constants._colorsVO+"word_red";break;
			case "Pink":
				voiceOver = NF_Constants._colorsVO+"word_pink";break;
			case "Purple":
				voiceOver = NF_Constants._colorsVO+"word_purple";break;
			case "Green":
				voiceOver = NF_Constants._colorsVO+"word_green";break;
			case "Gray":
				voiceOver = NF_Constants._colorsVO+"word_gray";break;
			default:
				voiceOver= "";break;
			}	
		return  (AudioClip)(Resources.Load(voiceOver));
	}
	
public IEnumerator playInitialVoiceOvers(AudioClip _instructionalVO,AudioClip _nameOfClip, float delay){
		
		
	yield return new WaitForSeconds(delay);
	if(currentGameMode==(int) GameModes.mode_different){
		listOfClips = new List<AudioClip>();
		listOfClips.Add(_instructionalVO);
		NF_SoundManager.playAudioList(listOfClips);
		}
	else{
		listOfClips = new List<AudioClip>();	
		listOfClips.Add(_instructionalVO);
		listOfClips.Add(_nameOfClip);
		if(AGGameState.currentGameIndex==(int) AGGameIndex.k_2DShapesColors && currentCategoryID ==(int) SimpleColors_CategoryID.id_Colors){
				if(answerCount>1)
						listOfClips.Add(_balloonsVO);
				else
					listOfClips.Add(_balloonVO);
			}
		NF_SoundManager.playAudioList(listOfClips);
		}
}

void FingureGestures_OnTap (Vector2 fingurePos, int tapCount) {
		
	RaycastHit hit;
	Ray ray;
	ray = Camera.main.ScreenPointToRay(fingurePos);
   	if(Physics.Raycast(ray, out hit,10000)){
	if(hit.collider.tag=="Question"){
				Debug.Log("Repeat VO on Tap");
				StartCoroutine(playInitialVoiceOvers(getInstructionalVO(),hangingObject_VO(selectedTitle),0.0f));
			}
		}
	}
	
public string selectedQuestionTitle(){
	return selectedTitle;
}

void createObjectsForQuestion(){
		
	NF_GameController GC = (NF_GameController) Camera.main.GetComponentInChildren<NF_GameController>();
	if(thoughtBubble !=null || GC.isResetInProgress==true){
			Destroy(thoughtBubble);
		}
	thoughtBubble = Instantiate(_thoughtBubble,_thoughtBubble.transform.position,Quaternion.identity) as GameObject;
	thoughtBubble.transform.parent = this.gameObject.transform;
	thoughtBubble.transform.localPosition =  new Vector3(0,0,0);
		
	if(AGGameState.currentGameIndex==(int) AGGameIndex.k_NamingFruits && currentCategoryID==(int) NamingFruits_CategoryID.id_AlplabeticOrder){	
		qShapeObj = Instantiate(_questionShape,_questionShape.transform.position,Quaternion.identity) as GameObject;
		qShapeObj.transform.parent = this.gameObject.transform;
		qShapeObj.transform.localPosition =  new Vector3(-5,30,-10);
		}
	else if(AGGameState.currentGameIndex==(int) AGGameIndex.k_NamingFruits && currentCategoryID==(int) NamingFruits_CategoryID.id_Food){	
		if(qTextObj !=null){
				Destroy(qTextObj);
			}
			if(currentGameMode !=(int) GameModes.mode_different){
				qTextObj = Instantiate(_textMesh,_textMesh.transform.position,Quaternion.identity) as GameObject;
				qTextObj.transform.parent = this.gameObject.transform;
				qTextObj.transform.localPosition =  new Vector3(-5,-65,-10);
			}
			qShapeObj = Instantiate(_questionShape,_questionShape.transform.position,Quaternion.identity) as GameObject;
			qShapeObj.transform.parent = this.gameObject.transform;
			qShapeObj.transform.localPosition = new Vector3(-5,30,-10);
	
		}
	else if(AGGameState.currentGameIndex==(int)AGGameIndex.k_2DShapesColors){	
		if(qTextObj !=null){
				Destroy(qTextObj);
			}
			if(currentGameMode !=(int) GameModes.mode_different){
				qTextObj = Instantiate(_textMesh,_textMesh.transform.position,Quaternion.identity) as GameObject;
				qTextObj.transform.parent = this.gameObject.transform;
				qTextObj.transform.localPosition =  new Vector3(-5,-65,-10);
			}
			qShapeObj = Instantiate(_questionShape,_questionShape.transform.position,Quaternion.identity) as GameObject;
			qShapeObj.transform.parent = this.gameObject.transform;
			qShapeObj.transform.localPosition = new Vector3(-5,30,-10);
	
		}
}

void changeMaterialPresentedQuestion(){
	
	if(AGGameState.currentGameIndex==(int)AGGameIndex.k_NamingFruits){
			
		if(currentCategoryID==(int) NamingFruits_CategoryID.id_AlplabeticOrder){	
			qShapeObj.transform.localScale = new Vector3(125,140,1);
			qShapeObj.transform.localPosition =  new Vector3(-5,-5,-10);
			qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._alphabetsPath+selectedTitle) as Texture;
			}
		
		if(currentCategoryID==(int) NamingFruits_CategoryID.id_Food){
			if(showReferent){
				qTextObj.GetComponent<TextMesh>().text= selectedTitle.ToUpper();
				qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._fruitsPath+selectedTitle) as Texture;	
				}
			else if(!showReferent){
				
					if(currentGameMode==(int) GameModes.mode_different){
						qShapeObj.transform.localScale = new Vector3(125,140,1);
						qShapeObj.transform.localPosition =  new Vector3(-5,-5,-10);
						qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._questionMark) as Texture;
					}
					else{
						qTextObj.GetComponent<TextMesh>().text= selectedTitle.ToUpper();
						qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._questionMark) as Texture;
					}
				}
			}
		}
	else if(AGGameState.currentGameIndex==(int)AGGameIndex.k_2DShapesColors){ 
		
		 if(currentCategoryID==(int) SimpleColors_CategoryID.id_2DShapes){
				if(showReferent){
					qTextObj.GetComponent<TextMesh>().text= selectedTitle.ToUpper();
					qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._2DShapesPath+questionTitleFor2DShapes) as Texture;	
					}
				else if(!showReferent){
					
						if(currentGameMode==(int) GameModes.mode_different){
							qShapeObj.transform.localScale = new Vector3(125,140,1);
							qShapeObj.transform.localPosition =  new Vector3(-5,-5,-10);
							qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._questionMark) as Texture;
						}
						else{
							qTextObj.GetComponent<TextMesh>().text= selectedTitle.ToUpper();
							qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._questionMark) as Texture;
						}
					}
			}
			else if(currentCategoryID==(int) SimpleColors_CategoryID.id_Colors){
				if(showReferent){
					qTextObj.GetComponent<TextMesh>().text= selectedTitle.ToUpper();
					qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._ColorsRefPath+selectedTitle) as Texture;	
					}
				else if(!showReferent){
					
						if(currentGameMode==(int) GameModes.mode_different){
							qShapeObj.transform.localScale = new Vector3(125,140,1);
							qShapeObj.transform.localPosition =  new Vector3(-5,-5,-10);
							qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._questionMark) as Texture;
						}
						else{
							qTextObj.GetComponent<TextMesh>().text= selectedTitle.ToUpper();
							qShapeObj.renderer.material.mainTexture = Resources.Load(NF_Constants._questionMark) as Texture;
						}
					}
			}
	}
}

void bubblePlayAnimation(){
	// play animation on object presented scale up and down.
	qShapeObj.animation.Play("heartBeat");
}

public void moveToIckyPosition(){
		
	GameObject ickyModel = GameObject.Find("Icky_Model");
	Vector3 bubblePos = new Vector3(ickyModel.transform.position.x-240,ickyModel.transform.position.y+70,ickyModel.transform.position.z);
	this.gameObject.transform.position = bubblePos;
}

IEnumerator  giveSomeDelay(float delay){	
	yield return new WaitForSeconds(delay);
			if(answerCount==1){
				Destroy(qTextObj);
				Destroy(qShapeObj);
				Destroy(thoughtBubble);
			}
}
	
public void fadeOutAllObjects(){
		
		Debug.Log("Fading out");
		iTween.FadeTo(this.gameObject,0,0.3f);
		StartCoroutine(giveSomeDelay(1));
	}
	
public void fadeInAllObjects(){
		Debug.Log("Fading in");
		if(answerCount>1){
			iTween.FadeTo(this.gameObject,255,1.0f);
			}
	}
	
public void resetQuestionLoader(){
	Destroy(thoughtBubble);
	Destroy(qShapeObj);
	Destroy(qTextObj);
	}
	
void OnDisable () {
	FingerGestures.OnTap -= FingureGestures_OnTap;
	}
	
}
