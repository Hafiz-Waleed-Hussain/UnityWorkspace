using UnityEngine;
using System.Collections;

public class SoundEffect : MonoBehaviour {


	public AudioClip explosionSound;

	public static SoundEffect Instance;

	void Awake(){

		Instance = this;
	}

	public void makeExplosionSound(){
		playSound (explosionSound);
	}

	private void playSound(AudioClip audioClip){

		AudioSource.PlayClipAtPoint (audioClip, transform.position);
	}

}
