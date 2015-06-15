using UnityEngine;
using System.Collections;

public class Battery : Collectible {

	public void Update(){
		if(pickUp && Input.GetButtonDown("Submit")){
			Destroy(this.gameObject);
		}
	}
}