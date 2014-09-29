using UnityEngine;
using System.Collections;

public class FW_Constants{

	
	// --------- Sound Clips
	public static string _soundClips = "FishWorld_HideNSeek/SoundClips/";
	
	public static float _idleTime = 10.0F;
	
	
	
	
	
	
	
	
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
}
