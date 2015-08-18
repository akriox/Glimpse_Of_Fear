using UnityEngine;
using System.Collections;

public class TriggerMusic : MonoBehaviour {
	public GameObject son;
	[SerializeField][Range(0, 1)]public int _case;

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if (son != null) son.SetActive(true);
			switch (_case) {
			case 0:
				if (son != null) son.SetActive(true);
				break;
			case 1:
				if (son != null) son.SetActive(false);
				break;
			}
		}
	}
}
