using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	private Light _light;
	private float _initIntensity;
	private float _flickerFrequency = 0.2f;
	private bool ready;
	
	public void Start(){
		_light = GetComponentInChildren<Light>();
		_initIntensity = _light.intensity;
		ready = true;
	}

	void Update(){
		if(ready)StartCoroutine(_Flash ());
	}

	public IEnumerator _Flash(){
		ready = false;
		_light.intensity -= Random.value * _initIntensity;
		yield return new WaitForSeconds(_flickerFrequency);
		_light.intensity = _initIntensity;
		ready = true;
	}
}
