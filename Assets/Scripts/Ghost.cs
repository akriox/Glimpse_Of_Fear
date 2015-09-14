using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent (typeof(Animator))]
public class Ghost : MonoBehaviour {

	public enum MovementTypes { Follow, Random}
	public enum AnimationTypes {Random, ShakeHead, Turn, Idle, Point, PentacleRoom}
	public enum Sex {homme, femme}

	public bool gazeContact;
	private float fadeSpeed = 4.0f;

	public MovementTypes _movementType = MovementTypes.Follow;
	public AnimationTypes _animationTypes = AnimationTypes.Random;
	public Sex _sexType = Sex.homme;
	public Transform pathToFollow;

	[SerializeField][Range(0.1F, 5.0F)] public float speed;

	private List<Transform> listPaths = new List<Transform>();
	private int index = 1;
	private bool walk = true;
	private bool lookPlayer = false;

	private Transform currentTarget;
	private Transform lastTarget;
	
	private Animator _anim;

	private int moving = Animator.StringToHash("Moving");
	private int turn = Animator.StringToHash("Turn");
	private int shakeHead = Animator.StringToHash("ShakeHead");
	private int point = Animator.StringToHash("Point");
	
	private float animationTimeShake_Head = 9.967f;
	private float animationTurn = 3.967f;
	private float animationPoint = 3.8f;
	private float animationIdle = 2.867f;

	private Vector3 direction;
	private Quaternion rotation;
	private GameObject _player;

	private GazeAwareComponent _gazeAwareComponent;
	private Material _material;

	private AudioClip sonPointage;
	private AudioClip _voice;
	private AudioSource _audioSource;

	void Start () {
		_anim = GetComponent<Animator>();
		_gazeAwareComponent = GetComponentInChildren<GazeAwareComponent>();
		_material = GetComponentInChildren<Renderer>().material;
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
			switch(_movementType){
			case MovementTypes.Follow:
				index = 1;
				break;
			case MovementTypes.Random:
				index = 1;
				break;
			}
			if(listPaths.Count > 0) GetNewPosition();
		}
	}

	void Update () {
		if (lookPlayer)
			faceTarget (_player.transform.position);
		if (gazeContact) {
			if (_gazeAwareComponent.HasGaze) {
				Disappear ();
			} else {
				Appear ();
			}
		}
		if(walk) {
			StartWalk();
			faceTarget (currentTarget.transform.position);
		}
	}

	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 2 * Time.deltaTime);
	}

	void GetNewPosition(){
		
		switch(_movementType){
		case MovementTypes.Follow:
			currentTarget = listPaths.Single(p => p.name == "Path" + index);
			index = (index < listPaths.Count) ? index +1 : 1;
			break;
		case MovementTypes.Random:
			currentTarget = listPaths[Random.Range(0, listPaths.Count)];
			break;
		}
	}

	public void StartWalk(){
		if(currentTarget != null){
			Walk ();
			transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
			if(CheckDistance() <= 0.5f){
				walk = false;
				switch(_animationTypes){
				case AnimationTypes.Random:
					StartCoroutine(playAnimation(Random.Range(0, 4)));
					break;
				case AnimationTypes.ShakeHead:
					StartCoroutine(playAnimation(0));
					break;
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
		case 0:
			ShakeHead();
			yield return new WaitForSeconds(animationTimeShake_Head);
			break;
		case 1:
			Turn();
			yield return new WaitForSeconds(animationTurn);
			break;
		case 2:
			Idle();
			yield return new WaitForSeconds(animationIdle);
			break;
		case 3:
			lookPlayer = true;
			Point();
			yield return new WaitForSeconds(animationPoint);
			lookPlayer = false;
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
	
	public void ShakeHead(){
		_anim.SetTrigger(shakeHead);
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