using UnityEngine;
using System.Collections;

public class Removal : GazeAwareComponent {
	
	private bool putDown;
	protected bool putedDown;
	protected Sprite widgetSprite;
	
	private bool gaze;
	
	public new void Start(){
		base.Start();
		widgetSprite =  (Sprite) Resources.Load("2D/Buttons/A", typeof(Sprite)); 
	}
	
	public new void Update(){
		base.Update ();
		if(HasGaze && gaze == false) gaze = true;
		putedDown = putDown && Input.GetButtonDown("Submit");
	}
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			GameController.Instance.displayWidget(gaze);
			putDown = true;
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			GameController.Instance.displayWidget(false);
			putDown = false;
			gaze = false;
		}
	}	
}
