using UnityEngine;
using System.Collections;

public class NF_VineAnimations : MonoBehaviour {
	
	ArrayList vineFrameSeq;
	public int colCount =  0;
	public int rowCount =  0;
	 
	//vars for animation
	public int  rowNumber  =  0; //Zero Indexed
	public int colNumber = 0; //Zero Indexed
	public int totalCells = 0;
	public int  fps     = 0;
	bool bLoop = false;
	int currentFrame = 0;
    private Vector2 offset;
	
	public void destroyVine(){
		Destroy(this.gameObject);
	}
	
	public void selfDestroy(Object obj){
		Destroy((GameObject) obj);
	}
	
	// Update is called once per frame
	void Update () {
		
		int index  = (int)(Time.time * fps);
		if(index > currentFrame) {
			currentFrame = index;
			vineAnimation();
		}
	}
	
	public void animateVineUpwards(){
		Vector3 pos = new Vector3(this.transform.position.x,this.transform.position.y + 700,this.transform.position.z);
		iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 30.0f/50.0f,
			"onComplete", "selfDestroy", "oncompletetarget", gameObject,"oncompleteparams",this.gameObject, "easetype", iTween.EaseType.linear));
	}
	
	void vineAnimation(){
		
		if(vineFrameSeq == null || vineFrameSeq.Count < 0)
				createAnimations();
			
			setCurrentFrame((int)vineFrameSeq[0]);
			vineFrameSeq.RemoveAt(0);
			if(vineFrameSeq.Count == 0) {
				createAnimations();
			}
	}
	void createAnimations(){
		
		vineFrameSeq = new ArrayList();
		for(int i = 0; i< 20; i++) {
			vineFrameSeq.Add(i);
		}
		for(int i = 19; i> 0; i--) {
			vineFrameSeq.Add(i);
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
