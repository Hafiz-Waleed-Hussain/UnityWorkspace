/// <summary>
/// Magnet.
/// this script use for control range(collider) of magnet
/// </summary>

using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {
	
	void OnTriggerEnter(Collider col){
		if(col.tag == "Item"){
			if(col.GetComponent<Item>().typeItem == Item.TypeItem.Coin){
				col.GetComponent<Item>().StartCoroutine(col.GetComponent<Item>().UseAbsorb(this.gameObject));
			}
		}
	}
}
