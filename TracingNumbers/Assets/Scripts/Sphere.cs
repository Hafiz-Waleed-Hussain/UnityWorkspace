using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

	void Start () {

		renderer.material.color = Color.blue;
	}

	void Update () {
	


	}

	void OnMouseDrag() {
		renderer.material.color -= Color.white * Time.deltaTime;
		Vector3 v = Input.mousePosition;
		if (v.x > transform.position.x ) {
		
			Debug.Log("OK");
		}
		//		Debug.Log (Input.mousePosition);
	}


}
