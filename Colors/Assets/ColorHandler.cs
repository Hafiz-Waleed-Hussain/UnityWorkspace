using UnityEngine;
using System.Collections;

public class ColorHandler : MonoBehaviour {

	public Color color;

	void Awake(){
		transform.renderer.material.color = color;
	}
}
