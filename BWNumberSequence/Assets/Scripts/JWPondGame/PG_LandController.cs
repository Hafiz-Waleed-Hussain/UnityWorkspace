using UnityEngine;
using System.Collections;

public class PG_LandController : MonoBehaviour {
	
	void Start(){
		//gameObject.SetActive(false);
		//Debug.Log("Start: Hide Land");
	}
	
	public void showLand()
	{
		gameObject.SetActive(true);
		//Debug.Log("Show Land");
		
	}
	
	public void hideLand()
	{
		gameObject.SetActive(false);
		//Debug.Log("Hide Land");
	}

}
