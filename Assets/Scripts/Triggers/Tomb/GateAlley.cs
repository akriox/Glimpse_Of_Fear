using UnityEngine;
using System.Collections;

public class GateAlley : MonoBehaviour {

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject.transform.parent.gameObject);
		}
	}
}
