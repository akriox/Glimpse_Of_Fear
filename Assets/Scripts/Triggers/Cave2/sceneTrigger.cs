using UnityEngine;
using System.Collections;

public class sceneTrigger : MonoBehaviour {
	
	[SerializeField] private enum scene {ToActive,ToDesactive}
	[SerializeField] private scene _scene = scene.ToActive;
	[SerializeField] private GameObject zone;
	[SerializeField] private GameObject thisTrigger;
	[SerializeField] private GameObject otherTrigger;
	
	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if(_scene == scene.ToActive)zone.SetActive (true);
			else{
				zone.SetActive (false);
			}
			thisTrigger.SetActive(false);
			if(otherTrigger != null)otherTrigger.SetActive(true);
		}
	}
}
