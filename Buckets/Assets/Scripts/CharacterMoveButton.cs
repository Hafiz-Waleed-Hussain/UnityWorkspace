using UnityEngine;
using System.Collections;

public class CharacterMoveButton : MonoBehaviour {

	public GameObject character;
	
	private float mGameObjectZaxis;
	private Vector3 mChararcterPosition;

	void Start () {
		mGameObjectZaxis = gameObject.transform.position.z;
	}
	
	void Update () {

		if( Input.GetButton("Fire1")) {
			Vector3 mouseInputAccordingToWorldPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			mouseInputAccordingToWorldPoint.z = mGameObjectZaxis;
			if(gameObject.renderer.bounds.Contains(mouseInputAccordingToWorldPoint)){
				Debug.Log("Move Click");
				character.GetComponent<Character>().move();
//				character.transform.position = new Vector3(

			}
		}

	}
}
