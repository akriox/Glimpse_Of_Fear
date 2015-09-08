using UnityEngine;
using System.Collections;

public class VoiceOver : MonoBehaviour {

	public static AudioSource _audioSource;

	public void Start () {
		_audioSource = GetComponent<AudioSource>();
	}

	public static void Talk(AudioClip clip){
		_audioSource.clip = clip;
		_audioSource.Play();
	}

	public static void stop(){
		if (_audioSource.isPlaying)
			_audioSource.Stop ();
	}
}