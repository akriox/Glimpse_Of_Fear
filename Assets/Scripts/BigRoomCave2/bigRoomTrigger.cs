using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class bigRoomTrigger: MonoBehaviour {
		
		[SerializeField]private GameObject[] Path;

		public void Start(){
			if (Path != null) {
				foreach (GameObject path in Path) {
					path.SetActive (false);
				}
			}
		}
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				if (Path != null) {
					foreach(GameObject path in Path)
					{
						path.SetActive(true);
					}
				}
			}
		}
		public void OnTriggerExit(Collider other) {
			if (other.gameObject.tag == "Player") {
				if (Path != null) {
					foreach(GameObject path in Path)
					{
						path.SetActive(false);
					}
				}
			}
		}
	}
}
