using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UserPresenceComponent))]
public class TeleportScene : MonoBehaviour {

    public int sceneIndex;
    private bool _playerOpenEyes;
    private UserPresenceComponent _userPresenceComponent;
	private AudioClip audioClip;
	private bool alreadyCall;

    public void Start(){
        _userPresenceComponent = GetComponent<UserPresenceComponent>();
		audioClip = (AudioClip) Resources.Load("Audio/Teleport", typeof(AudioClip));
		alreadyCall = false;
    }

	public void Update(){
		if(Input.GetButtonUp("BackGamepad")) LoadingScreen.Instance.fadeToBlack(sceneIndex);
	}

    public void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Player"){
			if(_userPresenceComponent.GazeTracking == EyeXGazeTracking.GazeNotTracked && !alreadyCall){
				EventSound.playClip(audioClip);
				alreadyCall = true;
				if(sceneIndex != 0)
            		LoadingScreen.Instance.fadeToBlack(sceneIndex);
				else{
					Application.LoadLevel (0);
				}
			}
        }
    }
}
