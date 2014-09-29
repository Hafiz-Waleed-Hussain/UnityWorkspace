using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BWNumberSeqGame : MonoBehaviour {
	
	public GameObject scrollCollider;
	public GameObject flowersLayer;
	public GameObject background;
	public GameObject foreground;
	public GameObject bee;
	
	public TextAsset progression;
	
	public GameObject bubbleNumber;
	public GameObject bubbleSymbol;
	
	private BWBackgroundLayer backgroundLayer;
	private BWForegroundLayer foregroundLayer;
	
	private bool isDragging;
	private bool touchEnabled = false;
	
	public int numberToFind;
	public int initialNumber = 0;
	public int numberLineMin = 0;
	public int numberLineMax = 20;
	
	private BWDataManager dataManager = null;
	
	bool enableScrolling = false;
	
	private BWGameState gameState = BWGameState.BWGameStateUnknown;
	
	private float lasty = 0.0f;
	private float xvel = 0.0f;
	private BounceDirection direction = BounceDirection.BounceDirectionStayingStill;
	
	private bool isPlayingScrollAnimation = false;
	private Vector3 scrollLastPos;
	
	private int attempts = -1;
	private int questionsInSession = 0;
	
	public AudioClip [] introAudioClips;
	public List<AudioClip> currentClips;
	
	AudioSource voiceOverSource;
	AudioSource soundEffects;
	private bool isPlayingSuccessSound = false;
	
	void OnDisable () {
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
	    FingerGestures.OnFingerDragMove -= FingerGestures_OnFingerDragMove;
    	FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd; 
		
		CancelInvoke("noInteraction");
	}
	// Use this for initialization
	void Start () {
		
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
	    FingerGestures.OnFingerDragMove += FingerGestures_OnFingerDragMove;
    	FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd; 
		
		Vector3 curPos = flowersLayer.transform.position;
		curPos.x = -640;
		flowersLayer.transform.position = curPos;
		
		backgroundLayer = background.GetComponent<BWBackgroundLayer>();
		foregroundLayer = foreground.GetComponent<BWForegroundLayer>();
		
		dataManager = new BWDataManager(AGGameIndex.k_Beeworld_NumberSequence, 0, progression.text);
		
		AudioClip introClip = introAudioClips[Random.Range(0,introAudioClips.Length)];
		
		currentClips = new List<AudioClip>();
		currentClips.Add(introClip);
		
		voiceOverSource = AddAudio(null, false, false, 1.0f);
		soundEffects = AddAudio(null, false, false, 1.0f);
//		introAudioSource = AddAudio(introClip, false, true, 1.0f);
//		introAudioSource.Play();
		
		questionsInSession = -1;
		nextQuestion();
	}
	
	// Update is called once per frame
	void Update () {
		checkVoiceOverSource();
		
		if(isPlayingScrollAnimation) {
			adjustLayers();
			return;
		}
		if(enableScrolling) updateScroll();
	}
	
	private void checkVoiceOverSource() {
		if(!voiceOverSource.isPlaying && currentClips.Count > 0) {
			
			if(voiceOverSource.clip == currentClips[0]) {
				Debug.Log("remove clip");
				currentClips.RemoveAt(0);
			}
			
			if(currentClips.Count > 0) {
				voiceOverSource.clip = currentClips[0];
				voiceOverSource.Play();
			} else {
				if(isPlayingSuccessSound) {
					successSoundFinished();
					isPlayingSuccessSound = false;
				}
			}
		} 
	}
	
	private void playAudioList(List<AudioClip> clips) {
		if(clips != null && clips.Count > 0) {
			voiceOverSource.Stop();
			currentClips = clips;
		}
	}
		
	public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
  		AudioSource newAudio = (AudioSource)gameObject.AddComponent(typeof(AudioSource)); //.AddComponent(AudioSource);
  		newAudio.clip = clip;
  		newAudio.loop = loop;
  		newAudio.playOnAwake = playAwake;
  		newAudio.volume = vol;
  		return newAudio;
	}
	
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
//		Debug.Log( "tapping");
		if(!touchEnabled) return;
		
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime);
		
		GameObject selection = PickObject(fingerPos);
		if(isSelectionFlower(selection)) {
			
			playSoundEffect("Bee_flowertap_new_01");
			
			BWFlower flowerObj = selection.GetComponent<BWFlower>();
			
			attempts++;
			
			if(flowerObj.getFlowerNumber() == numberToFind) {
				
				AGGameState.incrementStarCount();
				
				CancelInvoke("noInteraction");
				touchEnabled = false;
				flowerObj.setSelected();
				beeToFlower(numberToFind);
				gameState = BWGameState.BWGameStateGuessed;
				//nextQuestion();
			} else {
				
				flowerObj.setSelected();
				playWrongSound(flowerObj.getFlowerNumber());
				BWBee beeObj = bee.GetComponent<BWBee>();
				beeObj.playNoAnimation();
			}
		} else {
			BWBee beeObj = bee.GetComponent<BWBee>();
			
			if(selection == beeObj.body || selection == bubbleNumber) {
				voiceOverSource.clip = null;
				voiceOverSource.Stop();
				
				currentClips = new List<AudioClip>();
				playInstructionSound();
				
				beeObj.playTapAnimation();
			}
		}
    }
	
	int dragFingerIndex = -1;

    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
		CancelInvoke("noInteraction");
        
		if(!enableScrolling) return;
        GameObject selection = PickObject( startPos );
        if( selection == scrollCollider )
        {
            dragFingerIndex = fingerIndex;
			isDragging = true;
        }
    }

    void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        if( fingerIndex == dragFingerIndex && isDragging )
        {
            // update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
			Vector3 curPos = flowersLayer.transform.position;
			curPos.x += delta.x;
            flowersLayer.transform.position = curPos;
			backgroundLayer.moveBackground(delta.x);
			foregroundLayer.moveForeground(delta.x);
        }
    }

    void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime);
        if( fingerIndex == dragFingerIndex )
        {
            dragFingerIndex = -1;
			isDragging = false;
        }
    }
	
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

        // we solve for intersection with z = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint( t );
    }
	
	public void noInteraction() {
		
		playIdleSound();
		
		Invoke("noInteraction", BWConstants.idleTime + 5.0f);
	}
	
	private void adjustLayers() {
		float delta = flowersLayer.transform.position.x - scrollLastPos.x;
		backgroundLayer.moveBackground(delta);
		foregroundLayer.moveForeground(delta);
		scrollLastPos = flowersLayer.transform.position;
	}
	
	private void updateScroll () {
		Vector3 pos = flowersLayer.transform.position;
		// positions for scrollLayer
	
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		float right = pos.x + layer.contentWidth;
		float left = pos.x;
		float minX = -640;
		float maxX = 640;
		
		float bounceTime = 0.2f;
		int frameRate = 60;
		
		if(!isDragging) {
	     	
			float friction = 0.96f;
			
			if(left > minX && direction != BounceDirection.BounceDirectionGoingLeft) {
				xvel = 0;
				direction = BounceDirection.BounceDirectionGoingLeft;
			}
			else if(right < maxX && direction != BounceDirection.BounceDirectionGoingRight)	{
				xvel = 0;
				direction = BounceDirection.BounceDirectionGoingRight;
			}
			
			if(direction == BounceDirection.BounceDirectionGoingRight) {
				if(xvel >= 0) {
					float delta = (maxX - right);
					float yDeltaPerFrame = (delta / (bounceTime * frameRate));
					xvel = yDeltaPerFrame;
				}
				
				if((right + 0.5f) == maxX) {
					pos.x = right -  layer.contentWidth;
					xvel = 0;
					direction = BounceDirection.BounceDirectionStayingStill;
				}
			}
			
			else if(direction == BounceDirection.BounceDirectionGoingLeft) {
				if(xvel <= 0) {
					float delta = (minX - left);
					float yDeltaPerFrame = (delta / (bounceTime * frameRate));
					xvel = yDeltaPerFrame;
				}
				
				if((left + 0.5f) == minX) {
					pos.x = left;
					xvel = 0;
					direction = BounceDirection.BounceDirectionStayingStill;
				}
			} else {
	            
				xvel *= friction;
	            //lasty = pos.x;
			}
			pos.x += xvel;
			
			backgroundLayer.moveBackground(pos.x - flowersLayer.transform.position.x);
			foregroundLayer.moveForeground(pos.x - flowersLayer.transform.position.x);
	        flowersLayer.transform.position = pos;
			
		} else {
			if(left <= minX || right >= maxX) {
				direction = BounceDirection.BounceDirectionStayingStill;
			}
			
			if(direction == BounceDirection.BounceDirectionStayingStill) {
				xvel = (pos.x - lasty)/2;
				lasty = pos.x;
			}
		}
	}
	
	public void nextQuestion () {
		
		
		
		
		gameState = BWGameState.BWGameStateResetting;
		enableScrolling = false;
		
		if(attempts > 0) {
			
			
		}
		
		attempts = 0;
		dataManager.fetchNextQuestionData();
		
		numberLineMin = dataManager.numberLineMin;
		numberLineMax = dataManager.numberLineMax;
		initialNumber = dataManager.initialNumber;
		numberToFind = dataManager.numberToFind;
		
		string numberimg = string.Format("BW_NumberLine/Sprites/Referent/Referant_numbers{0}", AGGameState.modInt(numberToFind));
		bubbleNumber.renderer.material.mainTexture = (Texture2D)Resources.Load(numberimg);
		
		if(numberToFind < 0) {
			bubbleSymbol.SetActive(true);
		} else {
			bubbleSymbol.SetActive(false);
		}
		//bubbleNumber.text = string.Format("{0}", numberToFind);
		
		//flowersLayer.transform.position = new Vector3(-640, -300, -200);
		
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		layer.setFlowersLayer(numberLineMin, numberLineMax, initialNumber);
		
		if ((numberLineMax - numberLineMin) > 7) {
			enableScrolling = true;
			xvel = 0;
			lasty = 0;
			direction = BounceDirection.BounceDirectionStayingStill;
		}
		
		flowerLayerToInitialNumber(initialNumber);
		
		beeToSky();
		
		touchEnabled = true;
		
		playInstructionSound();
		
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime + 5.0f);
		
	}
	
	private bool isSelectionFlower (GameObject selection) {
		if(selection == null) return false;
		if(selection.name.StartsWith("Flower") || selection.name.StartsWith("Hive"))	return true; 
		return false;
	}
	
	private void beeToSky() {
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj.flipBee(dataManager.shouldFlipBee);
		
		if(dataManager.shouldFlipBee) {
			beeToPosition(new Vector3 (450, 200, -150));
		} else {
			beeToPosition(new Vector3 (-450, 200, -150));
		}
	}
	private void beeToFlower (int _flowerIndex) {
		
		BWFlowersLayer layerObj = flowersLayer.GetComponent<BWFlowersLayer>();
		GameObject flower = layerObj.flowerWithNumber(_flowerIndex);
		
		if(flower == null) return;
		
		Vector3 beePos = bee.transform.position;
		beePos.x = flower.transform.position.x;
		beePos.y = flower.transform.position.y + 320;
		beeToPosition(beePos);
//		bee.transform.position = beePos;
	}
	
	private void beeToPosition (Vector3 pos) {
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj._delegate = this;
		beeObj.moveToPoint(pos);
	}
	
	public void beeMoveFinished() {
		if(gameState == BWGameState.BWGameStateGuessed) {
			BWBee beeObj = bee.GetComponent<BWBee>();
			beeObj.playYesAnimation();
				
			if (dataManager.calculateResult(attempts, 1)) {
				beeObj.shouldPlayCelebration = true;
				dataManager.fetchLevelData();
			}
			
			playSucess();
			
			BWFlowersLayer layerObj = flowersLayer.GetComponent<BWFlowersLayer>();
			GameObject flower = layerObj.flowerWithNumber(numberToFind);
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			flowerObj.setPollinated();
		}
	}
	
	public void beeNoAnimationFinished () {
		resetCurrentQuestion();
	}
	
	public void destroyFlowerLayer() {
		//flower close anim
		gameState = BWGameState.BWGameStateResetting;
		
		BWFlowersLayer layerObj = flowersLayer.GetComponent<BWFlowersLayer>();
		layerObj._delegate = this;
		layerObj.destroyFlowersLayer();
		beeToSky();
		
		
		//scroll reset
		//
		//flowerLayerToInitialNumber(numberLineMin);
	}
	
	public void resetCurrentQuestion () {
		
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		
		foreach(GameObject flower in layer.flowers) {
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			flowerObj.setDisabled();
		}
	}
	
	public void flowerLayerDestroyed () {
		questionsInSession++;
		
		if(questionsInSession >= 5) {
			AGGameState.loadNextGameInLoop(AGGameIndex.k_Beeworld_NumberSequence);
			return;
		} else {
			nextQuestion();
		}
		
	}
	
	private void flowerLayerToInitialNumber (int _initial) {
		Debug.Log("moving");
		Vector3 initialPosition = flowersLayer.transform.position;
		initialPosition.x = BWConstants.flowersLayerMinX - ((_initial - numberLineMin) * 170);
		
		float deltaX = initialPosition.x - flowersLayer.transform.position.x;
		
		scrollLastPos = flowersLayer.transform.position;
		isPlayingScrollAnimation = true;
		iTween.MoveTo(flowersLayer, iTween.Hash("position", initialPosition, "time", 0.5f, "onComplete", "scrollAnimationComplete", "oncompletetarget", gameObject));
		
		//backgroundLayer.moveBackground(deltaX);
		//foregroundLayer.moveForeground(deltaX);
		//flowersLayer.transform.position = initialPosition;
	}
	
	private void scrollAnimationComplete () {
		isPlayingScrollAnimation = false;
	}
	
	private void successSoundFinished () {
		destroyFlowerLayer();
	}
	
	private void playIdleSound() {
		if(isPlayingSuccessSound)
			return;
		
		currentClips = new List<AudioClip>();
		
	    int _num = AGGameState.modInt(numberToFind);
		string numer = string.Format("number_{0}",_num);
		AudioClip numerClip = loadSound(numer);
		
	    int rand = UnityEngine.Random.Range(0,2);
    	if (rand == 0 || numberToFind == 0) {
			currentClips.Add(loadSound("flowers_intro_4"));
    	} else {
			
			currentClips.Add(loadSound("flowers_find_todo_1_a"));
	        if (numberToFind < 0) {
				currentClips.Add(loadSound("word_negative"));
			}
			currentClips.Add(numerClip);
	    }
	}
	
	private void playSucess (){
		
		if(isPlayingSuccessSound)
			return;
		
		
		currentClips = new List<AudioClip>();
		
	    int _num = AGGameState.modInt(numberToFind);
		string numer = string.Format("number_{0}",_num);
		AudioClip numerClip = loadSound(numer);
        
	
	    int rand = Random.Range(0,2);
	    
	    string o_number = null;
	    if (numberToFind > 0 && numberToFind <= 10) {
	        o_number = string.Format("o_number_{0}",_num);
	    } else {
	        o_number = string.Format("number_{0}",_num);
	    }
	    
	    AudioClip o_numerClip = loadSound(o_number);
	    
	    if (rand ==0 || numberToFind < 0) {
			currentClips.Add(loadSound("all_find_1"));
	        if (numberToFind < 0) {
				currentClips.Add(loadSound("word_negative"));
			}
			currentClips.Add(numerClip);
	    } else {
			currentClips.Add(loadSound("ordinal_number_correct_1_a"));
			currentClips.Add(o_numerClip);
			currentClips.Add(loadSound("flowers_find_todo_1_b"));
	    }
	    
	    if (numberToFind == 0) {
			currentClips = new List<AudioClip>();
			currentClips.Add(loadSound("all_find_1"));
	        currentClips.Add(numerClip);
	    }
	    
	    isPlayingSuccessSound = true;
			
		playSoundEffect("Bee_positive_new_01cut");
		playAudioList(currentClips);
	}
	
	void playWrongSound (int wNumber) {
		
		voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		int _num = AGGameState.modInt(wNumber);
		
    	string numer = string.Format("number_{0}",_num);
		AudioClip numberClip = loadSound(numer);
	        
		currentClips = new List<AudioClip>();
		
		currentClips.Add(loadSound("all_wrong_5_a"));
	    if (wNumber < 0) {
	        currentClips.Add(loadSound("word_negative.mp3"));
	    }

		currentClips.Add(numberClip);
	    	
		playSoundEffect("Bee_negative_new_01cut");
		playAudioList(currentClips);
	}
	
	void playInstructionSound () {
		
		string number = null;
	    
	    int _number = AGGameState.modInt(numberToFind);
	    AudioClip numberClip = loadSound(string.Format("number_{0}", _number));
	    AudioClip o_numberClip;
	    
	    if (_number > 0 && _number <= 10) {
	        o_numberClip = loadSound(string.Format("o_number_{0}", _number));
	    } else {
	        o_numberClip = loadSound(string.Format("number_{0}", _number));
	    }
	    
	    
	    if (numberToFind < 0) {
	        int rand = Random.Range(0,4);
	        
	        if (rand == 0) {
				currentClips.Add(loadSound("flowers_find_todo_1_a"));
				currentClips.Add(loadSound("word_negative"));
				currentClips.Add(numberClip);
	        } else  if (rand == 1) {
				currentClips.Add(loadSound("flowers_find_todo_1_a"));
				currentClips.Add(loadSound("word_negative"));
				currentClips.Add(numberClip);
	        } else if(rand == 2){
				currentClips.Add(loadSound("flowers_find_todo_2_a"));
				currentClips.Add(loadSound("word_negative"));
				currentClips.Add(numberClip);
	        } else {
				currentClips.Add(loadSound("all_find_todo_2"));
				currentClips.Add(loadSound("word_negative"));
				currentClips.Add(numberClip);
	        }
	    } else if (numberToFind > 0){
	        int rand = Random.Range(0,3);
	        
	        if (rand == 0) {
				currentClips.Add(loadSound("ordinal_number_todo_1_a"));
				currentClips.Add(o_numberClip);
				currentClips.Add(loadSound("flowers_find_todo_1_b"));
	        } else  if (rand == 1) {
				currentClips.Add(loadSound("flowers_find_todo_1_a"));
				currentClips.Add(numberClip);
	        
	        } else if (rand == 2){
				currentClips.Add(loadSound("flowers_find_todo_2_a"));
				currentClips.Add(o_numberClip);
				currentClips.Add(loadSound("flowers_find_todo_1_b"));
	            
	        } else {
				currentClips.Add(loadSound("all_find_todo_2"));
				currentClips.Add(numberClip);
	        }
	    } else {
	        currentClips.Add(loadSound("all_find_todo_2"));
			currentClips.Add(numberClip);
	    }
	    
	    playAudioList(currentClips);
	}
	
	void playSoundEffect (string effect) {
		soundEffects.clip = loadSound(effect);
		soundEffects.Play();
	}
	
	AudioClip loadSound(string clipName) {
		return (AudioClip)Resources.Load(string.Format("BW_NumberLine/NumberSequenceSounds/{0}", clipName));
	}
}
