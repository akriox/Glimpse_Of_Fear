using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UserPresenceComponent))]
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

	public AudioClip voiceRecharge;
	private bool rechargeHint = false;

	public AudioClip[] buttonSound;
	private AudioSource audioSource;

	private Color defaultColor;
	public GameObject batteryLevel;

	public void Awake(){
		Instance = this;
	}

	public void Start() { 
		_userPresenceComponent = GetComponent<UserPresenceComponent>();

		lum = GetComponentInChildren<Light>();
		audioSource = GetComponent<AudioSource>();

		defaultColor = batteryLevel.GetComponent<Renderer>().material.color;

		_state = State.ON;
		_maxIntensity = lum.intensity;
		_power = _maxIntensity / _lifespan;
	}

	public void Update() {

		bool b = _userPresenceComponent.GazeTracking == EyeXGazeTracking.GazeTracked ? true : false;
		lum.gameObject.SetActive(b);
		
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
			case State.ON:	discharge(); 
							updateBatteryLevel();
							break;

			case State.OFF: break;
		}
	}	
	
	private void discharge(){
		discharge (1f,1);
	}


	private void discharge(float _float, int thePower){
		/* Decrease light intensity of _power per second */
		_timer += Time.deltaTime;
		if(lum.intensity > 0.0f){ 
				if(_timer >= _float){
				lum.intensity -= thePower*_power;
				_timer = 0.0f;
			}
		}
		/* No batteries left */
		else{
			lum.gameObject.SetActive(false);
		}
	}

	public void charge(){
		lum.intensity = _maxIntensity;
		batteryLevel.GetComponent<Renderer>().material.color = defaultColor;
		rechargeHint = false;
	}

	public void noMoreBattery(){
		discharge (0.001f,2);
	}

	private void updateBatteryLevel(){
		Color c  = defaultColor;
		if(lum.intensity < _maxIntensity/2.0f){
			c.r = 1.0f;
		}
		if(lum.intensity < _maxIntensity/4.0f){
			if(!VoiceOver._audioSource.isPlaying && rechargeHint == false) {
				VoiceOver.Talk(voiceRecharge);
				rechargeHint = true;
			}
			c.g = 0.0f;
		}
		batteryLevel.GetComponent<Renderer>().material.color = c;
	}
}