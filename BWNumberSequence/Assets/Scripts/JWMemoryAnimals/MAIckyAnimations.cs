using UnityEngine;
using System.Collections;

public class MAIckyAnimations : MonoBehaviour {
	
	ArrayList giggleFrames;
	ArrayList wrongFrames;
	ArrayList wantFrames;
	ArrayList idleFrames;
	ArrayList celebrationFrames;
	
	public Texture2D ickyImage = null;
	
	int colCount = 3;
	int rowCount = 4;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 12;
	int fps = 50;
	bool bLoop = false;
	int currentFrame = 0;
	
	public MAIckyAnimationState animState = MAIckyAnimationState.unknown;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update (){
		
		if(animState == MAIckyAnimationState.unknown) {
			animState = MAIckyAnimationState.idle;
		}
		int index  = (int)(Time.time * fps);
		
		if(index > currentFrame) {
			currentFrame = index;
			playAnimations();
		}
	}
	
	void createAnimations () {
		
		int [] wrongOrder = {0,1,2,2,2,3,4,5,6,6,6,5,4,3,2,7,8,8,8,7,2,3,4,5,6,6,6,5,4,3,15};
		wrongFrames = new ArrayList();
		for(int i = 0; i< wrongOrder.Length; i++) {
			wrongFrames.Add(wrongOrder[i]);
		}
		
		idleFrames = new ArrayList();
		for(int i=0;i<59;i++){
			idleFrames.Add(i);
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
	}
	
	
	
	public void playAnimations(){ 
		
		if(animState == MAIckyAnimationState.unknown) {
			
			renderer.material.mainTexture = ickyImage;
			transform.localScale = new Vector3 (140, 165, 1);
			
			colCount = 1;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 1;
			fps = 30;
			setCurrentFrame(0);
			
		} else if (animState == MAIckyAnimationState.giggle) {
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("JWMemoryAnimals/Sprites/Icky/MA_Icky_Giggle");
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
				animState = MAIckyAnimationState.unknown;
				createAnimations();
			}
		}else if (animState == MAIckyAnimationState.celebration) {
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("JWMemoryAnimals/Sprites/Icky/MA_Icky_Celebration");
			
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
				animState = MAIckyAnimationState.unknown;
				createAnimations();
			}
			
		}  else if (animState == MAIckyAnimationState.want){
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("JWMemoryAnimals/Sprites/Icky/MA_Icky_Wants");
			
			transform.localScale = new Vector3(-145,160,1);
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
				animState = MAIckyAnimationState.idle;
				createAnimations();
			}
		} else if (animState == MAIckyAnimationState.idle){
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("JWMemoryAnimals/Sprites/Icky/MA_Icky_Idle_1");
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
				animState = MAIckyAnimationState.want;
				createAnimations();
			}
		} else if (animState == MAIckyAnimationState.wrong) {
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("JWMemoryAnimals/Sprites/Icky/MA_Icky_NoNo");
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
				fps = 30;
				currentFrame = 0;
				animState = MAIckyAnimationState.unknown;
				createAnimations();
			}
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
}
