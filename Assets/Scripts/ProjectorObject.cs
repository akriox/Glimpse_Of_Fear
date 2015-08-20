using UnityEngine;
using System.Collections;

public class ProjectorObject : MonoBehaviour {

	private Light _light;
	private float _initIntensity;
	private float _flickerFrequency = 0.1f;
	
	public void Start(){
		_light = GetComponentInChildren<Light>();
		_initIntensity = _light.intensity;
	}

	public IEnumerator Flicker(int times){
		int count = 0;
		while (count < times) {
			count += 1;          
			_light.intensity -= Random.value * _initIntensity;
			yield return new WaitForSeconds(_flickerFrequency);
			_light.intensity = _initIntensity;
		}
		_light.intensity = _initIntensity;
	}
}
