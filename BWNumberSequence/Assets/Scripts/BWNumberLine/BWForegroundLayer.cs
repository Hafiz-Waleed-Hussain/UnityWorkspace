using UnityEngine;
using System.Collections;

public class BWForegroundLayer : MonoBehaviour {
	
	public Transform above1;
	public Transform above2;
	public Transform above3;
	
	public Transform below1;
	public Transform below2;
	public Transform below3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void moveForeground(float delta) {
		
		moveObject(above1, delta, 3.0f);
		moveObject(above2, delta, 3.0f);
		moveObject(above3, delta, 3.0f);
		
		checkPositionForObject(above1, above2, above3);
		checkPositionForObject(above2, above3, above1);
		checkPositionForObject(above3, above1, above2);
		
		moveObject(below1, delta, 3.0f);
		moveObject(below2, delta, 3.0f);
		moveObject(below3, delta, 3.0f);
		
		checkPositionForObject(below1, below2, below3);
		checkPositionForObject(below2, below3, below1);
		checkPositionForObject(below3, below1, below2);
		
	}
	
	private void moveObject(Transform obj, float distance, float factor) {
		Vector3 pos = obj.position;
		pos.x += distance*factor;
		obj.position = pos;
	}
	
	private void checkPositionForObject(Transform obj, Transform next, Transform prev) {
		if (obj.position.x <= -1.5f * obj.localScale.x) {
			Vector3 pos = obj.position;
			pos.x = prev.position.x + prev.localScale.x;
			obj.position = pos;
    	} else if (obj.position.x >= 1.5f*obj.localScale.x) {
			Vector3 pos = obj.position;
			pos.x = next.position.x - obj.localScale.x;
			obj.position = pos;
    	}
	}
}
