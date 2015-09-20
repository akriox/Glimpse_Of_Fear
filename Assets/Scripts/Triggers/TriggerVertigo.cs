using UnityEngine;
using System.Collections;

public class TriggerVertigo : MonoBehaviour {

	public Transform target;
	public bool enable;

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			if(target != null) CameraController.Instance.setVertigoTarget(target);
			CameraController.Instance.setVertigoEffect(enable);
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}
}
