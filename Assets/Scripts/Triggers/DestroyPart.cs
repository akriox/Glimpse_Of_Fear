using UnityEngine;
using System.Collections;

public class DestroyPart : MonoBehaviour {

	[SerializeField]private GameObject _partToDestroy;

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Destroy(_partToDestroy);
		}
	}
	public void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			Destroy(this.gameObject);
		}
	}
}
