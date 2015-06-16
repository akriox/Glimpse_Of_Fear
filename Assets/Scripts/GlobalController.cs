using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour {

	public GameObject inGameMenu;

	public void Start(){
		Cursor.visible = false;
	}
	
	public void Update () {
	
		if(Input.GetButtonDown("Menu")){
			if(inGameMenu!= null) inGameMenu.SetActive(!inGameMenu.activeSelf);
			Cursor.visible = !Cursor.visible;
		}
	}

	public void ExitGame(){
		Application.LoadLevel("Menu");
	}

	public void CloseApplication(){
		Application.Quit();
	}
}