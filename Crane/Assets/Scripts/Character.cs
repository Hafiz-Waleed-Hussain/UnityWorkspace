using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	// Use this for initialization
	private enum MoveInDirection{UP,DOWN,LEFT,RIGHT};
	private MoveInDirection moveInDirection;
	private bool isRotating = false;
	void Start () {
		moveInDirection = MoveInDirection.UP;
	}
	
	public void move (int direction) {
	
		if (direction == 1)
						moveInDirection = MoveInDirection.RIGHT;

		else if (direction == 2)
			
			moveInDirection = MoveInDirection.DOWN;
		else if (direction == 3)
			
			moveInDirection = MoveInDirection.UP;
		else
			moveInDirection = MoveInDirection.LEFT;
		if (isRotating == false) {
				
						Vector3 currentPosition = transform.position;
						Vector3 latestPosition = new Vector3 ();

						switch (moveInDirection) {
						case MoveInDirection.UP:
								currentPosition.y = currentPosition.y + .1f;
								break;
						case MoveInDirection.RIGHT:
								currentPosition.x = currentPosition.x + .1f;
								break;
						case MoveInDirection.DOWN:
								currentPosition.y = currentPosition.y - .1f;
								break;
						case MoveInDirection.LEFT:
								currentPosition.x = currentPosition.x - .1f;
								break;
						}

						transform.position = currentPosition;
				}
	}

	public void setRotating(bool isRotating){
		this.isRotating = isRotating;
	}

	public bool IsRotating(){
		return this.isRotating;
	}

	public void stopRotate90Animation(){
		isRotating = false;
		GetComponent<Animator> ().SetBool("Rotate90",false);


		switch (moveInDirection) {
		case MoveInDirection.UP:
			moveInDirection = MoveInDirection.RIGHT;
			break;
		case MoveInDirection.RIGHT:
			moveInDirection = MoveInDirection.DOWN;
			break;
		case MoveInDirection.DOWN:
			moveInDirection = MoveInDirection.LEFT;
			break;
		case MoveInDirection.LEFT:
			moveInDirection = MoveInDirection.UP;
			break;
			
		}

	}

	public void removeJoint(){
		gameObject.GetComponent<FixedJoint> ().connectedBody = null;
	}
}
