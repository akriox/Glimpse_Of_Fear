using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public int sceneIndex;

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			LoadingScreen.Instance.fadeToBlack(sceneIndex);
		}
	}
}