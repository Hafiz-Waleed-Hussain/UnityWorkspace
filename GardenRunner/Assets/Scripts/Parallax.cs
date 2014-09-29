using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public float speed = .3f;

	void Update () {
	
		float offset = Time.time * speed;
		renderer.material.mainTextureOffset = new Vector2(0,-offset);
	}
}
