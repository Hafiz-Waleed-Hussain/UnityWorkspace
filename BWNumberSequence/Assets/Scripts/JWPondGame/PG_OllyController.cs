using UnityEngine;
using System.Collections;

public class PG_OllyController : MonoBehaviour {
	 GameObject olly=null;
	
	void Start(){
		olly = GameObject.Find("pg_olly");
	}
	
	public void ollySetInitialPos(Vector3 pos , GameObject olly)
	{
//			Debug.Log("setting olly...");
		transform.position=pos;
		olly.transform.localPosition = new Vector3(0F , 0F , 0F);
	}
	
	public void ollyCrossPond(Vector3 pos)
	{
		transform.position=pos;
	}
}
