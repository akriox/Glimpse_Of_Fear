using UnityEngine;
using System.Collections;

public class Rock : Collectible {

	private GameObject player;
	private Rigidbody rb;

	public void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
	}

	public void Update(){
		if(pickUp){
			rb.isKinematic = true;
			this.gameObject.transform.parent = player.transform;
			ThrowObject.setObjectToThrow(this.gameObject);
		}

		if(rb.isKinematic == false){
			this.gameObject.transform.parent = null;
		}
	}
}