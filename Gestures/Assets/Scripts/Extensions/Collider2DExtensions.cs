using UnityEngine;
using System.Collections;

public static class Collider2DExtensions  {

	public static bool isCollide (this Collider2D collider2D){

		if (Input.touchCount == 1 && collider2D!=null ) {
				Vector3 position = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
				RaycastHit2D hit = Physics2D.Raycast (position, Vector2.zero);
				if(hit.collider == null) 
					return false;
				return hit.collider.name == collider2D.name;
		}
		return false;
	}
	
}
