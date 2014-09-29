using UnityEngine;
using System.Collections;

public class FW_NumberController : MonoBehaviour {

	
	public GameObject particles = null;

	
	public void setNumber(int number) {
		string imageName = string.Format("FishWorld_HideNSeek/Sprites/Numbers/FishWorld_HidenSeek_{0}_iPadHD", number);
		renderer.material.mainTexture = Resources.Load(imageName) as Texture2D;
	}
	
	
}
