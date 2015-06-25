using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject inGameMenu;

	public void Start(){
		Cursor.visible = false;
	}

	public void Update () {
		if(Input.GetButtonDown("Menu") && inGameMenu != null){
			inGameMenu.SetActive(!inGameMenu.activeSelf);
			Cursor.visible = !Cursor.visible;
		}
	}

	public void ExitGame(){
		Application.LoadLevel("Menu");
	}
	
	public void Restart(){
		LoadingScreen.Instance.DisplayLoadingScreen(true);
		StartCoroutine(LoadingScreen.Instance.loadScene(Application.loadedLevel));
	}
}