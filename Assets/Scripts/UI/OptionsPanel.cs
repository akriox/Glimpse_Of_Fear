using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsPanel : MonoBehaviour {

	public Slider sliderResolution;
	public Slider sliderQuality;
	public Text labelResolution;
	public Text labelQuality;
	public Toggle fullscreen;

	private int qualityLevel;
	private Resolution resolution;

	public void Start(){
		if(sliderResolution != null) sliderResolution.maxValue = Screen.resolutions.Length-1;
		UpdateQuality();
		UpdateResolution();
	}

	public void ApplyResolution(){
		if(!Screen.currentResolution.Equals(resolution) || Screen.fullScreen != fullscreen.isOn){
			Screen.SetResolution(resolution.width, resolution.height, fullscreen.isOn);
		}
	}
	
	public void ApplyQuality(){
		if(QualitySettings.GetQualityLevel() != qualityLevel){
			QualitySettings.SetQualityLevel(qualityLevel, true);
		}
	}
	
	public void UpdateResolution(){
		resolution = Screen.resolutions[(int)sliderResolution.value];
		labelResolution.text = resolution.width + "x" + resolution.height;
	}
	
	public void UpdateQuality(){
		qualityLevel = (int)sliderQuality.value;
		labelQuality.text = QualitySettings.names[qualityLevel];
	}
}