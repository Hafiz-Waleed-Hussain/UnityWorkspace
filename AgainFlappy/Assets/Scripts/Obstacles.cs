using UnityEngine;
using System.Collections;

public class Obstacles : MonoBehaviour {

	public Vector2 velocity = new Vector2(-4,0);
	public int range = 4;

	void Start () {
		rigidbody2D.velocity = velocity;
		transform.position = new Vector3 (transform.position.x, transform.position.y - range * Random.value, transform.position.z); 
	}
	
}
