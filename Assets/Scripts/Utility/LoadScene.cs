using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScene : MonoBehaviour {
	
	public string sceneToLoad;
	public GameObject menuCanvas;
	public GameObject loadingCanvas;
	public Text display;
	private int progress = 0;

	public void Start () {
		menuCanvas.SetActive(true);
		loadingCanvas.SetActive(false);
	}
	
	public void loadScene(){
		StartCoroutine(DisplayLoadingScreen());
	}

	private IEnumerator DisplayLoadingScreen(){

		menuCanvas.SetActive(false);
		loadingCanvas.SetActive(true);

		display.text = "LOADING " + progress + "%";

		AsyncOperation async = sceneToLoad != "" ? Application.LoadLevelAsync(sceneToLoad) : Application.LoadLevelAsync(Application.loadedLevel);

		while(!async.isDone){
			progress = (int)(async.progress * 100);
			display.text = "LOADING " + progress + "%";
			yield return null;
		}
	}
}