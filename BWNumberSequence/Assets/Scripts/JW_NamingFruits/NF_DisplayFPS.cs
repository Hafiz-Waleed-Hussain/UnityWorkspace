#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;
 
public class NF_DisplayFPS : MonoBehaviour 
{
public  float updateInterval = 0.5F;
public GUIText gText;
private float accum   = 0; // FPS accumulated over the interval
private int   frames  = 0; // Frames drawn over the interval
private float timeleft; // Left time for current interval
 
void Start(){
	
	Application.targetFrameRate=60;
    if( !gText )
    {
        Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
        enabled = false;
        return;
    }
    timeleft = updateInterval;  
}
 
void Update(){
		
    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
 
    // Interval ended - update GUI text and start new interval
    if( timeleft <= 0.0 )
    {
	float fps = accum/frames;
	string format = System.String.Format("{0:F2} FPS",fps);
	gText.text = format;
 
	if(fps < 30)
		gText.material.color = Color.yellow;
	else 
		if(fps < 10)
			gText.material.color = Color.red;
		else
			gText.material.color = Color.white;
        timeleft = updateInterval;
        accum = 0.0F;
        frames = 0;
    	}
	}
}