using UnityEngine;
using System.Collections;

public class TextTest : MonoBehaviour {

	private Vector2 midPoint;
	private Vector2 currentDistance;
	private Vector2 previousDistance;

	private Vector3 currentPosition;

	private float scaleFactor = .03f;

	void Update () {


		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {

			midPoint = new Vector2((Input.GetTouch(0).position.x + Input.GetTouch(0).deltaPosition.x)/2, (Input.GetTouch(0).position.y + Input.GetTouch(0).deltaPosition.y)/2 );
			midPoint = Camera.main.ScreenToWorldPoint(midPoint);

			currentPosition = Input.GetTouch (0).position;
			currentPosition = new Vector3 (currentPosition.x, currentPosition.y, transform.position.z);
			currentPosition = Camera.main.ScreenToWorldPoint (currentPosition);

								currentPosition = new Vector3 (currentPosition.x, currentPosition.y, transform.position.z);
								transform.position = Vector3.Lerp (transform.position, currentPosition, Time.deltaTime * 10);
		}
		else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {

			midPoint = new Vector2((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x)/2, (Input.GetTouch(0).position.y + Input.GetTouch(1).position.y)/2 );
			midPoint = Camera.main.ScreenToWorldPoint(midPoint);
		

			if(renderer.bounds.Contains(midPoint)){

							currentDistance = Input.GetTouch(0).position - Input.GetTouch(1).position;
							previousDistance = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - 
								(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
		

							float touchDelta = currentDistance.magnitude - previousDistance.magnitude;
							if(touchDelta > 0){
				
					Vector3 scale = new Vector3(transform.localScale.x+scaleFactor, transform.localScale.y+scaleFactor,1);
								transform.localScale = scale;
				
							}else if(touchDelta < 0){
				
					Vector3 scale = new Vector3(transform.localScale.x+scaleFactor *-1, transform.localScale.y+scaleFactor *-1,1);
								transform.localScale = scale;
				
							}

			}

		}
	}

}
