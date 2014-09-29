using UnityEngine;
using System.Collections;

public class BWHand : MonoBehaviour {
	
	public GameObject prefab = null;
	GameObject trailRenderer = null;
	// Use this for initialization
	void Start () {
		idleAnimation();
	}
	
	void OnDisable () {
		Destroy(trailRenderer);
	}
	
	// Update is called once per frame
	void Update () {
		if(trailRenderer != null) {
			Vector3 newPos = transform.position;
			//newPos.z = 10;
			newPos.x -= 20;
			newPos.y += 20;
			trailRenderer.transform.position = newPos;
			
		}
	}
	
	public void idleAnimation () {
		
		trailRenderer = (GameObject)Instantiate(prefab);
		
		if(trailRenderer != null) {
			trailRenderer.transform.parent = this.transform;
			trailRenderer.renderer.enabled = true;
		//	Vector3 pos = trailRenderer.transform.localPosition;
		//	pos.z = 10;
		//	trailRenderer.transform.localPosition = pos;
		}

		idleDownComplete();
	}
	
	public void cancelIdleAnimation () {
		
	}
	
	private void idleUpComplete () {
		Vector3 scaleUp = transform.localScale;
		scaleUp.x = 86.0f*1.05f; 
		scaleUp.y = 78.0f*1.05f; 
		iTween.ScaleTo(gameObject, iTween.Hash("scale", scaleUp, "time", 0.2, "onComplete", "idleDownComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	private void idleDownComplete () {
		Vector3 scaleDown = transform.localScale;
		scaleDown.x = 86.0f/1.05f; 
		scaleDown.y = 78.0f/1.05f; 
		iTween.ScaleTo(gameObject, iTween.Hash("scale", scaleDown, "time", 0.2, "onComplete", "idleUpComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
}
