using UnityEngine;
using System.Collections;

public class SingleTouchScript : MonoBehaviour {


	private Vector2 previous;
	private Vector2 current;
	private float delta;
	private int minSwipeZone = 3;
	private ColorPalleteGUI mColorPalleteGUI;

//	public ZoomRotateGesture zoomRotateGestureReference;

//	private ZoomRotateGesture.GestureState mGestureState;
	private Animator anim;
//
//	private static bool isAnimRunning = false;
//
//

	void Start(){
//		mGestureState = zoomRotateGestureReference.mGestureState;
		mColorPalleteGUI = GameObject.Find ("_Scripts").GetComponent<ColorPalleteGUI> ();
	}

	void Update () {


		if( Input.touchCount == 1 && (GestureStateManager.mGestureState == GestureStateManager.GestureState.IDLE || GestureStateManager.mGestureState == GestureStateManager.GestureState.MOVE)){
		 
			if(Input.GetTouch(0).phase == TouchPhase.Began)
				previous = Input.GetTouch(0).position;


			if(Input.GetTouch(0).phase == TouchPhase.Ended){
				current = Input.GetTouch(0).position;
				delta = current.magnitude - previous.magnitude;
				if(Mathf.Abs(delta) > minSwipeZone){

					if(delta > 0){

						if(Mathf.Abs(current.x - previous.x) > Mathf.Abs(current.y - previous.y)){

						}else{
							Debug.Log("Bottom Swipe"+Mathf.Abs(delta));

//							if(Mathf.Abs(delta) > 300  && GestureStateManager.mGestureState != GestureStateManager.GestureState.ROTATE ){
//								if(isAnimRunning == false){
//									anim = gameObject.GetComponent<Animator>();
//									isAnimRunning = true;
//									removeText();
//								}
//							}
						}

					}else{
						if(Mathf.Abs(current.x - previous.x) > Mathf.Abs(current.y - previous.y)){
							Debug.Log("Right Swipe");
						}else{
							Debug.Log("Top Swipe");
						}

					}


				}else{
					if(Input.GetTouch(0).tapCount == 1){
						Debug.Log("Single Tap");
					}else if(Input.GetTouch(0).tapCount == 2){

						if(isCollide()){
							Debug.Log("Double Tap: "+name);
							mColorPalleteGUI.setTextMesh(transform.GetComponent<TextMesh>());
							mColorPalleteGUI.showPallete();
						}
					}

				}
			}
		}
	}

			

	public void removeText(){
		anim = gameObject.GetComponent<Animator>();
		anim.enabled = true;
		anim.SetBool("not_delete", true);
	}

	public void deleteObject(){
		DestroyObject (gameObject);
	}

	
	private bool isCollide(){
		Vector3 position =  Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		RaycastHit2D hit = Physics2D.Raycast(position,Vector2.zero);
		if(hit.collider != null)
			return hit.collider.name == transform.name;
		return false;
	}

}
