/// <summary>
/// Sound manager.
/// This script use for manager all sound(bgm,sfx) in game
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	
	[System.Serializable]
	public class SoundGroup{
		public AudioClip audioClip;
		public string soundName;
	}
	
	public AudioSource bgmSound;
	
	public List<SoundGroup> sound_List = new List<SoundGroup>();
	
	public static SoundManager instance;
	
	public void Start(){
		instance = this;	
		StartCoroutine(StartBGM());
	}
	
	public void PlayingSound(string _soundName){
		AudioSource.PlayClipAtPoint(sound_List[FindSound(_soundName)].audioClip, Camera.main.transform.position);
	}
	
	private int FindSound(string _soundName){
		int i = 0;
		while( i < sound_List.Count ){
			if(sound_List[i].soundName == _soundName){
				return i;	
			}
			i++;
		}
		return i;
	}
	
	void ManageBGM()
	{
		StartCoroutine(StartBGM());
	}
	
	//Start BGM when loading complete
	IEnumerator StartBGM()
	{
		yield return new WaitForSeconds(0.5f);
		
		while(PatternSystem.instance.loadingComplete == false)
		{
			yield return 0;
		}
		
		//Debug.Log("play");
		bgmSound.Play();
	}
	
}
