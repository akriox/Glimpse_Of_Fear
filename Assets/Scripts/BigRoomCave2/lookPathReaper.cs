using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GazeAwareComponent))]
public class lookPathReaper : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	[SerializeField] private Transform positionStart;
	[SerializeField] private Transform positionEnd;
	[SerializeField] private GameObject triggerToDesactive;
	[SerializeField] private GameObject[] pathToDesactive;
	[SerializeField] private AudioClip _audioClip;
	private float timeSetActiv = 0.5F;
	private float timeUntilDesactivItSelf; 

	private bool alreadyCall = false;


	public void Start(){
		//get component for eye tracker
		_gazeAwareComponent = GetComponent<GazeAwareComponent> ();
	}
	void Update () {
		if (_gazeAwareComponent.HasGaze && !alreadyCall) {
			if(_audioClip != null)
				EventSound.playClip(_audioClip);

			moveReaperBigRoom.Instance.setPosition (positionStart.position, positionEnd.position);
			timeUntilDesactivItSelf = Time.time + timeSetActiv;
			if(triggerToDesactive != null)
				triggerToDesactive.SetActive(false);
			if (pathToDesactive != null) {
				foreach (GameObject path in pathToDesactive) {
					path.SetActive (false);
				}
			}
			alreadyCall = true;
		}
		if (alreadyCall && Time.time >timeUntilDesactivItSelf) {
			HeartBeat.playLoop();
			this.enabled = false;
		}
	}
}
