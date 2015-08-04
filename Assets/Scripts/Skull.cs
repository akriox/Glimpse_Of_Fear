using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GazeAwareComponent))]
public class Skull : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;

	//private AudioSource audioSource;
	//private bool facing;	

	private GameObject player;
	private Vector3 dir;
	private Quaternion rotation;
	private float angleOrigine;
	private float rotationSpeed = 4.0f;
	
	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();

		//audioSource = GetComponent<AudioSource>();

		angleOrigine = transform.eulerAngles.x;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Update () {
		if(_gazeAwareComponent.HasGaze){
			faceTargetAt(player.transform.position);

			/*
			if(!audioSource.isPlaying && facing == false){
				audioSource.Play();
				facing = true;
			}
			*/
		}
		/*
		else{
			facing = false;
			audioSource.Pause();
		}
		*/
	}

	private void faceTargetAt(Vector3 at){
		dir = (at - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (dir.x, angleOrigine, dir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}
}
