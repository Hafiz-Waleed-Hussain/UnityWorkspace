using UnityEngine;
using System.Collections;

public class CalculateAngle : MonoBehaviour {
	
	public static float GetAngle(Vector3 form, Vector3 to){
		Vector3 nVector = Vector3.zero;
		nVector.x = to.x;
		nVector.y = form.y;
		float a = to.y - nVector.y;
		float b = nVector.x - form.x;
		float tan = a/b;
		return RadToDegree(Mathf.Atan(tan));
	}
	
	public static float RadToDegree(float radius){
		return (radius * 180) / Mathf.PI;
	}
}
