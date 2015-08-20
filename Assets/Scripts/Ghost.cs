using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour {

	public bool gazeContact;
	public float fadeSpeed = 2.0f;

	private Animator _anim;
	private GazeAwareComponent _gazeAwareComponent;
	private Material _material;
	
	private int moving = Animator.StringToHash("Moving");
	private int turn = Animator.StringToHash("Turn");
	private int shakeHead = Animator.StringToHash("ShakeHead");
	private int point = Animator.StringToHash("Point");


	public void Start () {
		_anim = GetComponent<Animator>();
		_gazeAwareComponent = GetComponentInChildren<GazeAwareComponent>();
		_material = GetComponentInChildren<Renderer>().material;
	}

	public void Update () {
		if(gazeContact){
			if(_gazeAwareComponent.HasGaze){
				Disappear();
			}
			else{
				Appear();
			}
		}
	}

	private void Appear(){
		if(_material.color != Color.white){
			_material.color = Color.Lerp (_material.color, Color.white, fadeSpeed * Time.deltaTime);
		}
	}

	private void Disappear(){
		if(_material.color != Color.black){
			_material.color = Color.Lerp (_material.color, Color.black, fadeSpeed * Time.deltaTime);
		}
	}

	private void Idle(){
		_anim.SetBool(moving, false);
	}

	private void Walk(){
		_anim.SetBool(moving, true);
	}

	private void ShakeHead(){
		_anim.SetTrigger(shakeHead);
	}

	private void Turn(){
		_anim.SetTrigger(turn);
	}

	private void Point(){
		_anim.SetTrigger(point);
	}
}
