using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenu: MonoBehaviour {

	public GameObject playTab;
	public GameObject graphicsTab;
	public GameObject inputTab;
	public GameObject creditsTab;

	private GameObject currentTab;

	public void Start(){
		currentTab = playTab;
	}

	public void DisplayPlayTab(){
		SwitchTab(playTab);
	}

	public void DisplayGraphicsTab(){
		SwitchTab(graphicsTab);
	}

	public void DisplayInputTab(){
		SwitchTab(inputTab);
	}

	public void DisplayCreditsTab(){
		SwitchTab(creditsTab);
	}

	public void ExitGame(){
		if(Cursor.visible == false) Cursor.visible = true;
		Application.LoadLevel(0);
	}

	public void ExitToDesktop(){
		Application.Quit();
	}

	private void SwitchTab(GameObject tab){
		if(tab.name != currentTab.name){
			currentTab.SetActive(false);
			currentTab = tab;
			currentTab.SetActive(true);
		}
	}
}