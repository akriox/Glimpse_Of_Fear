using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rock : Collectible {

	private GameObject player;
	private Rigidbody rb;

	public new void Start(){
		base.Start();
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
	}

	public new void Update(){
		base.Update ();
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			if(ThrowObject.setObjectToThrow(this.gameObject)){
				rb.isKinematic = true;
				this.gameObject.transform.parent = player.transform;
			}
		}

		/* Rock throwed */
		if(rb.isKinematic == false){
			this.gameObject.transform.parent = null;
		}
	}
}