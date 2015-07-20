using UnityEngine;
using System.Collections;

public class Maglite : Collectible {

	public GameObject rightHand;

	public new void Update(){
		base.Update();
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			rightHand.SetActive(true);
			Destroy(this.gameObject);
		}
	}
}