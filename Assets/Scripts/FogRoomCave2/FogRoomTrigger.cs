using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class FogRoomTrigger: MonoBehaviour {
		
		
		[SerializeField][Range(0, 3)]public int _case = 1;
		public GameObject desactivAnObject;
		public GameObject activAnObject;

		public void OnTriggerEnter(Collider other) {

			if (other.gameObject.tag == "Player") {
				if (activAnObject != null)
					activAnObject.SetActive(true);
				switch (_case) {
				case 0:
					canDesactiveAnObject();
					if(!FollowPath.Instance.isReady()){
						FollowPath.Instance.setReady();
						FollowPath.Instance.resetBeginningRoom ();
					}
					break;
				case 1:
					resetImageEffect();
					FollowPath.Instance.setFinish();
					canDesactiveAnObject();
					break;
				case 2:
					if(!FollowPath.Instance.isReady()){
						FollowPath.Instance.setReady();
						FollowPath.Instance.resetEndRoom ();
					}
					break;
				case 3:
						canDesactiveAnObject();
					break;
				}
			}
		}

		public void resetImageEffect(){
			CameraController.Instance.setVortexState (CameraController.VortexState.DEC);
			CameraController.Instance.setNoiseAndScratches(CameraController.NoiseAndScratchesState.DEC);
		}

		public void canDesactiveAnObject(){
			if (desactivAnObject != null && desactivAnObject.activeSelf) {
				desactivAnObject.SetActive (false);
			}
		}
	}
}
