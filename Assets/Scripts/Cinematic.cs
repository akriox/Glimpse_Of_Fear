using UnityEngine;
using System.Collections;

public class Cinematic : MonoBehaviour {

	private Camera _camera;
	private Animator _animCamera;
	public GameObject wraith;
	public GameObject portal;
	public GameObject plane;

	private AudioSource _audioSource;
	public AudioClip[] playerVoice;
	public AudioClip[] reaperVoice;

	private int lookAround = Animator.StringToHash("LookAround");
	private int turnAround = Animator.StringToHash("TurnAround");
	private int LookEnd = Animator.StringToHash("LookEnd");

	public void Start () {
		//LoadingScreen.Instance.fadeToClear ();
		_camera = Camera.main;
		_animCamera = _camera.GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();

		StartCoroutine(AnimateCamera());
		StartCoroutine (AnimateScene ());
		StartCoroutine(TestDialog());
	}
	private IEnumerator AnimateScene(){
		yield return new WaitForSeconds(27);
		portal.SetActive (true);
		yield return new WaitForSeconds(3);
		wraith.SetActive (true);
		yield return new WaitForSeconds(13);
		_animCamera.SetTrigger(LookEnd);
		plane.SetActive (true);


	}

	private IEnumerator AnimateCamera(){
		yield return new WaitForSeconds(2.0f);
		_animCamera.SetTrigger(lookAround);
		_animCamera.SetTrigger(turnAround);
	}

	private IEnumerator TestDialog(){
		yield return new WaitForSeconds(4f);
		_audioSource.clip = playerVoice[0];
		_audioSource.Play();
		yield return new WaitForSeconds(6f);
		
		_audioSource.clip = reaperVoice[0];
		_audioSource.Play();
		yield return new WaitForSeconds(7f);
		
		_audioSource.clip = playerVoice[1];
		_audioSource.Play();
		yield return new WaitForSeconds(9.5f);
		
		_audioSource.clip = reaperVoice[1];
		_audioSource.Play();
	}
}
