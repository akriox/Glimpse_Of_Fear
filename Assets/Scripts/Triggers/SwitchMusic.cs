using UnityEngine;
using System.Collections;

public class SwitchMusic : MonoBehaviour {
	[SerializeField] private GameObject _soundtrackToPlay;
	[SerializeField] private AudioClip soundToPlay;
	[SerializeField][Range(0.0F, 1.0F)] private float volume;
	[SerializeField] private GameObject _soundtrackToStop;
	[SerializeField] private bool loop;
	[SerializeField] private GameObject thisTrigger;
	[SerializeField] private GameObject otherTrigger;
	private AudioClip _audioClip;
	void Start(){
		_audioClip = (AudioClip)Resources.Load("Audio/blank_sound", typeof(AudioClip));
	}

	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			_soundtrackToPlay.GetComponent<FadingAudioSource> ().Fade (soundToPlay, volume, loop);
			_soundtrackToStop.GetComponent<FadingAudioSource> ().Fade (_audioClip, 0.0f, false);
			yield return new WaitForSeconds(0.5f);
			thisTrigger.SetActive(false);
			otherTrigger.SetActive(true);
		}
	}
}
