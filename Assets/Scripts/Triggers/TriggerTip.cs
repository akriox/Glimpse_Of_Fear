using UnityEngine;
using System.Collections;

public class TriggerTip : MonoBehaviour {

	public string tip;

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			switch(tip){
				case "Sprint": TipsTracker.Instance.displayTip(TipsTracker.Tips.Sprint); break;
				case "Crouch": TipsTracker.Instance.displayTip(TipsTracker.Tips.Crouch); break;
				case "LightOff": TipsTracker.Instance.displayTip(TipsTracker.Tips.SwitchOffFlashlight); break;
			}
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
}
