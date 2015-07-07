using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class GameController : MonoBehaviour {

	public static GameController Instance {get; private set;}
	public GameObject inGameMenu;

	private GameObject widget;

	public void Awake(){
		Instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	public void Start(){
		Cursor.visible = false;
		widget = GameObject.FindGameObjectWithTag("Widget");
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

	public void setWidgetSprite(Sprite s){
		widget.GetComponent<Image>().sprite = s;
	}

	public void displayWidget(bool b){
		widget.GetComponent<Image>().enabled = b;
	}
}