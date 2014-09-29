using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PG_SoundManager : MonoBehaviour {
	
	public AudioClip[] allCorrectAnswerClips;
	public AudioClip[] allWrongAnswerClips;
	public AudioClip[] celebrationClips;
	public AudioClip[] differentModClips;
	public AudioClip[] counting;
	
	public AudioClip[] shapesNames_Single;
	public AudioClip[] shapesNames_Plural;
	private AudioSource engine;
	public AudioClip [] introAudioClips;
	public List<AudioClip> currentClips;
	
	AudioSource voiceOverSource;
	AudioSource soundEffects;
	private bool isPlayingSuccessSound = false;
	
	public static bool playIntroClip = false;
	
	void Start()
	{
		voiceOverSource = AddAudio(null, false, false, 1.0f);
		soundEffects = AddAudio(null, false, false, 1.0f);
	}
	
	void Update(){
		checkVoiceOverSource();
		if(playIntroClip)
		  addIntroClips();
		}
	
	
	void addIntroClips(){
		playIntroClip = false;
		AudioClip introClip = introAudioClips[Random.Range(0,introAudioClips.Length)];
		currentClips = new List<AudioClip>();
		currentClips.Add(introClip);
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
				voiceOverSource.Stop();
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
	
	
	
	
	public void playSucess (){
		
		//if(isPlayingSuccessSound)
		//	return;
		stopAudioList();
		int _win=Random.Range(1,10);
		string numer = PG_Constants._soundclipPath + PG_Constants._correctAnswer + string.Format("all_correct_{0}",_win);
		AudioClip numerClip = loadSound(numer);
        currentClips.Add(numerClip);
	     
	    isPlayingSuccessSound = true;
		playAudioList(currentClips);
	}
	
 public	void playWrongSound () {

		//if(isPlayingSuccessSound)
		//return;
		
		stopAudioList();
		int _wrong   = Random.Range(1,4);
		string numer = PG_Constants._soundclipPath + PG_Constants._wrongAnswer + string.Format("wrong_{0}",_wrong);
		AudioClip numerClip = loadSound(numer);
        currentClips.Add(numerClip);
		 isPlayingSuccessSound = true;
		playAudioList(currentClips);
	}
	
	public void playInstructionSound (string[] instructions) {

		AudioClip numerClip;
		
		stopAudioList();
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
	
	
 }
