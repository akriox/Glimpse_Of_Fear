using UnityEngine;
using System.Collections;

public class PlayTab : MonoBehaviour {

	public GameObject loadingScreenGO;

	public void Play(){
		if(loadingScreenGO != null) loadingScreenGO.SetActive(true);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(player != null) Destroy(player);
		StartCoroutine(LoadingScreen.Instance.loadScene(1));
	}

	public void Load(){

	}
}