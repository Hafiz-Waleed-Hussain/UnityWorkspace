using UnityEngine;
using System.Collections;

public class PG_IckyController : MonoBehaviour {

	public void ickySetInitialPos(Vector3 pos)
	{
		//Debug.Log("setting icky...");
		transform.position=pos;
	}
	
	
	public void ickyJumpToLandmass(Vector3 pos)
	{
		transform.position=pos;
	}
	
	
	public void ickyCrossPond(Vector3 pos)
	{
		transform.position=pos;
	}
}
