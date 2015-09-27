using UnityEngine;
using System.Collections;

public class InitFog : MonoBehaviour {

	public GameObject activFog;
	
	public IEnumerator OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Player") {
            if(activFog != null)
                activFog.SetActive (true);
			Spirit.setNewPosition();
			yield return new WaitForSeconds(1f);
			this.enabled = false;
		}
	}
}
