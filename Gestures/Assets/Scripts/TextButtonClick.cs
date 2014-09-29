using UnityEngine;
using System.Collections;

public class TextButtonClick : MonoBehaviour {


	void Update(){

		Debug.Log(transform.collider2D.isCollide());
	}
}
