using UnityEngine;
using System.Collections;

public class Collectible : GazeAwareComponent {

	protected bool pickUp;
	protected bool pickedUp;
	protected Sprite widgetSprite;

	public new void Start(){
		base.Start();
		widgetSprite =  (Sprite) Resources.Load("2D/Buttons/A", typeof(Sprite)); 
	}

	public new void Update(){

		base.Update ();
		GameController.Instance.displayWidget(HasGaze && pickUp || pickedUp);
		pickedUp = pickUp && Input.GetButtonDown("Submit");
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			pickUp = true;
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			pickUp = false;
		}
	}
}
