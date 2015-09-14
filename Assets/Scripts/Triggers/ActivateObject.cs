using UnityEngine;
using System.Collections;

public class ActivateObject : MonoBehaviour {
	
	public GameObject[] target;

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			foreach(GameObject go in target){
				go.SetActive(true);
			}
			Destroy(this.gameObject);
		}
	}
}