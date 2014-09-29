using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour {

	private Transform cameraTransform;
	private float spriteWidth;

	void Start () {
	
		cameraTransform = Camera.main.transform;
		SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
		spriteWidth = spriteRenderer.bounds.size.x;
	
		Debug.Log ("Sprite Width: "+spriteWidth);
	}
	
	void Update () {
		if( (transform.position.x + spriteWidth) < cameraTransform.position.x) {
			Vector3 newPos = transform.position;
			newPos.x += 2.0f * spriteWidth; 
			transform.position = newPos;
		}
	}

}
