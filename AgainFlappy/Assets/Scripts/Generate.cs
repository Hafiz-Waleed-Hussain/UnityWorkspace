using UnityEngine;
using System.Collections;

public class Generate : MonoBehaviour {

	public GameObject rocks;

	int score = 0;

	void Start () {
		InvokeRepeating ("CreateObstacles", 1f, 1.5f);	
	}

	void OnGUI(){

		GUI.color = Color.black;
		GUILayout.Label ("Score : " + score.ToString());
	}

	void CreateObstacles(){
		Instantiate (rocks);
		score += 10;
	}
	
}
