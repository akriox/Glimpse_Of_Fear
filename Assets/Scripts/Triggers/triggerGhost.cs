using UnityEngine;
using System.Collections;

public class TriggerGhost : MonoBehaviour {

	[SerializeField] private AudioClip _audioClip;
	private bool alreadyCall;
	[SerializeField] private GameObject _activ;
	private bool delay;

	public void Start(){
		alreadyCall = false;
	}

	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && !alreadyCall) {
			alreadyCall = true;
			if(delay)yield return new WaitForSeconds(7f);
			_activ.SetActive(true);
			StartCoroutine(ProjectorObject.Flicker(50));
			yield return new WaitForSeconds(0.3f);
			EventSound.playClip(_audioClip, 0.3f);
			yield return new WaitForSeconds(25f);
			_activ.SetActive(false);
			Destroy(this.gameObject);
		}
	}
}
