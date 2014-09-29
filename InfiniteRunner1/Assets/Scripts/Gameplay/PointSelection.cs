using UnityEngine;
using System.Collections;

public class PointSelection : MonoBehaviour {

	public Color color;

	public void OnDrawGizmos(){
		Gizmos.color = color;
		Gizmos.DrawSphere (transform.position, 0.1f);
	}
}
