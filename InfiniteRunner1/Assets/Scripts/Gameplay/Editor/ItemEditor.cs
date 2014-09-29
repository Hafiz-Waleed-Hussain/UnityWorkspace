using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
	
	public Object instance;
	public Item itemTarget;
	public Object targetPref;

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		itemTarget = target as Item;

		if(itemTarget.isEditing == false){
			if(PrefabUtility.GetPrefabParent(target) == null && PrefabUtility.GetPrefabObject(target) != null){
				if(GUILayout.Button("Setting Item")){
					OpenWindowSettingItem();
				}
			}
		}else{
			if(GUILayout.Button("Apply")){
				CloseWindowSettingItem();
			}
		}
	}

 	private void OpenWindowSettingItem(){

		string scenePath = EditorApplication.currentScene;
		EditorApplication.NewScene();
		RenderSettings.ambientLight = Color.white;
		instance =  Instantiate(target, Vector3.zero, Quaternion.identity);
		instance.name = target.name;
		targetPref = target;
		Selection.activeObject = instance;
		itemTarget = instance as Item;
		itemTarget.isEditing = true;
		itemTarget.targetPref = targetPref;
		itemTarget.scenePath = scenePath;


	}

	private void CloseWindowSettingItem(){
		DestroyImmediate (itemTarget.point1);
		DestroyImmediate (itemTarget.point2);
		DestroyImmediate (itemTarget.point3);
		DestroyImmediate (itemTarget.textY);
		DestroyImmediate (itemTarget.textZ);
		itemTarget.isEditing = false;
		if(itemTarget.transform.parent != null){
			PrefabUtility.ReplacePrefab(itemTarget.transform.parent.gameObject, itemTarget.targetPref, ReplacePrefabOptions.ConnectToPrefab);
			EditorApplication.OpenScene (itemTarget.scenePath);
		}else{
			PrefabUtility.ReplacePrefab(itemTarget.gameObject, itemTarget.targetPref, ReplacePrefabOptions.ConnectToPrefab);
			EditorApplication.OpenScene (itemTarget.scenePath);
		}
	}

}
