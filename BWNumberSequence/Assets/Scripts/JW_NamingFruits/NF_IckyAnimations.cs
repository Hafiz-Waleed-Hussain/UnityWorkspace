#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NF_IckyAnimationStates{
	unknown,
	icky_want,
	icky_idle,
	icky_eating,
	icky_giggle,
	icky_jumping,
	icky_wrongAns,
	icky_reseting,
	icky_celebration,
	icky_hop,
	icky_angleJumpNEat,
	icky_bounceNEat,
	icky_bouncing,
	icky_weeping,
	icky_weeping2
}; 

public enum ickySounds{
	s_yummy=0,
	s_eating,
	s_nono,
};

public class NF_IckyAnimations : MonoBehaviour {
	
	public AudioClip[] voiceOversIcky;	
	List<AudioClip> listOfClips = null;
	
	public Texture2D jump1Sheet;
	public Texture2D eating1Sheet;
	public Texture2D angleJumpNEatSheet;
	public Texture2D wrongSheet;
	public Texture2D idleSheet;
	public Texture2D wantSheet;
	public Texture2D giggleSheet;
	public Texture2D celebrationSheet;
	public Texture2D bouncingSheet;
	public Texture2D weepingSheet;
	public Texture2D ickyImage;
	public bool shouldPlayCelebration = false;
	public bool isAnimating;
	bool shouldFlipHop;
	
	ArrayList giggleFrames;
	ArrayList wrongFrames;
	ArrayList eating1Frames;
	ArrayList wantFrames;
	ArrayList angleJumpFrames;
	ArrayList jump1Frames;
	ArrayList idleFrames;
	ArrayList celebrationFrames;
	ArrayList weepingFrames;
	
	public NF_IckyAnimationStates ickyStates;
	
	int colCount = 3;
	int rowCount = 4;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 12;
	int fps = 50;
	bool bLoop = false;
	int currentFrame = 0;
	float idleAnimTimer =0.5f;
	float[] xPos = {-535.0f,-265.0f,0.0f};
	
	void Start(){
		fps =30;
		currentFrame =0;
		isAnimating=true;
		//createAnimations();
	}
	
	public void setInitialStateOfIcky(){
		fps =30;
		currentFrame =0;
		isAnimating=true;
		ickyStates = NF_IckyAnimationStates.icky_idle;
		createAnimations();
		
	}
	
	 void intro_HopAnimation_1(float xPos){
		
		ickyStates =NF_IckyAnimationStates.icky_hop;
		createAnimations();
		Vector3 pos = transform.position;
		pos.x = xPos;
		currentFrame = 0;
		
		iTween.MoveTo(gameObject, iTween.Hash("position", pos,"delay", 0.5f,
			"time", 18.0f/50.0f, "easetype", iTween.EaseType.linear));
		iTween.RotateTo(gameObject, iTween.Hash("Quartenion",new Vector3(0,0,0),"delay", 0.5f,"time", 25.0f/50.0f, "onComplete", "intro_HopAnimation_2", "oncompletetarget", gameObject, "oncompleteparams", -250.0f));
		
	}
	
	 void intro_HopAnimation_2(float xPos){
		NF_SoundManager.playSFX(voiceOversIcky[14]);
		ickyStates =NF_IckyAnimationStates.icky_hop;
		createAnimations();
		Vector3 pos = transform.position;
		pos.x = xPos;
		currentFrame = 0;
		
		iTween.MoveTo(gameObject, iTween.Hash("position", pos,
			"time", 18.0f/50.0f, "easetype", iTween.EaseType.linear));
		iTween.RotateTo(gameObject, iTween.Hash("Quartenion",new Vector3(0,0,0),"time", 25.0f/50.0f, "onComplete", "intro_HopAnimation_3", "oncompletetarget", gameObject, "oncompleteparams", 0.0f));
		
	}
	
	void intro_HopAnimation_3(float xPos){
		NF_SoundManager.playSFX(voiceOversIcky[14]);
		ickyStates =NF_IckyAnimationStates.icky_hop;
		createAnimations();
		Vector3 pos = transform.position;
		pos.x = xPos;
		currentFrame = 0;
		iTween.MoveTo(gameObject, iTween.Hash("position", pos,
			"time",18.0f/50.0f, "onComplete", "switchToIdle", "oncompletetarget", gameObject, "easetype", iTween.EaseType.linear));
		
	}
	
	void switchToIdle(){
		NF_SoundManager.playSFX(voiceOversIcky[14]);
		ickyStates =NF_IckyAnimationStates.icky_idle;
	}
	
	public void IckyIntroAnimation(){

		intro_HopAnimation_1(-500.0f);
	}
	
	public void setIckyPosition(){
		
		Vector3 getCameraPosition = Camera.main.transform.position;
		this.transform.position = new Vector3(getCameraPosition.x,getCameraPosition.y-280,this.transform.position.z);
	}
	
	public void setIckyPosition_HopAnimation(float xPos, NF_IckyAnimationStates state){
		
		
		if (xPos < transform.position.x) {
			shouldFlipHop = true;
		} else {
			shouldFlipHop = false;
		}
		ickyStates =state;
		
		Vector3 pos = transform.position;
		pos.x = xPos;
		currentFrame = 0;
		iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 18.0f/50.0f, "easetype", iTween.EaseType.linear));
		//NF_SoundManager.playSFX(voiceOversIcky[14]);
	}
	
	public void playSoundClip(){
		
 	 if((ickyStates == NF_IckyAnimationStates.icky_eating) || (ickyStates == NF_IckyAnimationStates.icky_angleJumpNEat)){
			
			listOfClips = new List<AudioClip>();
			listOfClips.Add(voiceOversIcky[(int)ickySounds.s_yummy]);
			NF_SoundManager.playAudioList(listOfClips);
			NF_SoundManager.playSFX(voiceOversIcky[(int)ickySounds.s_eating]);
		}
	else if(ickyStates == NF_IckyAnimationStates.icky_wrongAns){
			int randomIndex = Random.Range(8,13);
			if(randomIndex==12){
					NF_QuestionLoader questionLoader= GameObject.Find("Question").GetComponent<NF_QuestionLoader>();
					listOfClips = new List<AudioClip>();
					listOfClips.Add(voiceOversIcky[randomIndex]);
					listOfClips.Add(questionLoader.hangingObject_VO(questionLoader.selectedTitle));
					NF_SoundManager.playAudioList(listOfClips); 
					}
			else{
				listOfClips = new List<AudioClip>();
				listOfClips.Add(voiceOversIcky[randomIndex]);
				}
				NF_SoundManager.playAudioList(listOfClips); 
		}
	else if(ickyStates == NF_IckyAnimationStates.icky_celebration){
		listOfClips = new List<AudioClip>();
		listOfClips.Add(voiceOversIcky[7]);
		NF_SoundManager.playAudioList(listOfClips);
		NF_SoundManager.playSFX(voiceOversIcky[13]);
		}
	else if (ickyStates == NF_IckyAnimationStates.icky_giggle) {
			int s = UnityEngine.Random.Range(3,6);
			listOfClips = new List<AudioClip>();
			listOfClips.Add(voiceOversIcky[s]);
			NF_SoundManager.playAudioList(listOfClips); 
		}
	}
	
	public void icky_stopAllTweensAndInvoke(){
		iTween.Stop(this.gameObject);
		CancelInvoke();
	}
	void Update (){
		
		int index  = (int)(Time.time * fps);
		if(index > currentFrame) {
			currentFrame = index;
			playIcky_Animations();
		}
	}
	public void playIcky_Animations(){ 
		
		NF_InputController IC = (NF_InputController)Camera.mainCamera.GetComponentInChildren<NF_InputController>();
		
		if(ickyStates == NF_IckyAnimationStates.unknown || ickyStates == NF_IckyAnimationStates.icky_reseting) {
			
			IC.enableTouches();
			renderer.material.mainTexture = ickyImage;
			transform.localScale = new Vector3 (140, 165, 1);
			
			colCount = 1;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 1;
			fps = 30;
			setCurrentFrame(0);
			
		} else if (ickyStates == NF_IckyAnimationStates.icky_weeping) {
			
			IC.disableTouches();
			renderer.material.mainTexture = weepingSheet;
			transform.localScale = new Vector3(166,126,1);
			colCount = 2;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 2;
			fps = 30;
			
			if(weepingFrames == null || weepingFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)weepingFrames[0]);
			weepingFrames.RemoveAt(0);
			if(weepingFrames.Count == 0) {	
				ickyStates = NF_IckyAnimationStates.unknown;
					createAnimations();
					callLoadNextQuestion();
			}
		}
		else if (ickyStates == NF_IckyAnimationStates.icky_weeping2) {
			
			IC.disableTouches();
			renderer.material.mainTexture = weepingSheet;
			transform.localScale = new Vector3(166,126,1);
			colCount = 2;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 2;
			fps = 30;
			
			if(weepingFrames == null || weepingFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)weepingFrames[0]);
			weepingFrames.RemoveAt(0);
			if(weepingFrames.Count == 0) {	
					currentFrame=0;
					fps = 30;
					ickyStates = NF_IckyAnimationStates.unknown;
					createAnimations();
					callLoadNextQuestion();
			}
		}
		
		else if (ickyStates == NF_IckyAnimationStates.icky_giggle) {
			
			IC.disableTouches();
			renderer.material.mainTexture = giggleSheet;
			transform.localScale = new Vector3(135,148,1);
			colCount = 8;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 23;
			fps = 30;
			
			if(giggleFrames == null || giggleFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)giggleFrames[0]);
			giggleFrames.RemoveAt(0);
			if(giggleFrames.Count == 0) {
				currentFrame=0;
					fps = 30;
				ickyStates = NF_IckyAnimationStates.icky_idle;
				createAnimations();
			}
		}else if (ickyStates == NF_IckyAnimationStates.icky_celebration) {
			IC.disableTouches();
			renderer.material.mainTexture = celebrationSheet;
			transform.localScale = new Vector3(200,175,1);
			colCount = 8;
			rowCount = 7;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 53;
			fps = 30;
			
			if(celebrationFrames == null || celebrationFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)celebrationFrames[0]);
			celebrationFrames.RemoveAt(0);
			if(celebrationFrames.Count == 0) {
				fps = 30;
				currentFrame = 0;
				ickyStates = NF_IckyAnimationStates.icky_idle;
				createAnimations();
			}
		}  else if (ickyStates == NF_IckyAnimationStates.icky_want){
			
			IC.enableTouches();
			renderer.material.mainTexture = wantSheet;
			transform.localScale = new Vector3(145,160,1);
			colCount = 5;
			rowCount = 5;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 25;
			fps = 30;
			
			if(wantFrames == null || wantFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)wantFrames[0]);
			wantFrames.RemoveAt(0);
			if(wantFrames.Count == 0) {
				currentFrame=0;
					fps = 30;
				ickyStates = NF_IckyAnimationStates.icky_idle;
				createAnimations();
			}
		} else if (ickyStates == NF_IckyAnimationStates.icky_idle){
			
			IC.enableTouches();
			renderer.material.mainTexture = idleSheet;
			transform.localScale = new Vector3(140,165,1);
			colCount = 10;
			rowCount = 6;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 59;
			fps = 30;
			
			if(idleFrames == null || idleFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)idleFrames[0]);
			idleFrames.RemoveAt(0);
			if(idleFrames.Count == 0) {
				currentFrame=0;
					fps = 30;
				ickyStates = NF_IckyAnimationStates.icky_want;
				createAnimations();
			}
		}else if (ickyStates == NF_IckyAnimationStates.icky_eating) {
			
			IC.disableTouches();
			renderer.material.mainTexture = eating1Sheet;
			transform.localScale = new Vector3(150, 158,1);
			colCount = 8;
			rowCount = 7;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 53;
			fps = 50;
			if(eating1Frames == null || eating1Frames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)eating1Frames[0]);
			eating1Frames.RemoveAt(0);
			if(eating1Frames.Count == 0) {
				if(IC._totalAnswersCount >0){
						setIckyPosition_HopAnimation(0.0f,NF_IckyAnimationStates.icky_hop);
					}
				else{
					if(shouldPlayCelebration){
							shouldPlayCelebration=false;
						ickyStates = NF_IckyAnimationStates.icky_celebration;
						playSoundClip();
						}
					else
						ickyStates = NF_IckyAnimationStates.icky_idle;
					
						fps = 30;
					currentFrame = 0;
					createAnimations();
				}
			}
		} else if (ickyStates == NF_IckyAnimationStates.icky_wrongAns) {
			
			renderer.material.mainTexture = wrongSheet;
			transform.localScale = new Vector3(115, 145,1);
			colCount = 8;
			rowCount = 2;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 24;
			fps = 30;
			
			if(wrongFrames == null || wrongFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)wrongFrames[0]);
			wrongFrames.RemoveAt(0);
			if(wrongFrames.Count == 0) {
				IC.disableTouches();
				fps = 30;
					currentFrame = 0;
				ickyStates = NF_IckyAnimationStates.icky_idle;
				createAnimations();
			}
		} else if (ickyStates == NF_IckyAnimationStates.icky_jumping) {
			
			IC.disableTouches();
			renderer.material.mainTexture = jump1Sheet;
			if(shouldFlipHop) {
				transform.localScale = new Vector3(-180,250,1);
			} else {
				transform.localScale = new Vector3(180,250,1);
			}
			colCount = 6;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 18;
			fps = 50;
			
			if(jump1Frames == null || jump1Frames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)jump1Frames[0]);
			jump1Frames.RemoveAt(0);
			if(jump1Frames.Count == 0) {
				fps = 30;
				currentFrame = 0;
				ickyStates = NF_IckyAnimationStates.icky_eating;
				playSoundClip();
				createAnimations();
			}
		}
		else if (ickyStates == NF_IckyAnimationStates.icky_bouncing) {
			
			IC.disableTouches();
			renderer.material.mainTexture = bouncingSheet;
			if(shouldFlipHop) {
				transform.localScale = new Vector3(-125,165,1);
			} else {
				transform.localScale = new Vector3(125,165,1);
			}
			colCount = 6;
			rowCount = 4;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 22;
			fps = 50;
			
			if(jump1Frames == null || jump1Frames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)jump1Frames[0]);
			jump1Frames.RemoveAt(0);
			if(jump1Frames.Count == 0) {
				fps = 30;
				currentFrame = 0;
				ickyStates = NF_IckyAnimationStates.icky_eating;
				playSoundClip();
				createAnimations();
			}
		}
		
		else if (ickyStates == NF_IckyAnimationStates.icky_angleJumpNEat) {
			
			IC.disableTouches();
			renderer.material.mainTexture = angleJumpNEatSheet;
			if(shouldFlipHop) {
				transform.localScale = new Vector3(-165,355,1);
			} else {
				transform.localScale = new Vector3(165,335,1);
			}
			colCount = 9;
			rowCount = 6;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 51;
			fps = 50;
			if(angleJumpFrames == null || angleJumpFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)angleJumpFrames[0]);
			angleJumpFrames.RemoveAt(0);
			if(angleJumpFrames.Count == 0) {
				
				Vector3 newPos = this.transform.position;
				newPos.y = newPos.y - 95;
				this.transform.position = newPos;
				transform.localScale = new Vector3(140,165,1);
				NF_QuestionLoader questionLoader1 = GameObject.Find("Question").GetComponent<NF_QuestionLoader>();				
				questionLoader1.moveToIckyPosition();
				
				NF_GameController gameController = (NF_GameController) Camera.mainCamera.GetComponentInChildren<NF_GameController>();
				gameController.thoughtBubbleFadeIn();
				
				fps = 30;
				currentFrame = 0;
					if(shouldPlayCelebration){
						shouldPlayCelebration=false;
						ickyStates = NF_IckyAnimationStates.icky_celebration;
						playSoundClip();
						}
					else
						ickyStates = NF_IckyAnimationStates.icky_idle;
				
					createAnimations();
			}
		}
		
		else if (ickyStates == NF_IckyAnimationStates.icky_hop || ickyStates == NF_IckyAnimationStates.icky_bounceNEat) {
			
			IC.disableTouches();
			renderer.material.mainTexture = jump1Sheet;
			if(shouldFlipHop) {
				transform.localScale = new Vector3(-180,250,1);
			} else {
				transform.localScale = new Vector3(180,250,1);
			}
			colCount = 6;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 18;
			fps = 50;
			if(jump1Frames == null || jump1Frames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)jump1Frames[0]);
			jump1Frames.RemoveAt(0);
			if(jump1Frames.Count == 0) {
				
				if(ickyStates == NF_IckyAnimationStates.icky_hop){
						NF_QuestionLoader questionLoader1 = GameObject.Find("Question").GetComponent<NF_QuestionLoader>();				
						questionLoader1.moveToIckyPosition();
						
						NF_GameController gameController = (NF_GameController) Camera.mainCamera.GetComponentInChildren<NF_GameController>();
						gameController.thoughtBubbleFadeIn();
							fps = 30;
							currentFrame = 0;
							ickyStates = NF_IckyAnimationStates.icky_idle;
							createAnimations();
						}
				else{
						fps = 30;
						currentFrame = 0;
						ickyStates = NF_IckyAnimationStates.icky_bouncing;
						NF_SoundManager.playSFX(voiceOversIcky[14]);
						playSoundClip();
						createAnimations();
				}
			}
		}
		
	}
	
	void callLoadNextQuestion(){
					NF_GameController gameController = (NF_GameController) Camera.main.GetComponentInChildren<NF_GameController>();
					gameController.loadNextQuestion(24.0f/30.0f);
	}
	void setCurrentFrame (int index) {
		
		if(bLoop) {
			index = index % totalCells;
		} else if(index >= totalCells) {  // Repeat when exhausting all cells
			index = totalCells - 1;
		}
		
	    // Size of every cell
	    float sizeX = 1.0f / colCount;
	    float sizeY = 1.0f / rowCount;
	    Vector2 size =  new Vector2(sizeX,sizeY);
	 
	    // split into horizontal and vertical index
	    var uIndex = index % colCount;
	    var vIndex = index / colCount;
	 
	    // build offset
	    // v coordinate is the bottom of the image in opengl so we need to invert.
	    float offsetX = (uIndex+colNumber) * size.x;
	    float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
	    Vector2 offset = new Vector2(offsetX,offsetY);
	 
	    renderer.material.SetTextureOffset ("_MainTex", offset);
	    renderer.material.SetTextureScale  ("_MainTex", size);
	}
	
	void createAnimations () {
		
		int [] wrongOrder = {0,1,2,2,2,3,4,5,6,6,6,5,4,3,2,7,8,8,8,7,2,3,4,5,6,6,6,5,4,3,15};
		wrongFrames = new ArrayList();
		for(int i = 0; i< wrongOrder.Length; i++) {
			wrongFrames.Add(wrongOrder[i]);
		}
		angleJumpFrames = new ArrayList(51);
		for(int i = 0; i< 51; i++) {
			angleJumpFrames.Add(i);
		}
		eating1Frames = new ArrayList(53);
		for(int i = 0; i< 53; i++) {
			eating1Frames.Add(i);
		}
		idleFrames = new ArrayList();
		for(int i=0;i<59;i++){
			idleFrames.Add(i);
		}
		
		int[] weepOrder = {0,0,0,1,1,1};
		weepingFrames = new ArrayList();
		
		for(int k=0;k<4;k++){
		
			for(int i =0; i<weepOrder.Length; i++) {
				weepingFrames.Add(weepOrder[i]);
			}
		}
		
		celebrationFrames = new ArrayList();
		for(int i =0; i<53; i++) {
			celebrationFrames.Add(i);
		}
		
		giggleFrames = new ArrayList();
		for(int i =0; i<23; i++) {
			giggleFrames.Add(i);
		}
		wantFrames = new ArrayList();
		for(int i = 0; i< 20; i++) {
			wantFrames.Add(i);
		}
		for(int i = 7; i< 20; i++) {
			wantFrames.Add(i);
		}
		for(int i = 7; i< 17; i++) {
			wantFrames.Add(i);
		}
		int [] idle1Order = {20,21,22,23,24};
		for(int i = 0; i<idle1Order.Length; i++) {
			wantFrames.Add(idle1Order[i]);
		}
		jump1Frames = new ArrayList();
		for(int i = 0; i<18; i++) {
			jump1Frames.Add(i);
		}
	}
	
	public void playEatingAnimation (float xPos,NF_IckyAnimationStates state) {

		ickyStates = state;
		if (xPos < transform.position.x) {
			shouldFlipHop = true;
		} else {
			shouldFlipHop = false;
		}
		
		if(ickyStates == NF_IckyAnimationStates.icky_jumping){
				Vector3 pos = transform.position;
				pos.x = xPos;
				currentFrame = 0;
				iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 18.0f/50.0f, "easetype", iTween.EaseType.linear));
				}
		else if(ickyStates == NF_IckyAnimationStates.icky_angleJumpNEat){
				playSoundClip();
				Vector3 pos = transform.position;
				pos.y = pos.y + 95;
				this.transform.position = pos;
			
				pos.x = xPos;
				currentFrame = 0;
				iTween.MoveTo(gameObject, iTween.Hash("position", pos,"delay",15.0f/50.0f, "time", 6.0f/50.0f, "easetype", iTween.EaseType.linear));
				}
	}
	
	void OnExit(){
		isAnimating=false;
	}
}
