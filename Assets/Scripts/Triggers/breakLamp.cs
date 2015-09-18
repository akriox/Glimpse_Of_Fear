using UnityEngine;
using System.Collections;

public class breakLamp : MonoBehaviour {

	private AudioSource audioSource;
	public GameObject lamp;
	
	public void Start(){
		audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
	}
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			audioSource.Play();
			lamp.SetActive(false);
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
}
