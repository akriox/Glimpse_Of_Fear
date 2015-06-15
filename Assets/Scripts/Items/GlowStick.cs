using UnityEngine;
using System.Collections;

public class GlowStick : Collectible {

	private Vector3 rotation;

	public void Start(){
		rotation = new Vector3(Random.Range(100,360),Random.Range(100,360),Random.Range(100,360));
		transform.Rotate(rotation);
	}

	public void Update(){
	
		if(pickUp && Input.GetButtonDown("Submit")){
			Inventory.Instance.addGlowStick(1);
			Destroy(this.gameObject);
		}
	}
}