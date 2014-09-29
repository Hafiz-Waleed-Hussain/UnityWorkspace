#region AGNITUS 2013
/* JungleWorld- Pond Game
 * Developer- Abid Ali
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;

public enum ickyAnimStates{
	unknown,
	icky_hop,
	icky_landToWater,
	icky_lookAround,
	icky_WaterToLand,
	icky_WaterToLanded,
	icky_shakeOff,
	icky_turn,
	icky_idle1,
	icky_come,
	icky_tickle,
	icky_weap,
	icky_celebration
}; 



public class PG_IckyAnimations : MonoBehaviour {


	public bool isAnimating;
	public float currentFrame=0.0f;
	public ickyAnimStates ickyState;
	public ickyAnimStates ickyPrevState;
	public float animationTime;
	GameObject camera;
	
	int colCount = 3;
	int rowCount = 3;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 9;
	int fps = 30;
	int hopAnimCount=0;
	int count=0;
	bool bLoop = false ,isIckyFilipped=false;
	public static bool changeTexture;
	float frame=0;
	int counter=0;

	ArrayList hopAnimFrames 				= 	new ArrayList();
	ArrayList landToWaterAnimFrames 		= 	new ArrayList();
	ArrayList lookAroundAnimFrames 			= 	new ArrayList();
	ArrayList waterToLandAnimFrames 		= 	new ArrayList();
	ArrayList waterToLandedAnimFrames 		= 	new ArrayList();
	ArrayList shakeOffAnimFrames 			= 	new ArrayList();
	ArrayList turnAnimFrames 				= 	new ArrayList();
	
	ArrayList crossPondAnimFrames 			= 	new ArrayList();
	
	
	ArrayList idleAnimFrames 				= 	new ArrayList();
	ArrayList comeAnimFrames		 		=	new ArrayList();
	ArrayList tickleAnimFrames				=  	new ArrayList();
	ArrayList weapAnimFrames				=  	new ArrayList();
	ArrayList celebrationFrames				=  	new ArrayList();
	ArrayList celebrationLastTwoFrames		=  	new ArrayList();
	ArrayList celebrationFramesFlipped		=  	new ArrayList();
	
	PG_SoundManager 							soundManager;
	
		private void createIdleAnim(){ 
		int[] frames = {0,1,2,3};	
		
		for(int i=0 ; i<frames.Length;i++)
			idleAnimFrames.Add(frames[i]);
		for(int i=0 ; i<10 ;i++)
			idleAnimFrames.Add(4);
		idleAnimFrames.Add(5); idleAnimFrames.Add(6);
		
		for(int i=0 ; i<71 ;i++)
			idleAnimFrames.Add(7);
		int[] frames1 = {8,9,10,11,12,13,14};
			for(int i=0 ; i<frames1.Length ;i++)
			idleAnimFrames.Add(frames1[i]);
		
		for(int i=0 ; i<12 ;i++)
			idleAnimFrames.Add(15);
		idleAnimFrames.Add(16); idleAnimFrames.Add(17); idleAnimFrames.Add(18); idleAnimFrames.Add(19);
		
		for(int i=0 ; i<8 ;i++)
			idleAnimFrames.Add(20);
		idleAnimFrames.Add(21); idleAnimFrames.Add(22); idleAnimFrames.Add(23); idleAnimFrames.Add(24);
	}
	
	private void createComeAnim(){
		int[] frames = {0,0,0,1,2,3,4,5,6,7,8,9,10,11,11,6,7,8,9,10,11,11,12,13,14,5,4,3,2,1};
		for(int i=0 ; i<frames.Length;i++){
			comeAnimFrames.Add(frames[i]);
		}
	}
	
	
	private void createCelebrationAnim(){
		for(int i=0 ; i<46;i++)
			celebrationFrames.Add(i);
		for(int i=1 ; i<21;i++)
			celebrationFramesFlipped.Add(i);
		for(int i=0 ; i<3 ;i++)
			celebrationLastTwoFrames.Add(i);
		
	}
	
	
	
  private void createTickleAnim(){
		int[] frames = {0,1,2,3,4,5,6,7,8,9,10,8,8,10,10,8,8,10,10,8,8,10,10,4,3,2,1,0};
		for(int i=0 ; i<frames.Length ; i++)
			tickleAnimFrames.Add(frames[i]);
	}
	
	
	 private void createWeapAnim(){
		int[] frames = {0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,1,1,1};
		for(int i=0 ; i<frames.Length ; i++)
			weapAnimFrames.Add(frames[i]);
	}
	
	 private void createCrossPondAnim(){
		
		//hop, landtowater, look around, water to land, water to landed,shake off,turn, idle
		int[] frames = {0,1,1,2,3,3,3,3,3,4,5,6,6,6,6,6,6,6,6,6,7,8,9,9,9,9,9,9,9,9,10,11,12,12,12,12,12,12,12,12,12,12,12,12,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,
		                15,16,17,18,19,20,21,22,23,23,23,23,23,23,23,24,25,26,27,28,29,30,31,32,33,34,35,35,35,35,35,35,35,35,35,35,35,35};
		
		for(int i=0 ; i<frames.Length ; i++)
			crossPondAnimFrames.Add(frames[i]);
		
		for(int j=36;j<=82;j++)
			crossPondAnimFrames.Add(j);
		
		for(int k=0; k<15; k++)
		   crossPondAnimFrames.Add(83);
		
		for(int k=84; k<=92; k++)
		   crossPondAnimFrames.Add(k);
		for(int k=0; k<7; k++)
		   crossPondAnimFrames.Add(93);
		
		for(int k=84; k<=92; k++)
		   crossPondAnimFrames.Add(k);
		for(int k=0; k<7; k++)
		   crossPondAnimFrames.Add(93);
		
		for(int k=94; k<=119; k++)
		   crossPondAnimFrames.Add(k);
		for(int k=0; k<15; k++)
		   crossPondAnimFrames.Add(120);
		
		for(int k=121; k<=125; k++)
		   crossPondAnimFrames.Add(k);
			crossPondAnimFrames.Add(126);
			crossPondAnimFrames.Add(126);
		
		int[] frames1 = {125,124,125,126,125,124,125,126,124,124,124,127,128,129};
		for(int i=0 ; i<frames1.Length ; i++)
			crossPondAnimFrames.Add(frames1[i]);
		
		for(int i=0;i<12;i++)
		crossPondAnimFrames.Add(120);
		crossPondAnimFrames.Add(130);
		crossPondAnimFrames.Add(131);
		crossPondAnimFrames.Add(132);
		for(int k=0; k<8; k++)
		   crossPondAnimFrames.Add(133);
		crossPondAnimFrames.Add(134);
		crossPondAnimFrames.Add(135);
		crossPondAnimFrames.Add(136);
		crossPondAnimFrames.Add(137);
		crossPondAnimFrames.Add(138);
		crossPondAnimFrames.Add(139);
		
		for(int k=0; k<8; k++)
		   crossPondAnimFrames.Add(140);
		crossPondAnimFrames.Add(141);
		crossPondAnimFrames.Add(142);
		crossPondAnimFrames.Add(143);
		crossPondAnimFrames.Add(144);
		
		for(int k=0; k<10; k++)
		   crossPondAnimFrames.Add(145);
		

	}
	void createHopAnim(){
		for(int k=0; k<18; k++)
			hopAnimFrames.Add(k);
	}
	
	void createLandToWaterAnim(){
		int[] frames = {26,27,28,29,30,31,32,33,34,35,35,35,35,35,35,35,35,35,35,35,35};
		for(int i=0 ; i<frames.Length ; i++)
			landToWaterAnimFrames.Add(frames[i]);
	}
	
	void createLookAroundAnim(){
		for(int k=36; k<83; k++)
		   lookAroundAnimFrames.Add(k);
		for(int k=0; k<=15; k++)
		   lookAroundAnimFrames.Add(83);
	}
	
	void createWaterToLandAnim(){
		int[] frames = {94,95,96,97,98,99,100,101,102,103,104,105};
		for(int i=0 ; i<frames.Length ; i++)
			waterToLandAnimFrames.Add(frames[i]);
	}
	
	
	void createWaterToLandedAnim(){
		int[] frames = {106,107,108,109,110,111,112,113,114,115,116,118,117,118,119};
		for(int i=0 ; i<frames.Length ; i++)
			waterToLandedAnimFrames.Add(frames[i]);
		for(int k=0; k<15; k++)
		   lookAroundAnimFrames.Add(120);
	}
	
	
	void createShakeOffAnim(){
		int[] frames = {121,122,123,124,125,126,126,125,124,125,126,125,124,125,126,124,124,124,127,128,129};
		
		for(int i=0 ; i<frames.Length ; i++)
			shakeOffAnimFrames.Add(frames[i]);
		
		for(int k=0; k<12; k++)
		   lookAroundAnimFrames.Add(120);
	}
	
	
	void createTurnAnim(){
		int[] frames = {0,1,2,3,4,5,6,7,8,9,10,11,12};
		
		for(int i=0 ; i<frames.Length ; i++)
			turnAnimFrames.Add(frames[i]);
	}
	
	
	
	
	
	void Awake(){
		createHopAnim();
		createLandToWaterAnim();
		createLookAroundAnim();
		createWaterToLandAnim();
		createWaterToLandedAnim();
		createShakeOffAnim();
		createTurnAnim();
		//createCrossPondAnim();
		createIdleAnim();
		createComeAnim();
		createCelebrationAnim();
		createTickleAnim();
		createWeapAnim();
	
	}	
	
	void Start(){
		GameObject shapesRef	=	GameObject.FindGameObjectWithTag("pg_shapes_container");
		if(shapesRef!=null)
		soundManager					=	shapesRef.GetComponent<PG_SoundManager>();
		currentFrame =0.0f;
		isAnimating   = true;
		changeTexture = true;
		ickyState =ickyAnimStates.icky_idle1;
		ickyPrevState=ickyState;
	}
	
	void Update (){	
		int index=(int)(Time.time*fps);
		if(ickyState!=ickyPrevState){
			playIcky_Animations();
			ickyPrevState=ickyState;
		} else 
		if(index > currentFrame){
		 	currentFrame = index;
			playIcky_Animations();
		}
	}


	

public void playIcky_Animations(){ 
	  
			currentFrame += Time.deltaTime;

			if(ickyState == ickyAnimStates.icky_hop){
				if(changeTexture){
				transform.localScale = new Vector3(100F,180F,1);
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ickyCrossPond) as Texture2D;	
				isAnimating=true; changeTexture = false;
				colCount = 15;  rowCount = 10; totalCells = 150;
			}
			playHopAnim();
		}
		else if(ickyState == ickyAnimStates.icky_landToWater){
			playLandToWaterAnim();
		}
		else if(ickyState == ickyAnimStates.icky_lookAround){
			playLookAroundAnim();
		}
		else if(ickyState == ickyAnimStates.icky_WaterToLand){
			playWaterToLandAnim();
		}
		else if(ickyState == ickyAnimStates.icky_WaterToLanded){
			playWaterToLandedAnim();
		}
		else if(ickyState == ickyAnimStates.icky_shakeOff){
			playTurnAnim();
		}
		else if(ickyState == ickyAnimStates.icky_turn){
			playShakeOffAnim();
		}
		
		else if(ickyState == ickyAnimStates.icky_idle1){
//				Debug.Log("icky idle animation...");
				if(changeTexture){
				transform.localScale = new Vector3(100F,180F,1);
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ickyIdle) as Texture2D;	
				isAnimating=true; changeTexture = false;
				colCount = 5;  rowCount = 5; totalCells = 25;
			}
			playIdleAnim();
		}
		else if(ickyState == ickyAnimStates.icky_come){
			//Debug.Log("icky come animation...");
				if(changeTexture){
				transform.localScale = new Vector3(100F,180F,1);
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ickyCome) as Texture2D;
				isAnimating=true; changeTexture = false;
				colCount = 5;  rowCount = 3; totalCells = 15;
			}
			playComeAnimation();
		 }else if(ickyState == ickyAnimStates.icky_celebration){
			//Debug.Log(" Icky celebration...");
			if(changeTexture){
				transform.localScale = new Vector3(145F,180F,1);
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ickyCelebration) as Texture2D;
				isAnimating=false; changeTexture = false;
				colCount = 8;  rowCount = 6; totalCells = 48 ;
			}
			playCelebrationAnim();
		 }else if(ickyState == ickyAnimStates.icky_tickle){
		//	Debug.Log("Icky Tickle...");
			if(changeTexture){
				transform.localScale = new Vector3(100F,180F,1);
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ickyTickle) as Texture2D;
				isAnimating=false; changeTexture = false;
				colCount = 6;  rowCount = 2; totalCells = 12;
			}
			playTickleAnim();
		 }else if(ickyState == ickyAnimStates.icky_weap){
			//Debug.Log("Icky Tickle...");
			if(changeTexture){
				transform.localScale = new Vector3(83F,63F,1);
				Vector3 pos=transform.localPosition;
				pos.y -=58.5F;
				transform.localPosition=pos;
				renderer.material.mainTexture = Resources.Load(PG_Constants._animations+PG_Constants._ickyWeap) as Texture2D;
				isAnimating=false; changeTexture = false;
				colCount = 2;  rowCount = 1; totalCells = 2;
			}
			playWeapAnim();
		}
		
	}	
		
	
		void playHopAnim(){
		    
			if(hopAnimFrames.Count>0){	
			setCurrentFrame((int)hopAnimFrames[count]);
			count++;
			if(count==hopAnimFrames.Count){
			   count=0;
				if(soundManager!=null)
					soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath +PG_Constants._introClips +PG_Constants._jump});
			   hopAnimCount++;
			if(hopAnimCount==3){
			    count=0;
				ickyState = ickyAnimStates.icky_landToWater;
			}
		  }
		}
	}
	
	void playLandToWaterAnim(){
		if(landToWaterAnimFrames.Count>0){
			setCurrentFrame((int)landToWaterAnimFrames[0]);
			landToWaterAnimFrames.RemoveAt(0);
		}
		if(landToWaterAnimFrames.Count == 0){
			createLandToWaterAnim();
			ickyState = ickyAnimStates.icky_lookAround;
		}
	}
		
	void playLookAroundAnim(){
		if(lookAroundAnimFrames.Count>0){
			setCurrentFrame((int)lookAroundAnimFrames[0]);
			lookAroundAnimFrames.RemoveAt(0);
		}
		if(lookAroundAnimFrames.Count == 0){
			createLookAroundAnim();
			ickyState = ickyAnimStates.icky_WaterToLand;
		}
	}
	
	void playWaterToLandAnim(){
		if(waterToLandAnimFrames.Count>0){
			setCurrentFrame((int)waterToLandAnimFrames[0]);
			waterToLandAnimFrames.RemoveAt(0);
		}
		if(waterToLandAnimFrames.Count == 0){
			createWaterToLandAnim();
			ickyState = ickyAnimStates.icky_WaterToLanded;
		}
	}
	
	void playWaterToLandedAnim(){
		if(waterToLandedAnimFrames.Count>0){
			setCurrentFrame((int)waterToLandedAnimFrames[0]);
			waterToLandedAnimFrames.RemoveAt(0);
		}
		if(waterToLandedAnimFrames.Count == 0){
			createWaterToLandedAnim();
			ickyState = ickyAnimStates.icky_shakeOff;
		}
	}
		void playShakeOffAnim(){
		if(shakeOffAnimFrames.Count>0){
			setCurrentFrame((int)shakeOffAnimFrames[0]);
			shakeOffAnimFrames.RemoveAt(0);
		}
		if(shakeOffAnimFrames.Count == 0){
			createShakeOffAnim();
			ickyState = ickyAnimStates.icky_turn;
		}
	}
	
	void playTurnAnim(){
		if(turnAnimFrames.Count>0){
			setCurrentFrame((int)turnAnimFrames[0]);
			turnAnimFrames.RemoveAt(0);
		}
		if(turnAnimFrames.Count == 0){
			createTurnAnim();
			changeTexture = true;
			ickyState = ickyAnimStates.icky_idle1;
			
		}
	}
	
	// ---- Icky cross pond animation
	

	// ---- Icky idle animation
	public void playIdleAnim(){
		if(idleAnimFrames.Count>0){	
		setCurrentFrame((int)idleAnimFrames[0]);
			idleAnimFrames.RemoveAt(0);
		}
		if(idleAnimFrames.Count == 0){
			createIdleAnim();
			changeTexture = true;
			ickyState = ickyAnimStates.icky_come;
		}
	}
	
	// ---- Icky  come animation
	public void playComeAnimation(){
	   if(comeAnimFrames.Count>0){
		  setCurrentFrame((int)comeAnimFrames[0]);
		  comeAnimFrames.RemoveAt(0);
		}
		if(comeAnimFrames.Count == 0){
		  changeTexture = true;
		  createComeAnim();
		  ickyState = ickyAnimStates.icky_idle1;
		}		
		}
	
	// ---- celebration aniamtion
	public void playCelebrationAnim(){
		if(celebrationFrames!=null && celebrationFrames.Count>0){
			setCurrentFrame((int)celebrationFrames[0]);
			celebrationFrames.RemoveAt(0);
		}
		

		if(celebrationFrames.Count == 0 && !isIckyFilipped){
			gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x , gameObject.transform.localScale.y,1);
			isIckyFilipped = true;
		}
			
		if(celebrationFrames.Count==0 && celebrationFramesFlipped!=null && celebrationFramesFlipped.Count>0 && isIckyFilipped){
			
			setCurrentFrame((int)celebrationFramesFlipped[0]);
			celebrationFramesFlipped.RemoveAt(0);
		}
		
		if(celebrationFrames.Count== 0  && celebrationFramesFlipped.Count == 0 && isIckyFilipped){
			gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x , gameObject.transform.localScale.y,1);
			isIckyFilipped = false;
		}
		
		
		if(celebrationFrames.Count== 0  && celebrationFramesFlipped.Count == 0 && !isIckyFilipped && celebrationLastTwoFrames.Count>0){
			setCurrentFrame((int)celebrationLastTwoFrames[0]);
			celebrationLastTwoFrames.RemoveAt(0);
		}
		
		if(celebrationFrames.Count== 0  && celebrationFramesFlipped.Count == 0 && !isIckyFilipped && celebrationLastTwoFrames.Count == 0){
			createCelebrationAnim();
			changeTexture = true;
			ickyState = ickyAnimStates.icky_idle1;
		}
		}

	// ---- tickle animation
	public void playTickleAnim(){
		if(tickleAnimFrames.Count>0){
		   setCurrentFrame((int)tickleAnimFrames[0]);
		   tickleAnimFrames.RemoveAt(0);
		 }
		if(tickleAnimFrames.Count == 0){
			createTickleAnim();
			changeTexture = true;
			ickyState = ickyAnimStates.icky_idle1;
		}	
	}
	
	
	public void playWeapAnim(){
		if(weapAnimFrames.Count>0){
			 setCurrentFrame((int)weapAnimFrames[0]);
		  	 weapAnimFrames.RemoveAt(0);
		}
		if(weapAnimFrames.Count==0){
			createWeapAnim();
			changeTexture = true;
			Vector3 pos=transform.localPosition;
			pos.y +=58.5F;
			transform.localPosition=pos;
			ickyState = ickyAnimStates.icky_idle1;
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





	
	
	
	
	

	

	


