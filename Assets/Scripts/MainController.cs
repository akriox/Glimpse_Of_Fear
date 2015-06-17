using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	public GameObject inGameMenu;
	public GameObject loadingScreen;

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
		loadingScreen.SetActive(true);
		StartCoroutine(LoadingScreen.Instance.loadScene(Application.loadedLevel));
	}
}