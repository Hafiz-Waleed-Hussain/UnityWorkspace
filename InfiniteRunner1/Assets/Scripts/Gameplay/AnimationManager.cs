/// <summary>
/// Animation manager
/// This script use for control animation of character.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {
	
	//Class Animation
	[System.Serializable]
	public class AnimationSet{
		public AnimationClip animation;
		public float speedAnimation = 1;
	}
	
	//AnimationSet
	public AnimationSet run, turnLeft, turnRight, jumpUp, jumpLoop, jumpDown, roll, dead;
	
	
	//Delegate update function
	public delegate void AnimationHandle();
	public AnimationHandle animationState;
	
	//Variable private field 
	private Controller controller;
	private float speed_Run;
	private float default_Speed_Run;
	
	void Start(){
		controller = this.GetComponent<Controller>();
		default_Speed_Run = GameAttribute.gameAttribute.speed;
		animationState = Run;
	}
	
	void Update () {
		if(animationState != null){
			animationState();	
		}
	}
	
	//Run State
	public void Run(){
		animation.Play(run.animation.name);
		speed_Run = (GameAttribute.gameAttribute.speed/default_Speed_Run)*(run.speedAnimation);
		animation[run.animation.name].speed = speed_Run;
	}
	
	//Jump State
	public void Jump(){
		animation.Play(jumpUp.animation.name);
		if(animation[jumpUp.animation.name].normalizedTime > 0.95f){
			animationState = JumpLoop;
		}
	}
	
	//Double Jump State
	public void JumpSecond(){
		animation.Play(roll.animation.name);
		if(animation[roll.animation.name].normalizedTime > 0.95f){
			animationState = JumpLoop;
		}
	}
	
	//Alway play animation jump if character on air
	public void JumpLoop(){
		animation.CrossFade(jumpLoop.animation.name);
		if(controller.characterController.isGrounded){
			animationState = Run;	
		}
	}
	
	//Turn Left State
	public void TurnLeft(){
		animation.Play(turnLeft.animation.name);
		animation[turnLeft.animation.name].speed = turnLeft.speedAnimation;
		if(animation[turnLeft.animation.name].normalizedTime > 0.95f){
			animationState = Run;	
		}
	}
	
	//Turn Right State
	public void TurnRight(){
		animation.Play(turnRight.animation.name);
		animation[turnRight.animation.name].speed = turnRight.speedAnimation;
		if(animation[turnRight.animation.name].normalizedTime > 0.95f){
			animationState = Run;	
		}
	}
	
	//Roll State
	public void Roll(){
		animation.Play(roll.animation.name);
		if(animation[roll.animation.name].normalizedTime > 0.95f){
			controller.isRoll = false;
			animationState = Run;
		}else{
			controller.isRoll = true;	
		}
	}
	
	//Dead State
	public void Dead(){
		animation.Play(dead.animation.name);
	}
	
}
