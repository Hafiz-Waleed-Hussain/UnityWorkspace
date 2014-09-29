using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public GameObject mSensorLayer;
	private enum MoveInDirection{UP,DOWN,LEFT,RIGHT};
//	private bool isRotating = false;

	private List<GameObject> sensors;

	private
	void Start () {

		sensors = new List<GameObject> ();
		for (int i=0; i < mSensorLayer.transform.childCount; i++) {
			sensors.Add(mSensorLayer.transform.GetChild(i));		
		}
	}


	void Update(){

		if (Input.GetKey (KeyCode.UpArrow)) {
			
					move (MoveInDirection.UP);
		}else if(Input.GetKey(KeyCode.DownArrow)){
			
					move (MoveInDirection.DOWN);
		}else if(Input.GetKey(KeyCode.RightArrow)){
					move (MoveInDirection.RIGHT);
		}else if(Input.GetKey(KeyCode.LeftArrow)){
					move (MoveInDirection.LEFT);
		}
	}


			private void move ( MoveInDirection moveInDirection) {
					
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
//
//	public void setRotating(bool isRotating){
//		this.isRotating = isRotating;
//	}
//
//	public bool IsRotating(){
//		return this.isRotating;
//	}
//
//	public void stopRotate90Animation(){
//		isRotating = false;
//		GetComponent<Animator> ().SetBool("Rotate90",false);
//
//
//		switch (moveInDirection) {
//		case MoveInDirection.UP:
//			moveInDirection = MoveInDirection.RIGHT;
//			break;
//		case MoveInDirection.RIGHT:
//			moveInDirection = MoveInDirection.DOWN;
//			break;
//		case MoveInDirection.DOWN:
//			moveInDirection = MoveInDirection.LEFT;
//			break;
//		case MoveInDirection.LEFT:
//			moveInDirection = MoveInDirection.UP;
//			break;
//			
//		}
//
//	}
}
