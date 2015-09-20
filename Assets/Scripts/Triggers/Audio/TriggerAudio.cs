using UnityEngine;
using System.Collections;

public class TriggerAudio : MonoBehaviour {

	private AudioSource audioSource;

	public void Start(){
		audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			audioSource.Play();
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
}
