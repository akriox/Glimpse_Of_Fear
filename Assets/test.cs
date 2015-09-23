using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			LoadingScreen.Instance.fadeBlack ();
		}
	}
}
