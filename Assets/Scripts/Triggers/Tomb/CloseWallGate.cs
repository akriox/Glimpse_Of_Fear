using UnityEngine;
using System.Collections;

public class CloseWallGate : MonoBehaviour {

	
	private GameObject gate;
	private Animator _anim;

	private int _open = Animator.StringToHash("Open");
	
	public void Start(){
		gate = this.gameObject.transform.parent.gameObject;
		_anim = gate.GetComponent<Animator> ();
	}
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Close ();
		}
	}
	
	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
	
	private void Close(){
		gate.GetComponent<AudioSource>().Play();
		_anim.SetBool(_open, false);
	}	
}
