using UnityEngine;
using System.Collections;

public class CharacterRotateButton : MonoBehaviour {
	
	public GameObject character;

	private float mGameObjectZaxis;
	private Animator mAnimator;
	private Character mCharacterScript;

	void Start(){
		mGameObjectZaxis = gameObject.transform.position.z;
		mAnimator = character.GetComponent<Animator> ();
		mCharacterScript = character.GetComponent<Character> ();
	}
	
	void Update () {

		if( Input.GetButton("Fire1") && mCharacterScript.IsRotating()== false) {
			Vector3 mouseInputAccordingToWorldPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			mouseInputAccordingToWorldPoint.z = mGameObjectZaxis;
			if(gameObject.renderer.bounds.Contains(mouseInputAccordingToWorldPoint)){
				Debug.Log("Rotate Click");
				mCharacterScript.setRotating(true);
				mAnimator.SetBool("Rotate90",true);
			}
		}

	}
	
	
}
