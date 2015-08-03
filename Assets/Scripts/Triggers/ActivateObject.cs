using UnityEngine;
using System.Collections;

public class ActivateObject : MonoBehaviour {
	
	public GameObject target;

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			target.SetActive(true);
			Destroy(this.gameObject);
		}
	}
}