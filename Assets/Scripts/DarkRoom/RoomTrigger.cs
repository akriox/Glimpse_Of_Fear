using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class RoomTrigger: MonoBehaviour {
		
		
		[SerializeField][Range(0, 2)]public int numRoom = 1;
		
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				switch (numRoom) {
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
