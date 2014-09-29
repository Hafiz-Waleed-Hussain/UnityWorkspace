using UnityEngine;
using System.Collections.Generic;

public class Parallax : MonoBehaviour {

	private float speed = 5f;

	void Update () {
		float z = gameObject.transform.position.z;
			z -= .1f * speed;
			Vector3 position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,z );
			gameObject.transform.position = position;
		}

	private float zPosition = 50.0f;

//	void OnTriggerEnter(Collider other) {
//
//			Vector3 v = gameObject.transform.position;
//			v.z = zPosition;
//			gameObject.transform.position = v;
//
//	}

	void OnBecameInvisible(){
		Debug.Log (gameObject.name);
		Vector3 position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,65 );
		gameObject.transform.position = position;

	}
}
