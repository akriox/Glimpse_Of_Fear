using UnityEngine;
using System.Collections;


public class ScriptWraith : MonoBehaviour {
		
		
	private Animator _anim;
		
	private int _IdleDiff = Animator.StringToHash("IdleDiff");
	private int _CatchDiff = Animator.StringToHash("CatchDiff");
	private int _LookDiff = Animator.StringToHash("LookDiff");
	private int _LookBehindDiff = Animator.StringToHash("LookBehindDiff");
	private int _RunDiff = Animator.StringToHash("RunDiff");

	private int _Walk = Animator.StringToHash("Walk");
	private int _Run = Animator.StringToHash("Run");
	private int _Catch = Animator.StringToHash("Catch");
	private int _CatchFinal1 = Animator.StringToHash("CatchFinal1");
	private int _CatchFinal2 = Animator.StringToHash("CatchFinal2");
	private int _Look = Animator.StringToHash("Look");
	private int _LookBehind = Animator.StringToHash("LookBehind");
	private int _Crawl = Animator.StringToHash("Crawl");
	private int _WalkAround = Animator.StringToHash("WalkAround");
		
	void Start () {
		_anim = GetComponentInParent<Animator> ();
	}
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.A))
			Walk ();
		if (Input.GetKeyDown (KeyCode.Z))
			Idle ();
		if (Input.GetKeyDown (KeyCode.E))
			WalkAround ();
		if (Input.GetKeyDown (KeyCode.R))
			Run ();
		if (Input.GetKeyDown (KeyCode.T))
			Crawl ();
		if (Input.GetKeyDown (KeyCode.Y))
			Look ();
		if (Input.GetKeyDown (KeyCode.U))
			LookBehind ();
		if (Input.GetKeyDown (KeyCode.I))
			Catch ();
		if (Input.GetKeyDown (KeyCode.O))
			CatchFinal ();
		if (Input.GetKeyDown (KeyCode.P))
			CatchFinalEnd ();


	}
		
	public void Idle(){
		_anim.SetBool(_Walk, false);
	}
		
	public void Walk(){
		_anim.SetBool(_Walk, true);
		_anim.SetBool(_Run, false);
		_anim.SetBool(_WalkAround, false);
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
		_anim.SetTrigger(_CatchFinal1);
	}

	public void CatchFinalEnd(){
		_anim.SetTrigger(_CatchFinal2);
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
} 