using UnityEngine;
using System.Collections;

public class Battery : Collectible {

	public new void Update(){
		base.Update();
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			Flashlight.Instance.charge();
			Destroy(this.gameObject);
		}
	}
}