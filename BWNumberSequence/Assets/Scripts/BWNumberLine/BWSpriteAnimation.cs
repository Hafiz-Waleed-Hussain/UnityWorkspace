using UnityEngine;
using System.Collections;

public class BWSpriteAnimation : MonoBehaviour {

	// Use this for initialization
	int colCount = 0;
	int rowCount = 0;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 0;
	int fps = 30;
	bool bLoop = false;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
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
