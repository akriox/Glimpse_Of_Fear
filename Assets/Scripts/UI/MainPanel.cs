using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainPanel: MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject optionsPanel;
	public EventSystem eventSystem;

	public void Play(){
		StartCoroutine(LoadingScreen.Instance.loadScene(1));
		mainCanvas.SetActive(false);
	}
	
	public void OpenOptions(){
		optionsPanel.SetActive(!optionsPanel.activeSelf);
		eventSystem.SetSelectedGameObject(optionsPanel.GetComponentInChildren<Button>().gameObject);
	}

	public void CloseOptions(){
		optionsPanel.SetActive(!optionsPanel.activeSelf);
		eventSystem.SetSelectedGameObject(gameObject.GetComponentInChildren<Button>().gameObject);
	}
	
	public void ExitToDesktop(){
		Application.Quit();
	}

	public void Update(){
		if(Input.GetButtonDown("Cancel") && optionsPanel.activeSelf) CloseOptions();
	}
}
