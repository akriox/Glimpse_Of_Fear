using UnityEngine;
using System.Collections;

public class Tablet : Collectible {


	
	public AudioClip pickUpSound;
	
	public new void Start(){
		base.Start ();
	}
	
	public new void Update () {
		base.Update();
		if(pickedUp){
			Inventory.Instance.hasTablet = true;
			GameController.Instance.displayWidget(false);
			EventSound.playClip(pickUpSound);
			Destroy(this.gameObject);
		}
	}
}
