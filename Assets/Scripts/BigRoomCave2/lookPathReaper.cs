using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GazeAwareComponent))]
public class lookPathReaper : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	[SerializeField] private Transform positionStart;
	[SerializeField] private Transform positionEnd;
	[SerializeField] private GameObject triggerToDesactive;
	[SerializeField] private GameObject[] pathToDesactive;
	[SerializeField] [Range(1.5F, 3.0F)] private float timeSetActiv = 2.0F;
	private float timeUntilDesactivItSelf; 

	private bool alreadyCall = false;


	public void Start(){
		//get component for eye tracker
		_gazeAwareComponent = GetComponent<GazeAwareComponent> ();
	}
	void Update () {
		if (_gazeAwareComponent.HasGaze && !alreadyCall) {
			moveReaperBigRoom.Instance.setPosition (positionStart.position, positionEnd.position);
			alreadyCall = true;
			timeUntilDesactivItSelf = Time.time + timeSetActiv;
			if(triggerToDesactive != null)
				triggerToDesactive.SetActive(false);
			if (pathToDesactive != null) {
				foreach (GameObject path in pathToDesactive) {
					path.SetActive (false);
				}
			}
		}
		if (alreadyCall && timeUntilDesactivItSelf > Time.time)
			this.enabled = false;
	}
}
