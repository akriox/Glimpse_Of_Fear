using UnityEngine;
using System.Collections;

public class TriggerGhost : MonoBehaviour {
	
	[SerializeField] private AudioClip _audioClip;
	[SerializeField] private GameObject _activ;
	[SerializeField] private bool delay;

	public void Start(){
		//_audioClip = (AudioClip)Resources.Load("Audio/scream_ghost02", typeof(AudioClip));
	}

	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			this.enabled = false;
			if(delay)yield return new WaitForSeconds(7f);
			_activ.SetActive(true);
			StartCoroutine(ProjectorObject.Flicker(50));
			EventSound.playClip(_audioClip, 0.3f);
			yield return new WaitForSeconds(15f);
			_activ.SetActive(false);
			Destroy(this.gameObject);
		}
	}
}
