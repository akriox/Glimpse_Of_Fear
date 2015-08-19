using UnityEngine;
using System.Collections;

public class TriggerMusic : MonoBehaviour {
	public GameObject son;
	public bool playMusic;

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if(playMusic)
				if (son != null)son.SetActive(true);
			else{
				if (son != null) son.SetActive(false);
			}
		}
	}
}
