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
		if(other.name == "Cube" && other.GetComponent<Character>().IsRotating() == false)
		renderer.material.color = Color.red;
	}
}