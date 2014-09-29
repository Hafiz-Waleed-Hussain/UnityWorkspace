
using UnityEngine;
using System.Collections;
 

public class TapMove : MonoBehaviour {
	
	private AndroidJavaObject javaManager;
	
	private string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
	private WWW downloader = null;
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	
	void OnGUI() {
        /*
        if (GUI.Button(new Rect(10, 70, 150, 100), "Launch Dashboard")){
			Application.Quit();
				return;       
		}
		
		if (GUI.Button(new Rect(200, 70, 150, 100), "Get Child")){
			//using(var managerClass = new AndroidJavaClass("com.agnitus.unitygame.UnityJavaManager"))
			//	javaManager = managerClass.CallStatic<AndroidJavaObject>("sharedManager");
			//javaManager.Call("printlog");
			
			//AndroidJavaClass pManager = new AndroidJavaClass("com.agnitus.db.PersistanceManager");
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject pManager = new AndroidJavaObject ("com.agnitus.db.PersistanceManager", ctx);
			
			AndroidJavaObject result = pManager.Call<AndroidJavaObject>("getResultForCategoryId", 179);
			
			int level = result.Call<int>("getCurrentLevel");
			int tLevels = result.Call<int>("getTotalLevels");
			string dict = result.Call<string>("getResultDataDic");
			
			if(dict != null) {
				Hashtable resultdict = MiniJSON.jsonDecode(dict) as Hashtable;
				//if(object.GetType() == typeof(Hashtable)) {
				Debug.Log(resultdict.GetType());
				//} else {
				
			}
			//string des = string.Format("Level: {0} - Total Levels: {1} - Result: {2}", level, tLevels, dict);
			
			//Debug.Log(des);
		}
		
		if(GUI.Button(new Rect(400, 70, 150, 100), "download")) {
			if(downloader == null) {
				downloader = new WWW(url);
			}
		}
		
		if(GUI.Button(new Rect(400, 70, 150, 100), "Bee World")) {
			AGGameState.switchWorlds(AGWorldIndex.k_BeeWorld);
		}
		if(GUI.Button(new Rect(600, 70, 150, 100), "Jungle World")) {
			AGGameState.switchWorlds(AGWorldIndex.k_JungleWorld);
		}
		/*
		/*
		if (downloader != null && downloader.isDone == false ) {
			GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), progressBarEmpty);
			GUI.DrawTexture(new Rect(pos.x, pos.y, size.x * Mathf.Clamp01(downloader.progress), size.y), progressBarFull);
		}*/
    }

	void Update() {
	}
}
