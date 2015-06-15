using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public void Start(){
		Cursor.visible = false;
	}
	
	public void Update () {
	
		if(Input.GetButtonDown("Menu")){
			//Cursor.visible = !Cursor.visible;
			Application.Quit();
		}
	}
}