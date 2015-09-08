using UnityEngine;
using System.Collections;

public class TriggerMusic : MonoBehaviour {
	public GameObject soundtrack;
	[SerializeField] private AudioClip soundtrackToPlay;
	[SerializeField][Range(0.0F, 1.0F)] private float volume;
	[SerializeField] private bool loop;

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			soundtrack.GetComponent<FadingAudioSource> ().Fade (soundtrackToPlay, volume, loop);
			Destroy(this.gameObject);
		}
	}
}
