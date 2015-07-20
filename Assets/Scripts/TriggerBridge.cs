using UnityEngine;
using System.Collections;

public class TriggerBridge : MonoBehaviour {

	public GameObject weakspot;
	private Rigidbody[] plank;

	public void Start(){
		plank = weakspot.GetComponentsInChildren<Rigidbody>();
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Collapse();
		}
	}

	private void Collapse(){
		int i;
		for(i=0; i<plank.Length; i++){
			plank[i].isKinematic = false;
		}
	}
}
