using UnityEngine;
using System.Collections;

public class SingleTouchScript : MonoBehaviour {


	private Vector2 previous;
	private Vector2 current;
	private float delta;
	private int minSwipeZone = 3;

	void Update () {


		if( Input.touchCount == 1){
		
			if(Input.GetTouch(0).phase == TouchPhase.Began)
				previous = Input.GetTouch(0).position;


			if(Input.GetTouch(0).phase == TouchPhase.Ended){
				current = Input.GetTouch(0).position;
				delta = current.magnitude - previous.magnitude;
				if(Mathf.Abs(delta) > minSwipeZone){

					if(delta > 0){

						if(Mathf.Abs(current.x - previous.x) > Mathf.Abs(current.y - previous.y)){
							Logger.showDebugLog("Left Swipe");

						}else{
							Logger.showDebugLog("Bottom Swipe");
						
						}

					}else{
						if(Mathf.Abs(current.x - previous.x) > Mathf.Abs(current.y - previous.y)){
							Logger.showDebugLog("Right Swipe");
						
						}else{
							Logger.showDebugLog("Top Swipe");
						
						}

					}


				}else{
					if(Input.GetTouch(0).tapCount == 1){
						Logger.showDebugLog("Single Tap");
					}else if(Input.GetTouch(0).tapCount == 2){
						Logger.showDebugLog("Double Tap");
					} 
				}
			}
		}
	}
}
