using UnityEngine;
using System.Collections;

public class BWBeeHead : MonoBehaviour {

	// Use this for initialization
	
	public BWBee beeDelegate = null; 
	
	ArrayList blinkAnimFrames = new ArrayList(60);	
	int counter = 0;
	ArrayList yesAnimFrames = null;
	ArrayList noAnimFrames = null;
	
	public int animState = 0; //0-idle, 1-YES, 2-NO, 3-invisible
	
	int colCount = 3;
	int rowCount = 4;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 12;
	int fps = 30;
	bool bLoop = false;
	
	int currentFrame = 0;
	
	void Start () {
		
		for(int i=0; i<56; i++) {
			blinkAnimFrames.Add(10);
		}
		for(int i=56; i<60; i++) {
			blinkAnimFrames.Add(6);
		}
		counter = 0;
		
		createAnimations();
	}
	
	// Update is called once per frame
	void Update () {
		int index  = (int)(Time.time * fps);
		if(index > currentFrame) {
			currentFrame = index;
			checkAnimations();
		}
	}
	
	void yesFinished () {
		
	}
	
	void noFinished () {
		BWNumberSeqGame numberSeq = Camera.mainCamera.GetComponent<BWNumberSeqGame>();
		BWNumberArithmetic numberArth = Camera.mainCamera.GetComponent<BWNumberArithmetic>();
		
		if(numberSeq != null) {
			//
		} else if (numberArth != null) {
			numberArth.beeNoAnimationFinished();
		}
	}
	
	void checkAnimations () {
		if(animState == 0) {
			setCurrentFrame((int)blinkAnimFrames[counter]);
			counter++;
			if(counter >= 60) counter = 0;
		} else if (animState == 1) { //YES
			setCurrentFrame((int)yesAnimFrames[0]);
			yesAnimFrames.RemoveAt(0);
			if(yesAnimFrames.Count == 0) {
				animState = 0;
				if(beeDelegate != null)
					beeDelegate.yesFinished();
				createAnimations();
			}
		} else if (animState == 2) { //NO
			setCurrentFrame((int)noAnimFrames[0]);
			noAnimFrames.RemoveAt(0);
			if(noAnimFrames.Count == 0) {
				animState = 0;
				if(beeDelegate != null)
					beeDelegate.noFinished();
				createAnimations();
			}
		} else {
			setCurrentFrame(11);
		}
	}
	
	void createAnimations() {
		int [] noHeadFramesOrder = {7,7,5,5,4,4,9,9,4,4,5,5,4,4,9,9,4,7};
		noAnimFrames = new ArrayList();
		for (int i = 0; i < noHeadFramesOrder.Length; i++) {
			noAnimFrames.Add(noHeadFramesOrder[i]);
		}
		
		int [] yesHeadFramesOrder = {3,3,2,2,1,1,1,0,0,0,1,1,1,0,0,0,1,1};
		yesAnimFrames = new ArrayList();
		for (int i = 0; i < yesHeadFramesOrder.Length; i++) {
			yesAnimFrames.Add(yesHeadFramesOrder[i]);
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
