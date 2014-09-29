using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "loader" && gameObject.name == "testblock") {
			other.GetComponent<FixedJoint>().connectedBody = gameObject.rigidbody;	
			renderer.material.color = Color.red;
				}
	}
}