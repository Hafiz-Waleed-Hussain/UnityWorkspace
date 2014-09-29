using UnityEngine;
using System.Collections;

public class CategoriesData {
	
	private static CategoriesData sharedData = null;
	
	public ArrayList allCateriesData = null;
	
	Hashtable progressionData = null;
	
	public CategoriesData (string file) {
		loadDataForAllGames(file);
	}
	
	public static CategoriesData getSharedData (string file) {
		
		if (file == null) {
			Debug.Log("empty log");
			return null;
		}
		
		if(sharedData == null) {
			sharedData = new CategoriesData(file);
		}
		
		if(sharedData == null) {
			Debug.Log("something went wrong ... cannot access categories data from plist");
		}
		
		return sharedData;
	}
	
	public void loadDataForAllGames (string file) {
		
		progressionData = null;
		bool noError = PListManager.ParsePListFile(file, ref progressionData);
		
		if(noError) {
			allCateriesData = (ArrayList)progressionData["PreSchoolGames"];
		} else {
			Debug.Log("cannot access progression data");
		}
	}
	
	public Hashtable getGameDataForIndex(AGGameIndex gameIndex) {
		if(allCateriesData == null) {
			Debug.Log("all game data not initialzed call getSharedData with progression file");
			return null;
		}
		return (Hashtable)allCateriesData[(int)gameIndex];
	}
}
