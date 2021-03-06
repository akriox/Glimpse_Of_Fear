﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class EndWraith : MonoBehaviour {

	private Quaternion rotation;
	private Vector3 direction;
	public GameObject takeSoul;
	public GameObject pointToLook;

	private float rotationSpeed = 6f;
	private float speed = 3f;
	private Animator _animator;
	
	public Image titleTip;

	private bool move;
	private int _CatchFinal = Animator.StringToHash("CatchFinal");

	// Use this for initialization
	void Start () {
		_animator = GetComponentInParent<Animator> ();
		move = true;
		StartCoroutine (Wait ());
	}
	
	// Update is called once per frame
	void Update () {
		//faceTarget ();
		if (move) {
			faceTarget ();
			flyTowardTarget ();
		}
	}
	
	IEnumerator Wait(){
		yield return new WaitForSeconds(2.75f);
		move = false;
		CatchFinal();
		yield return new WaitForSeconds(2.1f);
		takeSoul.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		LoadingScreen.Instance.fadeEndGame();
		yield return new WaitForSeconds(4.1f);
		CatchFinalEnd ();
		yield return new WaitForSeconds(5f);
		LoadingScreen.Instance.fadeToClear ();
		titleTip.enabled = true;
		yield return new WaitForSeconds(1f);
		StartCoroutine (DisplayTitle ());
		yield return new WaitForSeconds(14f);
		Application.LoadLevel (0);
	}

	IEnumerator DisplayTitle(){
		while (titleTip.fillAmount < 1f) {
			titleTip.fillAmount += Random.value * 0.05f;
			yield return new WaitForSeconds(Random.Range(0.1f,0.2f));
		}
	}

	public void CatchFinal(){
		_animator.SetBool(_CatchFinal,true);
	}

	
	public void CatchFinalEnd(){
		_animator.SetBool(_CatchFinal,false);
	}

	//face the target
	private void faceTarget(){
		direction = (pointToLook.transform.position - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}

	//move forward and face the position of the "vector3 to"
	private void flyTowardTarget(){
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (26.34325f, -1.69f, -55.21652f), speed * Time.deltaTime);
	}
}
