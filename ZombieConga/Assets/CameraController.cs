using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed = 5f;
	private Vector3 newPosition;

	void Start () {
		newPosition = transform.position;
	}
	
	void Update () {

		newPosition.x += Time.deltaTime * speed;
		transform.position = newPosition;
	}
}
