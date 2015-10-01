using UnityEngine;
using System.Collections;

public class RotateFaster : MonoBehaviour {

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			TipsTracker.Instance.displayTip (TipsTracker.Tips.Rotate);
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
}
