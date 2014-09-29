using UnityEngine;
using System.Collections;

public class BWBackgroundLayer : MonoBehaviour {
	
	public Transform bushes1;
	public Transform bushes2;
	public Transform bushes3;
	
	public Transform ground1;
	public Transform ground2;
	public Transform ground3;
	
	public Transform trees1;
	public Transform trees2;
	public Transform trees3;
	
	public Transform clouds1;
	public Transform clouds2;
	public Transform clouds3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void moveBackground(float delta) {
		
		moveObject(bushes1, -delta, 0.25f);
		moveObject(bushes2, -delta, 0.25f);
		moveObject(bushes3, -delta, 0.25f);
		
		checkPositionForObject(bushes1, bushes2, bushes3);
		checkPositionForObject(bushes2, bushes3, bushes1);
		checkPositionForObject(bushes3, bushes1, bushes2);
		
		moveObject(ground1, delta, 1);
		moveObject(ground2, delta, 1);
		moveObject(ground3, delta, 1);
		
		checkPositionForObject(ground1, ground2, ground3);
		checkPositionForObject(ground2, ground3, ground1);
		checkPositionForObject(ground3, ground1, ground2);
		
		moveObject(trees1, delta, 0.2f);
		moveObject(trees2, delta, 0.2f);
		moveObject(trees3, delta, 0.2f);
		
		checkPositionForTrees(trees1, trees2, trees3);
		checkPositionForTrees(trees2, trees3, trees1);
		checkPositionForTrees(trees3, trees1, trees2);
		
		moveObject(clouds1, -delta, 0.5f);
		moveObject(clouds2, -delta, 0.5f);
		moveObject(clouds3, -delta, 0.5f);
		
		checkPositionForTrees(clouds1, clouds2, clouds3);
		checkPositionForTrees(clouds2, clouds3, clouds1);
		checkPositionForTrees(clouds3, clouds1, clouds2);
		
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
	
	private void checkPositionForTrees(Transform obj, Transform next, Transform prev) {
		if (obj.position.x <= -1.5f * 1200) {
			Vector3 pos = obj.position;
			pos.x = prev.position.x + 1200;
			obj.position = pos;
    	} else if (obj.position.x >= 1.5f*1200) {
			Vector3 pos = obj.position;
			pos.x = next.position.x - 1200;
			obj.position = pos;
    	}
	}
}
