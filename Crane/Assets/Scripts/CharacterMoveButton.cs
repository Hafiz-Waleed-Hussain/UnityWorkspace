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

				if( gameObject.name == "Right")
				character.GetComponent<Character>().move(1);
				else if( gameObject.name == "Down")
					character.GetComponent<Character>().move(2);
				else if( gameObject.name == "Up")
					character.GetComponent<Character>().move(3);
				else if( gameObject.name == "remove")
					character.GetComponent<Character>().removeJoint();
				else
					character.GetComponent<Character>().move(4);

			}
		}

	}
}
