using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainPanel: MonoBehaviour {

	public GameObject optionsPanel;
	public GameObject loadingScreen;
	public EventSystem eventSystem;
	
	public void Play(){
		loadingScreen.SetActive(true);
		StartCoroutine(LoadingScreen.Instance.loadScene(1));
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
}
