using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public static LoadingScreen Instance { get; private set; }

	public enum Fading{IN, OUT, OFF};
	public Fading fading { get; set; }

	public float fadeSpeed;
	private Image fade;
	private RawImage background;
	private Text display;
	private bool levelLoaded;
	private int sceneTransition = 0;

	public void Awake(){
		Instance = this;
		DontDestroyOnLoad(this.gameObject);
		background = GetComponentInChildren<RawImage>();
		display = GetComponentInChildren<Text>();
		fade = GetComponentInChildren<Image>();
		fading = Fading.OFF;
	}

	public void Update(){

		switch(fading){
			case Fading.IN:	
				fadeIn();
				break;
			case Fading.OUT:
				fadeOut(); 
				break;
			case Fading.OFF:
				if(levelLoaded){
					DisplayLoadingScreen(false);
					levelLoaded = false;
				}
				if(sceneTransition != 0){
					DisplayLoadingScreen(true);
					StartCoroutine(loadScene(sceneTransition));
					sceneTransition = 0;
				}
				break;
		}

	}

	public IEnumerator loadScene(int index){

		fading = Fading.IN;
		display.text = "LOADING...";
		
		AsyncOperation async = Application.LoadLevelAsync(index);
		
		while(!async.isDone){
			yield return null;
		}
	}

	public void OnLevelWasLoaded(int level){
		fading = Fading.OUT;
		levelLoaded = true;

		//MainMenu
		if(level == 0){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			Destroy(player);
			Destroy(this.gameObject);
		}

		//Cave1
		if(level == 1){
			setPlayerTransform(new Vector3(38.0f, 2.0f, -55.0f), new Quaternion(0.0f, -1.0f, 0.0f, 1.0f));
		}
		//Cave2
		if(level == 2){
			FirstPersonController.enableRightHand(false);
			resetCameraTransform();
			setPlayerTransform(new Vector3(-36.0f, 4.0f, 46.0f), new Quaternion(0.0f, 1.0f, 0.0f, 1.0f));
			CameraController.Instance.GettingUpAnimation();
		}
		//Tomb1
		if(level == 3){
			FirstPersonController.enableRightHand(true);
			resetCameraTransform();
			setPlayerTransform(new Vector3(0.0f, 2.0f, -8.0f), new Quaternion(0.0f, 0.0f, 0.0f, 1.0f));
		}
	}

	private void fadeIn(){
		if(fade.color.a >= 0.05f){ 
			fade.color = Color.Lerp(fade.color, Color.clear, fadeSpeed * Time.deltaTime);
		}
		else{
			fade.color = Color.clear;
			fading = Fading.OFF;
		}
	}

	private void fadeOut(){
		if(fade.color.a <= 0.95f){ 
			fade.color = Color.Lerp(fade.color, Color.black, fadeSpeed * Time.deltaTime);
		}
		else{
			fade.color = Color.black;
			fading = Fading.OFF;
		}
	}
	
	public void DisplayLoadingScreen(bool b){
		background.gameObject.SetActive(b);
		display.gameObject.SetActive(b);
		fade.gameObject.SetActive(b);
	}

	public void fadeToBlack(int sceneIndex){
		sceneTransition = sceneIndex;
		fade.color = Color.clear;
		fading = Fading.OUT;
		fade.gameObject.SetActive(true);
	}

	public void fadeToClear(){
		fade.color = Color.black;
		fading = Fading.IN;
		fade.gameObject.SetActive(true);
	}

	private void setPlayerTransform(Vector3 position, Quaternion rotation){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = position;
		player.transform.rotation = rotation;
	}

	private void resetCameraTransform(){
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localRotation = Quaternion.identity;
	}
}