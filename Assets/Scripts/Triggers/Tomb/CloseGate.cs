using UnityEngine;
using System.Collections;

public class CloseGate : MonoBehaviour {

	private GameObject gate;

	public void Start(){
		gate = this.gameObject.transform.parent.gameObject;
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Close ();
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}

	private void Close(){
		gate.GetComponent<AudioSource>().Play();
		gate.GetComponent<Animation>().Play();
	}
}
