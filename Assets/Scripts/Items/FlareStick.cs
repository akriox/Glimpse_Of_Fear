using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlareStick : Collectible {

	private Vector3 rotation;
	
	public new void Awake(){
		base.Awake();
		// Ignore collisions with guardrail colliders
		Physics.IgnoreLayerCollision(10, 11);
		// Ignore collisions with water
		Physics.IgnoreLayerCollision(10, 4);
	}

	public new void Start(){
		base.Start ();
		rotation = new Vector3(Random.Range(100,360),Random.Range(100,360),Random.Range(100,360));
		transform.Rotate(rotation);
	}

	public new void Update(){

		base.Update();

		if(pickedUp){
			if(Inventory.Instance.FlareStickStash < 10){
				GameController.Instance.displayWidget(false);
				Inventory.Instance.addFlareStick(1);
				Destroy(this.gameObject);
			}
			else{
				GameController.Instance.displayWidget(false);
				StartCoroutine(GameController.Instance.displayTimedDebug("too many", 1.0f));
			}
		}
	}
}