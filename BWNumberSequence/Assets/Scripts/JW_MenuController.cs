#region AGNITUS 2013
/* JungleWorld- Menu
 * Developer - Asema Hassan
 * Unity3D*/
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JW_MenuController : MonoBehaviour{
	
public GameObject gameIconPrefab;
private List<GameObject> _gameIcons = new List<GameObject>();
private AGGameIndex _gameIconIndex;
private JW_GameIcon gameIconScript;
private Hashtable gameIconsTable = new Hashtable();
	
public Material labelPondGameMat;
public Material labelFruitGameMat;
public Material labelMemoryAnimalMat;
public Material label2DShapesMat;
public GameObject nameLabelPrefab;

// Use this for initialization	
void gameIconProperties(){
	
	gameIconsTable.Add(AGGameIndex.k_NamingFruits,"HangGame-Fruits_HomeScreen_icon");
	gameIconsTable.Add(AGGameIndex.k_ShapePond,"ShapePondGame_HomeScreen_icon");
	gameIconsTable.Add(AGGameIndex.k_MemoryAnimal,"Mem_Animal_Game_HomeScreen_icon");
	gameIconsTable.Add(AGGameIndex.k_2DShapesColors,"HangGame-Shapes_HomeScreen_icon");
}

void Awake(){
	gameIconProperties();
	}
void Start (){
	createGameIcons();
}
	
void OnEnable () {
	FingerGestures.OnTap += FingureGestures_OnTap;
}
	
Vector3 gameIconPositions(){
	
	switch(_gameIconIndex){
		case AGGameIndex.k_NamingFruits:
			return new Vector3(-265,120,-150);
		case AGGameIndex.k_ShapePond:
			return new Vector3(0,120,-150);
		case AGGameIndex.k_2DShapesColors:
			return new Vector3(-265,-120,-150);
		case AGGameIndex.k_MemoryAnimal:
			return new Vector3(265,120,-150);
		default: return new Vector3(0,0,0);
		}
	}
	
void createGameIcons(){
		
	string textureName;
	Vector3 iconPosition;
		
	foreach(DictionaryEntry entry in gameIconsTable){
			
		GameObject gameIcon =Instantiate(gameIconPrefab) as GameObject;
		gameIconScript = (JW_GameIcon)gameIcon.GetComponent<JW_GameIcon>();
		_gameIconIndex = (AGGameIndex)System.Convert.ToInt32(entry.Key);
		textureName = (string)entry.Value;
		iconPosition = gameIconPositions();
		gameIconScript.gameIconInitializer((int)_gameIconIndex, textureName, iconPosition);
		_gameIcons.Add(gameIcon);
			
			GameObject label = Instantiate(nameLabelPrefab) as GameObject;
			if(_gameIconIndex == AGGameIndex.k_ShapePond) {
				label.renderer.material = labelPondGameMat;
				label.transform.position = new Vector3(3,10,-150);
				label.transform.localScale = new Vector3(160, 28, 1);
			} else if(_gameIconIndex == AGGameIndex.k_NamingFruits) {
				label.renderer.material = labelFruitGameMat;
				label.transform.position = new Vector3(-262,10,-150);
				label.transform.localScale = new Vector3(160, 28, 1);
			} else if(_gameIconIndex == AGGameIndex.k_MemoryAnimal) {
				label.renderer.material = labelMemoryAnimalMat;
				label.transform.position = new Vector3(262,10,-150);
				label.transform.localScale = new Vector3(160, 28, 1);
			}
			else if(_gameIconIndex == AGGameIndex.k_2DShapesColors) {
				label.renderer.material = label2DShapesMat;
				label.transform.position = new Vector3(-265,-224,-150);
				label.transform.localScale = new Vector3(160, 28, 1);
			}
		}
}
	
void FingureGestures_OnTap (Vector2 fingurePos, int tapCount){
		
	RaycastHit hit;
	Ray ray;
	ray = Camera.main.ScreenPointToRay(fingurePos);
   	if(Physics.Raycast(ray, out hit,10000)){
			GameObject iconSelected = hit.transform.gameObject;
			gameIconScript = (JW_GameIcon)iconSelected.GetComponent<JW_GameIcon>();
			if(gameIconScript){
				gameIconScript.goToGame();
				}
		}
}
void OnDisable () {
	FingerGestures.OnTap -= FingureGestures_OnTap;
	}
	
}
