using UnityEngine;
using System.Collections;

public class COnveyerBeltHandler : MonoBehaviour {
	float speed  = 120.0f;
	private float beltVelocity;
	private float direction;

	public GameObject g;
	void Start () {
	
	}
	
	void Update () {
	


	}

	void OnCollisionStay2D(Collision2D obj){
		Debug.Log (gameObject.name);

		if( Input.GetButton("Fire1")) {
			Vector3 mouseInputAccordingToWorldPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			mouseInputAccordingToWorldPoint.z = gameObject.transform.position.z;
			if(gameObject.renderer.bounds.Contains(mouseInputAccordingToWorldPoint)){
				if(gameObject.name == "left")
					direction = -1;
				else
					direction = 1;
				beltVelocity  = speed * Time.deltaTime;
				beltVelocity *= direction;
				g.gameObject.rigidbody2D.velocity = beltVelocity * transform.right; 
				//				character.transform.position = new Vector3(
				
			}
		}

	}
}
