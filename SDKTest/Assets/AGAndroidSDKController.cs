using UnityEngine;
using System.Collections;

public class AGAndroidSDKController : MonoBehaviour {

	public static AGAndroidSDKController instance = null;

	public static AGAndroidSDKController GetInstance () {
		if (instance == null) {
			GameObject sdkController = new GameObject("AGAndroidSDKController");
			instance = sdkController.AddComponent<AGAndroidSDKController>();
		} 

		return instance;
	}

	public static void DestroyInstance () {
		Destroy(instance.gameObject);
		instance = null;
	}

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
