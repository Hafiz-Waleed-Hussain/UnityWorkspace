using UnityEngine;
using System.Collections;

public class BWBeeBody : MonoBehaviour {
	
	// Use this for initialization
	ArrayList diveFrames = null;
	ArrayList spinFrames = null;
	ArrayList circleFrames = null;
	
	public bool changePositionForCircle = true;
	
	public int animState = 0; //0-idle, 2-dive, 1-spin, 3-circle
	
	int colCount = 1;
	int rowCount = 1;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 1;
	int fps = 30;
	bool bLoop = false;
	
	int currentFrame = 0;
	
	public Texture2D idleMaterial;
	
	public BWBee beeDelegate;
	
	void Start () {
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
	
	void checkAnimations () {
		if(animState == 0) {
			renderer.material.mainTexture = idleMaterial;
			
			colCount = 1;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 1;
			fps = 30;
			
			setCurrentFrame(0);
			
		} else if (animState == 1) { //spin
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("BW_NumberLine/Sprites/bee_body_spin");
			
			colCount = 7;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 21;
			fps = 30;
			
			if(spinFrames == null || spinFrames.Count < 0)
				createAnimations();
			
			setCurrentFrame((int)spinFrames[0]);
			spinFrames.RemoveAt(0);
			if(spinFrames.Count == 0) {
				
				if(beeDelegate != null)
					beeDelegate.celebrationAnimationFinished();
				
				animState = 0;
				createAnimations();
			}
			
		} else if (animState == 2) { //dive
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("BW_NumberLine/Sprites/bee_body_dive");
			
			colCount = 5;
			rowCount = 4;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 20;
			fps = 30;
			
			if(diveFrames == null || diveFrames.Count < 0)
				createAnimations();
			
			setCurrentFrame((int)diveFrames[0]);
			diveFrames.RemoveAt(0);
			if(diveFrames.Count == 0) {
				
				if(beeDelegate != null)
					beeDelegate.celebrationAnimationFinished();
				
				animState = 0;
				createAnimations();
			}
			
		} else if (animState == 3) { //circle
			
			renderer.material.mainTexture = (Texture2D)Resources.Load("BW_NumberLine/Sprites/bee_body_circle");
			transform.localScale = new Vector3(600.0f, 500.0f, 1.0f);
			
			if(changePositionForCircle) {
				Vector3 newPos = transform.localPosition;
				newPos.x -= 175.0f;
				transform.localPosition = newPos;
				changePositionForCircle = false;
			}
			
			colCount = 3;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 9;
			fps = 30;
			
			if(circleFrames == null || circleFrames.Count < 0)
				createAnimations();
			
			setCurrentFrame((int)circleFrames[0]);
			circleFrames.RemoveAt(0);
			if(circleFrames.Count == 0) {
				transform.localScale = new Vector3(225.0f, 225.0f,1.0f);
				renderer.material.mainTexture = idleMaterial;
				
				Vector3 newPos = transform.localPosition;
				newPos.x += 175.0f;
				transform.localPosition = newPos;
				
				if(beeDelegate != null)
					beeDelegate.tapAnimationFirstHalfFinished();
				
				animState = 0;
				createAnimations();
				
			}
			
		} else {
			setCurrentFrame(0);
		}
	}
	
	void createAnimations() {
		
		diveFrames = new ArrayList();
		
    	for (int i=0; i<20; i++) {
        	diveFrames.Add(i);
        	if (i == 5) {
	            diveFrames.Add(i);
	        }
	        if (i == 2) {
	            for (int j=0; j<9; j++) {
	                diveFrames.Add(i);
	            }
	        }
	    }
	    diveFrames.Add(0);
		
		
		spinFrames = new ArrayList();
	    for (int i=0; i<4; i++) {
	        spinFrames.Add(0);
	    }
		spinFrames.Add(1);
		spinFrames.Add(2);
		spinFrames.Add(2);
		
	    for (int i=0; i<14; i++) {
	        spinFrames.Add(3);
	    }
	    
	    for (int i = 0; i < 4; i++) {
	        for (int j=4; j<=7; j++) {
	            spinFrames.Add(j);
	        }
	    }
	    for (int j = 8; j < 21; j++) {
	        spinFrames.Add(j);
	        if (j == 11 || j == 17 || j== 18 || j==14 || j==19 || j==20) {
	            spinFrames.Add(j);
	        }
	        if (j== 15 || j ==20 || j==21) {
	            spinFrames.Add(j);
	        }
	    }
		
		circleFrames = new ArrayList();
		int [] circleOrder = {0,1,2,1,3,3,4,4,5,5,6,7,8};
		for(int i = 0; i<circleOrder.Length; i++) {
			circleFrames.Add(circleOrder[i]);
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
	
	void diveFinished () {
		
	}
	
	void spinFinished () {
		BWNumberSeqGame numberSeq = Camera.mainCamera.GetComponent<BWNumberSeqGame>();
		BWNumberArithmetic numberArth = Camera.mainCamera.GetComponent<BWNumberArithmetic>();
		
		if(numberSeq != null) {
			//numberSeq
		} else if (numberArth != null) {
			//numberArth
		}
	}
	
}
