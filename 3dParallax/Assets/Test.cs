using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private float zPosition = 50.0f;

	void OnCollisionEnter(Collision other){
		Vector3 v = other.gameObject.transform.position;
		v.z = zPosition;
		other.gameObject.transform.position = v;
	}


}
