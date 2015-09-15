using UnityEngine;
using System.Collections;

public class Maglite : Collectible {

	public new void Update(){
		base.Update();
		if(pickedUp){
			EventSound.playClip(GetComponent<AudioSource>().clip);
			GameController.Instance.displayWidget(false);
			FirstPersonController.enableRightHand(true);
			TipsTracker.Instance.displayTip(TipsTracker.Tips.UseFlashlight);
			Destroy(this.gameObject);
		}
	}
}