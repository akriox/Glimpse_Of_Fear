using UnityEngine;
using System.Collections;

public class Maglite : Collectible {
	
	public new void Update(){
		base.Update();
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			FirstPersonController.enableRightHand(true);
			Destroy(this.gameObject);
		}
	}
}