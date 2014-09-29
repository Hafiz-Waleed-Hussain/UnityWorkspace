using UnityEngine;
using System.Collections;

public class Conveyerbelt : MonoBehaviour {

	float speed  = 120.0f;
	private float beltVelocity;
	public float direction;

	void OnCollisionStay2D(Collision2D obj){
		beltVelocity  = speed * Time.deltaTime;
		beltVelocity *= direction;
		obj.gameObject.rigidbody2D.velocity = beltVelocity * transform.right; 
		Debug.Log (gameObject.name);
	}



}
