#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion
using UnityEngine;
using System.Collections;

public class NF_AnimationController : MonoBehaviour {
	
float timer = 24.0f; //length of time to run
float startTime = 0;
private bool isAnimating = false;
float animationSpeed;

// Use this for initialization
void Start () {
		
	startTime = Time.realtimeSinceStartup;
	animationSpeed = this.animation["ropeCurves"].speed;
}

// Update is called once per frame
void Update () {

	if(Time.realtimeSinceStartup <= startTime + timer){
       		this.animation.Play("ropeCurves");
			if(Time.realtimeSinceStartup>12.0f){
					animationSpeed = 0.6f;
					this.animation["ropeCurves"].speed=animationSpeed;
					}
    		}else{
       			if(!isAnimating)
       			{
        			 this.animation.Stop();
        			 isAnimating = true;
      			 }
   			 }
	}
public void resetTimer(){
		startTime = Time.realtimeSinceStartup;
		 isAnimating = false;
	}

public void stopAnimation(){
	this.animation.Stop("ropeCurves");
}
	
}
