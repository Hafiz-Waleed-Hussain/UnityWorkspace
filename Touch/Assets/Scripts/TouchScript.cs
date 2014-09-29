using UnityEngine;
using System.Collections;

public class TouchScript : MonoBehaviour {

	public int i_comfort;
	private GameObject targetObject;
	private Vector2 previous;
	private Vector2 current;
	private float delte;



	void Update () {

		if (Input.touchCount == 1) {

			if(Input.GetTouch(0).phase == TouchPhase.Began){
				previous = Input.GetTouch(0).position;
			}

			if( Input.GetTouch(0).phase == TouchPhase.Ended){
				current = Input.GetTouch(0).position;
				delte = current.magnitude - previous.magnitude;

				if( Mathf.Abs(delte) > i_comfort){
					Debug.Log ("Swipe");

					if( delte > 0){
						if( Mathf.Abs(current.x - previous.x) > Mathf.Abs(current.y - previous.y)){
							Debug.Log("Left");
						} else{
							Debug.Log("Bottom");
						}
					}else{

						if( Mathf.Abs(current.x - previous.x) > Mathf.Abs(current.y - previous.y)){
							Debug.Log("Right");
						} else{
							Debug.Log("Top");
						}
					}


				}else{
					if(Input.GetTouch(0).tapCount == 1){
						Debug.Log ("Single Tap");
						
						RaycastHit hit ;
						Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
						if(Physics.Raycast(ray,out hit)){
							Debug.Log(hit.transform.name);
							
							if(targetObject != null &&  targetObject.name !=hit.transform.name){
								targetObject.particleSystem.Stop();
							}
							targetObject = GameObject.Find(hit.transform.name);
							targetObject.particleSystem.Play();
						}
					}
					else if(Input.GetTouch(0).tapCount == 2){
						
						if( targetObject != null){
							var pos = Vector3.zero;
							pos.x = Input.GetTouch(0).position.x;
							pos.y = Input.GetTouch(0).position.y;
							pos.z = Mathf.Abs(Camera.main.transform.position.z - targetObject.transform.position.z);
							
							targetObject.transform.position = Camera.main.ScreenToWorldPoint(pos);
						}
					}

				}
			}
		}

	}
}
