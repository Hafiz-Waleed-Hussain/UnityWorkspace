using UnityEngine;
using System.Collections;

public class MAMemoryAnimals : MonoBehaviour {
	
	public TextAsset gameData = null;
	public TextAsset progression = null;
	
	Hashtable gameDataTable = null;
	Hashtable allPositions = null;
	MADataManager dataManager = null;
	
	ArrayList availableAnimals = null;
	int totalSlots = 4;
	
	int questionsPlayed = 0;
	
	public GameObject allAnimalsObj = null;
	
	public GameObject currentSelection = null;
	
	int attempts = 0;
	int corrects = 0;
	
	bool isTouchEnabled = false;
	
	ArrayList animals = null;
	
	ArrayList tempSelections = null;
	
	public GameObject ickyModel;
	MAIckyAnimations ickyAnimations;
	// Use this for initialization
	
	void OnDisable () {
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
	}
	
	void Start () {
		
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
		
		bool noError;
		noError = PListManager.ParsePListFile(gameData.text, ref gameDataTable);
		
		if(noError) {
			allPositions = (Hashtable) gameDataTable["positions"];
			availableAnimals = (ArrayList) gameDataTable["availableAnimals"];
		} else {
			//next game
			AGGameState.loadNextGameInLoop(AGGameIndex.k_MemoryAnimal);
		}
			
		dataManager = new MADataManager(AGGameIndex.k_MemoryAnimal, 0, progression.text);
		
		ickyAnimations = ickyModel.GetComponent<MAIckyAnimations>();
		
		nextQuestion();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
		
		if(!isTouchEnabled) return;
		
		GameObject selection = PickObject(fingerPos);
		
		if(selection.name.Equals(ickyModel.name)) {
			ickyAnimations.animState = MAIckyAnimationState.giggle;
			return;
		}
		
		if(isAnimal(selection)) {
			if(currentSelection == null) {
				currentSelection = selection;
				
				MAAnimal selectionScript = currentSelection.GetComponent<MAAnimal>();
				selectionScript.onlyBody();
				
			} else if (currentSelection != selection){
				
				if (currentSelection.name.Equals(selection.name)) {
					//correct
					
					corrects++;
					attempts++;
					
					MAAnimal curSelectionScript = currentSelection.GetComponent<MAAnimal>();
					curSelectionScript.yesAnimation();
					
					MAAnimal selectionScript = selection.GetComponent<MAAnimal>();
					selectionScript.yesAnimation();
					
					isTouchEnabled = false;
					
					tempSelections = new ArrayList();
					tempSelections.Add(selection);
					tempSelections.Add(currentSelection);
					
					Invoke("correctGuess", 1.0f);
					
				} else {
					//incorrect
					
					attempts++;
					
					ickyAnimations.animState = MAIckyAnimationState.wrong;
					
					MAAnimal curSelectionScript = currentSelection.GetComponent<MAAnimal>();
					curSelectionScript.onlyBody();
					
					MAAnimal selectionScript = selection.GetComponent<MAAnimal>();
					selectionScript.onlyBody();
					
					isTouchEnabled = false;
					Invoke("hideAll", 0.5f);
					currentSelection = null;
				}
			}
		}
    }
	
	public void correctGuess () {
		
		foreach(GameObject anim in tempSelections) {
			animals.Remove(anim);
			Destroy(anim);
		}
		
		if(animals.Count == 0) {
			AGGameState.incrementStarCount();
			
			ickyAnimations.animState = MAIckyAnimationState.celebration;
				
			Invoke("celebrationAnimationFinished", 53F/30F);
			return;	
		}
		
		isTouchEnabled = true;
		currentSelection = null;
		tempSelections = null;
	}
	
	void celebrationAnimationFinished () {
		if(questionsPlayed >= 5) {
			AGGameState.loadNextGameInLoop(AGGameIndex.k_MemoryAnimal);
		} else {
			questionsPlayed++;
			if(dataManager.calculateResult(attempts, corrects)) {
				dataManager.fetchLevelData();
			}
			nextQuestion();
		}
	}
	
	public static GameObject PickObject( Vector2 screenPos )
	{
        Ray ray = Camera.main.ScreenPointToRay( screenPos );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit ) )
            return hit.collider.gameObject;

        return null;
    }
	
	public bool isAnimal(GameObject selection) {
		if(selection != null && selection.transform.parent != null && selection.transform.parent.name.Equals("AllAnimals")) {
			return true;
		}
		return false;
	}
	
	
	void nextQuestion() {
		
		attempts = 0;
		corrects = 0;
		
		dataManager.fetchNextQuestionData();
		totalSlots = dataManager.currentSlots;

		ArrayList animalsForLevel = new ArrayList();
		foreach(Hashtable animalStr in availableAnimals) {
			animalsForLevel.Add(animalStr);
		}
		animalsForLevel = MAConstants.ShuffleList(animalsForLevel);
		
		int animalRequired = totalSlots / 2;
		
		while(animalsForLevel.Count > animalRequired) {
			animalsForLevel.RemoveAt(0);
		}
		
		ArrayList tempAnimals = new ArrayList();
		
		string slotStr = string.Format("{0}", totalSlots);
		Hashtable currentPositions = (Hashtable)allPositions[slotStr];
		
		
		foreach(Hashtable animalData in animalsForLevel) {
			
			for(int i = 0; i < 2; i++) { //create two of every kind
				GameObject animalObj = Instantiate(Resources.Load("JWMemoryAnimals/Prefabs/Animal")) as GameObject;
				MAAnimal animalScript = animalObj.GetComponent<MAAnimal>();
				animalScript.setAnimal(animalData);
				animalObj.transform.parent = allAnimalsObj.transform;
				tempAnimals.Add(animalObj);
			}
		}
		
		animals = MAConstants.ShuffleList(tempAnimals);
		
		foreach(GameObject animalObj in animals) {
			string indexStr = string.Format("{0}", animals.IndexOf(animalObj));
			Hashtable pos = (Hashtable) currentPositions[indexStr];
			Vector3 position = new Vector3(MAConstants.GetAsFloat(pos["x"]), MAConstants.GetAsFloat(pos["y"]));
			animalObj.transform.localPosition = position;
		}
		
		Invoke("hideAll", 1.0f);

	}
	
	void hideAll() {
		foreach(GameObject animalObj in animals) {
			MAAnimal animalScript = animalObj.GetComponent<MAAnimal>();
			animalScript.onlyEyes();
		}
		
		isTouchEnabled = true;
	}
}
