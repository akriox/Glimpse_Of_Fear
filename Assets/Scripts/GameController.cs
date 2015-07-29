using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class GameController : MonoBehaviour {

	public static GameController Instance {get; private set;}
	public GameObject inGameMenu;

	public GameObject keyboardWidget;
	public GameObject gamepadWidget;

	public void Awake(){
		Instance = this;
	}

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

	public void startVibration(float left, float right){
		GamePad.SetVibration(0, left, right);
	}

	public void stopVibration(){
		GamePad.SetVibration(0, 0.0f, 0.0f);
	}

	public IEnumerator timedVibration(float left, float right, float s){
		GamePad.SetVibration(0, left, right);
		yield return new WaitForSeconds(s);
		GamePad.SetVibration(0, 0.0f, 0.0f);
	}

	public void displayWidget(bool b){
		GamePadState state = GamePad.GetState(PlayerIndex.One);
		if(state.IsConnected) gamepadWidget.GetComponent<Image>().enabled = b;
		else keyboardWidget.GetComponent<Image>().enabled = b;
	}
}