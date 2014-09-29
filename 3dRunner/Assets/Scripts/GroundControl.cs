using UnityEngine;
using System.Collections;

public class GroundControl : MonoBehaviour {


	float speed = .5f;


	void Update () {
		float offset = Time.time * speed;
		renderer.material.mainTextureOffset = new Vector2 (0, -offset);
	}
}
