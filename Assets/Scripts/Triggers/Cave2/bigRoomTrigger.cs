using UnityEngine;
using System.Collections;


public class bigRoomTrigger: MonoBehaviour {
		
	[SerializeField]private GameObject Path;
	[SerializeField] [Range(10.0F, 30.0F)]  private float wraithSpeed = 10.0f;
	private static bool _actif = false;

	public void Start(){
		Path.SetActive (false);
	}
	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Path.SetActive(true);
			moveWraithScripte.setSpeed(wraithSpeed);
		}
	}
	public IEnumerator OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			if(!_actif) Path.SetActive(false);
			yield return new WaitForSeconds(1f);
			//CameraController.Instance.resetShake();
			GameController.Instance.stopVibration();
			}
	}
	public static void isActif(){
		_actif = true;
	}
	public static void isUnActif(){
		_actif = false;
	}
}