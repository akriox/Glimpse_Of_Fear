using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputTab : MonoBehaviour {

	public Slider sliderEyeX;
	public Slider sliderMousePad;

	private float defaultValue = 3;
	
	public void Start(){
		sliderEyeX.value = PlayerPrefs.HasKey("EyeXSensitivity") ? PlayerPrefs.GetFloat("EyeXSensitivity") : defaultValue;
		UpdateEyeXSensitivity();

		sliderMousePad.value = PlayerPrefs.HasKey("MousePadXYSensitivity") ? PlayerPrefs.GetFloat("MousePadXYSensitivity") : defaultValue;
		UpdateMousePadXYSensitivity();
	}

	public void UpdateEyeXSensitivity(){
		PlayerPrefs.SetFloat("EyeXSensitivity", sliderEyeX.value);
		Settings.EyeXSensitivity = sliderEyeX.value;
	}
	
	public void UpdateMousePadXYSensitivity(){
		PlayerPrefs.SetFloat("MousePadXYSensitivity", sliderMousePad.value);
		Settings.MousePadXYSensitivity = sliderMousePad.value;
	}
}
