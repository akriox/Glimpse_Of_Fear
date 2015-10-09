using UnityEngine;
using System.Collections;

public class PlayTab : MonoBehaviour {

	public GameObject loadingScreenGO;

	public void Play(int levelIndex){
		MainMenu.playClip(MainMenu.submit);
		if(loadingScreenGO != null) loadingScreenGO.SetActive(true);
		StartCoroutine(LoadingScreen.Instance.loadScene(levelIndex));
	}

	public void RestartLevel(){
		MainMenu.playClip(MainMenu.submit);

		// Avoid the presence of 2 players in the scene cave1 or in the demo
		int currentLevel = Application.loadedLevel;
		if(currentLevel == 1 || currentLevel == 5){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Destroy(player);
		}

		StartCoroutine(LoadingScreen.Instance.loadScene(currentLevel));
		GameController.Instance.displayInGameMenu(false);
	}

	public void Resume(){
		MainMenu.playClip(MainMenu.submit);
		GameController.Instance.displayInGameMenu(false);
	}
}