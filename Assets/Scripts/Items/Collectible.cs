using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

	protected bool pickUp;

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			pickUp = true;
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			pickUp = false;
		}
	}
}
