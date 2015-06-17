using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public static LoadingScreen Instance { get; private set; }
	
	private Text display;
	private int progress = 0;

	public void Awake(){
		Instance = this;
		display = GetComponentInChildren<Text>();
	}

	public IEnumerator loadScene(int index){

		display.text = "LOADING " + progress + "%";
		
		AsyncOperation async = Application.LoadLevelAsync(index);
		
		while(!async.isDone){
			progress = (int)(async.progress * 100);
			display.text = "LOADING " + progress + "%";
			yield return null;
		}
	}
}