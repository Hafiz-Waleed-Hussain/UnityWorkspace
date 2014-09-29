using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempCategoryData {
	
	public static void addDummyCategoryData() {
		int [] categoryIDs = {169, 178, 179, 180, 51, 132, 32, 131};
		
		for(int i = 0; i<categoryIDs.Length; i++) {
			CoreDataManager coreManager = new CoreDataManager(categoryIDs[i]);
			Hashtable allCategoriesCoreData = coreManager.getResultDict();
			
			foreach(string key in allCategoriesCoreData.Keys) {
				Hashtable objDict = (Hashtable)allCategoriesCoreData[key];
				
				int attempted = AGGameState.GetAsInt(objDict["attempted"]);
				int solved = AGGameState.GetAsInt(objDict["solved"]);
				
				objDict["attempted"] = attempted + UnityEngine.Random.Range(10,13);
				objDict["solved"] = solved + UnityEngine.Random.Range(6,10);
			}
			
			coreManager.setResultDict(allCategoriesCoreData);
			coreManager.saveManager();
		}
		
		
	}
	
	
}
