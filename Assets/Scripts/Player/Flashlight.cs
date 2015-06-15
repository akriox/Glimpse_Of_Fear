using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UserPresenceComponent))]
public class Flashlight : MonoBehaviour {
	
	public static Flashlight Instance {get; private set;}
	
	private UserPresenceComponent _userPresenceComponent;
	public Light lum {get; private set;}
	private enum State{ON, OFF};
	private State _state;
	private float _lifespan = 300.0f; // 5 minutes
	private float _maxIntensity;
	private float _power;
	private float _timer = 0.0f;

	public AudioClip[] buttonSound;
	private AudioSource audioSource;

	public void Awake(){
		Instance = this;
	}

	public void Start() {
		_userPresenceComponent = GetComponent<UserPresenceComponent>();
		lum = GetComponent<Light>();
		audioSource = GetComponent<AudioSource>();
		_state = State.OFF;
		_maxIntensity = lum.intensity;
		_power = _maxIntensity / _lifespan;
	}

	public void Update() {
		lum.enabled = _userPresenceComponent.IsUserPresent;
		if(lum.isActiveAndEnabled){
			if(_state == State.OFF){
				audioSource.clip = buttonSound[0];
				audioSource.PlayOneShot(audioSource.clip);
			}
			_state = State.ON;
		}
		else{
			if(_state == State.ON){ 
				audioSource.clip = buttonSound[1];
				audioSource.PlayOneShot(audioSource.clip);
			}
			_state = State.OFF;
		}


		switch(_state){
			case State.ON: discharge(); break;
			case State.OFF: break;
		}
	}	
	
	private void discharge(){
		/* Decrease light intensity of _power per second */
		_timer += Time.deltaTime;
		if(lum.intensity > 0 && _timer >= 1.0f){
			lum.intensity -= _power;
			_timer = 0.0f;
		}
	}
}