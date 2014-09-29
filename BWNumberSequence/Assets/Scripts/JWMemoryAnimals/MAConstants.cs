using UnityEngine;
using System.Collections;

public enum MAIckyAnimationState{
	unknown = 0,
	giggle = 1,
	celebration,
	want,
	idle,
	wrong
};

public class MAConstants {
	
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
	
	public static ArrayList ShuffleList(ArrayList _list){
        ArrayList randomizedList = new ArrayList();
        while (_list.Count > 0) {
            int index = Random.Range(0, _list.Count); 
            randomizedList.Add(_list[index]); 
            _list.RemoveAt(index);
        }
        return randomizedList;
   	}
}
