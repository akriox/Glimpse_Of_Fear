using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GazeAwareComponent))]
public class Skull : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	
	private Light[] eye;
	private GameObject player;
	private Vector3 dir;
	private Quaternion rotation;
	private float angleOrigine;
	private float rotationSpeed = 4.0f;
	
	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		eye = GetComponentsInChildren<Light>();
		angleOrigine = transform.eulerAngles.x;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Update () {
		if(_gazeAwareComponent.HasGaze && eye[0].enabled && eye[1].enabled){
			faceTargetAt(player.transform.position);
		}
	}

	private void faceTargetAt(Vector3 at){
		dir = (at - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (dir.x, angleOrigine, dir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}

}
