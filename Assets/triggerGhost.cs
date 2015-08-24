using UnityEngine;
using System.Collections;

public class triggerGhost : MonoBehaviour {

	private AudioClip _audioClip;
	[SerializeField] private GameObject _activ;

	public void Start(){
		_audioClip = (AudioClip)Resources.Load("Audio/scream_ghost02", typeof(AudioClip));
	}

	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			EventSound.playClip(_audioClip,0.8f);
			_activ.SetActive(true);

			yield return new WaitForSeconds(12f);
			_activ.SetActive(false);
			this.enabled = false;
		}
	}
}
