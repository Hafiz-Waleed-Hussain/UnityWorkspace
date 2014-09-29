using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]

public class ZoomRotateGesture : MonoBehaviour {



	#region Public fields
	public float zoomComfortZone = 5;
	public float zoomSmoothNess = 40;
	public float scaleFactor = 1f;
	public float rotationSmoothNess = 0;
	public float movementSpeed = 150;

//	[SerializeField]
//	public GestureState mGestureState = GestureState.IDLE;


	#endregion	

	#region Private Fields
	private Vector2 current;
	private Vector2 midPoint;
	private Vector2 previous;
	private float delta;
	private float angle;
	private float MINIMUM_SCALE = .5f;
	private float MAXIMUM_SCALE = 8f;
	private float SCREEN_HALF_WIDTH;
	private float SCREEN_HALF_HIGHT;

	#endregion

	#region MonoBehaviour Life Cycle Methods

	void Start(){
		SCREEN_HALF_HIGHT = Camera.main.orthographicSize ;
		SCREEN_HALF_WIDTH = Camera.main.aspect * Camera.main.orthographicSize;
	}
	
	void Update () {
		if (isTwoFingerGuster () && isCollide () && (GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE || GestureStateManager.mGestureState != GestureStateManager.GestureState.MOVE )) {
						setUpValues ();
						rotationGestureHandler ();
						zoomGestureHandler ();
		} else if (isMoveGesture () && isCollide () && (GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE || GestureStateManager.mGestureState == GestureStateManager.GestureState.MOVE )) {
						move ();
			GestureStateManager.mGestureState = GestureStateManager.GestureState.MOVE;
				} else if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
			GestureStateManager.mGestureState = GestureStateManager.GestureState.IDLE;
			}
	}

	void OnDestroy(){
		GestureStateManager.mGestureState = GestureStateManager.GestureState.IDLE;
	}

	#endregion

	#region Gesture Event Handlers


	private void move(){

		Vector3 pos =  new Vector3();
		pos.x =  Input.GetTouch(0).position.x;
		pos.y =  Input.GetTouch(0).position.y;
		pos.z =  Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
		pos = Camera.main.ScreenToWorldPoint(pos);
		if (checkXBounds (pos) && checkYBounds (pos)) {
			transform.position =  Vector3.Lerp(transform.position,pos, (Time.deltaTime* movementSpeed));
		}
		
	}

	private void zoomIn(){

		debug ("Zoom In");
		float factor = transform.localScale.x * scaleFactor;

		Vector3 scale = new Vector3 (transform.localScale.x + factor, transform.localScale.y + factor, 1);
		scale.x = (scale.x > MAXIMUM_SCALE) ? MAXIMUM_SCALE : scale.x;
		scale.y = (scale.y > MAXIMUM_SCALE) ? MAXIMUM_SCALE : scale.y;
		scaleFromPosition (scale, midPoint);


	}
	
	private void zoomOut(){
		debug ("Zoom Out");
		float factor = transform.localScale.x * scaleFactor;

		Vector3 scale = new Vector3 (transform.localScale.x - factor, transform.localScale.y - factor, 1);
		scale.x = (scale.x < MINIMUM_SCALE) ? MINIMUM_SCALE : scale.x;
		scale.y = (scale.y < MINIMUM_SCALE) ? MINIMUM_SCALE : scale.y;
		scaleFromPositionForZoomOut (scale, midPoint);

	}

	private void clockwiseRotate(float rotationAngle){
		debug ("Clockwise Rotate");
		if (checkXBounds (transform.position) && checkYBounds (transform.position)) {

			float a = rotationAngle * rotationSmoothNess*-1;
			Debug.Log("Clock :"+rotationAngle+" : "+a);

			transform.Rotate (Vector3.forward, a);
//			transform.Rotate (Vector3.forward, -1 * .1f);

//						transform.rotation = Quaternion.Slerp(transform.rotation,
//			                                      Quaternion.Euler(0, 0, -1*rotationAngle*rotationSmoothNess),
//			                                      Time.deltaTime * 200);

		}
	}

	private void antiClockwiseRotate(float rotationAngle){
		debug ("Anti Clockwise Rotate");
		if (checkXBounds (transform.position) && checkYBounds (transform.position)) {
			float a = rotationAngle * rotationSmoothNess;
			Debug.Log("AntiClock :"+rotationAngle+" : "+a);

			transform.Rotate (Vector3.forward, a);
//			transform.Rotate (Vector3.forward, 0.1f);
//			transform.Rotate(Vector3.forward,0.1f);

//						transform.rotation = Quaternion.Slerp(transform.rotation,
//			                                      Quaternion.Euler(0, 0, rotationAngle*rotationSmoothNess),
//			                                      Time.deltaTime * 200);
		}
	}

	#endregion



	#region Private Methods
	private bool isTwoFingerGuster(){
		return Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Moved && Input.GetTouch (1).phase == TouchPhase.Moved;
	}

	private bool isMoveGesture(){
		return Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved;
	}
	
	private bool isDoubleTap(){
		return Input.touchCount == 1 && Input.GetTouch(0).tapCount == 2; 
	}


	private void setUpValues(){
		current = Input.GetTouch(0).position - Input.GetTouch(1).position;
		previous = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
		delta = current.magnitude - previous.magnitude;
		angle = Vector2.Angle(current,previous); 

//		Debug.Log ("First: " + angle);
		//


//		 Vector2 movement = current - previous;
//		movement.Normalize();
//
//		angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
		//		Debug.Log ("second: " + angle);

		//



		midPoint = new Vector2(((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x)/2), ((Input.GetTouch(0).position.y + Input.GetTouch(1).position.y)/2));
		midPoint = Camera.main.ScreenToWorldPoint(midPoint);

	}
	
	private void rotationGestureHandler(){

		if(angle > .3f && (GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE || GestureStateManager.mGestureState == GestureStateManager.GestureState.ROTATE || GestureStateManager.mGestureState == GestureStateManager.GestureState.ZOOM)){
			GestureStateManager.mGestureState = GestureStateManager.GestureState.ROTATE;
			Vector3 crossProduct = Vector3.Cross(current,previous);
			if(crossProduct.z > 0){
				clockwiseRotate(angle);
			}else{
				antiClockwiseRotate(angle);
			}
		}

	}

	private void zoomGestureHandler(){
		if(Mathf.Abs(delta) > zoomComfortZone && (GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE || GestureStateManager.mGestureState == GestureStateManager.GestureState.ZOOM || GestureStateManager.mGestureState == GestureStateManager.GestureState.ROTATE) ){
			GestureStateManager.mGestureState = GestureStateManager.GestureState.ZOOM;
			transform.position = midPoint;
			if(delta > 0){
				zoomIn();
			}else{
				zoomOut();
			}
			
		}	 
	}

	private bool isCollide(){
		PaitingBoard.isNotMovingGesture = false;
		Vector3 position =  Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		RaycastHit2D hit = Physics2D.Raycast(position,Vector2.zero);
		if (hit.collider != null) {
			return hit.collider.name == transform.name;
		}
		PaitingBoard.isNotMovingGesture = true;
		return false;
	}

	private void debug(object message){
		Debug.Log (message);
	}


	static Vector3 prevPos = Vector3.zero;
	private void scaleFromPositionForZoomOut(Vector3 scale, Vector3 fromPos)
	{
		if (!fromPos.Equals (prevPos)) {
				Vector3 prevParentPos = transform.position;
				transform.position = fromPos;    
				Vector3 diff = transform.position - prevParentPos;
				Vector3 pos = new Vector3 (diff.x / transform.localScale.x * -1, diff.y / transform.localScale.y * -1, transform.position.z);
				transform.localPosition = new Vector3 (transform.localPosition.x + pos.x, transform.localPosition.y + pos.y, pos.z);
		}
			
		transform.localScale = Vector3.Lerp(transform.localScale,scale,Time.deltaTime * zoomSmoothNess);

		prevPos = fromPos;

	}

	private void scaleFromPosition(Vector3 scale, Vector3 fromPos)
	{

		if (!fromPos.Equals (prevPos)) {
			Vector3 prevParentPos = transform.position;

			if (checkXBounds (prevParentPos) && checkYBounds (prevParentPos) && checkXBounds (fromPos) && checkYBounds (fromPos)) {
					transform.position = fromPos;    
					Vector3 diff = transform.position - prevParentPos;
					Vector3 pos = new Vector3 (diff.x / transform.localScale.x * -1, diff.y / transform.localScale.y * -1, transform.position.z);
					transform.localPosition = new Vector3 (transform.localPosition.x + pos.x, transform.localPosition.y + pos.y, pos.z);
				}
			}
		if (checkXBounds (scale) && checkYBounds (scale) && checkXBounds (fromPos) && checkYBounds (fromPos)) {

			transform.localScale = Vector3.Lerp(transform.localScale,scale,Time.deltaTime * zoomSmoothNess);
			prevPos = fromPos;

		}

	}

	private bool checkXBounds(Vector3 pos){
		Bounds textBounds = transform.renderer.bounds;
		if (pos.x >= -SCREEN_HALF_WIDTH + textBounds.size.x / 2 && pos.x <= SCREEN_HALF_WIDTH - textBounds.size.x / 2)
						return true;
		return true;
	}

	private bool checkYBounds(Vector3 pos){
		Bounds textBounds = transform.renderer.bounds;
		if (pos.y >= -SCREEN_HALF_HIGHT + textBounds.size.y / 2 && pos.y <= SCREEN_HALF_HIGHT - textBounds.size.y / 2)
						return true;
		return true;
	}


	#endregion
	
}