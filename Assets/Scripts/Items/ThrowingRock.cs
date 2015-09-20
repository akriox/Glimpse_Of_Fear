using UnityEngine;
using System.Collections;

public class ThrowingRock : Collectible {

	private Rigidbody rb;
	private float lifespan = 5.0f;
	
	public new void Start(){
		base.Start();
		rb = GetComponentInChildren<Rigidbody>();
		//Ignore collisions with GuardRails(11)
		Physics.IgnoreLayerCollision(14, 11);
	}
	
	public new void Update(){
		base.Update ();
		if(pickedUp && ThrowObject.objectToThrow == null){
			//Duplicate rock so there is always one available in the pile of rocks
			GameObject newRock = (GameObject) Instantiate(this.gameObject, this.transform.position , this.transform.rotation);
			newRock.name = "ThrowingRock";
			newRock.transform.parent = this.transform.parent;

			GetComponent<Renderer>().enabled = true;
			GameController.Instance.displayWidget(false);
			if(ThrowObject.setObjectToThrow(rb.gameObject)){
				rb.isKinematic = true;
				this.gameObject.transform.parent = Camera.main.transform;
			}
		}
		
		/* Rock throwed */
		if(rb.isKinematic == false){
			this.gameObject.transform.parent = null;
			lifespan -= Time.deltaTime;
		}

		if(lifespan <= 0.0f) Destroy(this.gameObject);
	}
}
