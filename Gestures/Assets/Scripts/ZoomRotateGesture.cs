using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]

public class ZoomRotateGesture : MonoBehaviour {


	#region Public fields
	public float zoomComfortZone = 3;
	public float scaleFactor = .07f;
	public float rotationSmoothNess = 10;

	#endregion	

	#region Private Fields
	private Vector2 current;
	private Vector2 midPoint;
	private Vector2 previous;
	private float delta;
	private float angle;
	private float MINIMUM_SCALE = 1f;
	private float MAXIMUM_SCALE = 6f;


	#endregion

	#region MonoBehaviour Life Cycle Methods
	void Update () {

		if (isTwoFingerGuster () && isCollide ()) {
			setUpValues ();
			rotationGestureHandler ();
			zoomGestureHandler ();
		} else if (isMoveGesture () && isCollide()) {
			move();
		}
	}

	#endregion

	#region Gesture Event Handlers
	private void move(){
		Vector3 pos =  new Vector3();
		pos.x =  Input.GetTouch(0).position.x;
		pos.y =  Input.GetTouch(0).position.y;
		pos.z =  Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
		pos = Camera.main.ScreenToWorldPoint(pos);
		transform.position = pos;
		
	}

	private void zoomIn(){
		debug ("Zoom In");
		Vector3 scale = new Vector3 (transform.localScale.x + scaleFactor, transform.localScale.y + scaleFactor, 1);
		scale.x = (scale.x > MAXIMUM_SCALE) ? MAXIMUM_SCALE : scale.x;
		scale.y = (scale.y > MAXIMUM_SCALE) ? MAXIMUM_SCALE : scale.y;

		scaleFromPosition (scale, midPoint);
	}

	private void zoomOut(){
		debug ("Zoom Out");
		Vector3 scale = new Vector3 (transform.localScale.x - scaleFactor, transform.localScale.y - scaleFactor, 1);
		scale.x = (scale.x < MINIMUM_SCALE) ? MINIMUM_SCALE : scale.x;
		scale.y = (scale.y < MINIMUM_SCALE) ? MINIMUM_SCALE : scale.y;

		scaleFromPosition (scale, midPoint);

	}

	private void clockwiseRotate(float rotationAngle){
		debug ("Clockwise Rotate");
		transform.Rotate (Vector3.forward, rotationAngle*-1*rotationSmoothNess);
	}

	private void antiClockwiseRotate(float rotationAngle){
		debug ("Anti Clockwise Rotate");
		transform.Rotate (Vector3.forward, rotationAngle*rotationSmoothNess);
	}

	#endregion

	#region Private Methods
	private bool isTwoFingerGuster(){
		return Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Moved && Input.GetTouch (1).phase == TouchPhase.Moved;
	}

	private bool isMoveGesture(){
		return Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved;
	}


	private void setUpValues(){
		current = Input.GetTouch(0).position - Input.GetTouch(1).position;
		previous = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
		delta = current.magnitude - previous.magnitude;
		angle = Vector2.Angle(current,previous);    
		midPoint = new Vector2(((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x)/2), ((Input.GetTouch(0).position.y + Input.GetTouch(1).position.y)/2));
		midPoint = Camera.main.ScreenToWorldPoint(midPoint);

	}
	
	private void rotationGestureHandler(){
		if(angle > .5f){
			Vector3 crossProduct = Vector3.Cross(current,previous);
			if(crossProduct.z > 0){
				clockwiseRotate(angle);
			}else{
				antiClockwiseRotate(angle);
			}
		}

	}

	private void zoomGestureHandler(){
		if(Mathf.Abs(delta) > zoomComfortZone){
			transform.position = midPoint;
			if(delta > 0){
				zoomIn();
			}else{
				zoomOut();
			}
			
		}	 
	}

	private bool isCollide(){
		Vector3 position =  Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		RaycastHit2D hit = Physics2D.Raycast(position,Vector2.zero);
		return hit.collider.name == transform.name;

	}

	private void debug(object message){
		Debug.Log (message);
	}


	static Vector3 prevPos = Vector3.zero;
	private void scaleFromPosition(Vector3 scale, Vector3 fromPos)
	{
		if(!fromPos.Equals(prevPos))
		{
			Vector3 prevParentPos = transform.position;
			transform.position = fromPos;    
			Vector3 diff = transform.position - prevParentPos;
			Vector3 pos = new Vector3(diff.x/transform.localScale.x*-1, diff.y/transform.localScale.y*-1, transform.position.z);
			transform.localPosition = new Vector3(transform.localPosition.x + pos.x, transform.localPosition.y+pos.y, pos.z);
		}
		transform.localScale = scale;
		prevPos = fromPos;
	}

	#endregion
	
}