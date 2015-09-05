﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputTab : MonoBehaviour {

	public Slider sliderEyeX;
	public Slider sliderMousePad;
	public Toggle toggleInvertedAxis;
	public Toggle toggleSubtitles;

	private float defaultValue = 3;
	
	public void Start(){
		sliderEyeX.value = PlayerPrefs.HasKey("EyeXSensitivity") ? PlayerPrefs.GetFloat("EyeXSensitivity") : defaultValue;
		UpdateEyeXSensitivity();

		sliderMousePad.value = PlayerPrefs.HasKey("MousePadXYSensitivity") ? PlayerPrefs.GetFloat("MousePadXYSensitivity") : defaultValue;
		UpdateMousePadXYSensitivity();

		toggleInvertedAxis.isOn = PlayerPrefs.HasKey("InvertedAxis") && PlayerPrefs.GetInt("InvertedAxis") == 1 ? true : false;
		UpdateInvertedAxis();

		toggleSubtitles.isOn = PlayerPrefs.HasKey("Subtitles") && PlayerPrefs.GetInt("Subtitles") == 1 ? true : false;
		UpdateSubtitles();
	}

	public void UpdateEyeXSensitivity(){
		PlayerPrefs.SetFloat("EyeXSensitivity", sliderEyeX.value);
		Settings.EyeXSensitivity = sliderEyeX.value;
	}
	
	public void UpdateMousePadXYSensitivity(){
		PlayerPrefs.SetFloat("MousePadXYSensitivity", sliderMousePad.value);
		Settings.MousePadXYSensitivity = sliderMousePad.value;
	}

	public void UpdateInvertedAxis(){
		int value = toggleInvertedAxis.isOn ? 1 : 0;
		PlayerPrefs.SetInt("InvertedAxis", value);
		Settings.invertedAxis = toggleInvertedAxis.isOn; 
	}

	public void UpdateSubtitles(){
		int value = toggleSubtitles.isOn ? 1 : 0;
		PlayerPrefs.SetInt("Subtitles", value);
		Settings.subtitles = toggleSubtitles.isOn;
	}
}
