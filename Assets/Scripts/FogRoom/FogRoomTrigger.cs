using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class FogRoomTrigger: MonoBehaviour {
		
		
		[SerializeField][Range(0, 2)]public int _case = 1;
		public GameObject Spirit;

		public void OnTriggerEnter(Collider other) {

			if (other.gameObject.tag == "Player") {
				if (Spirit != null)
					Spirit.SetActive(false);
				switch (_case) {
				case 0:
					if(!FollowPath.Instance.isReday()){
						FollowPath.Instance.setReady();
						FollowPath.Instance.resetBeginningRoom ();
					}
					break;
				case 1:
					FollowPath.Instance.resetImageEffect();
					FollowPath.Instance.setFinish();
					break;
				case 2:
					if(!FollowPath.Instance.isReday()){
						FollowPath.Instance.setReady();
						FollowPath.Instance.resetEndRoom ();
					}
					break;
				}
			}
		}
	}
}
