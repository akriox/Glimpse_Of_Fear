using UnityEngine;
using System.Collections;

public class TriggerMusic : MonoBehaviour {
	public GameObject soundtrack;
	[SerializeField] private AudioClip soundtrackToPlay;
	[SerializeField][Range(0.0F, 1.0F)] private float volume;
	[SerializeField] private bool loop;
	[SerializeField] private AudioSource soundtrackWithoutFading;

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			soundtrack.GetComponent<FadingAudioSource> ().Fade (soundtrackToPlay, volume, loop);
			if(soundtrackWithoutFading !=null)soundtrackWithoutFading.Stop();
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
}
