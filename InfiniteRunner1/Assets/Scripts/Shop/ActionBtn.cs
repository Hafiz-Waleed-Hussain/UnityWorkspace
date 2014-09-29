using UnityEngine;
using System.Collections;

public class ActionBtn : MonoBehaviour {

	public Texture2D normal;
	public Texture2D actived;
	public string levelName;
	
	public AudioClip sfxButton;

	private bool ready;

	void OnMouseDown(){
		this.guiTexture.texture = actived;
		
		if(sfxButton != null)
			AudioSource.PlayClipAtPoint(sfxButton,transform.position);
	}


	void OnMouseUpAsButton(){
		this.guiTexture.texture = normal;
		Application.LoadLevel (levelName);
	}
	

	void OnMouseExit(){
		this.guiTexture.texture = normal;
	}
}
