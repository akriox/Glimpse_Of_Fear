using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class bigRoomTrigger: MonoBehaviour {

		[SerializeField]private GameObject Path;
		void Start(){
			Path.SetActive(false);
		}
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				Path.SetActive(true);
			}
		}
		public void OnTriggerExit(Collider other) {
			if (other.gameObject.tag == "Player") {
				Path.SetActive(false);
			}
		}
	}
}
