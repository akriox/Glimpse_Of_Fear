using UnityEngine;
using System.Collections;

public class fogTrigger : MonoBehaviour {

	public void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Player") {
			changeFog.Instance.decrementFogEndDistance ();
		}
	}
}
