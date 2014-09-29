using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	private CharacterController player;
	private Vector3 moveDirection = Vector3.zero;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	#if UNITY_ANDROID
	Vector3 zeroAcc;  //zero reference input.acceleration
	Vector3 currentAcc;  //In-game input.acceleration
	float sensitivityH = 3; //alter this to change the sensitivity of the accelerometer
	float smooth = 0.5f; //determines how smooth the acceleration(horizontal movement, in our case) control is
	float GetAxisH = 0;  //variable used to hold the value equivalent to Input.GetAxis("Horizontal")
	#endif

	void Start () {
	
		player = GetComponent<CharacterController> ();
		#if UNITY_ANDROID
		zeroAcc = Input.acceleration;
		currentAcc = Vector3.zero;
		#endif
	}

	void Update () {

		#if UNITY_ANDROID
		currentAcc = Vector3.Lerp(currentAcc, Input.acceleration-zeroAcc, Time.deltaTime/smooth);
		GetAxisH = Mathf.Clamp(currentAcc.x * sensitivityH, -1, 1);
		int fingerCount = 0;
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				fingerCount++; 
		}
		#endif


		if (player.isGrounded) {
						moveDirection = new Vector3 (Input.GetAxis ("Horizontal")*2f, 0, 0);
			#if UNITY_ANDROID
			moveDirection = new Vector3(GetAxisH*2, 0, 0);
			#endif
						moveDirection = transform.TransformDirection (moveDirection);


						if (Input.GetMouseButton (0)) {
								moveDirection.y = jumpSpeed;   
						}
				}

		moveDirection.y -= gravity * Time.deltaTime;
		player.Move(moveDirection * Time.deltaTime);

	}
}
