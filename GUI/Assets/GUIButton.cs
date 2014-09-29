using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIButton : MonoBehaviour {

	private float guiXRatio;
	private float guiYRatio;


	void Awake(){

		guiXRatio = Screen.width / 800.0f;
		guiYRatio = Screen.height / 480.0f;

		Debug.Log ("Width Scale: " + guiXRatio);
		Debug.Log ("Height Scale: " + guiYRatio);
	}


	void OnGUI(){


		GUI.matrix = Matrix4x4.TRS (transform.position, Quaternion.identity, new Vector3 (guiXRatio, guiYRatio, 1));
		GUI.Button (new Rect (0, 0, 400, 50), "Text");
	
	}

}
