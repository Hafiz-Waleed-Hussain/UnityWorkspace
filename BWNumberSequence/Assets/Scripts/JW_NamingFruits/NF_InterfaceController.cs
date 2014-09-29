#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;

public class NF_InterfaceController : MonoBehaviour {
	
public GameObject agnitusBtnPrefab;
public GameObject jungleBtnPrefab;
GameObject agnitusButton;
GameObject jungleWButton;
// Use this for initialization
void Start () {
		
	interfaceIcons(); // setup interface icons
}
	
	// Update is called once per frame
void Update (){
		
	}
	
void OnEnable () {
	FingerGestures.OnTap += FingureGestures_OnTap;
	}
	
void interfaceIcons(){
	// instantiate interface Icons and set their position

	// agnitus Home Button
		agnitusButton= Instantiate(agnitusBtnPrefab,agnitusBtnPrefab.transform.position,Quaternion.identity) as GameObject;
	// jungle world button
		jungleWButton = Instantiate(jungleBtnPrefab,jungleBtnPrefab.transform.position,Quaternion.identity) as GameObject;
	// Child Pic and Name
		
	// Star Count
	
}

void agnitusHomeButton(){
		Debug.Log("Clicked on AgnitusHomeButton");
}

void jungleWorldMenu(){
		Debug.Log("Clicked on JungleWorldHomeButton");
		Application.LoadLevel("FruitGame");
}

void getStarCount(){
}

void FingureGestures_OnTap (Vector2 fingurePos, int tapCount) {
	RaycastHit hit;
	Ray ray;
	ray = Camera.main.ScreenPointToRay(fingurePos);
   	if(Physics.Raycast(ray, out hit,10000)){
		
		if(hit.collider.tag == "agnitusHomeBtn"){
				
					agnitusHomeButton();
				}
		else if(hit.collider.tag == "jungleWorldBtn"){
				
					jungleWorldMenu();
				}
		}
}

void OnDisable () {
	FingerGestures.OnTap -= FingureGestures_OnTap;
	}
	

}
