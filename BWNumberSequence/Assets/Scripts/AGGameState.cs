#define MAC_PLATFORM


using UnityEngine;
using System.Collections;

public class AGGameState {
	
	private static AndroidJavaObject persistanceManager = null;
	private static AndroidJavaObject currentChild = null;
	public static float startingTime;
	public static int starCount = 0;
	
	public static int currentGameIndex =0;
	public static void loadNextGameInLoop(AGGameIndex gameIndex){
		
		Debug.Log("Game switched");
		int gIndex = (int) gameIndex;
		gIndex++;
		startGame((AGGameIndex)gIndex);
	}
	
	public static void startGame (AGGameIndex gameIndex) {
		
		currentGameIndex = (int)gameIndex;
		switch (gameIndex) {
		
		case AGGameIndex.k_Beeworld_NumberSequence:
			Application.LoadLevel("BW_NumberSequence");
			break;
			
		case AGGameIndex.k_Beeworld_CoutingUpDown:
			Application.LoadLevel("BW_NumberLineArithmetic");
			break;	
			
		case AGGameIndex.k_NamingFruits:
			Application.LoadLevel("NamingFruits");
			break;
			
		case AGGameIndex.k_ShapePond:
			Application.LoadLevel("JWPondGame");
			break;
			
		case AGGameIndex.k_MemoryAnimal:
			Application.LoadLevel("JW_MemoryAnimals");
			break;
		case AGGameIndex.k_2DShapesColors:{
				Application.LoadLevel("NamingFruits");
		}break;
			
		case AGGameIndex.k_FishWorld_HideNSeek:{
				Application.LoadLevel("FishWorld_HideNSeek");
		}break;	
			
		default:
			startGame(AGGameIndex.k_NamingFruits);
			return;
		}
	}
	
	public static void switchWorlds (AGWorldIndex worldIndex) {
		switch (worldIndex) {
		case AGWorldIndex.k_JungleWorld:
			Application.LoadLevel("JungleWorldMenu");
			break;
		case AGWorldIndex.k_BeeWorld:
			Application.LoadLevel("BeeWorldMenu");
			break;
		case AGWorldIndex.k_ShapeWorld:
			Application.LoadLevel("ShapeWorldMenu");
			break;
		case AGWorldIndex.k_IckyWorld:
			Application.LoadLevel("IckyWorldMenu");
			break;
		case AGWorldIndex.k_FishWorld:
			Application.LoadLevel("FishWorldMenu");
			break;
		case AGWorldIndex.k_CircusWorld:
			Application.LoadLevel("CircusWorldMenu");
			break;
		case AGWorldIndex.k_VideoWorld:
			Application.LoadLevel("VideoWorldMenu");
			break;
			
		default:
			break;
		}
	}
	public static void gotoHomeScreen () {
		Application.LoadLevel("HomeScreen");
	}
	
	public static void gotoDashboard () {
		
		double timeSpent = (double) (Time.time - AGGameState.startingTime);
		AndroidJavaObject persistanceManager = getPersistanceManager();
		persistanceManager.Call("addChildSessionTimeLog", (double) timeSpent);
		
		AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass dashboard = new AndroidJavaClass("com.agnitus.ui.AgnitusMain");
		dashboard.CallStatic("launch", ctx);
		
		//Application.Quit();	
	}
	
	public static void switchChild () {
		//startFromGame
			
		AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass switchChild = new AndroidJavaClass("com.agnitus.ui.AddChildActivity");
		switchChild.CallStatic("startFromGame", ctx);
	}
	
	/*public static void gotoBeeWorldMenu () {
		Application.LoadLevel("BeeWorldMenu");
	}
	
	public static void gotoJungleWorld(){
		Application.LoadLevel("JungleWorldMenu");
	}*/
	
	
	public static AndroidJavaObject getPersistanceManager (){
		
		if(persistanceManager == null) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			persistanceManager = new AndroidJavaObject ("com.agnitus.db.PersistanceManager", ctx);	
			
			AGGameState.startingTime = Time.time;
			
			if(PlayerPrefs.GetInt("DummyDataAdded") == 0) {
				TempCategoryData.addDummyCategoryData();
				PlayerPrefs.SetInt("DummyDataAdded", 1);
			}
		}
		
		if(persistanceManager == null) {
			Debug.Log("something went wrong ... cannot access persistance manager from android java");
			return null;
		}
		
		return persistanceManager;
	}
	
	public static void setChild (AndroidJavaObject _child) {
		currentChild = _child;
	}
	
	public static AndroidJavaObject getCurrentChild () {
#if (!MAC_PLATFORM)
			if(persistanceManager == null) {
				getPersistanceManager();
			}
			currentChild = persistanceManager.Call<AndroidJavaObject>("getCurrentChild");
#endif
		return currentChild;
	}
	
	public static int getChildCount () {
		int childCount = 0;
#if (!MAC_PLATFORM)
		if(persistanceManager == null) {
			getPersistanceManager();
		}
		childCount = (int)persistanceManager.Call<long>("getChildrenCount");
#endif
		return childCount;
	}
	
	public static byte [] getChildImage () {
#if (!MAC_PLATFORM)
		
		currentChild = getCurrentChild();
		byte [] data = null;
		data = currentChild.Call<byte[]>("getChildPhoto");
		return data;
#else
		return null;
#endif
	}
	
	public static void incrementStarCount() {
		starCount++;
#if (!MAC_PLATFORM)
		setStarCount(getStarCount() + 1);
#endif
	}
	
	public static void setStarCount (int star) {
		starCount = star;
#if (!MAC_PLATFORM)
		currentChild = getCurrentChild();
		currentChild.Call("setStarsCount", star);
		
		persistanceManager.Call("saveChild", currentChild);
#endif
	}
	
	public static int getStarCount () {
#if (!MAC_PLATFORM)
		currentChild = getCurrentChild();
		starCount = currentChild.Call<int>("getStarsCount");
		return starCount;
#else
		return starCount = 0;
#endif
		
	}
	
	
	public static string getChildName () {
#if (!MAC_PLATFORM)
		currentChild = getCurrentChild();
		return currentChild.Call<string>("getChildName");
#else
		return "Child";
#endif
		
	}
	
	/*
	public static Hashtable resultDataForSkill (int skillId) {
		
		if(persistanceManager == null) {
			AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject ctx = player.GetStatic<AndroidJavaObject>("currentActivity");
			persistanceManager = new AndroidJavaObject ("com.agnitus.db.PersistanceManager", ctx);	
		}
		
		if(persistanceManager == null) {
			Debug.Log("something went wrong cannot access persistance manager from android java");
			return null;
		}
		
		AndroidJavaObject result = persistanceManager.Call<AndroidJavaObject>("getResultForCategoryId", skillId);		
		AndroidJavaObject resultDictString = result.Call<AndroidJavaObject>("getResultDataDic");
		
		string str = (string)resultDictString.ToString();
		
		Debug.Log(str);
		
		return null;
	}
	*/
	
	public static void printHashtable (Hashtable dict) {
		if(dict == null) return;
		foreach(DictionaryEntry entry in dict) {
			Debug.Log(entry.Value + " : " + entry.Key + " key type" + entry.Key.GetType());
		}
	}
	
	public static void printArrayList (ArrayList arr) {
		if(arr == null) return;
		foreach(object obj in arr) {
			Debug.Log(obj);
		}
	}
	
	public static int modInt(int number) {
	    if (number >= 0) {
	        return number;
	    } else {
	        return -number;
	    }
	}
	
	public static float modFloat(float number) {
	    if (number >= 0) {
	        return number;
	    } else {
	        return -number;
	    }
	}
	
	
	public static float GetAsFloat(object theObject){
        if(theObject == null){
            return 0.0f;
        }
		
		if(theObject.GetType() == typeof(int)) {
			return (float)(int)theObject;
		} else if(theObject.GetType() == typeof(long)) {
			return (float)(long)theObject;
		} else if(theObject.GetType() == typeof(float)) {
			return (float)theObject;
		} else if(theObject.GetType() == typeof(double)) {
			return (float)(double)theObject;
		}
		
        return (float)theObject;
    }
	
	public static int GetAsInt(object theObject){
        if(theObject == null){
            return 0;
        }
        
		if(theObject.GetType() == typeof(int)) {
			return (int)theObject;
		} else if(theObject.GetType() == typeof(long)) {
			return (int)(long)theObject;
		} else if(theObject.GetType() == typeof(float)) {
			return (int)(float)theObject;
		} else if(theObject.GetType() == typeof(double)) {
			return (int)(double)theObject;
		}
		
        return (int)theObject;
    }
}
