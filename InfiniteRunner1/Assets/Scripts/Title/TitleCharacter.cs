using UnityEngine;
using System.Collections;

public class TitleCharacter : MonoBehaviour {

	public GameObject[] players;

	void Start(){
		for(int i = 0; i < players.Length; i++){
			players[i].SetActive(false);
		}
		players [PlayerPrefs.GetInt ("SelectPlayer")].SetActive (true);
	}
}
