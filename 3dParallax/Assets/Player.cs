using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	private CharacterController player;
	private float speed = 1;
	private float jump = 8;
	private float gravity = 30;
	private Vector3 moveDirection;

	void Start(){
		player = GetComponent<CharacterController> ();
	}

	void Update(){

		if (player.isGrounded) {
		
			moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,0);
			
		if (Input.GetMouseButton (0)) {
			moveDirection.y = jump;		
		}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		player.Move (moveDirection * Time.deltaTime);

	}
}
