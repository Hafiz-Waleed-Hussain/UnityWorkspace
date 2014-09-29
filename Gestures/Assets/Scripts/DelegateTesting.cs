using UnityEngine;
using System.Collections;

public class DelegateTesting : MonoBehaviour {

	delegate void MyDelegate(int num);
	MyDelegate myDelegate;

	void Start () {

		myDelegate = printInt;
		myDelegate (13);

		myDelegate = printDounle;
		myDelegate (13);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void printInt(int num){
		Debug.Log (num);
	}

	void printDounle(int num){
		Debug.Log (num*2);
	}
}
