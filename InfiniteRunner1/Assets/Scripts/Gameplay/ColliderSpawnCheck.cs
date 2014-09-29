/// <summary>
/// Collider spawn check.
/// This script use to check collider when character hit object(obstacle)
/// </summary>

using UnityEngine;
using System.Collections;

public class ColliderSpawnCheck : MonoBehaviour {

	public bool isCollision = false;
	public GameObject headParent;
	public string nameColliderHit;
	public bool checkByName;
	public bool checkByTag;
	public float nextPos;
	
	private GameObject player;
	
	public void Update(){
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player");
		}else{
			if(player.transform.position.z >= this.transform.position.z){
				isCollision = true;	
			}
		}
	}
}
