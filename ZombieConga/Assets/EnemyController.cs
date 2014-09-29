using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float speed = -1;
	private Transform spawnPoint;

	void Start () {

		spawnPoint = GameObject.Find ("SpawnPoint").transform;
		rigidbody2D.velocity = new Vector2 (speed, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible(){
		if (Camera.main == null)
						return;
		float yMax = Camera.main.orthographicSize;
		transform.position = new Vector3 (spawnPoint.position.x, Random.Range (-yMax, yMax), spawnPoint.position.z);
	}
}
