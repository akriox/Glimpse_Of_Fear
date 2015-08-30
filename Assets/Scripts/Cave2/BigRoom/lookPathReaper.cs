using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GazeAwareComponent))]
public class lookPathReaper : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	[SerializeField] private Transform positionStart;
	[SerializeField] private Transform positionEnd;
	[SerializeField] private GameObject triggerToDesactive;
	[SerializeField] private GameObject pathToDesactive;
	[SerializeField] private AudioClip _audioClip;
	private float timeSetActiv = 2F;
	private float timeUntilDesactivItSelf; 

	private bool alreadyCall = false;


	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent> ();
	}
	void Update () {
		if (_gazeAwareComponent.HasGaze && !alreadyCall) {
			if(_audioClip != null)
				EventSound.playClip(_audioClip);
			HeartBeat.playLoop();
			StartCoroutine (CameraController.Instance.Shake (1.0f, 0.5f, 2.0f));
			StartCoroutine (GameController.Instance.timedVibration (0.6f, 0.6f, 1.0f));
			moveWraithCave2.setPosition (positionStart.position, positionEnd.position);
			timeUntilDesactivItSelf = Time.time + timeSetActiv;
			if(triggerToDesactive != null)
				triggerToDesactive.SetActive(false);
			if (pathToDesactive != null) {
				pathToDesactive.SetActive (false);
			}
			alreadyCall = true;
		}
		if (alreadyCall && Time.time >timeUntilDesactivItSelf) {
			this.enabled = false;
		}
	}
}
