using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class RoomTrigger: MonoBehaviour {
		
		
		[SerializeField][Range(0, 1)]public int numRoom = 1;
		
		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				switch (numRoom) {
				case 0:
					FollowPath.Instance.isReady();
					break;
				case 1:
					FollowPath.Instance.resetImageEffect();
					FollowPath.Instance.resetBeginningRoom ();
					FollowPath.Instance.isFinish();
					break;
				}
			}
		}
	}
}
