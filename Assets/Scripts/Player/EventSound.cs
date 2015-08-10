using UnityEngine;
using System.Collections;

public class EventSound : MonoBehaviour {
	
	public static AudioSource audioSource;
	
	public void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	public static void playClip(AudioClip clip){
		audioSource.clip = clip;
		audioSource.Play();
	}
}
