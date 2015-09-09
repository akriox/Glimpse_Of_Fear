﻿using UnityEngine;
using System.Collections;

public class PutObject: GazeAwareComponent {
	
	protected Sprite widgetSprite;
	public GameObject gate;
	private Animator _anim;
	private bool putDown;
	private bool gaze;

	public enum ObjectToPut {Pentacle,Tablet}
	public ObjectToPut _object = ObjectToPut.Tablet;
	
	public GameObject trigger;
	
	public new void Start(){
		base.Start ();
		//GetComponent<MeshRenderer>().enabled = false;
		widgetSprite =  (Sprite) Resources.Load("2D/Buttons/A", typeof(Sprite)); 
		//Inventory.Instance.hasTablet = true;
		_anim = gate.GetComponent<Animator> ();
	}
	
	public new void Update(){
		base.Update ();
		if(HasGaze && gaze == false) gaze = true;
		if (putDown && Input.GetButtonDown ("Submit")) {
			Open ();
		}
	}
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player" &&((_object == ObjectToPut.Tablet&&Inventory.Instance.hasTablet)||(_object == ObjectToPut.Pentacle&&Inventory.Instance.hasPentacle))){
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
	
	private void Open(){
		//GetComponent<MeshRenderer>().enabled = true;
		gate.GetComponent<AudioSource>().Play();
		_anim.SetBool(Animator.StringToHash("Open"), true);
		GameController.Instance.displayWidget(false);
		if (_object == ObjectToPut.Tablet)
			Inventory.Instance.hasTablet = false;
		else {
			Inventory.Instance.hasPentacle = false;
		}
		if(trigger!=null)trigger.SetActive (true);
		Destroy (this);
	}
}