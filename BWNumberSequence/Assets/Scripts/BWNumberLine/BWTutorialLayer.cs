using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BWTutorialLayer : MonoBehaviour {
	
	public GameObject handPref;
	public bool isPlayingTutorial = false;
	public ArrayList positions;
	GameObject hand = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void stopAnimation () {
		if(!isPlayingTutorial) return;
		
		isPlayingTutorial = false;
		iTween.Stop(hand);
		Destroy(hand);
		hand = null;
	}
	
	public void handMoveComplete() {
		if(positions.Count < 2) {
			stopAnimation();	
		} else {
			playTutorial();
		}
	}
	
	public void playTutorial () {
		
		isPlayingTutorial = true;
		if(positions.Count < 2) {
			stopAnimation();	
		}
		
		if(hand == null) {
			hand = Instantiate(handPref) as GameObject;
			hand.transform.parent = transform;
			hand.transform.position = (Vector3)positions[0];
		}
		positions.RemoveAt(0);
		
		Vector3 destination = (Vector3)positions[0];
		
		List<Vector3> points = new List<Vector3>();
		points.Add(hand.transform.position);
		points.Add(new Vector3(hand.transform.position.x, hand.transform.position.y+150F, hand.transform.position.z));
		points.Add(new Vector3(destination.x, hand.transform.position.y+150F, hand.transform.position.z));
		points.Add(new Vector3(destination.x, destination.y, transform.position.z));
		
		BezierPath bezierPath = new BezierPath();
		bezierPath.SetControlPoints(points);
		List<Vector3> drawingPoints = bezierPath.GetDrawingPoints2();
		
		Vector3[] path = new Vector3[drawingPoints.Count];
		
		for(int i = 0; i< drawingPoints.Count; i++) {
			path[i] = drawingPoints[i];
		}
		
		
		iTween.MoveTo(hand, iTween.Hash("path", path, "time", 1.0, "onComplete", "handMoveComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.linear));
	}
}
