using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[RequireComponent (typeof (GazeAwareComponent))]
public class Spirit : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private AudioSource _audioSource;

	private bool activ;

	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_audioSource = GetComponent<AudioSource>();
	}

	public void Update(){
		if(_gazeAwareComponent.HasGaze){
			if(!_audioSource.isPlaying)_audioSource.Play();
			CameraController.Instance.setVortexState(CameraController.VortexState.INC);
			CameraController.Instance.setNoiseAndScratches(true);
			GameController.Instance.startVibration(0.8f, 0.8f);
			if(activ == false){
				StartCoroutine(CameraController.Instance.Shake(2.0f, 0.05f, 10.0f));
				activ = true;
			}
		}
		else{
			activ = false;
			CameraController.Instance.setVortexState(CameraController.VortexState.DEC);
			CameraController.Instance.setNoiseAndScratches(false);
			GameController.Instance.stopVibration();
		}
	}
}