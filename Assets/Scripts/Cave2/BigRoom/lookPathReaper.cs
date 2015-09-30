using UnityEngine;
using System.Collections;


[RequireComponent(typeof(GazeAwareComponent))]
public class lookPathReaper : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	[SerializeField] private Transform positionStart;
	[SerializeField] private Transform positionEnd;
	[SerializeField] private GameObject _thisPath;
	[SerializeField] private GameObject _otherPath;
	[SerializeField] private AudioClip _audioClip;

	private bool alreadyCall = false;
	public string str;
	public AudioClip voice;
	public AudioClip voice2;
	
	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent> ();
		EventSound.volume(1f);
	}
	void Update () {
		if (_gazeAwareComponent.HasGaze && !alreadyCall) {
			if(_audioClip != null)
				EventSound.playClip(_audioClip);
			StartCoroutine(fear());
			alreadyCall = true;
			bigRoomTrigger.isActif();
			moveWraithScript.setPosition (positionStart.position, positionEnd.position);
			if (_otherPath != null) {
				_otherPath.SetActive (false);
			}
		}
	}

	IEnumerator fear(){
		//StartCoroutine (CameraController.Instance.Shake (1.0f, 0.5f, 2.0f));
		StartCoroutine (GameController.Instance.timedVibration (0.6f, 0.6f, 1.0f));
		yield return new WaitForSeconds(0.5f);
		HeartBeat.playLoop();
		if(voice != null) VoiceOver.Talk(voice);
		yield return new WaitForSeconds(1.5f);
		if(voice2 != null) VoiceOver.Talk(voice2);
		if(Settings.subtitles) GameController.Instance.displayDebug(str);
		yield return new WaitForSeconds(1.5f);
		if(Settings.subtitles) GameController.Instance.displayDebug("");
		bigRoomTrigger.isUnActif();
		EventSound.volume(0.5f);
		yield return new WaitForSeconds(0.5f);
		Destroy(_thisPath);
	}
}