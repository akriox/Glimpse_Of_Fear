using UnityEngine;
using System.Collections;

public class RoomTrigger: MonoBehaviour {
	

	[SerializeField][Range(0, 5)]public int numRoom = 1;
	
	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			changeFog.Instance.setMaxFogEndDistance();
			switch (numRoom) {
			case 0:
				changeFog.Instance.activeFog();
				break;
			case 1:
				FollowPath.Instance.resetFirstRoom ();
				break;
			case 2:
				FollowPath.Instance.resetSecondRoom ();
				break;
			case 3:
				FollowPath.Instance.resetThirdRoom ();
				break;
			case 4:
				FollowPath.Instance.resetFourthRoom ();
				break;
			
			case 5:
				changeFog.Instance.desactiveFog();
				break;
			}
		}
	}
}
