/// <summary>
/// Game attribute.
/// this script use for set all attribute in game(ex speedgame,character life)
/// </summary>

using UnityEngine;
using System.Collections;

public class GameAttribute : MonoBehaviour {

	public float starterSpeed = 5; //Speed Character
	public float starterLife = 1; //Life character
	
	[HideInInspector]
	public float distance;
	[HideInInspector]
	public float coin;
	[HideInInspector]
	public int level = 0;
	[HideInInspector]
	public bool isPlaying;
	[HideInInspector]
	public bool pause = false;
	[HideInInspector]
	public bool ageless = false;
	[HideInInspector]
	public bool deleyDetect = false;
	[HideInInspector]
	public float multiplyValue;
	
	//[HideInInspector]
	public float speed = 5;
	[HideInInspector]
	public float life = 3;

	public static GameAttribute gameAttribute;
	
	void Start(){
		//Setup all attribute
		gameAttribute = this;
		DontDestroyOnLoad(this);
		speed = starterSpeed;
		distance = 0;
		coin = 0;
		life = starterLife;
		level = 0;
		pause = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
	}
	
	public void CountDistance(float amountCount){
		distance += amountCount * Time.smoothDeltaTime;	
	}
	
	public void ActiveShakeCamera(){
		CameraFollow.instace.ActiveShake();	
	}
	
	public void Pause(bool isPause){
		//pause varible
		pause = isPause;
	}
	
	public void Resume(){
		//resume
		pause = false;
	}
	
	public void Reset(){
		//Reset all attribute when character die
		speed = starterSpeed;
		distance = 0;
		coin = 0;
		life = starterLife;
		level = 0;
		pause = false;
		deleyDetect = false;
		ageless = false;
		isPlaying = true;
		Building.instance.Reset();
		Item.instance.Reset();
		PatternSystem.instance.Reseted();
		CameraFollow.instace.Reset();
		Controller.instace.Reset();
		Controller.instace.timeJump = 0;
		Controller.instace.timeMagnet = 0;
		Controller.instace.timeMultiply = 0;
		Controller.instace.timeSprint = 0;
		GUIManager.instance.Reset();
	}
}
