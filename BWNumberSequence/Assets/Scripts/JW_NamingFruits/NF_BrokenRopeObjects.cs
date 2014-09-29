using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NF_BrokenRopeObjects : MonoBehaviour {
	
public List<GameObject> _brokenRopeObj = new List<GameObject>();
private Vector3 _newParentPosition;	
private Transform obj;
public GameObject destroyedFruit = null;
	
public void sendBrokenRopeArray(List<GameObject> _brokenRope,Vector3 _newPosition){
	
		_brokenRopeObj = _brokenRope;
		_newParentPosition = _newPosition;
		breakAllJoints();
}

public void resetBrokenRope(){
		
		_newParentPosition = new Vector3(0,0,0);
		for(int i=0;i<_brokenRopeObj.Count;i++){
			Destroy(_brokenRopeObj[i]);
		}
		_brokenRopeObj = null;
	}
IEnumerator  MoveObject (Transform thisTransform ,Vector3 startPos ,Vector3 endPos ,float time){
	float i=0.0f;
	float rate=1.0f/time;
	while (i < 1.0f) {
	    i += Time.deltaTime * rate;
	    thisTransform.position = Vector3.Lerp(startPos, endPos, i);
	    yield return 0; 
	}
}

IEnumerator destroyFruitRope(Transform brokenRope){
		yield return new WaitForSeconds(0.01f);
		Destroy(brokenRope.gameObject);
}	

void animateBrokenRope(){
	//iTween.MoveTo(gameObject, _newParentPosition, 18.0f/30.0f + 10.0f);
	//	Destroy(this);
	//StartCoroutine(MoveObject(this.transform, this.transform.position,_newParentPosition,0.80f));
	StartCoroutine(destroyFruitRope(this.transform));
}
	
void breakAllJoints(){
		
	for(int i = 0; i<_brokenRopeObj.Count; i++){
		obj = _brokenRopeObj[i].transform;
		obj.rigidbody.useGravity=true;
		obj.rigidbody.isKinematic=false;
		obj.rigidbody.constraints &= RigidbodyConstraints.FreezeAll;
		obj.rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
		obj.rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
		obj.parent = this.transform;
		}
		animateBrokenRope();
	}
}
