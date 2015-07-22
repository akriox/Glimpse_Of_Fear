using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlowStick : Collectible {

	private Vector3 rotation;
	
	public new void Awake(){
		base.Awake();
		// Ignore collisions with guardrail colliders
		Physics.IgnoreLayerCollision(10, 11);
	}

	public new void Start(){
		base.Start ();
		rotation = new Vector3(Random.Range(100,360),Random.Range(100,360),Random.Range(100,360));
		transform.Rotate(rotation);
	}

	public new void Update(){

		base.Update();

		if(pickedUp){
			GameController.Instance.displayWidget(false);
			Inventory.Instance.addGlowStick(1);
			Destroy(this.gameObject);
		}
	}
}