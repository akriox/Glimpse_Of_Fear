using UnityEngine;
using System.Collections;

public class Collectible : GazeAwareComponent {

	private bool pickUp;
	protected bool pickedUp;
	protected Sprite widgetSprite;

	private bool gaze;

	public new void Start(){
		base.Start();
		widgetSprite =  (Sprite) Resources.Load("2D/Buttons/A", typeof(Sprite)); 
	}

	public new void Update(){
		base.Update ();
		if(HasGaze && gaze == false) gaze = true;
		pickedUp = pickUp && Input.GetButtonDown("Submit");
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			GameController.Instance.displayWidget(gaze);
			pickUp = true;
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			GameController.Instance.displayWidget(false);
			pickUp = false;
			gaze = false;
		}
	}
}
