using UnityEngine;
using System.Collections;

public class RotationHandler : MonoBehaviour {

	void Start () {
	
	}

	Vector3 point = new Vector3(20,0,0);

	float elaspedTime = 0;
	bool isFirstTime = true;
	void Update () {

		if (isFirstTime) {
			Vector3 s = transform.rotation.eulerAngles;
			s.Normalize();
			point.Normalize();		
			Vector3 cross = Vector3.Cross(point,s);
			Debug.Log(cross);

			rigidbody.AddTorque(cross*100);
			isFirstTime = false;
//			elaspedTime = 0;
		}
		elaspedTime += Time.deltaTime;
	}
}
