using UnityEngine;
using System.Collections;

public class PG_ShapeShadowController : MonoBehaviour {
	
	
	void Start(){
		hideShadow();
	}
	
	public void playShowShapesShadowAnim(){
		showShadow();
		playShadowScaleUP();
	}
	
	public  void playShadowScaleUP(){
		iTween.ScaleTo(gameObject, iTween.Hash( "delay",0.1 , "time",0.4 , "scale", new Vector3((float)gameObject.transform.localScale.x+8F , (float)gameObject.transform.localScale.y , 1F) ,"oncomplete","playShadowScaleDown" ));
	}
	
	
	public  void playShadowScaleDown(){
		iTween.ScaleTo(gameObject, iTween.Hash( "delay",0.1 , "time",0.4 , "scale", new Vector3((float)gameObject.transform.localScale.x-8F , (float)gameObject.transform.localScale.y , 1F) ,"oncomplete","playShadowScaleUP" ));
	}
	
	public void showShadow(){
		gameObject.transform.renderer.enabled = true;
	}
	
	public void hideShadow(){
		gameObject.transform.renderer.enabled = false;
	}
		
	
	
}
