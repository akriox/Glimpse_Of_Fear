using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GazeAwareComponent))]
public class StatueController : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	private GameObject target;
	private Vector3 dir;
	private Vector3 pos;
	private Quaternion rotation;
	private float angleYOrigine;
	private bool _hasAlreadyFocus = false;

	public float moveSpeed = 5.0f;
	public float rotationSpeed = 1.0f;
	public bool instantRotate;
	public bool mobile;
	
	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		angleYOrigine = transform.eulerAngles.y;
		target = GameObject.FindGameObjectWithTag("Player");
	}
	
	public void Update(){

		if (_gazeAwareComponent.HasGaze) {
			if(!mobile)
				faceTargetAt(target.transform.position);
			_hasAlreadyFocus = true;
		}
		if (mobile && !_gazeAwareComponent.HasGaze && _hasAlreadyFocus) {
			faceTargetAt(target.transform.position);
			moveTowardTarget(target.transform.position);
		}
	}
	
	private void faceTargetAt(Vector3 at){
		dir = (at - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (dir.x, angleYOrigine, dir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
		if(instantRotate) transform.rotation = rotation;
	}

	private void moveTowardTarget(Vector3 to){
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(to.x, 0.0f, to.z+5.0f), moveSpeed* Time.deltaTime);
	}
}