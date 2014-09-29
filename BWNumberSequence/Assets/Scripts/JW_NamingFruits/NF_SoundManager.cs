#region AGNITUS 2013
/* JungleWorld- Naming Fruits Game
 * Developer- Asema Hassan
 * Unity3D*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NF_SoundManager:MonoBehaviour{
	
public AudioSource a_Source = null;
public AudioSource sfx_Source = null;
public List<AudioClip> currentClips = null;	

void Update(){
	checkVoiceOverSource();	
	}
	
private void checkVoiceOverSource() {
		
		if(a_Source==null){
			return;
		}
		if(!a_Source.isPlaying && currentClips.Count > 0) {
			
			if(a_Source.clip == currentClips[0]) {
				currentClips.RemoveAt(0);
			}
			
			if(currentClips.Count > 0) {
				a_Source.clip = currentClips[0];
				a_Source.Play();
				} 
			}
	}
	
public static void playAudioList(List<AudioClip> clips) {
		
		NF_SoundManager soundMng =(NF_SoundManager)Camera.mainCamera.GetComponentInChildren<NF_SoundManager>();
			if(soundMng.a_Source==null){
				soundMng.a_Source = soundMng.AddAudio(null,false,false,1.0f);
					}
			if(clips != null && clips.Count > 0) {
			
			soundMng.a_Source.Stop();
			soundMng.currentClips = clips;
		}
	}
	
public static void playSFX(AudioClip _clip){
		
	NF_SoundManager soundMng =(NF_SoundManager)Camera.mainCamera.GetComponentInChildren<NF_SoundManager>();
		if(soundMng.sfx_Source==null){
				soundMng.sfx_Source = soundMng.AddAudio(null,false,false,1.0f);
			}
		soundMng.sfx_Source.PlayOneShot(_clip);
	}
public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
		
  		AudioSource newAudio = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
  		newAudio.clip = clip;
  		newAudio.loop = loop;
  		newAudio.playOnAwake = playAwake;
  		newAudio.volume = vol;
  		return newAudio;
	}
}
