using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BWBee : MonoBehaviour {
	
	public GameObject bee;
	public GameObject head;
	public GameObject wings;
	public GameObject body;

	public bool flip = false;
	
	public MonoBehaviour _delegate;
	
	public bool shouldPlayCelebration = false;
	
	// Use this for initialization
	void Start () {
		idleAnimation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void idleAnimation () {
		idleDownComplete();
	}
	
	public void cancelIdleAnimation () {
		
	}
	
	private void idleUpComplete () {
		Vector3 downPos = transform.position;
		downPos.y += 15;
		downPos.z = bee.transform.position.z;
		iTween.MoveTo(bee, iTween.Hash("position", downPos, "time", 0.4, "onComplete", "idleDownComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	private void idleDownComplete () {
		Vector3 downPos = transform.position;
		downPos.y -= 15;
		downPos.z = bee.transform.position.z;
		iTween.MoveTo(bee, iTween.Hash("position", downPos, "time", 0.4, "onComplete", "idleUpComplete", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	public void moveToPoint (Vector3 destination) {
		//Debug.Log("")
		iTween.Stop(bee);
		
		if(destination.x == transform.position.x && destination.y == transform.position.y) {
			moveToPointComplete();	
		}
		
		if(destination.x < transform.position.x) {
			Vector3 beeScale = bee.transform.localScale;
			beeScale.x = -1;
			bee.transform.localScale = beeScale;
			bee.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 15));
		} else {
			Vector3 beeScale = bee.transform.localScale;
			beeScale.x = 1;
			bee.transform.localScale = beeScale;
			bee.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -15));
		}
		
		List<Vector3> points = new List<Vector3>();
		points.Add(transform.position);
		points.Add(new Vector3(transform.position.x, transform.position.y+100, transform.position.z));
		points.Add(new Vector3(destination.x, transform.position.y+100, transform.position.z));
		points.Add(new Vector3(destination.x, destination.y, transform.position.z));
		
		BezierPath bezierPath = new BezierPath();
	//	bezierPath.Interpolate(points, .25f);
		bezierPath.SetControlPoints(points);
		List<Vector3> drawingPoints = bezierPath.GetDrawingPoints2();
		
		Vector3[] path = new Vector3[drawingPoints.Count];
		
		for(int i = 0; i< drawingPoints.Count; i++) {
			path[i] = drawingPoints[i];
		}
		
		float distance = AGGameState.modFloat(destination.x - transform.position.x);
		float speed = 1000.0f;
		float time = distance / speed;
		
		iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", time, "onComplete", "moveToPointComplete", "easetype", iTween.EaseType.linear));
	}
	
	private void moveToPointComplete () {
		idleAnimation();
		bee.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
			
		flipBee(flip);
		
		if(_delegate != null) {
			if(_delegate.GetType() == typeof(BWNumberSeqGame)) {
				BWNumberSeqGame game = _delegate as BWNumberSeqGame;
				game.beeMoveFinished();
			} else if(_delegate.GetType() == typeof(BWNumberArithmetic)) {
				//BWNumberArithmetic game = _delegate as BWNumberArithmetic;
			}
		}
	}
	
	public void flipBee(bool _flip) {
		flip = _flip;
		Vector3 beeScale = bee.transform.localScale;
		if (flip) {
			beeScale.x = -1;
		} else {
			beeScale.x = 1;
		}
		bee.transform.localScale = beeScale;
	}
	
	public void playYesAnimation () {
		BWBeeHead headObj = head.GetComponent<BWBeeHead>();
		headObj.beeDelegate = this;
		headObj.animState = 1;
	}
	
	public void playNoAnimation () {
		BWBeeHead headObj = head.GetComponent<BWBeeHead>();
		headObj.beeDelegate = this;
		headObj.animState = 2;
	}
	
	public void playTapAnimation () {
		BWBeeBody bodyObj = body.GetComponent<BWBeeBody>();
		if(bodyObj.animState != 3) {
			bodyObj.changePositionForCircle = true;
		}
		bodyObj.animState = 3;
		
		bodyObj.beeDelegate = this;
		head.SetActive(false);
		wings.SetActive(false);
		
		iTween.Stop(bee);
	}
	
	public void playCelebration () {
		shouldPlayCelebration = false;
		
		int rand = Random.Range(0,2);
		
		if(rand == 0)
			playDiveAnimation();
		else 
			playSpinAnimation();
	}
	
	public void playSpinAnimation () {
		BWBeeBody bodyObj = body.GetComponent<BWBeeBody>();
		bodyObj.animState = 1;
		
		bodyObj.beeDelegate = this;
		head.SetActive(false);
		
		iTween.Stop(bee);
		
		playSpinMovement1();
	}
	
	
	public void playDiveAnimation () {
		BWBeeBody bodyObj = body.GetComponent<BWBeeBody>();
		bodyObj.animState = 2;
		
		bodyObj.beeDelegate = this;
		head.SetActive(false);
		
		iTween.Stop(bee);
	}
	
	
	void playSpinMovement1 () {
		//Debug.Log("spin movement1");
		Vector3 pos = bee.transform.position;
		pos.y += -14;
		iTween.MoveTo(bee, iTween.Hash("position", pos, "time", 4.0f/30.0f, "delay", 4.0f/30.0f ,  "onComplete", "playSpinMovement2", "oncompletetarget", gameObject, "easetype", iTween.EaseType.linear));
	}
	
	void playSpinMovement2 () {
		//Debug.Log("spin movement2");
		Vector3 pos = bee.transform.position;
		pos.y += 93;
		iTween.MoveTo(bee, iTween.Hash("position", pos, "time", 9.0f/30.0f, "delay", 14.0f/30.0f , "onComplete", "playSpinMovement3", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	void playSpinMovement3 () {
		Vector3 pos = bee.transform.position;
		pos.y += -106;
		iTween.MoveTo(bee, iTween.Hash("position", pos, "time", 11.0f/30.0f, "onComplete", "playSpinMovement4", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	void playSpinMovement4 () {
		Vector3 pos = bee.transform.position;
		pos.y += 27;
		iTween.MoveTo(bee, iTween.Hash("position", pos, "time", 4.0f/30.0f, "easetype", iTween.EaseType.linear));
	}
	
	public void tapAnimationFirstHalfFinished () {
		head.SetActive(true);
		wings.SetActive(true);
		
		iTween.Stop(bee);
		circleMoveUp();
	}
	
	public void tapAnimationFinished () {
		head.SetActive(true);
		wings.SetActive(true);
		
		iTween.Stop(bee);
		idleAnimation();
	}
	
	void circleMoveUp () {
		Vector3 pos = bee.transform.position;
		pos.y += 80;
		iTween.MoveTo(bee, iTween.Hash("position", pos, "time", 5.0f/30.0f, "onComplete", "circleMoveDown", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeOutSine));
	}
	
	void circleMoveDown () {
		Vector3 pos = bee.transform.position;
		pos.y -= 80;
		iTween.MoveTo(bee, iTween.Hash("position", pos, "time", 7.0f/30.0f, "onComplete", "tapAnimationFinished", "oncompletetarget", gameObject, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	public void celebrationAnimationFinished () {
		
		head.SetActive(true);
		
		iTween.Stop(bee);
		idleAnimation();
		
		BWNumberSeqGame numberSeq = Camera.mainCamera.GetComponent<BWNumberSeqGame>();
		BWNumberArithmetic numberArth = Camera.mainCamera.GetComponent<BWNumberArithmetic>();
			
		if(numberSeq != null) {
			//numberSeq.beeYesAnimationFinished();
		} else if (numberArth != null) {
			//numberArth.beeYesAnimationFinished();
		}
	}
	
	public void yesFinished () {
		
		if(shouldPlayCelebration) {
			playCelebration();
		} else {
			
			BWNumberSeqGame numberSeq = Camera.mainCamera.GetComponent<BWNumberSeqGame>();
			BWNumberArithmetic numberArth = Camera.mainCamera.GetComponent<BWNumberArithmetic>();
			
			if(numberSeq != null) {
				//numberSeq.beeYesAnimationFinished();
			} else if (numberArth != null) {
				//numberArth.beeYesAnimationFinished();
			}
		}
	}
	
	public void noFinished () {
		BWNumberSeqGame numberSeq = Camera.mainCamera.GetComponent<BWNumberSeqGame>();
		BWNumberArithmetic numberArth = Camera.mainCamera.GetComponent<BWNumberArithmetic>();
		
		if(numberSeq != null) {
			numberSeq.beeNoAnimationFinished();
		} else if (numberArth != null) {
			numberArth.beeNoAnimationFinished();
		}
	}
	
	
}
