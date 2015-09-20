using UnityEngine;
using System.Collections;

public class TriggerEventSound : MonoBehaviour {

	public AudioClip audioClip;
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			EventSound.playClip(audioClip);
		}
	}
}
