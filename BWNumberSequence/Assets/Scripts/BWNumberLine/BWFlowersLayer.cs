using UnityEngine;
using System.Collections;
using System;

public class BWFlowersLayer : MonoBehaviour {
	
	public int numberLineMin;
	public int numberLineMax;
	public int initialNumber;
	public float contentWidth;
	public ArrayList flowers;
	public GameObject flowerPrefab;
	public GameObject hivePrefab;
	public ArrayList tracingLines = null;
	
	public MonoBehaviour _delegate = null;

	// Use this for initialization
	void Start () {
		
	}
	
	public void setFlowersLayer (int min, int max, int initial) {
		numberLineMax = max;
		numberLineMin = min;
		initialNumber = initial;
		
		if(flowers != null)
			destroyFlowersLayer();
			
		flowers = new ArrayList(numberLineMax - numberLineMin + 1);
		Vector3 pos = new Vector3(85,0,0);
		for(int i = numberLineMin; i <= numberLineMax; i++) {
			GameObject flower;
			if( i != 0) {
				flower = Instantiate(flowerPrefab) as GameObject;
			} else {
				flower = Instantiate(hivePrefab) as GameObject;
			}
			flower.transform.parent = this.transform;
			flower.transform.localPosition = pos;
			
			pos.x += 170;
			flowers.Add(flower);
			
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			flowerObj.setFlowerNumber(i, i - numberLineMin);
			flowerObj.openFlower();
		}
		
		contentWidth = pos.x - 85;
	}
	
	public void setDiagonalFlowersLayer (int min, int max, int initial) {
		numberLineMax = max;
		numberLineMin = min;
		initialNumber = initial;
		
		if(flowers != null)
			destroyFlowersLayer();
			
		float diagonalAngle = BWConstants.diagonalAngle;
		
		flowers = new ArrayList(numberLineMax - numberLineMin + 1);
		Vector3 pos = new Vector3(85,0,0);
		for(int i = numberLineMin; i <= numberLineMax; i++) {
			GameObject flower;
			if( i != 0) {
				flower = Instantiate(flowerPrefab) as GameObject;
			} else {
				flower = Instantiate(hivePrefab) as GameObject;
			}
			
			float yPos = (float) (Math.Tan(diagonalAngle) * pos.x);
			pos.y = yPos;
			flower.transform.parent = this.transform;
			flower.transform.localPosition = pos;
			
			pos.x += 170;
			flowers.Add(flower);
			
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			flowerObj.setFlowerNumber(i, i - numberLineMin);
			flowerObj.openFlower();
		}
		
		contentWidth = pos.x - 85;
		
		tracingLines = new ArrayList();
	}
	
	public void destroyFlowersLayer () {
		foreach(GameObject flower in flowers) {
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			flowerObj.closeFlower(numberLineMax - numberLineMin - flowerObj.index);
		}
		
		float delay = (float)(numberLineMax - numberLineMin) * (2.0f/30.0f) + 6.0f/30.0f;
		iTween.RotateTo(gameObject, iTween.Hash("time", delay, "onComplete", "allFlowersClosed"));
		
		if(tracingLines != null) {
			while (tracingLines.Count > 0) {
				DestroyObject((GameObject)tracingLines[0]);
				tracingLines.RemoveAt(0);
			}	
		}
		tracingLines = null;
	}
	
	public void allFlowersClosed() {
		while (flowers.Count > 0) {
			DestroyObject((GameObject)flowers[0]);
			flowers.RemoveAt(0);
		}
		flowers = null;
		
		if(_delegate && _delegate.GetType() == typeof(BWNumberSeqGame)) {
			((BWNumberSeqGame)_delegate).flowerLayerDestroyed();
		}else if(_delegate && _delegate.GetType() == typeof(BWNumberArithmetic)) {
			((BWNumberArithmetic)_delegate).flowerLayerDestroyed();
		}
	}
	
	public void resetFlowersLayer () {
		foreach(GameObject flower in flowers) {
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			flowerObj.setDisabled();
		}
		
		if(tracingLines != null) {
			while (tracingLines.Count > 0) {
				DestroyObject((GameObject)tracingLines[0]);
				tracingLines.RemoveAt(0);
			}	
			tracingLines = new ArrayList();
		}
	}
	
	public GameObject flowerWithNumber(int number) {
		foreach(GameObject flower in flowers) {
			BWFlower flowerObj = flower.GetComponent<BWFlower>();
			if(flowerObj.getFlowerNumber() == number) {
				return flower;
			}
		}
		return null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
