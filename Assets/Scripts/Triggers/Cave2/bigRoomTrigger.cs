using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class bigRoomTrigger: MonoBehaviour {
		
		[SerializeField]private GameObject[] Path;
		[SerializeField] [Range(10.0F, 30.0F)]  private float wraithSpeed = 10.0f;

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
				moveWraithCave2.setSpeed(wraithSpeed);
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
