using UnityEngine;
using System.Collections;

public class PG_ShapeContainerStatController : MonoBehaviour {
 		PG_SoundManager soundManager;
	
	
	
	public void playAnimation()
	{
		 soundManager					=	gameObject.GetComponent<PG_SoundManager>();
		 animation.Play();
		
		if(soundManager!=null){
			 soundManager.playInstructionSound(new string[]{PG_Constants._soundclipPath +PG_Constants._rollingShapes});
			//Debug.Log("rolling shapes: "+PG_Constants._soundclipPath +PG_Constants._rollingShapes);
		}
		
		//else Debug.Log("soundManager is null");
		
	}
	
}
