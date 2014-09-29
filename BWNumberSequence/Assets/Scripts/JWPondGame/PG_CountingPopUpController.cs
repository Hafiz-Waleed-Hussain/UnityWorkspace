using UnityEngine;
using System.Collections;

public class PG_CountingPopUpController : MonoBehaviour {

	public void showCountingNumber(int _count){
		//renderer.enabled=true;
		renderer.material.mainTexture = Resources.Load(PG_Constants._sprites+PG_Constants._numbers+string.Format("Number{0}",_count)) as Texture2D;
		//Debug.Log("in SCN: "+PG_Constants._sprites+PG_Constants._numbers+string.Format("Number{0}",_count));
		Vector3 scale=gameObject.transform.localScale;
		scale.x*=2.3F;
		scale.y*=2.3F;
		iTween.ScaleTo(gameObject,iTween.Hash("time",1 ,"scale",scale, "easetype",iTween.EaseType.easeInSine));
		iTween.FadeTo(gameObject, iTween.Hash("a",0, "time",1.8, "oncompletetarget",gameObject,"oncomplete", "destroyGameObject"));
	} 
	
	void destroyGameObject(){
		GameObject.Destroy(this);
	}
	private void hideNumber(){
		renderer.enabled=false;
	}
}
