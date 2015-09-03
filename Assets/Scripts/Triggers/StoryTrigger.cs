using UnityEngine;
using System.Collections;

public class StoryTrigger : MonoBehaviour {
	
	public string str;
	public AudioClip voice;
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			if(Settings.subtitles) GameController.Instance.displayDebug(str);
			if(voice != null) VoiceOver.Talk(voice);
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			if(Settings.subtitles) GameController.Instance.displayDebug("");
			Destroy(this.gameObject);
		}
	}
}
