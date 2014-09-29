using UnityEngine;
using System.Collections;

public class PG_LandmassController : MonoBehaviour {
	
	private bool isfilled;
	
	public Material empty;
	public Material filled;
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	// set the prefab status fill or empty, True:=> means filled , False:=> means empty
	public void SetFilled()
	{
		isfilled=true;
	    transform.renderer.material=filled;
	}
	
	
	public void SetEmpty()
	{
		isfilled=false;
		transform.renderer.material=empty;
	}
	
	public bool getLandmassStatus()
	{
		return isfilled;
	}
}
