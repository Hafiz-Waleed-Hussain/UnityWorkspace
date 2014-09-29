using UnityEngine;
using System.Collections;

public class GenericGUIHandler : MonoBehaviour {

		

	public static float baseWidth = 1280.0f;
	public static float baseHeight = 800.0f;
	private static Vector3 scaleVector;
	void Awake(){
	
		float x = Screen.width / baseWidth;
		float y = Screen.height / baseHeight;
		scaleVector = new Vector3 (x, y, 1);
	}

		

		public static Matrix4x4 getTRS(){
			return Matrix4x4.TRS (new Vector3(0,0,0), Quaternion.identity, scaleVector);
		}

}




