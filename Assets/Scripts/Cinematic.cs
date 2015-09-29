using UnityEngine;
using System.Collections;

public class Cinematic : MonoBehaviour {

	private Camera _camera;
	private Animator _animCamera;

	private AudioSource _audioSource;
	public AudioClip[] playerVoice;
	public AudioClip[] reaperVoice;

	private int lookAround = Animator.StringToHash("LookAround");
	private int turnAround = Animator.StringToHash("TurnAround");

	public void Start () {
		_camera = Camera.main;
		_animCamera = _camera.GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();

		StartCoroutine(AnimateCamera(2.0f));

		/* 
		 *
		 *	SPAWN REAPER 
		 *
		 */

		StartCoroutine(TestDialog());
	}

	private IEnumerator AnimateCamera(float delay){
		yield return new WaitForSeconds(delay);
		_animCamera.SetTrigger(lookAround);
		_animCamera.SetTrigger(turnAround);
	}

	private IEnumerator TestDialog(){
		_audioSource.clip = playerVoice[0];
		_audioSource.Play();
		yield return new WaitForSeconds(1.8f);
		
		_audioSource.clip = reaperVoice[0];
		_audioSource.Play();
		yield return new WaitForSeconds(4.0f);
		
		_audioSource.clip = playerVoice[1];
		_audioSource.Play();
		yield return new WaitForSeconds(1.0f);
		
		_audioSource.clip = reaperVoice[1];
		_audioSource.Play();
	}
}
