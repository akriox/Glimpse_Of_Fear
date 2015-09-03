using UnityEngine;
using System.Collections;

public class PutTablet : GazeAwareComponent {
	
	protected Sprite widgetSprite;
	private GameObject gate;
	private Animator _anim;
	
	private bool putDown;
	private bool gaze;

	public GameObject trigger;

	private int _open = Animator.StringToHash("Open");

	public new void Start(){
		base.Start ();
		GetComponent<MeshRenderer>().enabled = false;
		widgetSprite =  (Sprite) Resources.Load("2D/Buttons/A", typeof(Sprite)); 
		//Inventory.Instance.hasTablet = true;
		gate = this.gameObject.transform.parent.parent.gameObject;
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
		if(other.gameObject.tag == "Player" && Inventory.Instance.hasTablet){
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
		GetComponent<MeshRenderer>().enabled = true;
		gate.GetComponent<AudioSource>().Play();
		_anim.SetBool(_open, true);
		GameController.Instance.displayWidget(false);
		Inventory.Instance.hasTablet = false;
		trigger.SetActive (true);
		Destroy (this);
	}
}
