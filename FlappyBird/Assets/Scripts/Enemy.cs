using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private Vector2 speed = new Vector3(3.0f,0);
	Vector2 direction = new Vector2(-1,0);

	void Start () {
	
	}
	
	void Update () {
		Vector3 movement = new Vector3 (speed.x * direction.x, speed.y * direction.y, 0);
		movement *= Time.deltaTime;
		transform.Translate (movement);

		if (transform.renderer.IsVisibleFrom(Camera.main) == false)
						DestroyObject (gameObject);

	}





}
