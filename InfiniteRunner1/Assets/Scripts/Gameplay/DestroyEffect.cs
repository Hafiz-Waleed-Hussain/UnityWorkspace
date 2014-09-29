/// <summary>
/// Destroy effect.
/// this script use for destroy effect(ex. particle)
/// </summary>

using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

	public float time;
	
	void Start(){
		Destroy(gameObject, time);	
	}
}
