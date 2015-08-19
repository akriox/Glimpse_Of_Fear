using UnityEngine;
using System.Collections;

public class iniatFog : MonoBehaviour {

	public GameObject activAnObject;
	public GameObject desactivAnObject;
	
	public void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Player") {
			if (activAnObject != null)
				activAnObject.SetActive (true);
			if (desactivAnObject != null)
				desactivAnObject.SetActive(false);
		}
	}
}
