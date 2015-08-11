using UnityEngine;
using System.Collections;

public class ScenarioTrigger : MonoBehaviour {
	
	public string str;
	public float s;
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			GameController.Instance.displayDebug(str);
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			GameController.Instance.displayDebug("");
			Destroy(this.gameObject);
		}
	}
}
