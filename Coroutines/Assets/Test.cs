using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {

//		transform.Translate (Vector3.forward * Time.deltaTime);
		var FWD = transform.forward * 10;
		rigidbody.AddForce(new Vector3(0,0,10));
	}
}
