using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public static void SaveCoin(int coin){
		PlayerPrefs.SetInt ("Coin", coin);
	}

	public static int LoadCoin(){
		return PlayerPrefs.GetInt ("Coin", 0);
	}
}
