using UnityEngine;
using System.Collections;

public class BWMenuLights : MonoBehaviour {

	// Use this for initialization
	void Start () {
		blinkingAnimation();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void blinkingAnimation () {
		fadeOut();
	}
	
	void fadeOut() {
		iTween.FadeTo(gameObject, iTween.Hash("time", Random.Range(1.0f,2.0f), "alpha", 0.3, "onComplete", "fadeIn", "easetype", iTween.EaseType.linear));
	}
	
	void fadeIn() {
		iTween.FadeTo(gameObject, iTween.Hash("time", Random.Range(1.0f,2.0f), "alpha", 1.0, "onComplete", "fadeOut", "easetype", iTween.EaseType.linear));
	}
}
