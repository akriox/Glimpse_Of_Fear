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

	private static AudioSource audioSource;
	public static AudioClip openTab;
	public static AudioClip submit;

	public void Start(){
		currentTab = playTab;
		audioSource = GetComponent<AudioSource>();
		openTab = (AudioClip) Resources.Load("Audio/Menu/open_tab", typeof(AudioClip));
		submit = (AudioClip) Resources.Load("Audio/Menu/submit", typeof(AudioClip));
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
		GameController.Instance.displayInGameMenu(false);
		Cursor.visible = true;
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
			playClip(openTab);
		}
	}

	public static void playClip(AudioClip clip){
		if(audioSource != null){
			audioSource.clip = clip;
			audioSource.Play();
		}
	}
}