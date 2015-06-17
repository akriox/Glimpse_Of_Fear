using UnityEngine;
using System.Collections;

public class fogTrigger : MonoBehaviour {

	public void OnTriggerStay(Collider other) {
		if (other.gameObject.name == "NewPlayer") {
			changeFog.Instance.decrementFogEndDistance ();
		}
	}
}
