using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class GameController : MonoBehaviour {

	public static GameController Instance {get; private set;}
	public GameObject inGameMenu;

	public GameObject keyboardWidget;
	public GameObject gamepadWidget;
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
			inGameMenu.SetActive(!inGameMenu.activeSelf);
			EyeLook.isActive = !EyeLook.isActive;
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
		EventSound.playClip(rockSlideClip);
		yield return new WaitForSeconds(2.0f);
		CameraController.Instance.setFadeState(CameraController.FadeState.IN, 0.8f);
		yield return new WaitForSeconds(2.0f);
		VoiceOver.Talk(whereAmIClip);
		if(Settings.subtitles) displayDebug("Where am I ? I need to find a way out.");
	}
}