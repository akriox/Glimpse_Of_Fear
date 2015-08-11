using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class bigRoomTrigger: MonoBehaviour {
		
		[SerializeField]private GameObject[] Path;

		private AudioClip audioClip;

		public void Start(){
			audioClip = (AudioClip) Resources.Load("Audio/ghost_apparition01", typeof(AudioClip));

			if (Path != null) {
				foreach (GameObject path in Path) {
					path.SetActive (false);
				}
			}
		}
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				EventSound.playClip(audioClip);
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
