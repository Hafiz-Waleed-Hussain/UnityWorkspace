using UnityEngine;
using System.Collections;

public class CoreDataManager {
	
	private int skillID;
	private AndroidJavaObject result = null;
	private Hashtable resultDict = null;
	
	private int currentLevel = 1;
	private int totalLevels = 1;
	private int categoryStatus = 0;
	
	public CoreDataManager (int skillId) {
		skillID = skillId;
		
		AndroidJavaObject persistanceManager = AGGameState.getPersistanceManager();
		
		if(persistanceManager != null) {
			result = persistanceManager.Call<AndroidJavaObject>("getResultForCategoryId", skillID);
			
			currentLevel = result.Call<int>("getCurrentLevel");
			totalLevels = result.Call<int>("getTotalLevels");
			categoryStatus = result.Call<int>("getCategoryStatus");
			
			string dict = result.Call<string>("getResultDataDic");
			
			AGGameState.setChild(persistanceManager.Call<AndroidJavaObject>("getCurrentChild"));
			
			if(dict != null) {
				resultDict = MiniJSON.jsonDecode(dict) as Hashtable;
			}
		}
	}
	
	public void setResultDict (Hashtable dict) {
		if(dict != null) {
			resultDict = dict;
			string json = dict.toJson();
			result.Call("setResultDataDic", json);
		}
	}
	
	public Hashtable getResultDict () {
		return resultDict;
	}
	
	public void setCurrentLevel (int level) {
		currentLevel = level;
		result.Call("setCurrentLevel", level);
	}
	
	public int getCurrentLevel () {
		return currentLevel;
	}
	
	public int getTotalLevels () {
		return totalLevels;
	}
	
	public void setCategoryStatus (int status) {
		categoryStatus = status;
		result.Call("setCategoryStatus", status);
	}
	
	public int getCategoryStatus () {
		return categoryStatus;
	}
	
	public void saveManager () {
		AndroidJavaObject persistanceManager = AGGameState.getPersistanceManager();
		
		if(persistanceManager != null) {
			persistanceManager.Call("saveResult", result);
		}
	}
}
