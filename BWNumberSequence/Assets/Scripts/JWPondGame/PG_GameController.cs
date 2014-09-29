using UnityEngine;
using System.Collections;
 

	


public class PG_GameController : MonoBehaviour {
	
	ArrayList answerList ;
	int totalAttempts=0, totalWrongAttempts=0, totalCorrects=0 , questionAttempts , questionCorrects , minQuestionReqToCalResult , answerSpriteCount , questionSpriteCount ,
		gameMode, dragFingerIndex=-1, shapesTappedIndex=-1 , jumpCount=0, questionPlayed=0 , hardModeTime=0, _hardModeCount=0 , _background=0, countModeShapeNumber=0;
	bool isRightShapeDragged=false	, isFingerTap=true , isUserPlayedTheQuestion=false , loadingQuestionOrLevelInProgress=false;
	GameObject answerShapeDrag=null	, icky=null , olly=null , referent_bubble=null , referent_shape=null , referent_hint=null , referent_shapeWithHint ,background_object=null , 
	land=null , olly_referent_Container, cross_tick_sign ;  // referent_shapeWithHint :=> question mark etc
	
	public GameObject GO_countingNumber;
	float scaleFactor=0F;
	public TextAsset plist=null;
	private string[] ollyAnimationSpriteSheetNames;
	public bool enableTouchesFlag=false , shapesReachedPlayVO=false;
	string[] questionInstructionVO;
	
	GameModes currentMode;
	
		// ----- Skill ID
	enum SkillID{
		skill_1=0
	};
	
	// ----- Game Modes
	enum GameModes{
		mode_same=0,
		mode_similiar=1,
		mode_counting=2,
		mode_different=3
	};
	
	// --- Check either to load next question or next level based on result
	enum nextLevelDecision{
		loadLevel,
		loadQuestion
	};
	 


	// ------ Script refrences to controll other game controlls
	PG_ShapesController 						shapesControllerSR;
	PG_IslandController 						islandControllerSR;
	PG_ShapeStatController 						shapeStatController;
	PG_ShapeContainerStatController 			shapeContainerStatControllerSR;
	PG_OllyController							ollyReferentControllerSR;
	PG_IckyController							ickyControllerSR;
	PG_ReferentBubble_Controller				referentBubbleControllerSR;
	PG_Referent_ShapeHint						referentHintControllerSR;
	PG_ReferentShape_Controller					referentShapeControllerSR;
	PG_SoundManager 							soundManager;
	PG_BackgroundController						backgroundControllerSR;
	PG_LandController							landControllerSR;
	PG_OllyAnimations							ollyAnimationsSR;
	PG_IckyAnimations							ickyAnimationsSR;
	JW_DataManager pgDataManager;
	PG_CountingPopUpController                  countingPOPUPController;
	
	
	
	void Awake(){
	}
	

	// Use this for initialization
	void Start ()
	{
		pgDataManager = new JW_DataManager (AGGameIndex.k_ShapePond,(int)SkillID.skill_1,plist.text); // Here 1 is the game Index, O is the first skill in available skills
		
		//----------- Scale Factor
		float screenResolution=Screen.currentResolution.width;
		scaleFactor=screenResolution/PG_Constants.PG_SCREEN_WIDTH;
		
		//-------- Refrences of the Objects controlled by GameController
		GameObject shapesRef	=	GameObject.FindGameObjectWithTag("pg_shapes_container");
		GameObject islandRef	=	GameObject.FindGameObjectWithTag("pg_island");
		 		   olly			=	GameObject.FindGameObjectWithTag("pg_olly");
				   icky			=	GameObject.FindGameObjectWithTag("pg_icky");
	        referent_bubble		=	GameObject.FindGameObjectWithTag("pg_referent_bubble");
		    referent_shape		=	GameObject.FindGameObjectWithTag("pg_referent_shape");
		    referent_hint		=	GameObject.FindGameObjectWithTag("pg_referent_hint");
	referent_shapeWithHint		=	GameObject.FindGameObjectWithTag("pg_referent_shapeWithHint");
		 background_object		=	GameObject.Find("Backgrounds");
					  land		=	GameObject.Find("Land");
 	olly_referent_Container		=	GameObject.Find("OllyReferentContainer");
			cross_tick_sign		=	GameObject.Find("cross_tick_sign");
		//  --------- Initialize script refrences 
		if(shapesRef!=null)
		shapesControllerSR				=	shapesRef.GetComponent<PG_ShapesController>();
		
		if(shapesRef!=null)
		soundManager					=	shapesRef.GetComponent<PG_SoundManager>();	
		
		if(islandRef!=null)
		islandControllerSR				=	islandRef.GetComponent<PG_IslandController>();
		
		if(shapesRef!=null)
		shapeContainerStatControllerSR	=	shapesRef.GetComponent<PG_ShapeContainerStatController>();
		
		if(olly_referent_Container!=null)
		ollyReferentControllerSR				=	olly_referent_Container.GetComponent<PG_OllyController>();
		//else Debug.Log("ollyReferentControllerSR is NULL!");
		
		if(olly!=null)
		ollyAnimationsSR				=	olly.GetComponent<PG_OllyAnimations>();

		if(icky!=null)
		ickyControllerSR				=	icky.GetComponent<PG_IckyController>();
		
		if(icky!=null)
		ickyAnimationsSR				=	icky.GetComponent<PG_IckyAnimations>();
		
		
		if(referent_bubble!=null)
		referentBubbleControllerSR		=	referent_bubble.GetComponent<PG_ReferentBubble_Controller>();
		
		if(referent_shape!=null)
		referentShapeControllerSR		=	referent_shape.GetComponent<PG_ReferentShape_Controller>();
		
		if(referent_hint!=null)
		referentHintControllerSR		=	referent_hint.GetComponent<PG_Referent_ShapeHint>();
		
		if(background_object!=null)
		backgroundControllerSR			=	background_object.GetComponent<PG_BackgroundController>();
		
		if(land!=null)
		landControllerSR				=	land.GetComponent<PG_LandController>();
		//else Debug.Log("Land Null");
		
		
		// ----- 
		loadLevel();
		cross_tick_sign.transform.renderer.enabled=false;
		enableTouches();
		// ----- Repeat question if user didn't played for 10 sec
		// InvokeRepeating("playQuestionInstructionVO",6, 10F);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(totalWrongAttempts==5){
			 StartCoroutine(hideReferentBubble(0F));
			 StartCoroutine(ResetLevel(0.5F,nextLevelDecision.loadQuestion)); // load next question
		}
		
		if(shapesReachedPlayVO)
		 StartCoroutine(playQuestionInstructionVO(7F));
		
	}
	
	
	// --------- Finger taps
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount ){
		 GameObject selection=PickObject(fingerPos);
		if(!enableTouchesFlag || selection==null )
			return;
	//	Debug.Log("in finger tap...");
		
		
		if(selection.name=="pg_olly" && jumpCount==0){
			int _tickle = Random.Range(1,4);
			soundManager.currentClips=new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath + PG_Constants._tickle + string.Format("olly_tap_{0}" , _tickle)});
			
			if(ollyAnimationsSR.ollyState != ollyAnimationStates.olly_tickle) {
				PG_OllyAnimations.changeTexture = true;	
			}
			ollyAnimationsSR.ollyState      = ollyAnimationStates.olly_tickle;
		   	return;
		}else if(selection.name=="bubble_referent" || selection.name=="cross_tick_sign" || selection.name=="referent_hint" || selection.name=="referent_shape"){
			 soundManager.currentClips=new System.Collections.Generic.List<UnityEngine.AudioClip>();
			 soundManager.playInstructionSound(questionInstructionVO);

		}			
		else if(selection.name=="pg_icky"){
			int _tickle = Random.Range(1,5);
			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath + PG_Constants._tickle + string.Format("icky_tickle_{0}" , _tickle)});
			
			if(ickyAnimationsSR.ickyState != ickyAnimStates.icky_tickle) {
				PG_IckyAnimations.changeTexture = true;	
			}
			ickyAnimationsSR.ickyState      = ickyAnimStates.icky_tickle;
		   	return;
		}	
		
		if (selection!=null){
			shapeStatController=selection.GetComponent<PG_ShapeStatController>();
			//StopAllCoroutines();
		}
		if(shapeStatController!=null && shapeStatController.getAnswerShape() && dragFingerIndex==-1){	
			dragFingerIndex=fingerIndex;
			decideForTappedDraggedShape(selection);
			
			if((int)currentMode == (int)GameModes.mode_counting){
			countModeShapeNumber++;
			GameObject countingNumber = GameObject.Instantiate(GO_countingNumber) as GameObject;
				countingNumber.transform.parent = camera.transform;
			if(countingNumber!=null)
			countingPOPUPController 	= countingNumber.GetComponent<PG_CountingPopUpController>();
			if(countingPOPUPController!=null){
					countingPOPUPController.showCountingNumber(countModeShapeNumber);
				}	
			string	shapeClipName = null;
			string shapeName	  = pgDataManager.getQuestionTitle();
			if(countModeShapeNumber==1)
			shapeClipName = "word_"+ shapeName.ToLower();
			else shapeClipName = "word_"+shapeName.ToLower()+"s";
			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath+ PG_Constants._counting + string.Format("number_{0}",countModeShapeNumber),
			PG_Constants._soundclipPath+ PG_Constants._basicShapes + shapeClipName});
			
			}
			else{
				soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
				soundManager.playSucess();	
			}
			//StartCoroutine(playQuestionInstructionVO(10F));
			
		}
		else if(shapeStatController!=null && !shapeStatController.getAnswerShape() && dragFingerIndex==-1){
			
					shapeStatController.shapeWrongTapAnimation();  // start wrong animation with VO
					totalWrongAttempts++;
					//StartCoroutine(playQuestionInstructionVO(5F));
				   if(ollyAnimationsSR.ollyState!=ollyAnimationStates.olly_wrongAnswer){
					PG_OllyAnimations.changeTexture = true;
					ollyAnimationsSR.ollyLastStat = ollyAnimationsSR.ollyState;
					}
					ollyAnimationsSR.ollyState = ollyAnimationStates.olly_wrongAnswer;
					soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
					soundManager.playWrongSound();
			        cross_tick_sign.transform.renderer.material.mainTexture=Resources.Load(PG_Constants._wrongTick) as Texture; 
			        cross_tick_sign.transform.renderer.enabled=true;
					StartCoroutine(unhideCrossSign(0.5F));
					if(dragFingerIndex==fingerIndex && isRightShapeDragged && answerShapeDrag !=null){
					decideForTappedDraggedShape(answerShapeDrag);
						
		}
			
			
//				shapeStatController.shapeWrongTapAnimation();  // start wrong animation with VO
//			    totalWrongAttempts++;
//				//StartCoroutine(playQuestionInstructionVO(5F));
//			 if(ollyAnimationsSR.ollyState!=ollyAnimationStates.olly_wrongAnswer){
//				PG_OllyAnimations.changeTexture = true;
//				ollyAnimationsSR.ollyLastStat = ollyAnimationsSR.ollyState;
//				}
//			ollyAnimationsSR.ollyState = ollyAnimationStates.olly_wrongAnswer;
//			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
//			soundManager.playWrongSound();
//			cross_tick_sign.transform.renderer.material.mainTexture=Resources.Load(PG_Constants._wrongTick) as Texture; 
//			cross_tick_sign.transform.renderer.enabled=true;
//			//StartCoroutine(unhideCrossSign(0.5F));
		}
		
	}
	

	// ------ Finger drag begin
    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos ){

		GameObject selection = PickObject(fingerPos);
		
		if(!enableTouchesFlag || selection==null )
			return;
		Debug.Log("in drag begin...");
		
		if (selection!=null){
			shapeStatController=selection.GetComponent<PG_ShapeStatController>();
			isUserPlayedTheQuestion = true; 
			StopAllCoroutines();	
		}
		if(shapeStatController!=null && shapeStatController.getAnswerShape() && dragFingerIndex==-1){	
				dragFingerIndex=fingerIndex;
				answerShapeDrag=selection;
				isRightShapeDragged=true;
				hideShadow(selection);
			
			if((int)currentMode == (int)GameModes.mode_counting){
			countModeShapeNumber++;
			GameObject countingNumber = GameObject.Instantiate(GO_countingNumber) as GameObject;
				countingNumber.transform.parent = camera.transform;
			if(countingNumber!=null)
			countingPOPUPController 	= countingNumber.GetComponent<PG_CountingPopUpController>();
			if(countingPOPUPController!=null){
			   countingPOPUPController.showCountingNumber(countModeShapeNumber);
			}	
			string	shapeClipName = null;
			string shapeName	  = pgDataManager.getQuestionTitle();
			if(countModeShapeNumber==1)
			shapeClipName = "word_"+ shapeName.ToLower();
			else shapeClipName = "word_"+shapeName.ToLower()+"s";
			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath+ PG_Constants._counting + string.Format("number_{0}",countModeShapeNumber),
			PG_Constants._soundclipPath+ PG_Constants._basicShapes + shapeClipName});
			
			}
			else{
				soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
				soundManager.playSucess();	
			}
			StartCoroutine(playQuestionInstructionVO(5F));			
			
		} 
		else if(shapeStatController!=null && !shapeStatController.getAnswerShape() && dragFingerIndex==-1){
					shapeStatController.shapeWrongTapAnimation();  // start wrong animation with VO
					PG_OllyAnimations.changeTexture = true;
					totalWrongAttempts++;
					StartCoroutine(playQuestionInstructionVO(5F));
				   if(ollyAnimationsSR.ollyState!=ollyAnimationStates.olly_wrongAnswer){
					ollyAnimationsSR.ollyLastStat = ollyAnimationsSR.ollyState;
					}
					ollyAnimationsSR.ollyState = ollyAnimationStates.olly_wrongAnswer;
					soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
					soundManager.playWrongSound();
			cross_tick_sign.transform.renderer.material.mainTexture=Resources.Load(PG_Constants._wrongTick) as Texture; 
			        cross_tick_sign.transform.renderer.enabled=true;
					StartCoroutine(unhideCrossSign(0.5F));
		if(dragFingerIndex==fingerIndex && isRightShapeDragged && answerShapeDrag !=null){
				decideForTappedDraggedShape(answerShapeDrag);
						
		}
	}
		
  }
	
		IEnumerator unhideCrossSign(float time){
		yield return new WaitForSeconds(time);
		cross_tick_sign.transform.renderer.enabled=false;
	}
		
	
	// ----- Hide shadow and quick sand
	void hideShadow(GameObject selection){ 
		    GameObject parent = selection.transform.parent.gameObject;
			if(parent!=null){	
			Transform [] childs = parent.GetComponentsInChildren<Transform>();
				if(childs.Length>=4){
				Transform quickSandObject  = childs[2];
				Transform shadowObject     = childs[3];
		
				GameObject shadow    = shadowObject.gameObject;
				GameObject quickSand = quickSandObject.gameObject;
				if(shadow!=null){
		 		PG_ShapeShadowController shadowController = shadow.GetComponent<PG_ShapeShadowController>();
					shadowController.hideShadow();
				}
				if(quickSand!=null)
				quickSand.SetActive(false);
	}
	}}

	
	void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta ){
		
		if(!enableTouchesFlag)
			return;
		Debug.Log("in finger move...");
		if(dragFingerIndex==fingerIndex && isRightShapeDragged && answerShapeDrag!=null){

				Vector3 pos=answerShapeDrag.transform.localPosition;
					pos.x+=(delta.x / scaleFactor);
					pos.y+=(delta.y / scaleFactor);
			        pos.z=-100F;
					answerShapeDrag.transform.localPosition=pos;
			}
	}
	
	
   	void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos ){
		if(!enableTouchesFlag)
			return;
		Debug.Log("in finger ended...");
		isUserPlayedTheQuestion=false;
		if(dragFingerIndex==fingerIndex && isRightShapeDragged && answerShapeDrag !=null){
			decideForTappedDraggedShape(answerShapeDrag);
//			Debug.Log("Drag ended ...");
		}
		
	}
	
	
	//------------------------------------------------
	private GameObject PickObject( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );
        RaycastHit hit;
	 if( Physics.Raycast( ray, out hit ) )
         return hit.collider.gameObject;
		 return null;
    }
	
	// Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
    private Vector3 GetWorldPos( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );
	    float t = -ray.origin.z / ray.direction.z;  // we solve for intersection with z = 0 plane
		return ray.GetPoint( t );
    }
	

	//------------------------------------------------
	private void setDataUsingScriptRefrences(){
		//Debug.Log("in setDataUsingScriptRefrences");
		
		_background = Random.Range(1,3); // pick randomly background
		//-------- Place Available Shapes
		shapesControllerSR.placeAvailableShapes(pgDataManager.getAvailableShapes(),pgDataManager.getQuestionTitle());
	    
		//-------- Place Available Landmasses
		islandControllerSR.placeIslandsAndLandmasses(shapesControllerSR.getAnswerSpriteIndex(),pgDataManager.getAnswersListCount(),_background);
		
		// -------------- Show/Hide Land
		if(_background==1){
			if(landControllerSR!=null)
			landControllerSR.showLand();
			else Debug.Log("land is null");
		}
		else if(landControllerSR!=null){
			landControllerSR.hideLand();
			
		}else Debug.Log("land is null");
		
		// ------ Change background
		backgroundControllerSR.setBackground(_background);
		
		if(pgDataManager.getShowReferent())
		referentShapeControllerSR.setReferentSprite(shapesControllerSR.getAnswerSpriteIndex());  // Setting Referent Sprite
		else referentShapeControllerSR.setReferentSprite(500);  // Setting 
		
		referentHintControllerSR.setShapeName(shapesControllerSR.getAnswerSpriteIndex());
			
		// ------- Play VO w.r.t game mode
		if((int)pgDataManager.gameModeSwitching()==(int)GameModes.mode_different)
		{	
			currentMode = GameModes.mode_different;
			int _different = Random.Range(1,3);
			questionInstructionVO =(new string[]{PG_Constants._soundclipPath + PG_Constants._differentMode + string.Format("all_diff_todo_{0}" , _different)});
			
		}
		else if((int)pgDataManager.gameModeSwitching()==(int)GameModes.mode_same || (int)pgDataManager.gameModeSwitching()==(int)GameModes.mode_similiar)
		{
			currentMode			 =  GameModes.mode_same;
			int shapesCount      = pgDataManager.getAnswersListCount();
			string shapeName	 = pgDataManager.getQuestionTitle();
			string shapeClipName = null;
			if(shapesCount==1)
			shapeClipName = "word_"+ shapeName.ToLower();
			else shapeClipName = "word_"+shapeName.ToLower()+"s";
			
			questionInstructionVO= (new string[]{PG_Constants._soundclipPath + PG_Constants._find + "all_same_todo_one_2", PG_Constants._soundclipPath+ PG_Constants._basicShapes + shapeClipName});	
			
		}
		else if((int)pgDataManager.gameModeSwitching()==(int)GameModes.mode_counting){
			currentMode			 = GameModes.mode_counting;
			int shapesCount      = pgDataManager.getAnswersListCount();
			string shapeName	 = pgDataManager.getQuestionTitle();
			string shapeClipName = null;
			if(shapesCount==1)
			shapeClipName = "word_"+ shapeName.ToLower();
			else shapeClipName = "word_"+shapeName.ToLower()+"s";
			
			questionInstructionVO =(new string[]{PG_Constants._soundclipPath + PG_Constants._find + "pond_count_todo_1a" , PG_Constants._soundclipPath+ PG_Constants._counting + string.Format("number_{0}",shapesCount),
			PG_Constants._soundclipPath+ PG_Constants._basicShapes + shapeClipName});
			
		}
		
		isUserPlayedTheQuestion = false;
		loadingQuestionOrLevelInProgress = false;
		shapeContainerStatControllerSR.playAnimation();
		
		// ------- Hard mode Implementation
		if(hardModeTime>0){
			hardModeTime +=4;
			_hardModeCount=0;
		}
		
	   }
	
	// ----- Show quick sand
 IEnumerator  showQuickSand(float delay){
		yield return new WaitForSeconds(delay);
		
		if(_background==1){
			_hardModeCount++;
			shapesControllerSR.showQuickSand(_hardModeCount);
		}
		else{
				_hardModeCount++;
				shapesControllerSR.showQuickSand(_hardModeCount+4);
		}
		
		
		
		if(_hardModeCount>=4){
			enableTouchesFlag=false;
			ollyAnimationsSR.ollyState 			= ollyAnimationStates.olly_weap;
			PG_OllyAnimations.changeTexture	    = true;
			
			ickyAnimationsSR.ickyState     		= ickyAnimStates.icky_weap;
			PG_IckyAnimations.changeTexture		= true;
			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath + PG_Constants._crying});
			StopAllCoroutines();
			//loadLevelOrQuestion();
		}
		else{
			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath + PG_Constants._quickSandHurry + "quicksand_feedback_1" , PG_Constants._soundclipPath+ PG_Constants._quickSandHurry + "exclaim_better_hurry"});
			StartCoroutine(showQuickSand(hardModeTime));
		}
		
	}
	
	
	
	IEnumerator playQuestionInstructionVO(float delay){
		shapesReachedPlayVO =false;
		//Debug.Log("In VO");
		yield return new WaitForSeconds(delay);
		if(!isUserPlayedTheQuestion && !loadingQuestionOrLevelInProgress){
			soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
			soundManager.playInstructionSound(questionInstructionVO);
		}
		
		if(hardModeTime>0)
		  StartCoroutine(showQuickSand(hardModeTime));
		else StartCoroutine(playQuestionInstructionVO(10F));
			
		
	}
	
	//------------------------------------------------
	private void loadLevel()
	{
		Debug.Log("Next level loaded...");
		PG_OllyAnimations.changeTexture =true;
		ollyAnimationsSR.isAnimating=true;
		ollyAnimationsSR.ollyState=ollyAnimationStates.olly_idle1;
		
		totalAttempts=0;
		totalWrongAttempts=0;
		totalCorrects=0;
		jumpCount =0;
		countModeShapeNumber=0;
		shapesTappedIndex=-1;
		pgDataManager.fetchLevelData();
		showReferentBubble();
		pgDataManager.PG_generateQuestion();
		setIckyOllyInitialPosition();
		
		
		ickyAnimationsSR.isAnimating = true;
		ickyAnimationsSR.ickyState   = ickyAnimStates.icky_idle1;
		hardModeTime = pgDataManager.getHardModeTime();
		setDataUsingScriptRefrences();
		PG_OllyAnimations.answerShapesCount=pgDataManager.getAnswersListCount();
	}
	
	//-------- Load next question of current level
	private void loadQuestion()
	{
		Debug.Log("Next question loaded...");
		PG_OllyAnimations.changeTexture =true;
		ollyAnimationsSR.isAnimating=true;
		ollyAnimationsSR.ollyState=ollyAnimationStates.olly_idle1;
	
		totalAttempts=0;
		totalWrongAttempts=0;
		totalCorrects=0;
		jumpCount    =0;
		countModeShapeNumber=0;
		shapesTappedIndex=-1;
		pgDataManager.PG_generateQuestion();
		setIckyOllyInitialPosition();
		showReferentBubble();
		
		ickyAnimationsSR.isAnimating = true;
		ickyAnimationsSR.ickyState   = ickyAnimStates.icky_idle1;
		hardModeTime = pgDataManager.getHardModeTime();
		setDataUsingScriptRefrences();
		PG_OllyAnimations.answerShapesCount=pgDataManager.getAnswersListCount();
		//enableTouchesFlag = true;
	}
	
	
	// ----- Decide if tapped/dragged shape is correct?, play respective animations and decide to load next question or next level?
	
	private void decideForTappedDraggedShape( GameObject selection)
	{
		//Debug.Log("Decide for dragged shape");
		ArrayList responseList=new ArrayList();
		bool allShapesOnCurrentScreenPlayed;

		
		if(selection!=null)
		 {
			//isUserPlayedTheQuestion = true;
			totalAttempts++;
			shapeStatController=selection.GetComponent<PG_ShapeStatController>();
			
			if(shapeStatController!=null && shapeStatController.getAnswerShape())
			 {
				hideShadow(selection);
				jumpCount++;
			if(jumpCount==1)
				shapeStatController.isFirstShape = true;
			else shapeStatController.isFirstShape = false;
				cross_tick_sign.transform.renderer.material.mainTexture=Resources.Load(PG_Constants._correctTick) as Texture; 
				cross_tick_sign.transform.renderer.enabled=true;
				enableTouchesFlag = false;

				totalCorrects++;
			    shapesTappedIndex++;
				if(gameMode==(int)GameModes.mode_counting)
					responseList=islandControllerSR.ReplaceEmptyLandMassesWithFilledOne(shapesControllerSR.getQSpriteIndices().Count);
				else
					responseList=islandControllerSR.ReplaceEmptyLandMassesWithFilledOne(pgDataManager.getAnswersListCount());	
				 
				if(responseList.Count>=2)
				allShapesOnCurrentScreenPlayed=(bool)responseList[1];
				else allShapesOnCurrentScreenPlayed=false;
				//shapesControllerSR.shapeCorrectTapAnimation(selection,(GameObject)responseList[0]);
				
				shapeStatController.isLastShape = allShapesOnCurrentScreenPlayed;
				shapeStatController.shapeCorrectTapAnimation((GameObject)responseList[0]);
				

				StartCoroutine(hideReferentBubble(0.2F));
				if(allShapesOnCurrentScreenPlayed){
				 AGGameState.incrementStarCount(); // increment stars
				 enableTouchesFlag = false;
				 StartCoroutine(ollyPondCrossAnimation(1.8F));	
						
				}
						
			}
			

    }
  
	isUserPlayedTheQuestion =false;	
	}
	
	public void loadLevelOrQuestion(){
		
		 questionPlayed++;
		 isUserPlayedTheQuestion=true;
		 totalWrongAttempts=0;
		loadingQuestionOrLevelInProgress=true;
		Debug.Log("Total Attempts: "+totalAttempts +" Total Corrects: "+totalCorrects);
		bool isleveldone = false;
		if(totalAttempts>0)
		 isleveldone = pgDataManager.calculateResult(totalAttempts,totalCorrects);
		if(questionPlayed>=5){
			AGGameState.loadNextGameInLoop(AGGameIndex.k_ShapePond);  // to play in loop
			return;
		}
		 if (isleveldone){     // Check result
			StartCoroutine(ResetLevel(0.5F,nextLevelDecision.loadLevel));    // load new level
		}
		else 
		    StartCoroutine(ResetLevel(0.5F,nextLevelDecision.loadQuestion)); // load next question
		
		
	}
	
	// Recreate the Scene Environment for the next question
	private IEnumerator ResetLevel(float waitTime,nextLevelDecision dec)  // Time to delay is based on the Olly animation time
	{
	    soundManager.stopAudioList();
		PG_SoundManager.playIntroClip =false;
		enableTouchesFlag = false;
		yield return new WaitForSeconds(waitTime);
			
			removeAllPlatforms();
			removeAllLandmasses();
		    this.StopAllCoroutines();
			removeALLIslands();
			removeAllShapes();
		
		if(dec==nextLevelDecision.loadQuestion)
				loadQuestion();
		else if(dec==nextLevelDecision.loadLevel)
			    loadLevel();
	}
	
	
	//--------Remove all shapes
	private void removeAllShapes()
	{
		GameObject shapesContainer_1=GameObject.Find("Shapes Container 1");
		GameObject shapesContainer_2=GameObject.Find("Shapes Container 2");
		
		foreach(Transform child in shapesContainer_1.transform)
				GameObject.Destroy(child.gameObject);
		
		foreach(Transform child in shapesContainer_2.transform)
				GameObject.Destroy(child.gameObject);
		
		// ----place shapes containers to their actual position
		shapesContainer_1.transform.position=new Vector3(0F,0F,0F);
		shapesContainer_2.transform.position=new Vector3(0F,0F,0F);
	}
	
	
	//----------Remove all platforms
	private void removeAllPlatforms()
	{
		GameObject platformContainer=GameObject.Find("Shapes Container 1");
	}
	
	
	//----------Remove all landmasses
	private void removeAllLandmasses()
	{
		GameObject landmassContainer=GameObject.Find("Land Mass Container");
		foreach(Transform child in landmassContainer.transform)
				GameObject.Destroy(child.gameObject);
		
		// ----place landmass containers to their actual position
		landmassContainer.transform.position=new Vector3(0F,76.83932F,0F);
	}
	
		
	//------------- Remove all islands
	private void removeALLIslands()
	{
		GameObject islandsContainer=GameObject.Find("Island");
		foreach(Transform child in islandsContainer.transform)
		{
		  GameObject childObject=child.gameObject;
		  if(!((string)childObject.name=="Land Mass Container" || (string)childObject.name=="olly_icky_referrent_holder"))
		  			GameObject.Destroy(childObject);
		}
	}
	
	
	
	
	// ---------- Set icly, olly initial position
	private void setIckyOllyInitialPosition()
	{
		int answerShapeCount = pgDataManager.getAnswersListCount();
		Vector3 ollyPosition , ickyPosition;
		float 	ollyPosX=0    , ollyPosY=0 , ollyPosZ=0 ;
		float 	ickyPosX=0    , ickyPosY=0 , ickyPosZ=0;
		
		if(answerShapeCount == 1)
		{
			ollyPosX	= (float) PG_Constants.ISLAND_ONE_SHAPE.ollyPositionX;
			ollyPosY	= (float) PG_Constants.ISLAND_ONE_SHAPE.ollyPositionY;
			ollyPosZ	= (float) PG_Constants.ISLAND_ONE_SHAPE.ollyPositionZ;
			
			ickyPosX	= (float) PG_Constants.ISLAND_ONE_SHAPE.ickyPositionX;
			ickyPosY	= (float) PG_Constants.ISLAND_ONE_SHAPE.ickyPositionY;
			ickyPosZ	= (float) PG_Constants.ISLAND_ONE_SHAPE.ickyPositionZ;
		}
		else if(answerShapeCount == 2)
		{
			ollyPosX	= (float) PG_Constants.ISLAND_TWO_SHAPES.ollyPositionX;
			ollyPosY	= (float) PG_Constants.ISLAND_TWO_SHAPES.ollyPositionY;
			ollyPosZ	= (float) PG_Constants.ISLAND_TWO_SHAPES.ollyPositionZ;
			
			ickyPosX	= (float) PG_Constants.ISLAND_TWO_SHAPES.ickyPositionX;
			ickyPosY	= (float) PG_Constants.ISLAND_TWO_SHAPES.ickyPositionY;
			ickyPosZ	= (float) PG_Constants.ISLAND_TWO_SHAPES.ickyPositionZ;
		}
		else if(answerShapeCount == 3)
		{
			ollyPosX	= (float) PG_Constants.ISLAND_THREE_SHAPES.ollyPositionX;
			ollyPosY	= (float) PG_Constants.ISLAND_THREE_SHAPES.ollyPositionY;
			ollyPosZ	= (float) PG_Constants.ISLAND_THREE_SHAPES.ollyPositionZ;
			
			ickyPosX	= (float) PG_Constants.ISLAND_THREE_SHAPES.ickyPositionX;
			ickyPosY	= (float) PG_Constants.ISLAND_THREE_SHAPES.ickyPositionY;
			ickyPosZ	= (float) PG_Constants.ISLAND_THREE_SHAPES.ickyPositionZ;
		}
		else if(answerShapeCount == 4)
		{
			ollyPosX	= (float) PG_Constants.ISLAND_FOUR_SHAPES.ollyPositionX;
			ollyPosY	= (float) PG_Constants.ISLAND_FOUR_SHAPES.ollyPositionY;
			ollyPosZ	= (float) PG_Constants.ISLAND_FOUR_SHAPES.ollyPositionZ;
			
			ickyPosX	= (float) PG_Constants.ISLAND_FOUR_SHAPES.ickyPositionX;
			ickyPosY	= (float) PG_Constants.ISLAND_FOUR_SHAPES.ickyPositionY;
			ickyPosZ	= (float) PG_Constants.ISLAND_FOUR_SHAPES.ickyPositionZ;
		}
		
		ollyPosition = new Vector3 (ollyPosX , ollyPosY , ollyPosZ);
		ickyPosition = new Vector3 (ickyPosX , ickyPosY , ickyPosZ);
		
		
		
		if(ollyReferentControllerSR!=null){
			ollyReferentControllerSR.ollySetInitialPos(ollyPosition,olly);
			//Debug.Log("Initial Pos: " + ollyPosition);
		}
	
		
		
		if(ickyControllerSR!=null)
		ickyControllerSR.ickySetInitialPos(ickyPosition);
		
	}
	
	
	// ------------- Hide referrent bubble
	IEnumerator hideReferentBubble(float time)
	{
		yield return new WaitForSeconds(time);
		if(referentBubbleControllerSR!=null)
		  referentBubbleControllerSR.hideReferentBubble();
		
		if(referentHintControllerSR!=null)
		   referentHintControllerSR.hideHint();
		
		if(referentShapeControllerSR!=null)
		   referentShapeControllerSR.hideReferentShape();
		cross_tick_sign.transform.renderer.enabled=false;
	}
	
	// ------------- Show referrent bubbble
	public void showReferentBubble()
	{
		if(referentBubbleControllerSR!=null)
		  referentBubbleControllerSR.showReferentBubble();
		
		if(referentHintControllerSR!=null)
		   referentHintControllerSR.showHint();
		
		if(referentShapeControllerSR!=null)
		   referentShapeControllerSR.showReferentShape();
	}
	
	
	// ------- Pond Cross Animaiton 
	private IEnumerator ollyPondCrossAnimation(float waitTime)
	{
		StopCoroutine("showQuickSand"); 
		loadingQuestionOrLevelInProgress=true;
		enableTouchesFlag=false;
		yield return new WaitForSeconds(waitTime);
		Vector3 pos=new Vector3(icky.transform.position.x+120 ,150F ,-36.22217F);
		
		ollyAnimationsSR.isAnimating  = false;
		//ollyAnimationsSR.currentFrame = 0.0F;
		PG_OllyAnimations.changeTexture=true;
		ollyAnimationsSR.ollyState    = ollyAnimationStates.olly_jumpWaterToLand;
		
		
		//ickyAnimationsSR.isAnimating  = false;
		//ickyAnimationsSR.currentFrame = 0.0F ;
		PG_IckyAnimations.changeTexture = true;
		ickyAnimationsSR.ickyState    	= ickyAnimStates.icky_celebration;
		iTween.MoveTo(olly_referent_Container , iTween.Hash("position" , pos , "time" , 0.4 , "delay" , 0.4));
		
		//iTween.MoveTo(olly_referent_Container,pos,0.4F);
		
		
	}
	
	public void setFingerIndexOnAnimationComplete(){
		
		//Debug.Log("Setting Finger Index ....");
		dragFingerIndex=-1;
		isRightShapeDragged=false;
	}
		
	
	
	public void enableTouches(){
		// --------- Initialize finger gestures
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
	    FingerGestures.OnFingerDragMove += FingerGestures_OnFingerDragMove;
    	FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd;
	}
	
  public void disableTouches(){
		// --------- Initialize finger gestures
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
	    FingerGestures.OnFingerDragMove -= FingerGestures_OnFingerDragMove;
    	FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd;
	
	}
	
	public void playCelebrationVoiceOvers(){
		soundManager.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
		soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath + PG_Constants._celebration + "celebration"});
	}
	
	void OnDisable(){
		disableTouches();
	}
}
