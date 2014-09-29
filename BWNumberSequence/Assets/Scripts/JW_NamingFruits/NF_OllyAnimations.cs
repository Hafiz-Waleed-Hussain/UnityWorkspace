using UnityEngine;
using System.Collections;

public enum NF_OllyStates{
	unknown,
	olly_idle,
	olly_walking,
	olly_walking2,
	olly_blinking,
	olly_handsUp,
	olly_stretch
	};

public class NF_OllyAnimations : MonoBehaviour {
	
	public NF_OllyStates ollyStates;
	private ArrayList ollyPositions = new ArrayList();
	//vars for the whole sheet
	public Texture2D idleSheet;
	public Texture2D walkingSheet;
	public Texture2D blinkingSheet;
	public Texture2D stretchSheet;
	public Texture2D handsUpSheet;
	public Texture2D ollyImage;
	
	ArrayList walkingFrames;
	ArrayList blinkingFrames;
	ArrayList stretchFrames;
	ArrayList handsUpFrames;
	public int colCount =  0;
	public int rowCount =  0;
	 
	//vars for animation
	public int  rowNumber  =  0; //Zero Indexed
	public int colNumber = 0; //Zero Indexed
	public int totalCells = 0;
	public int  fps     = 0;
	bool bLoop = false;
	int currentFrame = 0;
	//Maybe this should be a private var
    private Vector2 offset;
	
	void Start(){
		currentFrame = 0;
		fps =30;
	}
	void Update (){
		
		int index  = (int)(Time.time * fps);
		if(index > currentFrame) {
			currentFrame = index;
			ollyIntroAnimation();
		}
	}
	void resetRotation(){
		
			Quaternion myRotation = this.transform.rotation;
			myRotation.eulerAngles = new Vector3(0,0,0);
			this.transform.rotation = myRotation;
	}
	public void ollyIntroAnimation(){
		
		if(ollyStates == NF_OllyStates.unknown) {
			
			renderer.material.mainTexture = ollyImage;
			transform.localScale = new Vector3 (150, 280, 1);
			resetRotation();
			colCount = 1;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 1;
			fps = 30;
			setCurrentFrame(0);
			
		}else if(ollyStates == NF_OllyStates.olly_idle) {
			
			renderer.material.mainTexture = idleSheet;
			transform.localScale = new Vector3 (150, 280, 1);
			
			colCount = 4;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 4;
			fps = 10;
			
		}else if(ollyStates == NF_OllyStates.olly_walking){
			
			renderer.material.mainTexture =walkingSheet;
			transform.localScale = new Vector3(170,250,1);
			resetRotation();
			colCount = 6;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 18;
			fps = 30;
			
			if(walkingFrames == null || walkingFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)walkingFrames[0]);
			walkingFrames.RemoveAt(0);
			if(walkingFrames.Count == 0) {
				currentFrame=0;
				fps = 30;
				ollyStates = NF_OllyStates.olly_blinking;
				createAnimations();
			}
		}
		else if(ollyStates == NF_OllyStates.olly_walking2){
			
			renderer.material.mainTexture =walkingSheet;
			transform.localScale = new Vector3(170,250,1);
			resetRotation();
			colCount = 6;
			rowCount = 3;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 18;
			fps = 30;
			
			if(walkingFrames == null || walkingFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)walkingFrames[0]);
			walkingFrames.RemoveAt(0);
			if(walkingFrames.Count == 0) {
				currentFrame=0;
				fps = 30;
				ollyStates = NF_OllyStates.olly_handsUp;
				createAnimations();
			}
		}
		else if(ollyStates == NF_OllyStates.olly_handsUp){
			
			renderer.material.mainTexture =handsUpSheet;
			transform.localScale = new Vector3(150,280,1);
			resetRotation();
			colCount = 4;
			rowCount = 2;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 7;
			fps = 30;
			
			if(handsUpFrames== null || handsUpFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)handsUpFrames[0]);
			handsUpFrames.RemoveAt(0);
			if(handsUpFrames.Count == 0) {
				currentFrame=0;
				fps = 30;
				ollyStates = NF_OllyStates.olly_stretch;
				createAnimations();
				ollyStretchSequence(550.0f);
			}
		}
		else if(ollyStates == NF_OllyStates.olly_blinking){
			
			renderer.material.mainTexture = blinkingSheet;
			transform.localScale = new Vector3(150,280,1);
			resetRotation();
			colCount = 4;
			rowCount = 2;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 7;
			fps = 17;
			
			if(blinkingFrames == null || blinkingFrames.Count == 0)
				createAnimations();
			
			setCurrentFrame((int)blinkingFrames[0]);
			blinkingFrames.RemoveAt(0);
			if(blinkingFrames.Count == 0){
				currentFrame=0;
				fps = 30;
				ollyStates = NF_OllyStates.olly_walking2;
				createAnimations();
				olly_WalkAgain_1(150.0f);
				
			}
		} else if(ollyStates == NF_OllyStates.olly_stretch){
			
			renderer.material.mainTexture = stretchSheet;
			transform.localScale = new Vector3(160,300,1);
			resetRotation();
			colCount = 3;
			rowCount = 1;
			colNumber = 0;
			rowNumber = 0;
			totalCells = 3;
			fps =30;
			
			if(stretchFrames == null || stretchFrames.Count == 0)
				return;
			setCurrentFrame((int)stretchFrames[0]);
			stretchFrames.RemoveAt(0);
			if(stretchFrames.Count == 0) {
				currentFrame=0;
				fps = 30;
				Debug.Log("Animation Finished");
			}
		}
	}
	
	public void olly_stopAllTweensAndInvoke(){
		iTween.Stop(this.gameObject);
		CancelInvoke();
	}
	
	void loadGameControllerInitializer(Object olly){
		NF_GameController gameController = (NF_GameController)Camera.main.GetComponentInChildren<NF_GameController>();
		gameController.initializeGameController();
		destroyIntroObjects(olly);
	}
	void destroyIntroObjects(Object obj){
		Destroy((GameObject)obj);
	}
	
	void ollyStretchSequence(float yPos){
	 		
				GameObject vine = GameObject.Find("Vine");
				NF_VineAnimations vineAnim = (NF_VineAnimations) vine.GetComponent<NF_VineAnimations>();
				vineAnim.animateVineUpwards();
		
				createAnimations();
				Vector3 pos = transform.position;
				pos.y = yPos;
				currentFrame = 0;
				iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 30.0f/50.0f,
			"onComplete", "loadGameControllerInitializer", "oncompletetarget", gameObject,"oncompleteparams",this.gameObject, "easetype", iTween.EaseType.linear));
	}
	
	void switchState(NF_OllyStates state){
			ollyStates = state;
			createAnimations();	
	}
	
	void olly_WalkAgain_3(float xPos){
		
			Debug.Log("walk again 3");
				ollyStates = NF_OllyStates.olly_walking2;
				createAnimations();	
				Vector3 pos = transform.position;
				pos.x = xPos;
				currentFrame = 0;
				iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 18.0f/50.0f,"onComplete", "switchToState", "oncompletetarget", gameObject,"oncompleteparams",NF_OllyStates.olly_handsUp,"easetype", iTween.EaseType.linear));
	}
	
	void olly_WalkAgain_2(float xPos){
		
				Debug.Log("walk again 2");
				ollyStates = NF_OllyStates.olly_walking2;
				createAnimations();	
				Vector3 pos = transform.position;
				pos.x = xPos;
				currentFrame = 0;
				iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 18.0f/50.0f,"onComplete", "olly_WalkAgain_3", "oncompletetarget", gameObject,"oncompleteparams",475.0f,"easetype", iTween.EaseType.linear));
	}
	
	public void olly_WalkAgain_1(float xPos){
		
			Debug.Log("walk again 1");
			GameObject vine = GameObject.Find("Vine");
			ollyStates = NF_OllyStates.olly_walking2;
			createAnimations();	
			Vector3 pos = transform.position;
			pos.x = xPos;
			currentFrame = 0;
		
			iTween.MoveTo(gameObject, iTween.Hash("position", pos,
			"time",18.0f/50.0f, "easetype", iTween.EaseType.linear));
			iTween.RotateTo(gameObject, iTween.Hash("Quartenion",new Vector3(0,0,0),"time", 18.0f/50.0f, "onComplete", "olly_WalkAgain_2", "oncompletetarget", gameObject, "oncompleteparams", 300.0f));
		
	}
	void olly_Walk_3(float xPos){
				ollyStates = NF_OllyStates.olly_walking;
				createAnimations();	
				Vector3 pos = transform.position;
				pos.x = xPos;
				currentFrame = 0;
				iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", 25.0f/50.0f,"onComplete", "switchToState", "oncompletetarget", gameObject,"oncompleteparams",NF_OllyStates.olly_blinking,"easetype", iTween.EaseType.linear));
	}
	
	void olly_Walk_2(float xPos){
				ollyStates = NF_OllyStates.olly_walking;
				createAnimations();			
				Vector3 pos = transform.position;
				pos.x = xPos;
				currentFrame = 0;
		
			iTween.MoveTo(gameObject, iTween.Hash("position", pos,
			"time", 25.0f/50.0f, "easetype", iTween.EaseType.linear));
			iTween.RotateTo(gameObject, iTween.Hash("Quartenion",new Vector3(0,0,0),"time", 25.0f/50.0f, "onComplete", "olly_Walk_3", "oncompletetarget", gameObject, "oncompleteparams", -100.0f));
	}

	public void ollyTweenSequence(float xPos,float yPos,NF_OllyStates state){
		ollyStates = state;
		if(ollyStates == NF_OllyStates.olly_walking){
				Vector3 pos = transform.position;
				pos.x = xPos;
				currentFrame = 0;
			iTween.MoveTo(gameObject, iTween.Hash("position", pos,"delay", 0.5f,
			"time", 25.0f/50.0f, "easetype", iTween.EaseType.linear));
			iTween.RotateTo(gameObject, iTween.Hash("Quartenion",new Vector3(0,0,0),"delay", 0.5f,"time", 25.0f/50.0f, "onComplete", "olly_Walk_2", "oncompletetarget", gameObject, "oncompleteparams", -350.0f));

			}
	}
	
	public void ollyPeakingOut(){
		
		ollyPositions.Add(new Vector3(387.0f,425.0f,800.0f)); // angle 160 in z axis , scale 150 in X
		ollyPositions.Add(new Vector3(304.0f,398.0f,800.0f)); // angle 160 in z axis , //
		ollyPositions.Add(new Vector3(-283.0f,455.0f,800.0f)); // angle 205 , scale -150 in X to flip
		ollyPositions.Add(new Vector3(-512.0f,482.0f,800.0f)); // angle 205, //
		
		Quaternion myRotation = this.transform.rotation;
		int randomPosIndex = Random.Range(0,ollyPositions.Count);
		this.transform.position = (Vector3)ollyPositions[randomPosIndex];
			if(randomPosIndex==0 || randomPosIndex==1){
				myRotation.eulerAngles = new Vector3(0,0,160.0f);
				this.transform.rotation = myRotation;
				this.transform.localScale = new Vector3(150,280,1);
					}
			else{
				myRotation.eulerAngles = new Vector3(0,0,205.0f);
				this.transform.rotation = myRotation;
				this.transform.localScale = new Vector3(-150,280,1);
					}
		iTween.MoveTo(this.gameObject,new Vector3(this.transform.position.x,this.transform.position.y-170,this.transform.position.z),1.0f);
		// play animation
		InvokeRepeating("repeatIdleAnimation",0.25f,0.1f); 
		Invoke("cancelOllyAnimation",1.0f);
	}
	
	void cancelOllyAnimation(){
		
		CancelInvoke("repeatIdleAnimation");
		iTween.MoveTo(this.gameObject,iTween.Hash("position",new Vector3(this.transform.position.x,this.transform.position.y+170,this.transform.position.z),
		"time",0.5f,"onComplete", "destroyOllyModel", "oncompletetarget", gameObject, "oncompleteparams", this.gameObject, "easetype", iTween.EaseType.linear));
	}
	
	void destroyOllyModel(Object olly){
		
		Destroy((GameObject) olly);
		
	
	}

	void repeatIdleAnimation(){
		
		SetSpriteAnimation(colCount,rowCount,rowNumber,colNumber,totalCells,fps);
	}
	
	void createAnimations(){
		
		walkingFrames = new ArrayList(18);
		for(int i = 0; i< 18; i++) {
			walkingFrames.Add(i);
		}
		
		int[] blinkingOrder = {0,1,2,3,4,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,5,4,3,2,1,0};
		blinkingFrames = new ArrayList();
		for(int i = 0; i< 37; i++) {
			blinkingFrames.Add(blinkingOrder[i]);
		}
		
		stretchFrames = new ArrayList(3);
		for(int i=0;i<3;i++){
			stretchFrames.Add(i);
		}
		
		handsUpFrames = new ArrayList(7);
		for(int i=0;i<7;i++){
			handsUpFrames.Add(i);
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
	
	//SetSpriteAnimation
	void SetSpriteAnimation(int colCount ,int rowCount ,int rowNumber ,int colNumber,int totalCells,int fps ){
	 
	    // Calculate index
	    int index  = (int)(Time.time * fps);
	    // Repeat when exhausting all cells
	    index = index % totalCells;
	 
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