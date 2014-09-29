using UnityEngine;
using System.Collections;

public class ZoombieController : MonoBehaviour {

	public float moveSpeed;
	private Vector3 moveDirection;
	public float turnSpeed;

	[SerializeField]
	private PolygonCollider2D[] colliders;
	private int currentColliderIndex = 0;



	public void setColliderForSprite(int spriteNum){

		colliders [currentColliderIndex].enabled = false;
		currentColliderIndex = spriteNum;
		colliders [currentColliderIndex].enabled = true;
	}

	void Start(){
		moveDirection = Vector3.right;
	}

	// Update is called once per frame
	void Update () {
		Vector3 currentPosition = transform.position;
		if( Input.GetButton("Fire1")) {
			Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();
			Vector3 target = moveDirection * moveSpeed + currentPosition;
			transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
		}

		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = 
			Quaternion.Slerp( transform.rotation, 
			                 Quaternion.Euler( 0, 0, targetAngle ), 
			                 turnSpeed * Time.deltaTime );
		enforceBounds ();
	}

	public void OnTriggerEnter2D(Collider2D collider){
		Debug.Log ("Hit :" + collider.gameObject);
	}

	public void enforceBounds(){

		Vector3 newPosition = transform.position;
		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;
	
		float xDistance = mainCamera.aspect * mainCamera.orthographicSize;
		xDistance -= .5f;
		float xMax = cameraPosition.x + xDistance;
		float xMin = cameraPosition.x - xDistance;

		if (newPosition.x > xMax || newPosition.x < xMin) {
		
			newPosition.x = Mathf.Clamp(newPosition.x,xMin,xMax);
			moveDirection.x = -moveDirection.x;
		}


		float yDistance = mainCamera.orthographicSize;
		yDistance -= .5f;
		float yMax = cameraPosition.y + yDistance;
		float yMin = cameraPosition.y - yDistance;

		if( newPosition.y > yMax || newPosition.y < yMin){
			newPosition.y = Mathf.Clamp(newPosition.y,yMin,yMax);
			moveDirection.y = -moveDirection.y;
		}



		transform.position = newPosition;

	}
}
