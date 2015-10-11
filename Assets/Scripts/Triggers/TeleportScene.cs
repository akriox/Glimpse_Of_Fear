using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UserPresenceComponent))]
public class TeleportScene : MonoBehaviour {

    public int sceneIndex;
    private bool _playerOpenEyes;
    private UserPresenceComponent _userPresenceComponent;
	private AudioClip audioClip;

    public void Start(){
        _userPresenceComponent = GetComponent<UserPresenceComponent>();
		audioClip = (AudioClip) Resources.Load("Audio/Teleport", typeof(AudioClip));
    }

    public void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Player"){
			if(_userPresenceComponent.GazeTracking == EyeXGazeTracking.GazeNotTracked){
				EventSound.playClip(audioClip);
            	LoadingScreen.Instance.fadeToBlack(sceneIndex);
			}
        }
    }
}
