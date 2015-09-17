using UnityEngine;
using System.Collections;

public class Stash : Collectible {
	
	public new void Update(){
		
		base.Update();
		
		if(pickedUp){
			if(Inventory.Instance.canTake()){
				GameController.Instance.displayWidget(false);
				Inventory.Instance.addFlareStick(4);
				TipsTracker.Instance.displayTip(TipsTracker.Tips.UseFlareStick);
				Destroy(this.gameObject);
			}
			else{
				GameController.Instance.displayWidget(false);
				StartCoroutine(GameController.Instance.displayTimedDebug("too many", 1.0f));
			}
		}
	}
}