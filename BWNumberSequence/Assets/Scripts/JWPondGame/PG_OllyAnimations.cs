#region AGNITUS 2013
/* JungleWorld- Pond Game
 * Developer- Abid Ali
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;


public enum ollyAnimationStates{      
	unknown,
	olly_idle1,
	olly_still,
	olly_jumpLandToWater,
	olly_jumpWaterToWater,
	olly_jumpWaterToLand,
	olly_hop,
	olly_celebration,
	olly_tickle,
	olly_wrongAnswer,
	olly_weap
}; 

//public enum ollySounds{
//	s_yummy=0,
//	s_eating,
//	s_nono
//};

public class PG_OllyAnimations : MonoBehaviour {

//public AudioClip[] voiceOversIcky;	
//public Texture2D spriteSheet;
//public Texture2D wrongAnsSheet;

public bool isAnimating;
private PG_AniSprite aniPlay;

public 	ollyAnimationStates ollyState , ollyLastStat;
private ollyAnimationStates ollyPrevState;
public static int answerShapesCount;	
public float animationTime;
public bool	isLastShape;
GameObject camera;
	
	public GameObject shapesRefrence;
	
	int colCount = 3;
	int rowCount = 3;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 9;
	int fps = 30;
	bool bLoop = false ;
	public static bool changeTexture;
	int currentFrame = 0;
	float frame=0;
	int counter=0;
	
	
	
	ArrayList idleAnimFrames 			= 	new ArrayList();
	ArrayList jumpLandToWaterFrames 	=	new ArrayList();
	ArrayList jumpWaterToWaterFrames	=  	new ArrayList();
	ArrayList jumpWaterToLand			=  	new ArrayList();
	ArrayList hopAnimFrames				=  	new ArrayList();
	ArrayList celebrationFrames			=  	new ArrayList();
	ArrayList celebrationFrames2		=  	new ArrayList();
	ArrayList tickleFrames				=  	new ArrayList();
	ArrayList wrongAnswerFrames			=  	new ArrayList();
	ArrayList weapFrames				=  	new ArrayList();
	
	PG_GameController  gameController;
	PG_SoundManager soundManager;
	
	
	
	
	void Awake(){
		createIdleAnim();
		createLandToWaterAnim();
		createWaterToWaterAnim();
		createWaterToLandAnim();
		createHopAnim();
		createCelebrationAnim();
		createTickleAnim();
		createWrongAnswerAnim();
		createWeapAnim();
	}
	
	void Start(){
		isAnimating   = true;
		changeTexture = true;
		ollyState 	  = ollyAnimationStates.olly_idle1;
		ollyPrevState = ollyState;
		
		camera 				= 	GameObject.Find("Camera");
		gameController		= camera.GetComponent<PG_GameController>();
		
		if(shapesRefrence!=null)
		soundManager = shapesRefrence.GetComponent<PG_SoundManager>();
	}
		
	
	void Update(){
		int index  = (int)(Time.time * fps);
		
		if(ollyState!=ollyPrevState)
		{
			ollyPrevState = ollyState;
			playOlly_Animations();
		}
		else if(index > currentFrame) {
			currentFrame = index;
			playOlly_Animations();
		}
		
	}

	
	
	
	
	private void createIdleAnim(){ 
			for(int i=0 ; i<7 ;i++)
			idleAnimFrames.Add(i);
		}
	
	private void createLandToWaterAnim(){
		int[] frames = {0,1,1,2,2,3,3,3,4,5,6,7,8,9,10,11,12,13,13,14,15,16,17,18,19,20,20,21,21,22,22,23,23,23,23,24,25,26,27};
		for(int i=0 ; i<frames.Length;i++){
			jumpLandToWaterFrames.Add(frames[i]);
		}
	}
	
	private void createWaterToWaterAnim(){
		int[] frames = {0,1,1,2,2,3,3,3,4,5,6,7,8,9,10,11,12,13,13,14,15,16,17,18,19,20,20,21,21,22,22,23,23,23,23,24,25,26,26,27,28};
		for(int i=0 ; i<frames.Length;i++){
			jumpWaterToWaterFrames.Add(frames[i]);
		}
	}
	
	private void createWaterToLandAnim(){
		int[] frames = {0,1,1,2,2,3,3,4,5,6,7,8,9,10,11,12,13,13,14,15,16,17,18,19,20,21,22,22,23,23,24};
		for(int i=0 ; i<frames.Length;i++){
			jumpWaterToLand.Add(frames[i]);
		}
	}
	
	private void createHopAnim(){
		for(int i=0 ; i<10;i++){
			hopAnimFrames.Add(i);
		}
	}
	
	private void createCelebrationAnim(){
		int[] frames = {0,1,2,3,4,5,5,5,5,5,5,5,5,6,7,8,9,10,11,12,13,14,11,12,13,14,11,12,13,14,15,16,17,18,19,19,19,19,19};
		for(int i=0 ; i<frames.Length;i++){
			celebrationFrames.Add(frames[i]);
		}
		
		for(int i=0 ; i<16;i++){
			celebrationFrames.Add(20);
		}
		
		int[] frames1 = {21,22,23,24,24,24,25,26,27,28};
		for(int i=0 ; i<frames1.Length;i++){
			celebrationFrames.Add(frames1[i]);
		}
	}
	
	
	private void createCelebrationAnim2(){
		int[] frames = {0,1,2,3,4,4,3,2,1,0,};
		for(int i=0 ; i<frames.Length;i++){
			celebrationFrames2.Add(frames[i]);
		}
		
		for(int i=0 ; i<16;i++){
			celebrationFrames.Add(20);
		}
		
		int[] frames1 = {21,22,23,24,24,24,25,26,27,28};
		for(int i=0 ; i<frames1.Length;i++){
			celebrationFrames.Add(frames1[i]);
		}
	}
	
	
	private void createTickleAnim(){
		int[] frames = {0,1,2,3,3,4,5,5,3,3,5,5,3,3,5,5,4,3,3,2,1,0};
		for(int i=0 ; i<frames.Length;i++){
			tickleFrames.Add(frames[i]);
		}
	}

	private void createWrongAnswerAnim(){
		int[] frames = {0,1,1,2,3,4,4,4,4,4,4,5,5,6,6,7,7,6,6,7,7,6,6,7,7,8,8,9,9,10,10,0,0,0,0};
		for(int i=0 ; i<frames.Length;i++){
			wrongAnswerFrames.Add(frames[i]);
		}
	}
	
		private void createWeapAnim(){
		int[] frames = {0,0,0,1,1,2,2,3,3,3,3,3,4,4,4,4,4,3,3,3,3,3,4,4,4,4,4,3,3,3,3,3,4,4,4,4,4,3,3,3,3,3,4,4,4,4,4,3,3,3,3,3,4,4,4,4,4};
		for(int i=0 ; i<frames.Length;i++){
			weapFrames.Add(frames[i]);
		}
	}
	
public void playOlly_Animations(){ 
	// -----  Idle animation
			
			
			if(ollyState == ollyAnimationStates.olly_idle1){
				//Debug.Log("in idle animation...");
				if(changeTexture){
				transform.localScale=new Vector3(130F , 170F , 1F );
				Vector3 pos = transform.localPosition;
				pos.y =0.0F;
				transform.localPosition = pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyIdle) as Texture2D;	
				isAnimating=true; changeTexture = false;
				colCount = 4;  rowCount = 2; totalCells = 9;
			}
			playIdle1Anim();

			
		}
		else if(ollyState == ollyAnimationStates.olly_still){
//			Debug.Log("still anim");
			frame+=Time.deltaTime;
			if(changeTexture){
				transform.localScale=new Vector3(130F , 170F , 1F );
				Vector3 pos = transform.localPosition;
				//pos.y =0.0F;
				transform.localPosition = pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyStill) as Texture2D;
				isAnimating=true; changeTexture = false;
			}
			if(frame>=1){
				frame=0;
				StartCoroutine(setFlag(2.0F,ollyAnimationStates.olly_idle1));
			}
			
		}
		  else if(ollyState == ollyAnimationStates.olly_jumpLandToWater){
			//Debug.Log("jump land to water...");
			if(changeTexture){
				transform.localScale		  = new Vector3(180F , 325F , 1F );
				Vector3 pos = transform.localPosition;
				//pos.y +=75.5F; me
				pos.y +=75F;
				transform.localPosition = pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyJumpLandToWater) as Texture2D;
				soundManager.playSoundEffect(PG_Constants._soundclipPath +PG_Constants._introClips +PG_Constants._jump);
				isAnimating=false; changeTexture = false;
				colCount   = 8;
				rowCount   = 4;
				totalCells = 32;
			}
			playJumpLandToWaterAnim();
		 }else if(ollyState == ollyAnimationStates.olly_jumpWaterToWater){
			//Debug.Log("jump water to water...");
			if(changeTexture){
				transform.localScale = new Vector3(180F , 325F , 1F );
				Vector3 pos = transform.localPosition;
				//pos.y +=75.5F;
				transform.localPosition = pos;
				//renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyJumpWaterToWater) as Texture2D;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyJumpLandToWater) as Texture2D;
				soundManager.playSoundEffect(PG_Constants._soundclipPath +PG_Constants._introClips +PG_Constants._jump);
//				isAnimating=false; changeTexture = false;
//				
//				colCount   = 5;
//				rowCount   = 6;
//				totalCells = 30;
				
				isAnimating=false; changeTexture = false;
				colCount   = 8;
				rowCount   = 4;
				totalCells = 32;
			}
			  //playJumpWaterToWaterAnim();
			playJumpLandToWaterAnim();
		 }else if(ollyState == ollyAnimationStates.olly_jumpWaterToLand){
			//Debug.Log("jump water to land...");
				if(changeTexture){
				transform.localScale = new Vector3(180F , 325F , 1F );
				Vector3 pos = transform.localPosition;
				//pos.y +=75.5F;
				transform.localPosition = pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyWaterTOLand) as Texture2D;
				soundManager.playSoundEffect(PG_Constants._soundclipPath +PG_Constants._introClips +PG_Constants._jump);
				isAnimating=false; changeTexture = false;
				
				colCount   = 7;
				rowCount   = 4;
				totalCells = 28;
			}	
			playJumpWaterToLandAnim();
				
		}else if(ollyState == ollyAnimationStates.olly_hop){
			//Debug.Log("in hop animation...");
			if(changeTexture){
				transform.localScale = new Vector3(180F , 325F , 1F );
				Vector3 pos = transform.localPosition;
				pos.y =75;
				transform.localPosition = pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyHop) as Texture2D;
				isAnimating=false; changeTexture = false;
				counter     = 0;
				colCount 	= 5;
				rowCount	= 2;
				totalCells 	= 10;
			}	
			playHopAnimaiton();
		}else if(ollyState == ollyAnimationStates.olly_celebration){
			//Debug.Log("in celebration...");
			if(changeTexture){
				transform.localScale = new Vector3(180F , 325F , 1F );
				Vector3 pos = transform.localPosition;
				//pos.y -=75.5F;
				transform.localPosition = pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyCelebrationAnim) as Texture2D;
				isAnimating=false; changeTexture = false;
				
				colCount   = 8;
				rowCount   = 4;
				totalCells = 32;
			}
			playCelebrationAnim();
			
		}else if(ollyState == ollyAnimationStates.olly_tickle){

			//Debug.Log("in tickle animation...");
			if(changeTexture){
				transform.localScale = new Vector3(180F , 325F , 1F );
				Vector3 pos=transform.localPosition;
				//pos.y +=75.5F; me
				pos.y +=77.5F;
				transform.localPosition=pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyTickle) as Texture2D;
				isAnimating=false; changeTexture = false;
				
				colCount   = 3;
				rowCount   = 2;
				totalCells = 6;
				fps=30;
			}	
			playTickleAnim();
		} else if(ollyState == ollyAnimationStates.olly_wrongAnswer){
		
			//Debug.Log("in wrong answer animation...");
			if(changeTexture){
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyWrongAnswer) as Texture2D;
				if(ollyLastStat!=ollyAnimationStates.olly_idle1 && ollyLastStat!=ollyAnimationStates.olly_still ){
					
				}
				Vector3 pos=transform.localPosition;
					pos.y =0;
					transform.localPosition=pos;
					transform.localScale = new Vector3(130F , 170F , 1F );
				isAnimating=false; changeTexture = false;
				colCount   = 4;
				rowCount   = 3;
				totalCells = 12;
				fps=30;
			}
			
			playWrongAnswerAnim();
		}else if(ollyState == ollyAnimationStates.olly_weap){
		
		//	Debug.Log("in olly weap anim");
			if(changeTexture){
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ollyWeap) as Texture2D;
				transform.localScale = new Vector3(70F , 104F , 1F );
				Vector3 pos=transform.localPosition;
				if(pos.y>=75)
				   pos.y -=75F;
				transform.localPosition=pos;
				isAnimating=false; changeTexture = false;
				colCount   = 3;
				rowCount   = 2;
				totalCells = 6;
				fps=30;
			}
			playWeapAnim();
		}
	}
	
	// ---- idle animation
	public void playIdle1Anim(){
		if(idleAnimFrames!=null && idleAnimFrames.Count>0){
			setCurrentFrame((int)idleAnimFrames[0]);
			idleAnimFrames.RemoveAt(0);
		}    
		if(idleAnimFrames.Count==0){	
				ollyState=ollyAnimationStates.olly_still;	
				createIdleAnim();
			}
		}
	

		// ---- jump land to first landmass
	public void playJumpLandToWaterAnim(){
		if(jumpLandToWaterFrames!=null && jumpLandToWaterFrames.Count>0){
			setCurrentFrame((int)jumpLandToWaterFrames[0]);
			jumpLandToWaterFrames.RemoveAt(0);
		}
	    
		if(jumpLandToWaterFrames==null || jumpLandToWaterFrames.Count ==0){	
			createLandToWaterAnim();
			gameController.setFingerIndexOnAnimationComplete(); 
			
			
			if(!isLastShape){
				//Debug.Log("Enabling touches and showing referent bubble");
				gameController.showReferentBubble();
				gameController.enableTouchesFlag = true;
			}
			
			
			if(answerShapesCount>=2){
				changeTexture=true;
				ollyState  =  ollyAnimationStates.olly_hop;
				
				Vector3 pos = transform.localPosition;
				//pos.y += 75.5F;
				transform.localPosition=pos;
				
			}
				
			//else ollyState =  ollyAnimationStates.olly_jumpWaterToLand;
		}
	}
	
	// ----- hop animation
	private void playHopAnimaiton(){
		 setCurrentFrame((int)hopAnimFrames[counter]);	
			counter++;
		if(counter>=10) counter=0;
	}
	
	// ---- jump one landmass to other landmass
	public void playJumpWaterToWaterAnim(){
		if(jumpWaterToWaterFrames.Count>0){
			setCurrentFrame((int)jumpWaterToWaterFrames[0]);
			jumpWaterToWaterFrames.RemoveAt(0);
		}
		if(jumpWaterToWaterFrames.Count==0){
			gameController.setFingerIndexOnAnimationComplete(); 
		
		if(!isLastShape){
			gameController.showReferentBubble();
			gameController.enableTouchesFlag = true;
	}
		  createWaterToWaterAnim();
		  changeTexture=true;
		  ollyState =  ollyAnimationStates.olly_hop;
		}
	}
	
	// ---- 1. water to land animation ----- 2. on reaching land celebration aniamtion
	public void playJumpWaterToLandAnim(){
		if(jumpWaterToLand.Count>0){
			setCurrentFrame((int)jumpWaterToLand[0]);
			jumpWaterToLand.RemoveAt(0);
		if(jumpWaterToLand.Count==0){
			gameController.playCelebrationVoiceOvers();
			gameController.setFingerIndexOnAnimationComplete(); 
			changeTexture=true;
			ollyState = ollyAnimationStates.olly_celebration;
		    createWaterToLandAnim();
		}
		}
	}
	
	// ---- celebration aniamtion
	private void playCelebrationAnim(){
		if(celebrationFrames.Count>0){
			setCurrentFrame((int)celebrationFrames[0]);
			celebrationFrames.RemoveAt(0);
		}
		if(celebrationFrames.Count==0){
			ollyState = ollyAnimationStates.olly_idle1;
			changeTexture = true;
			gameController.loadLevelOrQuestion();
			createCelebrationAnim();
		}
	}
	
		// ---- tickle animation
	public void playTickleAnim(){
		if(tickleFrames.Count>0){
			setCurrentFrame((int)tickleFrames[0]);
			tickleFrames.RemoveAt(0);
		}
		if(tickleFrames.Count==0){
		  createTickleAnim();
		  changeTexture = true;
		  ollyState = ollyAnimationStates.olly_idle1;	
		}
	}
	
	private void playWrongAnswerAnim(){
		if(wrongAnswerFrames.Count>0){
			setCurrentFrame((int)wrongAnswerFrames[0]);
			wrongAnswerFrames.RemoveAt(0);
		}
		if(wrongAnswerFrames.Count==0){
		  	createWrongAnswerAnim();
		  	changeTexture = true;
		 if(ollyLastStat==ollyAnimationStates.olly_still)
			ollyState = ollyAnimationStates.olly_idle1;
		 else  ollyState = ollyLastStat;
		}
	}
	
 private void playWeapAnim(){
		if(weapFrames.Count>0){
			setCurrentFrame((int)weapFrames[0]);
			weapFrames.RemoveAt(0);
		}
		if(weapFrames.Count==0){
			 createWeapAnim();
			 changeTexture = true;
			ollyState = ollyAnimationStates.olly_idle1;
			gameController.setFingerIndexOnAnimationComplete();
			gameController.loadLevelOrQuestion();
		}
	}	
	
	
	IEnumerator setFlag(float time ,ollyAnimationStates stat){	
		yield return new WaitForSeconds(time);
		if(isAnimating){
			ollyState = stat;
			changeTexture = true;	
		}
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
	
	
	
void OnExit(){
		isAnimating=false;
	}

}
