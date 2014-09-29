using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BWNumberArithmetic : MonoBehaviour {
	
	public GameObject scrollCollider;
	public GameObject flowersLayer;
	public GameObject background;
	public GameObject foreground;
	public GameObject bee;
	public GameObject linePrefab;
	private GameObject prevFlower;
	
	public GameObject tutorialLayer;
	
	public TextAsset progression;
	
	public GameObject bubbleNumber;
	public GameObject bubbleSymbol;
	
	private BWDiagonalBackgroundLayer backgroundLayer;
	private BWDiagonalForegroundLayer foregroundLayer;
	
	private ArrayList expectedNumbers = null;
	private BWDataManager dataManager = null;
	public int numberToFind;
	public int initialNumber = 0;
	public int numberLineMin = 0;
	public int numberLineMax = 20;
	public int beeStartingPoint = 0;
	private int attempts = -1;
	private int questionsInSession = 0;
	
	bool isDragging = false;
	bool isSwiping = false;
	bool enableScrolling = false;
	bool currentAnswer = true;
	bool swipingWentOut = false;
	bool enableTypeWriter = false;
	bool touchEnabled = false;
	
	private bool isPlayingScrollAnimation = false;
	private Vector3 scrollLastPos;
	
	BWGameState gameState = BWGameState.BWGameStateUnknown;
	
	private float lasty = 0.0f;
	private float xvel = 0.0f;
	private BounceDirection direction = BounceDirection.BounceDirectionStayingStill;
	
	public AudioClip [] introAudioClips;
	public List<AudioClip> currentClips;
	
	AudioSource voiceOverSource;
	AudioSource soundEffects;
	private bool isPlayingSuccessSound = false;
	
	private int currentCategoryIndex = 0;
	
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
		
		backgroundLayer = background.GetComponent<BWDiagonalBackgroundLayer>();
		foregroundLayer = foreground.GetComponent<BWDiagonalForegroundLayer>();
		
		if(PlayerPrefs.GetInt("beeTutorialPlayed") > 0) {
			currentCategoryIndex = UnityEngine.Random.Range(0,2);	
		} else {
			currentCategoryIndex = 0;
		}
		
		dataManager = new BWDataManager(AGGameIndex.k_Beeworld_CoutingUpDown, currentCategoryIndex, progression.text);
		
		AudioClip introClip = introAudioClips[UnityEngine.Random.Range(0,introAudioClips.Length)];
		
		currentClips = new List<AudioClip>();
		currentClips.Add(introClip);
		
		voiceOverSource = AddAudio(null, false, false, 1.0f);
		soundEffects = AddAudio(null, false, false, 1.0f);
		
		questionsInSession = -1;
		nextQuestion();
		
		
	}
	
	void Awake () {
		
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
		if (!touchEnabled) return;
//		Debug.Log( "tapping");
		/*
		GameObject selection = PickObject(fingerPos);
		if(selection == null) return;
		//Debug.Log(selection);
		if(selection.name.StartsWith("Flower") || selection.name.StartsWith("Hive")) {
			
		}*/
		
		GameObject selection = PickObject(fingerPos);
		if(selection == null) return;
		
		BWBee beeObj = bee.GetComponent<BWBee>();
		
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime);
		
		if(prevFlower != null) {
			CancelInvoke("deduceResult");
			Invoke("deduceResult", 3.0f);
		}
		
		if(selection == beeObj.body || selection == bubbleNumber) {
			voiceOverSource.clip = null;
			voiceOverSource.Stop();
			
			currentClips = new List<AudioClip>();
			playInstructionSound();
			
			beeObj.playTapAnimation();
			
			playTutorialAnimation();
			
			CancelInvoke("noInteraction");
			Invoke("noInteraction", BWConstants.idleTime + 5.0f);
			
			if(prevFlower != null) {
				CancelInvoke("deduceResult");
				Invoke("deduceResult", 8.0f);
			}
		} else {
			
			if (isSelectionFlower(selection)) {
			
				BWFlower flowerObj = selection.GetComponent<BWFlower>();
				
				if(flowerObj.getFlowerNumber() == beeStartingPoint+numberToFind) {
					playHintSound(1);
				} else {
					playHintSound(-1);
				}
			}
		}
		
		
    }
	
	int dragFingerIndex = -1;

    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
		if (!touchEnabled) return;
	
		CancelInvoke("noInteraction");
		CancelInvoke("deduceResult");
		
		BWTutorialLayer tutorialScript = tutorialLayer.GetComponent<BWTutorialLayer>();
		tutorialScript.stopAnimation();
		
        GameObject selection = PickObject( startPos );
     	if(selection == null || dragFingerIndex != -1) return;
		//Scrolling begins
		if( selection == scrollCollider )
        {
			if(!enableScrolling) return;
            dragFingerIndex = fingerIndex;
			isDragging = true;
			
        } else if (isSelectionFlower(selection)) {
			
			BWFlower flowerObj = selection.GetComponent<BWFlower>();
			if(expectedNumbers != null && expectedNumbers.Count > 0 && flowerObj.getFlowerNumber() == (int)expectedNumbers[0]) {
				
				//beeToFlower((int)expectedNumbers[0]);
				dragFingerIndex = fingerIndex;
				isSwiping = true;
				swipingWentOut = false;
				currentAnswer = true;
				expectedNumbers.RemoveAt(0);
				
				prevFlower = selection;
			} else {
				//if started from incorrect flower
				
				if(flowerObj.getFlowerNumber() == beeStartingPoint+numberToFind) {
					playHintSound(1);
				} else {
					playHelpSound();
				}
			}
		}
    }

    void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
		if (!touchEnabled) return;
		
		
        if( fingerIndex == dragFingerIndex )
        {
			//Scrolling
			if(isDragging) {
				
				
				if(enableTypeWriter) {
					
					GameObject selection = PickObject( fingerPos );
					if((numberToFind > 0 && delta.x > 0) ||
					(numberToFind < 0 && delta.x < 0) ||
					(!isSelectionFlower(selection))) {
						
						xvel = 0;
						lasty = 0;
						direction = BounceDirection.BounceDirectionStayingStill;
						isDragging = false;
						return;
					}
				}
				
	            // update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
				Vector3 curPos = flowersLayer.transform.position;
				curPos.x += delta.x;
				
				float offset = (float)(Math.Tan(BWConstants.diagonalAngle)) * delta.x;
				curPos.y += offset;
				
	            flowersLayer.transform.position = curPos;
				backgroundLayer.moveBackground(delta.x);
				foregroundLayer.moveForeground(delta.x);
				
			} else if (isSwiping) {
			//Swiping
				
				GameObject selection = PickObject( fingerPos );
				
				if(isSelectionFlower(selection)) {
					
					//checking for type writer scroll 
					if (prevFlower == selection) {
						if(flowerOnTheEdge(selection)) {  
							enableTypeWriter = true;
							isDragging = true;
						} else {
							enableTypeWriter = false;
							isDragging = false;
						}
						
					} //check for tracing lines
					else {
						
						enableTypeWriter = false;
						isDragging = false;
						
						BWFlower flowerObj = selection.GetComponent<BWFlower>();
						if(expectedNumbers.Count > 0 && flowerObj.getFlowerNumber() == (int)expectedNumbers[0] && swipingWentOut) {
						
							playSoundEffect("Bee_flowertap_new_01");
							
							beeToFlower((int)expectedNumbers[0]);
							playNumberSound((int)expectedNumbers[0] - beeStartingPoint);
							
							swipingWentOut = false;
							expectedNumbers.RemoveAt(0);
							addTracingLine(prevFlower, selection);
							prevFlower = selection;
							
						} else if (expectedNumbers.Count == 0 && swipingWentOut){
							
							playSoundEffect("Bee_flowertap_new_01");
							incorrectSwipingBeyondTarget(prevFlower, selection);
						} else if (expectedNumbers.Count > 0 && swipingWentOut) {
							int rand = UnityEngine.Random.Range(2,4);
							playHintSound(rand);
						}
					}
					
				} else {
					enableTypeWriter = false;
					isDragging = false;
					
					//either the selection is null or selection is scrollcollider - so swiping went out
					swipingWentOut = true;
					
					//check for auto scroll
					Vector3 curPosition = GetWorldPos(fingerPos);
					
					if (numberToFind > 0 && curPosition.x > 560) {
						if(flowerOnTheEdge(prevFlower)) {
							//auto scroll
							BWFlower prevFlowerObj = prevFlower.GetComponent<BWFlower>();
							int lastNumberRegistered = prevFlowerObj.getFlowerNumber();
							
							if(lastNumberRegistered == dataManager.numberLineMax) {
								return;
							}
							
							int numberToScrollTo = lastNumberRegistered - 1;
							
		                    if (numberToScrollTo < dataManager.numberLineMin) {
		                        numberToScrollTo = dataManager.numberLineMin;
		                    }
		                    if (numberToScrollTo+BWConstants.numbersOnScreen > dataManager.numberLineMax) {
		                        numberToScrollTo = dataManager.numberLineMax - BWConstants.numbersOnScreen;
		                    }
							
							Debug.Log("scroll to "+numberToScrollTo);
							flowerLayerToInitialNumber(numberToScrollTo);
						}
					} else if (numberToFind < 0 && curPosition.x < -560) {
						if(flowerOnTheEdge(prevFlower)) {
							//auto scroll
							BWFlower prevFlowerObj = prevFlower.GetComponent<BWFlower>();
							int lastNumberRegistered = prevFlowerObj.getFlowerNumber();
							
							if(lastNumberRegistered == dataManager.numberLineMin) {
								return;
							}
							
							int numberToScrollTo = lastNumberRegistered - 6;
							
		                    if (numberToScrollTo < dataManager.numberLineMin) {
		                        numberToScrollTo = dataManager.numberLineMin;
		                    }
		                    
							if (numberToScrollTo+BWConstants.numbersOnScreen > dataManager.numberLineMax) {
		                        numberToScrollTo = dataManager.numberLineMax - BWConstants.numbersOnScreen;
		                    }
							Debug.Log("scroll to "+numberToScrollTo);
							flowerLayerToInitialNumber(numberToScrollTo);
							
						}
					}
				}
			}
        }
    }

    void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
		if (!touchEnabled) return;
		
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime);
		
		
        if( fingerIndex == dragFingerIndex )
        {
            dragFingerIndex = -1;
			isDragging = false;
			
			
			 if (isSwiping) {
			//Swiping
				
				isSwiping = false;
				
				GameObject selection = PickObject( fingerPos );
				
				if (selection != null && (selection.name.StartsWith("Flower") || selection.name.StartsWith("Hive"))) {
					
					BWFlower flowerObj = selection.GetComponent<BWFlower>();
					if(expectedNumbers.Count > 0 && flowerObj.getFlowerNumber() == (int)expectedNumbers[0] && swipingWentOut) {
						
						beeToFlower((int)expectedNumbers[0]);
						expectedNumbers.RemoveAt(0);
						addTracingLine(prevFlower, selection);
						prevFlower = selection;
					} else if (expectedNumbers.Count == 0 && swipingWentOut) {
						incorrectSwipingBeyondTarget(prevFlower, selection);
					}
				}
			}
			
			if(expectedNumbers.Count == 0) {
				if(currentAnswer == true) {
					
					prevFlower = null;
					
					CancelInvoke("noInteraction");
		
					AGGameState.incrementStarCount();
					
					attempts++;
					playSucess();
					BWBee beeObj = bee.GetComponent<BWBee>();
					beeObj.playYesAnimation();
					
					if (dataManager.calculateResult(attempts, 1)) {
						beeObj.shouldPlayCelebration = true;
						dataManager.fetchLevelData();
					}
					touchEnabled = false;
					
					BWFlowersLayer layerObj = flowersLayer.GetComponent<BWFlowersLayer>();
					GameObject flower = layerObj.flowerWithNumber(beeStartingPoint+numberToFind);
					BWFlower flowerObj = flower.GetComponent<BWFlower>();
					flowerObj.setPollinated();
				} else {
					
					touchEnabled = false;
					attempts++;
					playWrongSound(0);
					BWBee beeObj = bee.GetComponent<BWBee>();
					beeObj.playNoAnimation();
					prevFlower = null;
					
					CancelInvoke("noInteraction");
					Invoke("noInteraction", BWConstants.idleTime + 5.0f);
		
				}
			} else {
				if(prevFlower != null) {
					BWFlower flowerObj = prevFlower.GetComponent<BWFlower>();
					int newStarting = flowerObj.getFlowerNumber();
					setExpectedNumbers(newStarting, numberToFind - (newStarting - beeStartingPoint));
					
				}
			}
        }
		
		if(prevFlower != null) {
			CancelInvoke("deduceResult");
			Invoke("deduceResult", 3.0f);
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
		float minX = BWConstants.flowersLayerMinX;
		float maxX = BWConstants.flowersLayerMaxX;
		
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
			float offset = (float)(Math.Tan(BWConstants.diagonalAngle)) * (pos.x - flowersLayer.transform.position.x);
			
			pos.y += offset;
			
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
	
	public void deduceResult() {
		
		touchEnabled = false;
		attempts++;
		playWrongSound(0);
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj.playNoAnimation();
		prevFlower = null;
		
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime + 5.0f);
		
	}
	
	public void destroyCurrentLayer () {
		gameState = BWGameState.BWGameStateGuessed;	
		destroyFlowerLayer();
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
		
	}
	
	public void flowerLayerDestroyed () {
		/*if(currentCategoryIndex == 0)
			currentCategoryIndex = 1;
		else if(currentCategoryIndex == 1) 
			currentCategoryIndex = 0;
		*/
		//dataManager.saveData();
		//dataManager = new BWDataManager(AGGameIndex.k_Beeworld_CoutingUpDown, currentCategoryIndex, progression.text);
		
		questionsInSession++;
		
		if(questionsInSession >= 5) {
			AGGameState.loadNextGameInLoop(AGGameIndex.k_Beeworld_CoutingUpDown);
		} else {
			nextQuestion();
		}
		
	}
	
	public void resetCurrentQuestion () {
		
		touchEnabled = true;
		flowersLayer.transform.position = new Vector3(BWConstants.flowersLayerMinX, -350, -200);
		
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		layer.resetFlowersLayer();
		
		if ((numberLineMax - numberLineMin) > BWConstants.numbersOnScreen) {
			enableScrolling = true;
			xvel = 0;
			lasty = 0;
			direction = BounceDirection.BounceDirectionStayingStill;
		}
		
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj.flipBee(dataManager.shouldFlipBee);
		
		flowerLayerToInitialNumber(initialNumber);
		beeToFlower(beeStartingPoint);
		setExpectedNumbers(beeStartingPoint, numberToFind);
	}
	
	public void nextQuestion () {
		
		
		
		enableScrolling = false;
		isDragging = false;
		isSwiping = false;
		prevFlower = null;
		
		if(attempts > 0) {
			
		}
		
		attempts = 0;
		dataManager.fetchNextQuestionData();
		
		numberLineMin = dataManager.numberLineMin;
		numberLineMax = dataManager.numberLineMax;
		initialNumber = dataManager.initialNumber;
		numberToFind = dataManager.numberToFind;
		beeStartingPoint = dataManager.beeStartingIndex;
		
		
		string numberimg = string.Format("BW_NumberLine/Sprites/Referent/Referant_numbers{0}", AGGameState.modInt(numberToFind));
		bubbleNumber.renderer.material.mainTexture = (Texture2D)Resources.Load(numberimg);
		
		if(numberToFind < 0) {
			bubbleSymbol.renderer.material.mainTexture = (Texture2D)Resources.Load("BW_NumberLine/Sprites/Referent/Referant_exporterminus");
		} else {
			bubbleSymbol.renderer.material.mainTexture = (Texture2D)Resources.Load("BW_NumberLine/Sprites/Referent/Referant_exporterPlus");
		}
		
		flowersLayer.transform.position = new Vector3(BWConstants.flowersLayerMinX, -350, -200);
		
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		layer.setDiagonalFlowersLayer(numberLineMin, numberLineMax, initialNumber);
		
		if ((numberLineMax - numberLineMin) > BWConstants.numbersOnScreen) {
			enableScrolling = true;
			xvel = 0;
			lasty = 0;
			direction = BounceDirection.BounceDirectionStayingStill;
		}
		
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj.flipBee(dataManager.shouldFlipBee);
		
		flowerLayerToInitialNumber(initialNumber);
		beeToFlower(beeStartingPoint);
		setExpectedNumbers(beeStartingPoint, numberToFind);
		
		if(dataManager.currentLevel == 0) {
			Invoke("levelZeroTutorial", 3);
		} else {
			playInstructionSound();
		}
		
		CancelInvoke("noInteraction");
		Invoke("noInteraction", BWConstants.idleTime + 5.0f);
		
		touchEnabled = true;
	}
	
	private void levelZeroTutorial () {
		playIdleSound();
		playTutorialAnimation();
	}
	
	private void setExpectedNumbers(int startingPoint, int numberToCount) {
		int finalNumber = startingPoint + numberToCount;
		ArrayList numbersArray = new ArrayList();
		if(numberToCount > 0) {
			for(int i = startingPoint; i <= finalNumber; i++) {
				numbersArray.Add(i);
			}
		} else if (numberToCount < 0) {
			for(int i = startingPoint; i >= finalNumber; i--) {
				numbersArray.Add(i);
			}
		} else {
			//for counting up and down this should never happen
		}
		expectedNumbers = numbersArray;
	}
	
	private void addTracingLine (GameObject preFlower, GameObject curFlower) {
		
		if (preFlower == null || curFlower == null) return;
		
		GameObject tracingLine = Instantiate(linePrefab) as GameObject;
		tracingLine.transform.parent = flowersLayer.transform;
		
		Vector3 pos = new Vector3 (0,0,0);
		
		if(numberToFind > 0) {
			pos.x = preFlower.transform.localPosition.x + tracingLine.transform.localScale.x/2.0f;
			pos.y = preFlower.transform.localPosition.y + 270;
			pos.z = 0;
		} else if (numberToFind < 0) {
			pos.x = curFlower.transform.localPosition.x + tracingLine.transform.localScale.x/2.0f;
			pos.y = curFlower.transform.localPosition.y + 270;
			pos.z = 0;
		} else {
			DestroyObject(tracingLine);
			return;
		}
		tracingLine.transform.localPosition = pos;
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		layer.tracingLines.Add(tracingLine);
		
		BWFlower flowerObj = curFlower.GetComponent<BWFlower>();
		flowerObj.setSelected();
	}
	
	private void incorrectSwipingBeyondTarget (GameObject preFlower, GameObject curFlower) {
		BWFlower curFlowerObj = curFlower.GetComponent<BWFlower>();
		BWFlower preFlowerObj = preFlower.GetComponent<BWFlower>();
		
		if(numberToFind > 0) {
			if (curFlowerObj.getFlowerNumber() == preFlowerObj.getFlowerNumber()+1) {
				
				BWFlower flowerObj = curFlower.GetComponent<BWFlower>();
				beeToFlower(flowerObj.getFlowerNumber());
				
				addTracingLine(preFlower, curFlower);
				prevFlower = curFlower;
				currentAnswer = false;
			}
		} else if (numberToFind < 0) {
			if (curFlowerObj.getFlowerNumber()+1 == preFlowerObj.getFlowerNumber()) {
				
				BWFlower flowerObj = curFlower.GetComponent<BWFlower>();
				beeToFlower(flowerObj.getFlowerNumber());
				
				addTracingLine(preFlower, curFlower);
				prevFlower = curFlower;
				currentAnswer = false;
			}		
		}
		
	}
	
	private bool isSelectionFlower (GameObject selection) {
		if(selection == null) return false;
		if(selection.name.StartsWith("Flower") || selection.name.StartsWith("Hive"))	return true; 
		return false;
	}
	
	private bool flowerOnTheEdge (GameObject selection) {
		if((selection.transform.position.x > 520 && selection.transform.position.x < 640 && numberToFind > 0) ||  //left edge counting up
						(selection.transform.position.x < -520 && selection.transform.position.x > -640 && numberToFind < 0)) {
			return true;
		}
		return false;
	}
	
	public void noInteraction() {
		
		playIdleSound();
		playTutorialAnimation();
		Invoke("noInteraction", BWConstants.idleTime + 5.0f);
	}
	
	private void flowerLayerToInitialNumber (int _initial) {
		Vector3 initialPosition = flowersLayer.transform.position;
		initialPosition.x = BWConstants.flowersLayerMinX - ((_initial - numberLineMin) * 170);
		
		float deltaX = initialPosition.x - flowersLayer.transform.position.x;
		float offset = (float)(Math.Tan(BWConstants.diagonalAngle)) * deltaX;
		initialPosition.y += offset;
		
		
//		scrollLastPos = flowersLayer.transform.position;
//		isPlayingScrollAnimation = true;
//		iTween.MoveTo(flowersLayer, iTween.Hash("position", initialPosition, "time", 0.5f, "onComplete", "scrollAnimationComplete", "oncompletetarget", gameObject));
		
		backgroundLayer.moveBackground(deltaX);
		foregroundLayer.moveForeground(deltaX);
		flowersLayer.transform.position = initialPosition;
	}
	
	private void beeToPosition (Vector3 pos) {
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj._delegate = this;
		beeObj.moveToPoint(pos);
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
	
		if(flowersLayer == null) return;
		
		BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
		GameObject flower = layer.flowerWithNumber(_flowerIndex);
	
		if(flower == null) return;
		
		Vector3 beePos = bee.transform.position;
		beePos.x = flower.transform.position.x;
		beePos.y = flower.transform.position.y + 320;
		
		BWBee beeObj = bee.GetComponent<BWBee>();
		beeObj.moveToPoint(beePos);
//		bee.transform.position = beePos;
	}
	
	private void scrollAnimationComplete () {
		isPlayingScrollAnimation = false;
	}
	
	private void successSoundFinished () {
		destroyCurrentLayer();
	}
	
	private void playTutorialAnimation () {
		BWTutorialLayer tutorialScript = tutorialLayer.GetComponent<BWTutorialLayer>();
		
		
		if(tutorialScript.isPlayingTutorial) {
			return;
		}
		
		ArrayList positionArray = new ArrayList();
		
		foreach(int _number in expectedNumbers) {
			BWFlowersLayer layer =  flowersLayer.GetComponent<BWFlowersLayer>();
			GameObject flower = layer.flowerWithNumber(_number);
			
			if(flower == null) return;
			
			Vector3 pos = new Vector3(flower.transform.position.x+20F, flower.transform.position.y + 170F, -800F);	
			positionArray.Add(pos);
		}
		
		tutorialScript.positions = positionArray;
		tutorialScript.playTutorial();
	}
	
	private void playHintSound(int _hint) {
		
		voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		currentClips = new List<AudioClip>();
		
		if(_hint == -1) {
			int _num1 = AGGameState.modInt(numberToFind);
			string numer1 = string.Format("number_{0}",_num1);
			AudioClip numerClip1 = loadSound(numer1);
			if (_num1 > 1) {	
				currentClips.Add(loadSound("flowers_counting_todo_3_a"));
				currentClips.Add(numerClip1);
				currentClips.Add(loadSound("flowers_counting_todo_3_b"));
	        } else {
				currentClips.Add(loadSound("flowers_counting_todo_4"));
	        }
		} else {
			AudioClip hint = loadSound(string.Format("flowers_hint_{0}",_hint));
			currentClips.Add(hint);
		
		}
		
	    playAudioList(currentClips);
	}
	
	private void playHelpSound () {
		
		voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		currentClips = new List<AudioClip>();
		
	    int _num2 = expectedNumbers.Count;
	    AudioClip number2 = loadSound(string.Format("number_{0}",_num2));
	    
		currentClips.Add(loadSound("flowers_help_2_a"));
		currentClips.Add(number2);
		currentClips.Add(loadSound("flowers_help_2_b"));

		playAudioList(currentClips);
		
	}
	
	private void playIdleSound() {
		if(isPlayingSuccessSound)
			return;
		
		voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		currentClips = new List<AudioClip>();
		
	    int _num1 = AGGameState.modInt(numberToFind);
		string numer1 = string.Format("number_{0}",_num1);
		AudioClip numerClip1 = loadSound(numer1);
		
		int _num2 = AGGameState.modInt(beeStartingPoint);
		string numer2 = string.Format("number_{0}",_num2);
		AudioClip numerClip2 = loadSound(numer2);
		
	    int rand = UnityEngine.Random.Range(0,2);
    
	    if (rand == 0) {
	        if (_num1 > 1) {
				
				currentClips.Add(loadSound("flowers_counting_todo_3_a"));
				currentClips.Add(numerClip1);
				currentClips.Add(loadSound("flowers_counting_todo_3_b"));
	        } else {
				currentClips.Add(loadSound("flowers_counting_todo_4"));
	        }
	    } else {
	        if (_num1 > 1) {
				
				currentClips.Add(loadSound("flowers_counting_up_todo_1_a"));
		        if (beeStartingPoint < 0) {
					currentClips.Add(loadSound("word_negative"));
				}
				currentClips.Add(numerClip2);
				currentClips.Add(loadSound("flowers_counting_up_todo_1_b"));
				currentClips.Add(numerClip1);
				currentClips.Add(loadSound("flowers_counting_up_todo_1_c"));
				
	        } else {
	            
				currentClips.Add(loadSound("flowers_counting_up_todo_2_a"));
		        if (beeStartingPoint < 0) {
					currentClips.Add(loadSound("word_negative"));
				}
				currentClips.Add(numerClip2);
				currentClips.Add(loadSound("flowers_counting_up_todo_2_b"));
				currentClips.Add(numerClip1);
				currentClips.Add(loadSound("flowers_counting_up_todo_2_c"));
				
	        }
	    }
		
		playAudioList(currentClips);
	}
	
	private void playSucess (){
		
		if(isPlayingSuccessSound)
			return;
		
		
    	voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		currentClips = new List<AudioClip>();
		
	    int _num2 = AGGameState.modInt(numberToFind+dataManager.beeStartingIndex);
	    AudioClip number2 = loadSound(string.Format("number_{0}",_num2));
	    
		currentClips.Add(loadSound("flowers_praise_2_f"));
	    if (numberToFind+dataManager.beeStartingIndex < 0) {
	        currentClips.Add(loadSound("word_negative"));
	    } 
		currentClips.Add(number2);

	    isPlayingSuccessSound = true;
			
		playSoundEffect("Bee_positive_new_01cut");
		playAudioList(currentClips);
	}
	
	void playWrongSound (int wNumber) {
		
		voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		currentClips = new List<AudioClip>();
		currentClips.Add(loadSound("flowers_wrong"));
	    	
		playSoundEffect("Bee_negative_new_01cut");
		playAudioList(currentClips);
	}
	
	void playNumberSound (int _number) {
		
		if(numberToFind > 0 && dataManager.currentLevel >= 9 && dataManager.currentLevel <= 16 ) {
			_number = _number + beeStartingPoint;
		}
		
		if(numberToFind < 0 && dataManager.currentLevel >= 1 && dataManager.currentLevel <= 12 ) {
			_number = _number + beeStartingPoint;
		}
		
		voiceOverSource.clip = null;
		voiceOverSource.Stop();
		
		currentClips = new List<AudioClip>();
		
		if(_number < 0) {
			currentClips.Add(loadSound("word_negative"));
		}
		
		currentClips.Add(loadSound(string.Format("number_{0}", AGGameState.modInt(_number))));
		
		playAudioList(currentClips);
	}
	
	void playInstructionSound () {
		
		string number = null;
	    
	    int _number = AGGameState.modInt(numberToFind);
	    AudioClip numberClip = loadSound(string.Format("number_{0}", _number));
	    
		if(numberToFind > 0 && dataManager.currentLevel >= 9 && dataManager.currentLevel <= 16 ) {
			_number = numberToFind + dataManager.beeStartingIndex;
			numberClip = loadSound(string.Format("number_{0}", AGGameState.modInt(_number)));
	        
			currentClips.Add(loadSound("counting_todo_3"));
			if(_number < 0) {
				currentClips.Add(loadSound("word_negative"));
			}
			currentClips.Add(numberClip);
			
			playAudioList(currentClips);
			return;
		}
		
		if(numberToFind < 0 && dataManager.currentLevel >= 1 && dataManager.currentLevel <= 12 ) {
			_number = numberToFind + dataManager.beeStartingIndex;
			numberClip = loadSound(string.Format("number_{0}", AGGameState.modInt(_number)));
	        
			currentClips.Add(loadSound("counting_down_todo_2"));
			if(_number < 0) {
				currentClips.Add(loadSound("word_negative"));
			}
			currentClips.Add(numberClip);
			
			playAudioList(currentClips);
			return;
		}
		
		//currentClips.Add(loadSound("word_negative"));
		if (numberToFind > 0){
	        int rand = UnityEngine.Random.Range(0,3);
	        
	        if (rand == 0) {
				currentClips.Add(loadSound("counting_todo_6"));
				currentClips.Add(numberClip);
	        } else  if (rand == 1) {
				currentClips.Add(loadSound("adding_todo_1"));
				currentClips.Add(numberClip);
	        
	        } else {
				currentClips.Add(loadSound("adding_todo_1"));
				currentClips.Add(numberClip);	
	        } 
	    } else {
	        currentClips.Add(loadSound("counting_down_todo_1"));
			currentClips.Add(numberClip);
	    }
		
	    
	    playAudioList(currentClips);
	}
	
	
	void playSoundEffect (string effect) {
		soundEffects.clip = loadSound(effect);
		soundEffects.Play();
	}
	
	AudioClip loadSound(string clipName) {
		return (AudioClip)Resources.Load(string.Format("BW_NumberLine/NumberArithmeticSounds/{0}", clipName));
	}
	
	public void beeNoAnimationFinished () {
		resetCurrentQuestion();
	}
}
