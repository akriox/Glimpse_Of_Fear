using UnityEngine;
using System.Collections;

public class TriggerGhostBridge : MonoBehaviour
{

    private bool alreadyCall;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private GameObject _ghost;
    [SerializeField]
    private GameObject _blackTrail;

    public void Start()
    {
        alreadyCall = false;
    }
  
	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && !alreadyCall) {
			alreadyCall = true;
			_ghost.SetActive(true);
			StartCoroutine(ProjectorObject.Flicker(50));
			yield return new WaitForSeconds(1f);
			_blackTrail.SetActive(true);
			EventSound.playClip(_audioClip, 0.3f);
			yield return new WaitForSeconds(2.5f);
			_ghost.SetActive(false);
			yield return new WaitForSeconds(10f);
			_blackTrail.SetActive(false);
			Destroy(this.gameObject);
		}
	}
}
