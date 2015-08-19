using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GazeAwareComponent))]
public class Skull : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	
	private Light[] eye;
	private GameObject player;
	private Vector3 dir;
	private Vector3 originalLook;
	private Quaternion rotation;
	private float angleOrigine;
	private float rotationSpeed = 4.0f;
	
	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		eye = GetComponentsInChildren<Light>();
		angleOrigine = transform.eulerAngles.x;
		originalLook = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Update () {
		if (eye [0].enabled && eye [1].enabled) {
			if (_gazeAwareComponent.HasGaze) {
				faceTargetAt (player.transform.position);
			} else {
				returnOriginalPosition ();
			}
		}
	}

	private void faceTargetAt(Vector3 at){
		dir = (at - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (dir.x, angleOrigine, dir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}

	private void returnOriginalPosition(){
		var target = Quaternion.Euler (originalLook);
		transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 2.0f);
	}

}
