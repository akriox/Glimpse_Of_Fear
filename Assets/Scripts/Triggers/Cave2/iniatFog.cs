using UnityEngine;
using System.Collections;

public class iniatFog : MonoBehaviour {

	public GameObject activFog;
	public GameObject otherSpecter;
	public GameObject desactivRealSpecter;
	public GameObject trigger;

	private static bool jumpScare;

	public void start(){
		jumpScare = false;
	}
	
	public void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Player") {
			activFog.SetActive (true);
			if (!jumpScare){
				otherSpecter.SetActive(true);
				desactivRealSpecter.SetActive(false);
			}
			DestroyObject(trigger);
		}
	}

	public static void specterSeen(){
		jumpScare = true;
	}
}
