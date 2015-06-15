using UnityEngine;
using System.Collections;

public class LightBeacon : MonoBehaviour {

	private Light selfIllum;
	private float maxIntensity;
	private float frequency = 0.01f;
	private bool increase;

	public void Start () {
		selfIllum = GetComponent<Light>();
		maxIntensity = selfIllum.intensity;
	}

	public void Update () {
		if(increase){
			if(selfIllum.intensity < maxIntensity){
				selfIllum.intensity += frequency;
			}
			else{
				increase = false;
			}
		}
		else{
			if(selfIllum.intensity > 0){
				selfIllum.intensity -= frequency;
			}
			else{
				increase = true;
			}
		}
	}
}