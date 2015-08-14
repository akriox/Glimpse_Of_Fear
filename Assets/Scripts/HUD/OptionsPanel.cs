using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour {

	public GameObject graphicsTab;
	public GameObject inputsTab;

	public void DisplayGraphicsTab(){
		inputsTab.SetActive(false);
		graphicsTab.SetActive(true);
	}

	public void DisplayInputsTab(){
		graphicsTab.SetActive(false);
		inputsTab.SetActive(true);
	}
}