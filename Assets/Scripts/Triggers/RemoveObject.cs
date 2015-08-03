using UnityEngine;
using System.Collections;

public class RemoveObject : MonoBehaviour {
	
	public GameObject target;
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(target);
			Destroy(this.gameObject);
		}
	}
}