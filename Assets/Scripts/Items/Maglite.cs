using UnityEngine;
using System.Collections;

public class Maglite : Collectible {

	public Sprite keyboardTipSprite;
	public Sprite gamepadTipSprite;

	public new void Update(){
		base.Update();
		if(pickedUp){
			EventSound.playClip(GetComponent<AudioSource>().clip);
			GameController.Instance.displayWidget(false);
			FirstPersonController.enableRightHand(true);
			if(TipsTracker.magliteTip){
				GameController.Instance.setTipSprite(keyboardTipSprite, gamepadTipSprite);
				GameController.Instance.displayTip(3.0f);
				TipsTracker.magliteTip = false;
			}
			Destroy(this.gameObject);
		}
	}
}