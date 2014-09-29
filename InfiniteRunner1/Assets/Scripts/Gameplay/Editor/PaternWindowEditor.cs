using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PatternSystem))]
public class PaternWindowEditor : EditorWindow {
	
	private PatternSystem pattern;
	private int indexPatternItem;
	private int indexPatternBuilding;
	
	private float offset_X;
	private float offset_X_Building;
	
	public int[] sizeInt;
	public string[] stringSize;
	
	public int[] sizeInt_Building;
	public string[] stringSize_Building;
	
	private bool setHeight = false;

	[MenuItem ("Window/PatternSetUp")]
	static void Init(){
		EditorWindow.GetWindow(typeof (PaternWindowEditor));	
		
	}
	
	void OnGUI(){
		offset_X = 0;
		offset_X_Building = 300;
		if(GUI.Button(new Rect(55,580,100,50),"Create \n Pattern Item")){
			if(pattern != null){
				pattern.patternItem.Add(new PatternSystem.SetItem());	
				indexPatternItem = pattern.patternItem.Count-1;
			}
		}
		
		//EditorGUILayout.Space();
		
		setHeight = GUI.Toggle(new Rect(250,30,100,300), setHeight, "Set Pattern Height");
		
		if(GUI.Button(new Rect(165,580,100,50),"Delete \n Pattern Item")){
			if(pattern != null){
				if(indexPatternItem != 0){
					pattern.patternItem.RemoveRange(pattern.patternItem.Count-1,1);
				}
				indexPatternItem -= 1;
				if(indexPatternItem <= 0){
					indexPatternItem = 0;	
				}
			}
		}
		
		if(GUI.Button(new Rect(450,350,100,50),"Create \n Pattern Building")){
			if(pattern != null){
				pattern.patternBuilding.Add(new PatternSystem.SetBuilding());	
				indexPatternBuilding = pattern.patternBuilding.Count-1;
			}
		}
		
		//EditorGUILayout.Space();
		
		if(GUI.Button(new Rect(560,350,100,50),"Delete \n Pattern Building")){
			if(pattern != null){
				if(indexPatternBuilding != 0){
					pattern.patternBuilding.RemoveRange(pattern.patternBuilding.Count-1,1);
				}
				indexPatternBuilding -= 1;
				if(indexPatternBuilding <= 0){
					indexPatternBuilding = 0;	
				}
			}
		}
		
		EditorGUILayout.Space();
		
		if(GUI.Button(new Rect(500,580,100,50),"Apply")){
			if(Selection.gameObjects[0].GetComponent<PatternSystem>() != null){
				Debug.Log(PrefabUtility.GetPrefabObject(Selection.gameObjects[0].GetComponent<PatternSystem>()));
				PrefabUtility.ReplacePrefab((Selection.gameObjects[0]), Resources.Load("PatternSystemManager"));
				Debug.Log("Setting Complete");
				EditorUtility.SetDirty(pattern);
			}
		}
		
		ButtonActiveIndex();
		
		if(setHeight == false){
			SlotPatternItem();
		}else{
			SlotPatternItemHeight();	
		}
		
		SlotPatternBuilding();
		ShowDetial();

	}
	
	void Update(){
		if(pattern == null){
			GetTarget();	
		}else{
			return;	
		}
	}
	
	void OnSelectionChange(){
		GetTarget();
	}
	
	private void GetTarget(){
		if(Selection.gameObjects.Length > 0){
			if(Selection.gameObjects[0].GetComponent<PatternSystem>() != null){
				pattern = Selection.gameObjects[0].GetComponent<PatternSystem>();
				Debug.Log(pattern.name);
			}
		}
	}
	
	private void ButtonActiveIndex(){
		if(pattern != null){
			sizeInt = new int[pattern.patternItem.Count];
			stringSize = new string[pattern.patternItem.Count];
			for(int i = 0; i < sizeInt.Length; i++){
				sizeInt[i] = i;
				stringSize[i] = (i).ToString();
			}
			EditorGUI.LabelField(new Rect(100,90-80,200,50),"Active Index Pattern Item");
			indexPatternItem = EditorGUI.IntPopup(new Rect(100,110-80,100,50) ,"", indexPatternItem,stringSize,sizeInt);
			
			sizeInt_Building = new int[pattern.patternBuilding.Count];
			stringSize_Building = new string[pattern.patternBuilding.Count];
			for(int i = 0; i < sizeInt_Building.Length; i++){
				sizeInt_Building[i] = i;
				stringSize_Building[i] = (i).ToString();
			}
			EditorGUI.LabelField(new Rect(450,90-80,200,50),"Active Index Pattern Building");
			indexPatternBuilding = EditorGUI.IntPopup(new Rect(450,110-80,100,50) ,"", indexPatternBuilding,stringSize_Building,sizeInt_Building);
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
	}


	
	private void SlotPatternItem(){
		if(pattern != null){

			EditorGUI.LabelField(new Rect(125,140-80,200,15), "Item Pattern");
			Color normalColor = GUI.color;
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Left.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Left[i].x = EditorGUI.IntField(new Rect(100+offset_X,160-80+(i*15),20,15), (int)pattern.patternItem[indexPatternItem].itemType_Left[i].x);
				if(pattern.patternItem[indexPatternItem].itemType_Left[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Left[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Left[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Left[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(100+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(100+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(100+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}

			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_SubLeft.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x = EditorGUI.IntField(new Rect(125+offset_X,160-80+(i*15),20,15), (int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x);
				if(pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(125+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(125+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(125+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			
			}
			
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Middle.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Middle[i].x = EditorGUI.IntField(new Rect(150+offset_X,160-80+(i*15),20,15), (int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x);
				if(pattern.patternItem[indexPatternItem].itemType_Middle[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(150+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(150+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(150+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}

			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_SubRight.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_SubRight[i].x = EditorGUI.IntField(new Rect(175+offset_X,160-80+(i*15),20,15), (int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x);
				if(pattern.patternItem[indexPatternItem].itemType_SubRight[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(175+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(175+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(175+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}
			
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Right.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Right[i].x = EditorGUI.IntField(new Rect(200+offset_X,160-80+(i*15),20,15), (int)pattern.patternItem[indexPatternItem].itemType_Right[i].x);
				if(pattern.patternItem[indexPatternItem].itemType_Right[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Right[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Right[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Right[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(200+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(200+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(200+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			
			}
		}

	}
	
	private void SlotPatternItemHeight(){
		if(pattern != null){
			EditorGUI.LabelField(new Rect(125,140-80,200,15), "Item Pattern Height");
			Color normalColor = GUI.color;
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Left.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Left[i].y = EditorGUI.FloatField(new Rect(100+offset_X,160-80+(i*15),20,15), pattern.patternItem[indexPatternItem].itemType_Left[i].y);
				if(pattern.patternItem[indexPatternItem].itemType_Left[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Left[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Left[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Left[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(100+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(100+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(100+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}

			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_SubLeft.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_SubLeft[i].y = EditorGUI.FloatField(new Rect(125+offset_X,160-80+(i*15),20,15), pattern.patternItem[indexPatternItem].itemType_SubLeft[i].y);
				if(pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubLeft[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(125+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(125+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(125+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}

			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Middle.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Middle[i].y = EditorGUI.FloatField(new Rect(150+offset_X,160-80+(i*15),20,15), pattern.patternItem[indexPatternItem].itemType_Middle[i].y);
				if(pattern.patternItem[indexPatternItem].itemType_Middle[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Middle[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(150+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(150+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(150+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}

			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_SubRight.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_SubRight[i].y = EditorGUI.FloatField(new Rect(175+offset_X,160-80+(i*15),20,15), pattern.patternItem[indexPatternItem].itemType_SubRight[i].y);
				if(pattern.patternItem[indexPatternItem].itemType_SubRight[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_SubRight[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(175+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(175+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(175+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}
			
			for(int i = 0; i < pattern.patternItem[indexPatternItem].itemType_Right.Length; i++){
				pattern.patternItem[indexPatternItem].itemType_Right[i].y = EditorGUI.FloatField(new Rect(200+offset_X,160-80+(i*15),20,15), pattern.patternItem[indexPatternItem].itemType_Right[i].y);
				if(pattern.patternItem[indexPatternItem].itemType_Right[i].x != 0 && (int)pattern.patternItem[indexPatternItem].itemType_Right[i].x <= pattern.item_Pref.Count){
					float disZ = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Right[i].x-1].GetComponent<Item>().distanceZ;
					GUI.color = pattern.item_Pref[(int)pattern.patternItem[indexPatternItem].itemType_Right[i].x-1].GetComponent<Item>().colorPattern;
					GUI.Box(new Rect(200+offset_X,160-80+(i*15),20,15), "");
					if(disZ<=1)
						GUI.Box(new Rect(200+offset_X,160-80+(i*15),20,15*(-(disZ-1))), "");
					else
						GUI.Box(new Rect(200+offset_X,160-80+(i*15),20,15*(-(disZ))), "");
					GUI.color = normalColor;
				}
			}

		}
	}
	
	private void SlotPatternBuilding(){
		if(pattern != null){
			EditorGUI.LabelField(new Rect(140+offset_X_Building,140-80,200,15), "Building Pattern");
			
			for(int i = 0; i < pattern.patternBuilding[indexPatternBuilding].stateBuilding_Left.Length; i++){
				pattern.patternBuilding[indexPatternBuilding].stateBuilding_Left[i] = EditorGUI.IntField(new Rect(150+offset_X_Building,160-80+(i*15),20,15), pattern.patternBuilding[indexPatternBuilding].stateBuilding_Left[i]);
			}
			
			for(int i = 0; i < pattern.patternBuilding[indexPatternBuilding].stateBuilding_Right.Length; i++){
				pattern.patternBuilding[indexPatternBuilding].stateBuilding_Right[i] = EditorGUI.IntField(new Rect(200+offset_X_Building,160-80+(i*15),20,15), pattern.patternBuilding[indexPatternBuilding].stateBuilding_Right[i]);
			}	
		}
	}
	
	private void ShowDetial(){
		if(pattern != null){

			EditorGUI.LabelField(new Rect(250+offset_X,160-80,200,100),"[ 0 ] = Null");	
			for(int i = 0; i < pattern.item_Pref.Count; i++){
				if(setHeight == false){
					if(pattern.item_Pref[i].GetComponent<Item>() != null){
						pattern.item_Pref[i].GetComponent<Item>().colorPattern = EditorGUI.ColorField(new Rect(240+offset_X,175-80+(i*15),30,15),pattern.item_Pref[i].GetComponent<Item>().colorPattern);
						EditorGUI.LabelField(new Rect(270+offset_X,175-80+(i*15),200,100),"[ "+(i+1)+" ] = "+pattern.item_Pref[i].name+"");
					}
				}else{
					if(pattern.item_Pref[i].GetComponent<Item>() != null){
						EditorGUI.LabelField(new Rect(240+offset_X,175-80+(i*15),200,15),"[Height : "+pattern.item_Pref[i].GetComponent<Item>().distanceY.ToString("0.00")+"]");
						EditorGUI.LabelField(new Rect(320+offset_X,175-80+(i*15),200,100),"[ "+(i+1)+" ] = "+pattern.item_Pref[i].name+"");
					}
				}

			}

			
			EditorGUI.LabelField(new Rect(250+offset_X_Building,160-80,200,100),"[ 0 ] = Null");
			for(int i = 0; i < pattern.building_Pref.Count; i++){
				EditorGUI.LabelField(new Rect(250+offset_X_Building,175-80+(i*15),200,100),"[ "+(i+1)+" ] = "+pattern.building_Pref[i].name+"");
			}
		}
	}
}
