using UnityEngine;
using System.Collections;


	
	
[RequireComponent (typeof(Animator))]
public class ScriptWraith : MonoBehaviour {
		
	public enum MovementTypes { Follow, Random}
	public enum AnimationTypes {Random, ShakeHead, Turn, Idle, Point}
		
	public bool gazeContact;
	public float fadeSpeed = 2.0f;
		
	public MovementTypes _movementType = MovementTypes.Follow;
	public AnimationTypes _animationTypes = AnimationTypes.Random;
		
	[SerializeField][Range(0.1F, 5.0F)] public float speed;

		
	public Animator _anim;
		
	private int _IdleDiff = Animator.StringToHash("IdleDiff");
	private int _CatchDiff = Animator.StringToHash("CatchDiff");
	private int _LookDiff = Animator.StringToHash("LookDiff");
	private int _LookBehindDiff = Animator.StringToHash("LookBehindDiff");
	private int _RunDiff = Animator.StringToHash("RunDiff");

	private int _Walk = Animator.StringToHash("Walk");
	private int _Run = Animator.StringToHash("Run");
	private int _Catch = Animator.StringToHash("Catch");
	private int _CatchFinal = Animator.StringToHash("CatchFinal");
	private int _Look = Animator.StringToHash("Look");
	private int _LookBehind = Animator.StringToHash("LookBehind");
	private int _Crawl = Animator.StringToHash("Crawl");
	private int _WalkAround = Animator.StringToHash("WalkAround");
		
		
	private Vector3 direction;
	private Quaternion rotation;
	private GameObject _player;
		
	private Material _material;
		
	void Start () {
		_material = GetComponentInChildren<Renderer>().material;
	}
		
	void Update () {

	}
		
	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 2 * Time.deltaTime);
	}
		
	public void Idle(){
		_anim.SetBool(_Walk, false);
	}
		
	public void Walk(){
		_anim.SetBool(_Walk, true);
		_anim.SetBool(_Run, false);
	}

	public void WalkAround(){
		_anim.SetBool(_WalkAround, true);
		_anim.SetBool(_Run, false);

	}
		
	public void Run(){
		_anim.SetBool(_Walk, true);
		_anim.SetBool(_Run, true);
	}

	public void Crawl(){
		_anim.SetBool(_Walk, true);
		_anim.SetBool(_Crawl, true);
	}

	public void LookBehind(){
		_anim.SetTrigger(_LookBehind);
	}
		
	public void Look(){
		_anim.SetTrigger(_Look);
	}
		
	public void Catch(){
		_anim.SetTrigger(_Catch);
	}
		
	public void CatchFinal(){
		_anim.SetTrigger(_CatchFinal);
	}

	public void ChooseIdle(float f){
		_anim.SetFloat(_IdleDiff, f);
	}

	public void ChooseCatch(float f){
		_anim.SetFloat(_CatchDiff, f);
		Catch ();
	}

	public void ChooseLook(float f){
		_anim.SetFloat(_LookDiff, f);
		Look ();
	}

	public void ChooseLookBehind(float f){
		_anim.SetFloat(_LookBehindDiff, f);
		LookBehind ();
	}

	public void ChooseRun(float f){
		_anim.SetFloat(_RunDiff, f);
		Run ();
	}

	private void Appear(){
		if(_material.color != Color.white){
			_material.color = Color.Lerp (_material.color, Color.white, 0.8f*fadeSpeed * Time.deltaTime);
		}
	}
		
	private void Disappear(){
		if(_material.color != Color.black){
			_material.color = Color.Lerp (_material.color, Color.black, fadeSpeed * Time.deltaTime);
		}
	}
} 