using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class GameController : MonoBehaviour {

	public static GameController Instance {get; private set;}
	public GameObject inGameMenu;

	public Image keyboardWidget;
	public Image gamepadWidget;

	public Text debugText;

	private GameObject player;

	private AudioClip rockSlideClip;
	private AudioClip whereAmIClip;

	public void Awake(){
		Instance = this;
		rockSlideClip = (AudioClip) Resources.Load("Audio/Cave1/medium_landslide01", typeof(AudioClip));
		whereAmIClip = (AudioClip) Resources.Load("Audio/VoiceOver/Cave1/whereAmI", typeof(AudioClip));
	}

	public void Start(){
		Cursor.visible = false;
		player = GameObject.FindGameObjectWithTag("Player");
		DontDestroyOnLoad(player);
	}

	public void Update () {
		if(Input.GetButtonUp("Menu")){
			displayInGameMenu(!inGameMenu.activeSelf);
		}
	}

	public void ExitGame(){
		displayInGameMenu(false);
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
		GamePad.SetVibration (0, left, right);
		yield return new WaitForSeconds (s);
		GamePad.SetVibration (0, 0.0f, 0.0f);
	}

	public void displayInGameMenu(bool b){
		inGameMenu.SetActive(b);
		EyeLook.isActive = !b;
		FirstPersonController.ableToMove = !b;
		Cursor.visible = b;
	}

	public void displayWidget(bool b){
		GamePadState state = GamePad.GetState(PlayerIndex.One);
		if(state.IsConnected) gamepadWidget.enabled = b;
		else keyboardWidget.enabled = b;
	}

	public void displayDebug(string str){
		debugText.text = str;
	}

	public IEnumerator displayTimedDebug(string str, float timeout){
		debugText.text = str;
		yield return new WaitForSeconds(timeout);
		debugText.text = "";
	}

	public IEnumerator OpeningScene(){
		CameraController.Instance.fade.enabled = true;
		FirstPersonController.ableToMove = false;
		EventSound.playClip(rockSlideClip);
		yield return new WaitForSeconds(2.0f);
		CameraController.Instance.setFadeState(CameraController.FadeState.IN, 0.8f);
		yield return new WaitForSeconds(2.5f);
		VoiceOver.Talk(whereAmIClip);
		if(Settings.subtitles) displayDebug("Where am I ? I need to find a way out.");
		FirstPersonController.ableToMove = true;
	}
}