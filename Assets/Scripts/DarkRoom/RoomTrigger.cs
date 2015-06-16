using UnityEngine;
using System.Collections;

public class RoomTrigger: MonoBehaviour {


	[SerializeField][Range(1, 4)]public int numRoom = 1;
	
	public void OnCollisionStay(Collision other) {

		if (other.gameObject.name == "Player") {
			Debug.Log ("coucou");
			switch (numRoom) {
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
			}
		}
	}
}
