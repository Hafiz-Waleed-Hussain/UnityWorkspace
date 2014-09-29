#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NF_RopesGenerator : MonoBehaviour {

public GameObject _ropePrefab;
public GameObject _ropeWitOutTextPrefab;
public GameObject monkeyModel;	
public Material[] _ropeMat;
public Material[] _hangingObjectMat;
public AnimationClip dropDownAnim;
public ArrayList _ropePositions;
public Transform _ropesParent;
public List<GameObject> _generatedRopes;

private string questionTitle;
private Transform ropeObj;
private NF_BreakRopeJoints _breakRopeJoints;
private GameObject correctRopeRef;	
private ArrayList availShapesMaterials = new ArrayList();
int totalQuestionCount;
ArrayList positionForMonkey;	
private List<GameObject> monkeyList;
private ArrayList fruitPosition;	
bool monkeyGenerated=false;
string pathForObjectResource;
public void generateRopeForQuestion(int questionCount, string loadedQuestionTitle, ArrayList availableShapes, int currentCategoryID, int gameMode,string questionTitleToShow){
	
	GameObject rope = null; 
	GameObject hangingObj = null;	
	GameObject objText = null;
	_generatedRopes =new List<GameObject>();
	_ropePositions = new ArrayList();
	fruitPosition = new ArrayList();
	positionForMonkey = new ArrayList();
	monkeyList =new List<GameObject>();
	totalQuestionCount=questionCount;
	questionTitle = loadedQuestionTitle;
	Debug.Log("Answer:" +questionTitle);
	Debug.Log("GeneratedRopes Count:" + questionCount);	
		
	getPathOfTheCategory(currentCategoryID);
		
	// calculate positions of  Rope	
	for(int i=0;i<totalQuestionCount;i++){
		int xPos =(1280/(totalQuestionCount+1));
		int yPos=780;
			
		if(i%2==0){
				yPos = 750;
			}
		Vector3 ropePosition = new Vector3((xPos + xPos*i)-640,yPos,750);
		_ropePositions.Add(ropePosition);
		}	
		
	for(int i=0;i<availableShapes.Count;i++){
			
			Debug.Log(availableShapes[i]);
		}
	if(totalQuestionCount>0){
			
		for(int i =0; i<totalQuestionCount;i++){		
		if(AGGameState.currentGameIndex == (int) AGGameIndex.k_NamingFruits){
				if(currentCategoryID==(int)NamingFruits_CategoryID.id_Food){
						
					rope = Instantiate(_ropeWitOutTextPrefab,(Vector3)_ropePositions[i],Quaternion.identity) as GameObject;
					hangingObj = rope.transform.FindChild("Object").gameObject;
					hangingObj.renderer.material.mainTexture = Resources.Load(pathForObjectResource+availableShapes[i]) as Texture;
					hangingObj.name = (string)availableShapes[i];
					_breakRopeJoints= rope.GetComponent<NF_BreakRopeJoints>();
					_breakRopeJoints._hangingObject = hangingObj;
					}
				else if(currentCategoryID==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
						
					rope = Instantiate(_ropePrefab,(Vector3)_ropePositions[i],Quaternion.identity) as GameObject;
					hangingObj = rope.transform.FindChild("Object").gameObject;
					objText = rope.transform.FindChild("ObjectText").gameObject;
					hangingObj.renderer.material.mainTexture = Resources.Load(pathForObjectResource+availableShapes[i]) as Texture;
					objText.renderer.material.mainTexture = Resources.Load(NF_Constants._fruitsTextPath+availableShapes[i]) as Texture;
					hangingObj.name = (string)availableShapes[i];
					objText.name = (string)availableShapes[i];
					_breakRopeJoints= rope.GetComponent<NF_BreakRopeJoints>();
					_breakRopeJoints._hangingObject = hangingObj;
					_breakRopeJoints._hangingObjectText = objText;
					_breakRopeJoints.showTextAccordingToCategory(currentCategoryID);
					}
					
					if(hangingObj.name.Equals(loadedQuestionTitle)){
						_breakRopeJoints.setPropertiesOfObject(loadedQuestionTitle);
						correctRopeRef= rope;
					}
			}
			else if(AGGameState.currentGameIndex == (int) AGGameIndex.k_2DShapesColors){
					rope = Instantiate(_ropeWitOutTextPrefab,(Vector3)_ropePositions[i],Quaternion.identity) as GameObject;
					hangingObj = rope.transform.FindChild("Object").gameObject;
					hangingObj.transform.localScale = new Vector3(125,127,1);
					hangingObj.renderer.material.mainTexture = Resources.Load(pathForObjectResource+availableShapes[i]) as Texture;
					
					if(currentCategoryID == (int) SimpleColors_CategoryID.id_2DShapes){
							Debug.Log("Loaded questionTitle:  "+ questionTitleToShow);
							string nameOfShape = (string) availableShapes[i];
							if(nameOfShape.Contains(questionTitleToShow)){
								hangingObj.name = questionTitleToShow;
								}
							else
								hangingObj.name = (string)availableShapes[i];
						}
					else 
						hangingObj.name = (string)availableShapes[i];
					
					_breakRopeJoints= rope.GetComponent<NF_BreakRopeJoints>();
					_breakRopeJoints._hangingObject = hangingObj;
					if(hangingObj.name.Equals(questionTitleToShow)){
						_breakRopeJoints.setPropertiesOfObject(questionTitleToShow);
						correctRopeRef= rope;
						}
				}
				rope.transform.parent = _ropesParent;
				_generatedRopes.Add(rope);
				fruitPosition.Add(hangingObj.transform.position);
				positionForMonkey.Add(new Vector3(rope.transform.position.x,rope.transform.position.y-300,720));
				
			}
			totalQuestionCount--;
		}
		playRopeFallingAnimation();
}

void getPathOfTheCategory(int currentCategoryID){
	
		if(AGGameState.currentGameIndex == (int) AGGameIndex.k_NamingFruits){
				if(currentCategoryID==(int)NamingFruits_CategoryID.id_Food || currentCategoryID==(int)NamingFruits_CategoryID.id_AlplabeticOrder){
						pathForObjectResource = NF_Constants._fruitsPath;
					}
				}
		else if(AGGameState.currentGameIndex == (int) AGGameIndex.k_2DShapesColors){
			
			 if(currentCategoryID==(int)SimpleColors_CategoryID.id_2DShapes){
							pathForObjectResource = NF_Constants._2DShapesPath;
						}
			else if(currentCategoryID==(int)SimpleColors_CategoryID.id_Colors){
							pathForObjectResource = NF_Constants._ColorsPath;
						}
			}
		
}
	
void monkeyAnimatedDown(){
		
	GameObject icky = GameObject.Find("Icky_Model");
	NF_IckyAnimations ickyAnim = (NF_IckyAnimations) icky.GetComponent<NF_IckyAnimations>();
	
		if(ickyAnim.ickyStates == NF_IckyAnimationStates.icky_weeping){
			return;
		}
		else{
		//if(monkey.transform.position.y<50.0f){
			ickyAnim.ickyStates = NF_IckyAnimationStates.icky_weeping;
			ickyAnim.playSoundClip();
		//	}
		}
		
	}
public void createMonkeyForHardMode(){
		
	float randomTime;	
	for(int i=0;i<=totalQuestionCount;i++){
			randomTime = Random.Range(2.0f,6.0f);
			GameObject monkey = Instantiate(monkeyModel,(Vector3)positionForMonkey[i],Quaternion.identity) as GameObject;
			monkeyList.Add(monkey);
			Vector3 pos = (Vector3)fruitPosition[i];
			pos.y = 40.0f;
			iTween.MoveTo(monkey,iTween.Hash("position", pos, "time", randomTime,"onComplete", "monkeyAnimatedDown", "oncompletetarget", gameObject,
			"easetype", iTween.EaseType.linear));
			}
		
		monkeyGenerated=true;
		
	}
	
public GameObject getCorrectRope(){
		return correctRopeRef;
}	
void playRopeFallingAnimation(){
	// drop down the rope animation
	_ropesParent.animation.Play(dropDownAnim.name);
}
	
public void resetRopeGenerator(){
		
	totalQuestionCount=0;	
	correctRopeRef=null;
	_breakRopeJoints.resetRopeProperties();
	_ropesParent.transform.position = new Vector3(0,750,750);
		
	// delete current ropes from scene
	for(int k=0;k<_generatedRopes.Count;k++){
		Destroy(_generatedRopes[k]);
		}
	for(int k=0;k<_ropePositions.Count;k++){
		_ropePositions.RemoveAt(k);
		}
	_ropePositions=null;
	_generatedRopes=null;
		
		
	if(monkeyGenerated){
		for(int k=0;k<monkeyList.Count;k++){
			Destroy(monkeyList[k]);
			}
		for(int k=0;k<positionForMonkey.Count;k++){
			positionForMonkey.RemoveAt(k);
			}
		positionForMonkey=null;
		monkeyList=null;
			
		}
		
}
		
}
