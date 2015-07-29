using UnityEngine;
using System.Collections;

public class spiritTrigger : MonoBehaviour {

	[SerializeField][Range(0, 1)]public int _case = 1;
	public GameObject Spirit;
	
	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			switch (_case) {
			case 0:
				if (Spirit != null)
					Spirit.SetActive (true);
				break;
			case 1:
				if (Spirit != null)
					Spirit.SetActive (false);
				break;
			}
		}
	}
}
