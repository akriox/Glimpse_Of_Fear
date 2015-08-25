using UnityEngine;
using System.Collections;

public class triggerGhost : MonoBehaviour {

	private AudioClip _audioClip;
	private bool alreadyCall;
	[SerializeField] private GameObject _activ;

	public void Start(){
		_audioClip = (AudioClip)Resources.Load("Audio/scream_ghost02", typeof(AudioClip));
		alreadyCall = false;
	}

	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && !alreadyCall) {
			alreadyCall = true;
			_activ.SetActive(true);
			StartCoroutine(ProjectorObject.Flicker(50));
			yield return new WaitForSeconds(4.3f);
			EventSound.playClip(_audioClip,0.8f);
			yield return new WaitForSeconds(7f);
			_activ.SetActive(false);
			this.enabled = false;
		}
	}
}
