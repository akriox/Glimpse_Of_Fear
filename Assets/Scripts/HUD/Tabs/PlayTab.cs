﻿using UnityEngine;
using System.Collections;

public class PlayTab : MonoBehaviour {

	public GameObject loadingScreenGO;

	public void Play(){
		loadingScreenGO.SetActive(true);
		StartCoroutine(LoadingScreen.Instance.loadScene(1));
	}

	public void Load(){

	}
}