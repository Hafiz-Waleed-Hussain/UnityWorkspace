using UnityEngine;
using System.Collections;

public class MAEyeBlink : MonoBehaviour {

	// Use this for initialization
	
	ArrayList blinkAnimFrames = new ArrayList();	
	
	public int animState = 0; 
	
	int colCount = 4;
	int rowCount = 1;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 4;
	int fps = 20;
	bool bLoop = false;
	
	int currentFrame = 0;
	
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
		
		setCurrentFrame((int)blinkAnimFrames[0]);
		blinkAnimFrames.RemoveAt(0);
		if(blinkAnimFrames.Count == 0) {
			animState = 0;
			createAnimations();
		}
	}
	
	void createAnimations() {
		
		int [] blinkAnimFramesOrder = {0,0,1,1,2,2,3,3,2,2,1,1,0,0};
		blinkAnimFrames = new ArrayList();
		for (int i = 0; i < blinkAnimFramesOrder.Length; i++) {
			blinkAnimFrames.Add(blinkAnimFramesOrder[i]);
		}
		for (int i = 0; i < blinkAnimFramesOrder.Length; i++) {
			blinkAnimFrames.Add(blinkAnimFramesOrder[i]);
		}
		for (int i = 0; i < fps*2; i++) { //2 sec delay in animation
			blinkAnimFrames.Add(0);
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
