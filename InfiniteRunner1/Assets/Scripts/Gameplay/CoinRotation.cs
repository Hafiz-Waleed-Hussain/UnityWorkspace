/// <summary>
/// Coin rotation.
/// this script use for rotate coin when coin is near character
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinRotation : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if(col.tag == "Item"){
			if(col.GetComponent<Item>().itemRotate != null){
				col.GetComponent<Item>().itemRotate.PlayCoin();
			}

			if(col.GetComponent<Item>().typeItem == Item.TypeItem.Moving_Obstacle){
				col.GetComponent<Item>().UseMovingItem();
			}
		}
	}
	
}
