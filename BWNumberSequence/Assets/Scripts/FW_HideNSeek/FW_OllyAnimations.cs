using UnityEngine;
using System.Collections;

public class FW_OllyAnimations : MonoBehaviour {
	public bool playWrongAnimation;
	public string ollyState = null;
	
	ArrayList wrongAnimFrames = new ArrayList();
	ArrayList blinkAnimFrames = new ArrayList();
	
	int colCount = 2;
	int rowCount = 1;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 2;
	int fps = 30;
	
	bool bLoop = false;
	void createWrongAnimation(){
		int[] frames = {0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1};
		for(int i=0 ; i<frames.Length ; i++)
			wrongAnimFrames.Add(frames[i]);
	}
	
	void Start(){
		playWrongAnimation = false;
		createWrongAnimation();
	}
	
	void Update(){
		if(playWrongAnimation)
		playOllyWronAnimation();
	}
	
	
	public void ollyAskingNumber(string number){
		ollyState = number;
		gameObject.renderer.material.mainTexture = Resources.Load("FishWorld_HideNSeek/Sprites/OllyAnimFrames/"+"fishHnS_Olly02_"+number) as Texture2D;
	}
	
		public void ollyIdleFrame(){
		gameObject.renderer.material.mainTexture = Resources.Load("FishWorld_HideNSeek/Sprites/OllyAnimFrames/fishHnS_Olly02_0015") as Texture2D;
	}
	
	public void playOllyWronAnimation(){
		if(wrongAnimFrames.Count>0){
			setCurrentFrame((int)wrongAnimFrames[0]);
			wrongAnimFrames.RemoveAt(0);
		}
		if(wrongAnimFrames.Count ==0){
			
			 renderer.material.SetTextureOffset ("_MainTex", new Vector2(0,0));
			renderer.material.SetTextureScale  ("_MainTex", new Vector2(1,1));
			playWrongAnimation=false;
			ollyIdleFrame();
			if(ollyState!=null)
			ollyAskingNumber(ollyState);
			createWrongAnimation();
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
