using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FW_SoundManager : MonoBehaviour {

	public List<AudioClip> currentClips;
	AudioSource voiceOverSource;
	AudioSource soundEffects;
	private bool isPlayingSuccessSound = false;
	
	
	void Start(){
		
		currentClips = new List<AudioClip>();
		voiceOverSource = AddAudio(null, false, false, 1.0f);
		soundEffects = AddAudio(null, false, false, 1.0f);
	}
	
	void Update(){
		checkVoiceOverSource();
		}
	
	
		private void checkVoiceOverSource() {
		if(!voiceOverSource.isPlaying && currentClips.Count > 0) {
			
			if(voiceOverSource.clip == currentClips[0]) {
				currentClips.RemoveAt(0);
			}
			
			if(currentClips.Count > 0) {
				voiceOverSource.clip = currentClips[0];
				voiceOverSource.Play();
			} else {
				if(isPlayingSuccessSound) {
					isPlayingSuccessSound = false;
				}
			}
		} 
	}
	
	
	
	
		private void playAudioList(List<AudioClip> clips) {
		if(clips != null && clips.Count > 0) {
			if(voiceOverSource!=null && voiceOverSource.isPlaying)
				//voiceOverSource.Stop();
				currentClips = clips;
		}
	}
		
	public void stopAudioList(){
		    if(voiceOverSource!=null && voiceOverSource.isPlaying)
				voiceOverSource.Stop();
	}
	
	public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
  		AudioSource newAudio = (AudioSource)gameObject.AddComponent(typeof(AudioSource)); //.AddComponent(AudioSource);
  		newAudio.clip = clip;
  		newAudio.loop = loop;
  		newAudio.playOnAwake = playAwake;
  		newAudio.volume = vol;
  		return newAudio;
	}
	
	
	
	

 
	
	public void playInstructionSound (string[] instructions) {
		//if(isPlayingSuccessSound)
		//	return;
		AudioClip numerClip;
		
		if(voiceOverSource!=null && voiceOverSource.isPlaying)
				voiceOverSource.Stop();
		if(voiceOverSource!=null)
		{
			for(int i=0; i<instructions.Length;i++ )
			{
			numerClip = loadSound(instructions[i]);
        	currentClips.Add(numerClip);
			}
		}
		else 
		{
			for(int i=0; i<instructions.Length;i++ ){
			numerClip = loadSound(instructions[i]);
        	currentClips.Add(numerClip);
		  }
			 
	    	isPlayingSuccessSound = true;
			playAudioList(currentClips);
			
		}

	}
	
	public void playSoundEffect (string effect) {
		soundEffects.clip = loadSound(effect);
		soundEffects.Play();
	}
	
	AudioClip loadSound(string clipName) {
		 return (AudioClip)Resources.Load(string.Format(clipName));
	}
	
	
	public void playSuccessOnScreenComplete(){
		int _rand = Random.Range(3,5);
		playSoundEffect(FW_Constants._soundClips+string.Format("win_{0}",_rand));
	}
	
 }





