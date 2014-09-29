using UnityEngine;
using System.Collections;

public class Gestures : MonoBehaviour {

	public Camera camera;
	public int zoom = 5;
	public int rotate = 5;
	public GameObject target;

	private Vector2 previous;
	private Vector2 current;
	private float delta;
	private float comfort_zone = 3;
	private float angle;



	void Update () {
	
		if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Moved && Input.GetTouch (1).phase == TouchPhase.Moved) {

			current = Input.GetTouch(0).position - Input.GetTouch(1).position;
			previous = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));

			delta = current.magnitude - previous.magnitude;
			angle = Vector2.Angle(previous,current);
			if(angle > 1.0){
				if(Vector3.Cross(current,previous).z < 0){
					target.transform.Rotate(Vector3.up, angle*-1* rotate);
				}else{
					target.transform.Rotate(Vector3.forward, angle*rotate);
				}
			}



			if(Mathf.Abs(delta) > comfort_zone ){
				if( delta > 0 ){

					camera.fieldOfView = Mathf.Clamp(Mathf.Lerp(camera.fieldOfView, camera.fieldOfView - (Mathf.Abs(delta) * zoom), Time.deltaTime*zoom),10,70);
				}else{
					camera.fieldOfView = Mathf.Clamp(Mathf.Lerp(camera.fieldOfView, camera.fieldOfView + (Mathf.Abs(delta) * zoom), Time.deltaTime*zoom),10,70);
				}

			}

		}
	}
}
