/// <summary>
/// Building Script
/// This script is use for reset building on pattern when character die.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {
	
	[HideInInspector]
	public bool buildingActive;
	[HideInInspector]
	public int buildIndex;
	
	public static Building instance;
	
	void Start(){
		instance = this;
	}
	
	//Reset building when character die	
	public void Reset(){
		buildingActive = false;
		this.transform.parent = null;
	}
}
