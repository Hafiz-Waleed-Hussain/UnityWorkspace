using UnityEngine;
using System.Collections;


public class BWFlower : MonoBehaviour {
	
	public TextMesh numberMesh;
	public int number;
	public int index;
	
	public Material negativeSheet;
	public Material positiveSheet;
	public Material hiveSheet;
	public Material mushroomSheet;
	
	public GameObject bud = null;
	
	public GameObject mushroom = null;
	
	int animState = 0; //0 idle , 1 selected, 2 pollinated
	
	int colCount = 6;
	int rowCount = 2;
	int colNumber = 0;
	int rowNumber = 0;
	int totalCells = 12;
	int fps = 30;
	bool bLoop = false;
	
	int currentFrame = 0;
	int idleFrame = 0;
	int mushroomIdle = 11;
	ArrayList selectedFrames;
	ArrayList pollinatedFrames;
	ArrayList mushroomFrames;
	
	// Use this for initialization
	void Start () {
		
	}
	
	private void idleAnimation () {
		if(number%2 == 0)
			idleDownComplete();
		else 
			idleUpComplete();
	}
	
	private void idleUpComplete () {
		iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(0, 0, (Random.Range(1.5f, 2.0f))), "time", Random.Range(1.0f, 1.5f), "onComplete", "idleDownComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	private void idleDownComplete () {
		iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(0, 0, -1.0f*(Random.Range(1.5f, 2.0f))), "time", Random.Range(1.0f, 1.5f), "onComplete", "idleUpComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	// Update is called once per frame
	void Update () {
		if(bud == null) return;
		
		int index  = (int)(Time.time * fps);
		if(index > currentFrame) {
			currentFrame = index;
			checkAnimations();
		}
	}
	
	void checkAnimations () {
		if(animState == 0) {
			setCurrentFrame(idleFrame);
			
			setMushroomFrame(mushroomIdle);
		} else if (animState == 1) { //YES
			setCurrentFrame((int)selectedFrames[0]);
			int lastFrame = (int)selectedFrames[0];
			selectedFrames.RemoveAt(0);
			if(selectedFrames.Count == 0) {
				animState = 0;
				idleFrame = lastFrame;
			}
			
			setMushroomFrame(mushroomIdle);
		} else if (animState == 2) { //NO
			setCurrentFrame((int)pollinatedFrames[0]);
			int lastFrame = (int)pollinatedFrames[0];
			pollinatedFrames.RemoveAt(0);
			
			if(pollinatedFrames.Count == 0) {
				animState = 0;
				idleFrame = lastFrame;
			}
			
			
			if(mushroomFrames == null || mushroomFrames.Count == 0) {
				setMushroomFrame(mushroomIdle);
			} else {
				setMushroomFrame((int)mushroomFrames[0]);
				mushroomFrames.RemoveAt(0);
				mushroomIdle = 10;
			}
			
			
			
			
		} else {
			setCurrentFrame(0);
		}
	}
	
	public void setFlowerNumber (int _number, int _index) {
		number = _number;
		index = _index;
		if(numberMesh != null) {
			numberMesh.text = string.Format("{0}", _number);
		}
		
		if(_number > 0) 
			bud.renderer.material = positiveSheet;
		if (_number < 0) 
			bud.renderer.material = negativeSheet;
		if (_number == 0)
			bud.renderer.material = hiveSheet;
		
		if(_number != 0) //dont play idle animation for hive
			idleAnimation();
		createAnimations();
	}
	
	public int getFlowerNumber (){
		return number;
	}
	
	public void setDisabled () {
		
		idleFrame = 0;
		animState = 0;
	}
	
	public void setSelected () {
		createAnimations();
		animState = 1;
	}
	
	public void setPollinated () {
		createAnimations();
		animState = 2;
	}
	
	public void closeFlower (int _index) {
		float delay = (float)_index * (2.0f/30.0f);
		gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.1f, 0.1f, 1.0f), "time", 6.0f/30.0f, "delay", delay, "easeType", iTween.EaseType.easeOutBack));
	}
	
	public void openFlower () {		
		float delay = (float)index * (2.0f/30.0f);
		gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(1.0f, 1.0f, 1.0f), "time", 6.0f/30.0f, "delay", delay, "easeType", iTween.EaseType.easeOutBack));
	}
	
	void createAnimations() {
		
		if(number == 0) {
			
			int [] selectedOrder = {0,0,1,1,2,2,3,3,4,4,5,5};
			selectedFrames = new ArrayList();
			for (int i = 0; i < selectedOrder.Length; i++) {
				selectedFrames.Add(selectedOrder[i]);
			}
			
			int [] pollinatedOrder = {6,6,7,7,8,8,9,9,10,10,11,11};
			pollinatedFrames = new ArrayList();
			for (int i = 0; i < pollinatedOrder.Length; i++) {
				pollinatedFrames.Add(pollinatedOrder[i]);
			}
			
			mushroomFrames = new ArrayList(11);
			for (int i = 0; i < 11; i++) {
				mushroomFrames.Add(i);
			}
			
		} else {
			
			int [] selectedOrder = {0,1,1,2};
			selectedFrames = new ArrayList();
			for (int i = 0; i < selectedOrder.Length; i++) {
				selectedFrames.Add(selectedOrder[i]);
			}
			
			int [] pollinatedOrder = {3,3,4,4,5,6,7,7,8,9,10};
			pollinatedFrames = new ArrayList();
			for (int i = 0; i < pollinatedOrder.Length; i++) {
				pollinatedFrames.Add(pollinatedOrder[i]);
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
	 
	    bud.renderer.material.SetTextureOffset ("_MainTex", offset);
	    bud.renderer.material.SetTextureScale  ("_MainTex", size);
	}
	
	void setMushroomFrame (int index) {
		
		if(mushroom == null) return;
		
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
	 
	    mushroom.renderer.material.SetTextureOffset ("_MainTex", offset);
	    mushroom.renderer.material.SetTextureScale  ("_MainTex", size);
	}
}
