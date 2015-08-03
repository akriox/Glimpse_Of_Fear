using UnityEngine;
using System.Collections;

public class CollapseBridge : MonoBehaviour {

	public GameObject weakspot;
	private Rigidbody[] plank;

	public void Start(){
		plank = weakspot.GetComponentsInChildren<Rigidbody>();
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			StopAllCoroutines();
			StartCoroutine(CameraController.Instance.Shake(2.0f, 1.0f, 5.0f));
			Collapse();
		}
	}

	private void Collapse(){
		int i;
		for(i=0; i<plank.Length; i++){
			plank[i].isKinematic = false;
		}
		weakspot.GetComponent<AudioSource>().Play();
	}
}
