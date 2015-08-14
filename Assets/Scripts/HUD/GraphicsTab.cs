using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GraphicsTab : MonoBehaviour {
	
	public Slider sliderResolution;
	public Slider sliderQuality;
	public Text labelResolution;
	public Text labelQuality;
	public Toggle toggleFullscreen;
	
	private int qualityLevel;
	private Resolution resolution;
	
	public void Start(){
		sliderResolution.maxValue = Screen.resolutions.Length-1;
		sliderResolution.value = PlayerPrefs.HasKey("ResolutionValue") ? PlayerPrefs.GetFloat("ResolutionValue") : sliderResolution.maxValue;
		toggleFullscreen.isOn = Screen.fullScreen;
		UpdateResolution();
		
		sliderQuality.maxValue = QualitySettings.names.Length-1;
		sliderQuality.value = QualitySettings.GetQualityLevel();
		UpdateQuality();
	}
	
	public void ApplySettings(){
		if(!Screen.currentResolution.Equals(resolution) || Screen.fullScreen != toggleFullscreen.isOn){
			Screen.SetResolution(resolution.width, resolution.height, toggleFullscreen.isOn);
			PlayerPrefs.SetFloat("ResolutionValue", sliderResolution.value);
		}
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