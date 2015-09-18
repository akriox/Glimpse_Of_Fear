using UnityEngine;
using System.Collections;

public class TriggerGhost : MonoBehaviour {
	
	private bool alreadyCall= false;
	[SerializeField] private GameObject _activ;
	

	public IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && !alreadyCall) {
			alreadyCall = true;
			yield return new WaitForSeconds(7f);
			_activ.SetActive(true);
			Destroy(this.gameObject);
		}
	}
}
