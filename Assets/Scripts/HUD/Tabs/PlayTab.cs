using UnityEngine;
using System.Collections;

public class PlayTab : MonoBehaviour {

	public GameObject loadingScreenGO;

	public void Play(){
		if(loadingScreenGO != null) loadingScreenGO.SetActive(true);
		StartCoroutine(LoadingScreen.Instance.loadScene(1));
	}

	public void RestartLevel(){

		// Avoid the presence of 2 players in the scene
		int currentLevel = Application.loadedLevel;
		if(currentLevel == 1){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Destroy(player);
		}

		StartCoroutine(LoadingScreen.Instance.loadScene(currentLevel));
		GameController.Instance.displayInGameMenu(false);
	}

	public void Resume(){
		GameController.Instance.displayInGameMenu(false);
	}

	public void Load(){

	}
}