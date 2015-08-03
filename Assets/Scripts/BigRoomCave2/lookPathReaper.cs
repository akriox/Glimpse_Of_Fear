using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GazeAwareComponent))]
public class lookPathReaper : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	[SerializeField] private Transform positionStart;
	[SerializeField] private Transform PositionEnd;
	[SerializeField]private GameObject triggerToDesactive;

	private bool alreadyCall = false;


	public void Start(){
		//get component for eye tracker
		_gazeAwareComponent = GetComponent<GazeAwareComponent> ();
	}
	void Update () {
		if (_gazeAwareComponent.HasGaze && !alreadyCall) {
			moveReaperBigRoom.Instance.setPosition (positionStart.position, PositionEnd.position);
			alreadyCall = true;
			if(triggerToDesactive != null)
				triggerToDesactive.SetActive(false);
		}
	}
}
