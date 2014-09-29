using UnityEngine;
using System.Collections;

public class PG_RippleEffectController : MonoBehaviour {
	void Start(){
		showRippleEffect();
	}
	
	public void showRippleEffect(){
		transform.renderer.enabled = true;
		firstRippleEffect();
	}
	
	
	void firstRippleEffect(){
		Vector3 scale=gameObject.transform.localScale;
		scale.x*=1.7F;
		scale.y*=1.5F;
		iTween.ScaleTo(gameObject,iTween.Hash("time",0.4 ,"scale",scale, "easetype",iTween.EaseType.easeOutSine, "oncomplete", "destroyRippleEffect" , "oncompletetarget" ,gameObject ));
		iTween.FadeTo(gameObject, iTween.Hash("a",0.1 , "time",0.4));
	}
	
	
	void destroyRippleEffect(){
		GameObject.Destroy(gameObject);
	}
	

	public void hideRippleEffect(){
	transform.renderer.enabled = false;
	}
}
