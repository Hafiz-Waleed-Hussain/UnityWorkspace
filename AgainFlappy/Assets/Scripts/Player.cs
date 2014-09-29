using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Vector2 jumpForce = new Vector2(0,300);

	void Update () {

		if (Input.GetKeyUp ("space")) {
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(jumpForce);
		}

		Vector2 screenPosition = Camera.main.WorldToScreenPoint (transform.position);
		if (screenPosition.y > Screen.height || screenPosition.y < 0) {
			Die();		
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Die();
	}

	void Die(){
		Application.LoadLevel (Application.loadedLevel);
	}
}
