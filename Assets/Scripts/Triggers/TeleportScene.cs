using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UserPresenceComponent))]
public class TeleportScene : MonoBehaviour {

    public int sceneIndex;
    private bool _playerOpenEyes;
    private UserPresenceComponent _userPresenceComponent;

    public void Start()
    {
        _userPresenceComponent = GetComponent<UserPresenceComponent>();
    }

    public void Update()
    {
        _playerOpenEyes = _userPresenceComponent.GazeTracking == EyeXGazeTracking.GazeTracked ? true : false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(!_playerOpenEyes)
                LoadingScreen.Instance.fadeToBlack(sceneIndex, FirstPersonController.haveFlashLight());
        }
    }
}
