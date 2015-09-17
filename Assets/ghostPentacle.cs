using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ghostPentacle : MonoBehaviour {
	
	public enum AnimationTypes {Random, ShakeHead, Turn, Idle, Point, PentacleRoom}
	public enum Sex {homme, femme}
	

	
	[SerializeField] private AnimationTypes _animationTypes = AnimationTypes.Random;
	[SerializeField] private Sex _sexType = Sex.homme;
	[SerializeField] private Transform pathToFollow;
	[SerializeField] private GameObject toFace;
	
	[SerializeField][Range(0.1F, 5.0F)] public float speed;
	
	private List<Transform> listPaths = new List<Transform>();
	private int index = 1;
	private bool walk = true;
	private bool ready = true;
	private bool look = false;
	
	private Transform currentTarget;
	private Transform lastTarget;
	
	private Animator _anim;
	
	private int moving = Animator.StringToHash("Moving");
	private int turn = Animator.StringToHash("Turn");
	private int point = Animator.StringToHash("Point");
	

	private float animationTurn = 3.967f;
	private float animationPoint = 3.8f;
	private float animationIdle = 2.867f;
	
	private Vector3 direction;
	private Quaternion rotation;
	private GameObject _player;
	
	private GazeAwareComponent _gazeAwareComponent;
	
	private AudioClip sonPointage;
	private AudioClip _voice;
	private AudioSource _audioSource;
	
	void Start () {
		_anim = GetComponent<Animator>();
		_gazeAwareComponent = GetComponentInChildren<GazeAwareComponent>();
		_player = GameObject.FindGameObjectWithTag("Player");
		_audioSource = GetComponent<AudioSource> ();
		sonPointage = (AudioClip)Resources.Load("Audio/Ghost/Spirit_wander01", typeof(AudioClip));
		if (_sexType == Sex.homme)
			_voice = (AudioClip)Resources.Load ("Audio/Ghost/Spirit_malevoice01", typeof(AudioClip));
		else {
			_voice = (AudioClip)Resources.Load("Audio/Ghost/Spirit_femalevoice01", typeof(AudioClip));
		}
		if(pathToFollow == null){
			Debug.LogError("Un GameObject 'pathGhost' doit etre renseigné dans le script 'Ghost.cs'.");
		} else {
			foreach(Transform temp in pathToFollow){
				listPaths.Add(temp);
			}
			if(listPaths.Count > 0) GetNewPosition();
		}
	}
	
	void Update () {
		if (look) {
			if(toFace != null) faceTarget (toFace.transform.position);
			else{
				faceTarget (_player.transform.position);
			}
		}
		if(walk &&_gazeAwareComponent.HasGaze ) 
			ready = true;
		if (ready) {
			StartWalk ();
			faceTarget (currentTarget.transform.position);
		}
		else{
			Idle ();
		}
	}
	
	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 2 * Time.deltaTime);
	}
	
	void GetNewPosition(){
		if(index > listPaths.Count) Destroy(this.gameObject);
		currentTarget = listPaths.Single(p => p.name == "Path" + index);
		index += 1;
	}
	
	public void StartWalk(){
		if(currentTarget != null){
			Walk ();
			transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
			if(CheckDistance() <= 0.5f){
				walk = false;
				ready = false;
				switch(_animationTypes){
				case AnimationTypes.Turn:
					StartCoroutine(playAnimation(1));
					break;
				case AnimationTypes.Idle:
					StartCoroutine(playAnimation(2));
					break;
				case AnimationTypes.Point:
					StartCoroutine(playAnimation(3));
					break;
				case AnimationTypes.PentacleRoom:
					StartCoroutine(playAnimation(1));
					_animationTypes = AnimationTypes.Point;
					break;
				}
				GetNewPosition();
			}
		}
	}
	
	float CheckDistance(){
		return Vector3.Distance(transform.position, currentTarget.position);
	}
	
	IEnumerator playAnimation(int numAnimation){
		switch (numAnimation) {
		case 1:
			Turn();
			yield return new WaitForSeconds(animationTurn);
			break;
		case 2:
			Idle();
			yield return new WaitForSeconds(animationIdle);
			break;
		case 3:
			look = true;
			Point();
			yield return new WaitForSeconds(animationPoint);
			look = false;
			break;
		}
		walk = true;
	}
	
	public void Idle(){
		_anim.SetBool(moving, false);
	}
	
	public void Walk(){
		_anim.SetBool(moving, true);
	}
	
	public void Turn(){
		_audioSource.clip = _voice;
		_audioSource.Play();
		_anim.SetTrigger(turn);
	}
	
	public void Point(){
		_audioSource.clip = sonPointage;
		_audioSource.Play();
		_anim.SetTrigger(point);
	}
}
