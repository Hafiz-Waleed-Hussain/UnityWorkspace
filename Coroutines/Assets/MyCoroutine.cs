using UnityEngine;
using System.Collections;

public class MyCoroutine : MonoBehaviour {

	public Transform target;

	void Start(){
		StartCoroutine (MyRoutine(target));
		
	}


	IEnumerator MyRoutine(Transform target){

		while (Vector3.Distance(transform.position, target.position) > 2) {
			transform.position = Vector3.Lerp(transform.position,	 target.position, .1f*Time.deltaTime);	
			Debug.Log(Time.deltaTime);
			yield return null;
		}

	}

}
