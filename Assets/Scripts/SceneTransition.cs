using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			LoadingScreen.Instance.fadeToBlack(2);
		}
	}
}