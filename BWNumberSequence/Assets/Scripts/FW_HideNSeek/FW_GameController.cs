using UnityEngine;
using System.Collections;

public class FW_GameController : MonoBehaviour {

	public TextAsset gameData = null;
	Hashtable gameDataTable = null;
	
	ArrayList  allPositionsHashTable;
	ArrayList allHidingSpots = null;
	ArrayList availAbleNumberForLevel;
	public GameObject olly;
	
	public GameObject numbersContainer = null;
	Hashtable hidingObjects; 
	string ansNumber = "-100";
	
	
	int wrongAttempts=0;
	int totalAttempts=0;
	int correctAttempts=0;
	
	ArrayList numbers = null;
	string [] hidingObjectNames = new string[]{"Anchor_01" , "Anchor_Shell_01" , "anemone_01" , "anemone_Back_01" , "anemone_Back_02" , "Ben_B1" , "Ben_B2" , "Bushwave_01" , "DG_Back01" , 
											   "Gate" , "Green_Bush-01" , "Mid_Bush-01" , "Mid_Bush-02" , "Mid_cones-01" , "S_Shell_01" , "S_Shell_02" , "SH_01" , "SH_Back01" , "Side_Bush-01" , 
												"Side_Bush-02" , "Star_01" , "Star_Shell_01" , "voilet_cones-01"};
	string [] hidingObjectNamesInHierarchy = new string[]{"FWHnS-05" , "FWHnS-16" , "FWHnS-06" , "FWHnS-07" , "FWHnS-11" , "FWHnS-09" , "FWHnS-08" , "FWHnS-10" , "FWHnS-12" , "FWHnS-14" ,
											"FWHnS-17" , "FWHnS-04" ,"FWHnS-04 // to be changed" , "FWHnS-18" , "FWHnS-21" , "FWHnS-22" , "FWHnS-23" , "FWHnS-24" , "FWHnS-25" , "FWHnS-25 to be changed" , "FWHnS-26" ,
											"FWHnS-27" , "FWHnS-28"};	
	
	FW_OllyAnimations ollyAnimationSR;
	FW_SoundManager   soundManagerSR;
	// Use this for initialization
	void Start () {
		//prepareHidingObjects();
		if(olly!=null)
		ollyAnimationSR = olly.GetComponent<FW_OllyAnimations>();
		soundManagerSR  = gameObject.GetComponent<FW_SoundManager>();
		parsePlist();
		loadQuestion();
		enableFingerGestures();
		Invoke("noInteraction", FW_Constants._idleTime + 5.0f);
		
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	

	
	
	
	// -------- prepare hiding objects
	void prepareHidingObjects(){
		hidingObjects = new Hashtable();
		int i=0;
		foreach (string hideObjectName in hidingObjectNames){
			hidingObjects.Add(hideObjectName , hidingObjectNamesInHierarchy[i]);
			i++;
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
	
	 bool isNumber(GameObject selection) {
		if(selection != null && selection.transform.parent != null && selection.transform.parent.name.Equals("NumbersContainer")) {
			return true;
		}
		return false;
	}
	
	
	
	void loadQuestion() {
		wrongAttempts=0;
		ollyAnimationSR.ollyIdleFrame();
		availAbleNumberForLevel = new ArrayList();
		int maxNumberToDisplay = Random.Range(3,11);
		ArrayList _availablePositionForLevel = new ArrayList();
		Debug.Log("allPositionsHashTable.Count :"+allPositionsHashTable.Count);
	
		for(int i=0; i<allPositionsHashTable.Count ; i++)
			_availablePositionForLevel.Add(allPositionsHashTable[i]);
		
		
		
		for(int i=0 ; i<maxNumberToDisplay ;i++){
			
			int _rand = Random.Range(0,_availablePositionForLevel.Count);
			Hashtable singlePos = (Hashtable)_availablePositionForLevel[_rand];
			GameObject numberObj = Instantiate(Resources.Load("FishWorld_HideNSeek/Prefabs/Number")) as GameObject;
			numberObj.name=string.Format("{0}",i);
			FW_NumberController numberScript = numberObj.GetComponent<FW_NumberController>();
			
			numberScript.setNumber(i);
			numberObj.transform.parent = numbersContainer.transform;
		
			Vector3 numberPosition = new Vector3(FW_Constants.GetAsFloat(singlePos["x"]) ,FW_Constants.GetAsFloat(singlePos["y"]) ,FW_Constants.GetAsFloat(singlePos["z"]));
			numberObj.transform.position = numberPosition;
			_availablePositionForLevel.RemoveAt(_rand);	
			availAbleNumberForLevel.Add(i);
		}
		// Ask to find some number
		StartCoroutine(decideForAnswerNumber());
	}
	
	IEnumerator decideForAnswerNumber(){
		yield return new WaitForSeconds(1);
		if(availAbleNumberForLevel.Count==0){
		   loadQuestion();
		}
		else{
			int _rand = Random.Range(0,availAbleNumberForLevel.Count);
			ansNumber =""+availAbleNumberForLevel[_rand];
			ollyAnimationSR.ollyAskingNumber(ansNumber);
			soundManagerSR.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
		    soundManagerSR.playInstructionSound(new string[]{FW_Constants._soundClips+"pond_count_todo_1a",FW_Constants._soundClips+"ordinal_number_todo_1_b",FW_Constants._soundClips+"number_"+ansNumber});
			availAbleNumberForLevel.RemoveAt(_rand);
		}
		
	}
	
	
	void resetLevelSpecificData(){
			GameObject numbersContainer=GameObject.Find("NumbersContainer");
			foreach(Transform child in numbersContainer.transform)
				GameObject.Destroy(child.gameObject);
				loadQuestion();
	}
	
	
	// -------- read plist data
	void parsePlist(){
		allPositionsHashTable = new ArrayList();

		bool noError;
		noError = PListManager.ParsePListFile(gameData.text, ref gameDataTable);
		if(noError) 
			allHidingSpots = (ArrayList) gameDataTable["AllHidingSpots"];
		Debug.Log("AllHidingSpots: "+allHidingSpots);
		
		for(int i=0; i<allHidingSpots.Count ;i++){
			allPositionsHashTable.Add(allHidingSpots[i]);
			Debug.Log("Hiding Spot: "+allHidingSpots[i]);
		}
		
		
	}
	
	
	// --------- Finger taps
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount ){
		GameObject selection = PickObject(fingerPos);
		if(selection==null)
			return;
		CancelInvoke("noInteraction");
		Invoke("noInteraction", FW_Constants._idleTime + 5.0f);
		 bool isSelectedNumber 		  = isNumber(selection);
		if(isSelectedNumber){
			if(selection.name==ansNumber){   
				popupNumber(selection);
				soundManagerSR.currentClips= new System.Collections.Generic.List<UnityEngine.AudioClip>();
				soundManagerSR.playSoundEffect(FW_Constants._soundClips+"number_"+ansNumber);
			}
			
			else{
				 wrongAttempts++;
				if(wrongAttempts==8){
					resetLevelSpecificData();
					return;
				}
				 int _rand = Random.Range(1,3);
				 soundManagerSR.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
				 soundManagerSR.playInstructionSound(new string[]{FW_Constants._soundClips+ string.Format("wrong_{0}",_rand)});
				 olly.transform.renderer.material.mainTexture =Resources.Load("FishWorld_HideNSeek/Sprites/OllyAnimFrames/FW-OllyWrongAnim") as Texture2D;
				
				 ollyAnimationSR.playWrongAnimation = true;
				// soundManagerSR.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
				 soundManagerSR.playInstructionSound(new string[]{FW_Constants._soundClips+"pond_count_todo_1a",FW_Constants._soundClips+"ordinal_number_todo_1_b",FW_Constants._soundClips+"number_"+ansNumber});
				
			}
		}
		
	}
	
	
	void popupNumber(GameObject selection){
		Debug.Log("pop up number...");
		Vector3 scale=selection.transform.localScale;
		Vector3 pos=selection.transform.position;
		scale.x*=2.3F;
		scale.y*=2.3F;
		pos.z = -120F;
		selection.transform.position=pos;
		iTween.ScaleTo(selection,iTween.Hash("time",1 ,"scale",scale, "oncomplete","destroyNumber", "oncompleteparams",selection,"easetype",iTween.EaseType.easeInSine, "oncompletetarget",gameObject));
		//iTween.FadeTo(selection, iTween.Hash("a",0, "time",1.8, "oncompletetarget",gameObject,"oncomplete", "destroyNumber" , "oncompleteparams	",selection));
	} 
	
	
	void destroyNumber(GameObject selection){
		Debug.Log("Deleting game object.");
		GameObject.Destroy(selection);
		ollyAnimationSR.ollyIdleFrame();
		if(availAbleNumberForLevel.Count==0)
			soundManagerSR.playSuccessOnScreenComplete();
		StartCoroutine(decideForAnswerNumber());
		
	}
	
	
	void noInteraction(){
			soundManagerSR.currentClips = new System.Collections.Generic.List<UnityEngine.AudioClip>();
		    soundManagerSR.playInstructionSound(new string[]{FW_Constants._soundClips+"pond_count_todo_1a",FW_Constants._soundClips+"ordinal_number_todo_1_b",FW_Constants._soundClips+"number_"+ansNumber});
			Invoke("noInteraction", FW_Constants._idleTime + 5.0f);
	}
	
	 void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos ){
		GameObject selection = PickObject(fingerPos);
		 bool isSelectedNumber 		  = isNumber(selection);
			Debug.Log ("isSelectedNumber? "+isSelectedNumber);
		if(isSelectedNumber){
			Debug.Log ("number is selected");
		}
	}
	
	void enableFingerGestures(){
		FingerGestures.OnFingerTap 		 += FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
	}
	
	void disableFingerGestures(){
		FingerGestures.OnFingerTap 		 -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
	}
	
	
		void OnDisable () {
		FingerGestures.OnFingerTap 		 -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
		CancelInvoke("noInteraction");
		}
}


 