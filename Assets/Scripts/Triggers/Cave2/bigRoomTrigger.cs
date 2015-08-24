using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class bigRoomTrigger: MonoBehaviour {
		
		[SerializeField]private GameObject[] Path;

		public void Start(){
			if(Path == null){
				Debug.LogError("Un GameObject 'Path' doit etre renseigné dans le script 'bigRoomTrigger.cs'.");
			} 
			foreach (GameObject path in Path) {
				path.SetActive (false);
			}
		}
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				foreach(GameObject path in Path){
					path.SetActive(true);
				}
			}
		}
		public IEnumerator OnTriggerExit(Collider other) {
			if (other.gameObject.tag == "Player") {
				foreach(GameObject path in Path){
					path.SetActive(false);
				}
				yield return new WaitForSeconds(1f);
				CameraController.Instance.resetShake();
				GameController.Instance.stopVibration();
			}
		}
	}
}
