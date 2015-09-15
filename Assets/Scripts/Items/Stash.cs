using UnityEngine;
using System.Collections;

public class Stash : Collectible {
	
	public new void Update(){
		
		base.Update();
		
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			Inventory.Instance.addFlareStick(4);
			TipsTracker.Instance.displayTip(TipsTracker.Tips.UseFlareStick);
			Destroy(this.gameObject);
		}
	}
}