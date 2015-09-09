using UnityEngine;
using System.Collections;

public class EventSound : MonoBehaviour {
	
	public static AudioSource audioSource;
	
	public void Awake () {
		audioSource = GetComponent<AudioSource>();
	}
	
	public static void playClip(AudioClip clip){
		audioSource.clip = clip;
		audioSource.Play();
	}

	public static void playClip(AudioClip clip, float v){
		audioSource.volume = v;
		audioSource.clip = clip;
		audioSource.Play();
	}

	public static void volume(float v){
		audioSource.volume = v;
	}
}
