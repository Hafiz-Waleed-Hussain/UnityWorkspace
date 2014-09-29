using UnityEngine;
using System.Collections;

public class PG_BackgroundController : MonoBehaviour {
 GameObject landObject=null;
	
	void Start(){
		landObject = GameObject.Find("Land");
	}
	
	
	//   ------ Set background
	public void setBackground(int background ){
		if(background==1){
			if(landObject!=null)
			landObject.transform.renderer.enabled = true;  // show land
		//	else Debug.Log("land null");
			
			transform.renderer.material.mainTexture = Resources.Load(PG_Constants._background + "BG") as Texture;
		}
		else{
			if(landObject!=null)
			landObject.transform.renderer.enabled = false; // hide land
			//else Debug.Log("land null");
			transform.renderer.material.mainTexture = Resources.Load(PG_Constants._background + "BG2") as Texture;
		}
				
	}
	
}
