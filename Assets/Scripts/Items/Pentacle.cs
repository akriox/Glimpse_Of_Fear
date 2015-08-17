using UnityEngine;
using System.Collections;

public class Pentacle : Collectible {

	public GameObject blockingRocks;
	public GameObject firstCentralRoom;
	public GameObject secondCentralRoom;
	public GameObject skulls;
	private Light[] eyes;

	public new void Start(){
		base.Start ();
		eyes = skulls.GetComponentsInChildren<Light>();
		enableSkulls(false);
	}

	public new void Update () {
		base.Update();
		if(pickedUp){
			Destroy(blockingRocks);
			Inventory.Instance.hasPentacle = true;
			GameController.Instance.displayWidget(false);
			Destroy (firstCentralRoom);
			secondCentralRoom.SetActive(true);
			enableSkulls(true);
			Destroy(this.gameObject);
		}
	}

	private void enableSkulls(bool b){
		int i;
		for(i=0; i<eyes.Length; i++){
			eyes[i].enabled = b;
		}
	}
}
