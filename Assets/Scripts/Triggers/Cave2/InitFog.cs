using UnityEngine;
using System.Collections;

public class InitFog : MonoBehaviour {

	public GameObject activFog;

	private static bool jumpScare;

	public void start(){
		jumpScare = false;
	}
	
	public IEnumerator OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Player") {
			activFog.SetActive (true);
			if (!jumpScare){
				Spirit.setNewPosition();
			}
			yield return new WaitForSeconds(1f);
			this.enabled = false;
		}
	}

	public static void specterSeen(){
		jumpScare = true;
	}
}
