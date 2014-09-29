using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {


	private AndroidJavaObject mDOTMBanner;
	private AndroidJavaObject mDOTMInstantiase;

	private AndroidJavaObject mAdmobBanner;
	private AndroidJavaObject mAdmobInstantiase;

	private AndroidJavaObject mAdColonyBanner;
	private AndroidJavaObject mAdColonyInstantiase;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		if (GUI.Button (new Rect (100, 100, 100, 100), "AD Mob Banner")) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			
			AndroidJavaClass agnitusPlugin = new AndroidJavaClass ("com.agnitus.plugin.AgnitusPlugin");
			
			
			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",0,"ca-app-pub-1488171828339645/2109915816",1);
			mAdmobBanner = agnitusPlugin.CallStatic<AndroidJavaObject>("getAdNetwork", ctx, credential);
			mAdmobBanner.Call("ShowBannerAd");
		}

		if (GUI.Button (new Rect (300, 100, 200, 100), "AD Mob Banner Remove")) {
			mAdmobBanner.Call("DestroyBanner");			
		}


		if (GUI.Button (new Rect (100, 300, 200, 100), "AD Interstial Banner")) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			
			AndroidJavaClass agnitusPlugin = new AndroidJavaClass ("com.agnitus.plugin.AgnitusPlugin");
			

			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",0,"ca-app-pub-1488171828339645/3586649018",1);
			
			mAdmobInstantiase =  agnitusPlugin.CallStatic<AndroidJavaObject>("getAdNetwork", ctx, credential);
			mAdmobInstantiase.Call("ShowInterstitial");

		}


		if (GUI.Button (new Rect (100, 500, 200, 100), "ADColony Interstiali Banner")) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			
			AndroidJavaClass agnitusPlugin = new AndroidJavaClass ("com.agnitus.plugin.AgnitusPlugin");
			

//			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",1,"app923a253787894b62b5",1,"vzfb4c767f89d1445d98","version:2.1,store:google");
			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",1,"app923a253787894b62b5",1,"vzfb4c767f89d1445d98","version:2.1,store:google");	
			mAdColonyInstantiase =  agnitusPlugin.CallStatic<AndroidJavaObject>("getAdNetwork", ctx, credential);
			mAdColonyInstantiase.Call("ShowInterstitial");

		}

		if (GUI.Button (new Rect (100, 700, 200, 100), "MDOTM  Banner")) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			
			AndroidJavaClass agnitusPlugin = new AndroidJavaClass ("com.agnitus.plugin.AgnitusPlugin");
			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",2,"58368a7753170d1f6d0bcab05ef82037",1);
			mDOTMBanner = agnitusPlugin.CallStatic<AndroidJavaObject>("getAdNetwork", ctx, credential);


			mDOTMBanner.Call("ShowBannerAd");

		}


		if (GUI.Button (new Rect (300, 700, 200, 100), "MDOTM  Banner Remove")) {
			mDOTMBanner.Call("DestroyBanner");			
		}

		if (GUI.Button (new Rect (100, 900, 200, 100), "Vungle  Interstiali Banner")) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			
			AndroidJavaClass agnitusPlugin = new AndroidJavaClass ("com.agnitus.plugin.AgnitusPlugin");
			
			
			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",3,"53f49aa1e209781d7500008c",1);
			
			AndroidJavaObject obj =  agnitusPlugin.CallStatic<AndroidJavaObject>("getAdNetwork", ctx, credential);
			obj.Call("ShowInterstitial");
			
		}


		if (GUI.Button (new Rect (100, 900, 200, 100), "Buy In App")) {


			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			
//			AndroidJavaClass agnitusPlugin = new AndroidJavaClass ("com.agnitus.plugin.inapp.GoogleInApp",ctx,);
			
			
			AndroidJavaObject credential = new AndroidJavaObject("com.agnitus.plugin.model.Credentials",3,"53f49aa1e209781d7500008c",1);
			
			AndroidJavaObject obj =  agnitusPlugin.CallStatic<AndroidJavaObject>("getAdNetwork", ctx, credential);
			obj.Call("ShowInterstitial");
			
		}

	}
}
