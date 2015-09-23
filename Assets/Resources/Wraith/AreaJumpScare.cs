using UnityEngine;
using System.Collections;

public class AreaJumpScare : MonoBehaviour {

	public static bool isPlayerInAreaForJumpScare = false;
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			isPlayerInAreaForJumpScare = true;
		}
	}
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			isPlayerInAreaForJumpScare = false;
		}
	}
}
