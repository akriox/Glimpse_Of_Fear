using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[RequireComponent (typeof (GazeAwareComponent))]
public class Spirit : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private AudioSource _audioSource;

	private List<Transform> listPosition = new List<Transform>();
	private Transform currentTarget;
	private NavMeshAgent agent;
	private GameObject targetPlayer;

	private Vector3 direction;
	private Quaternion rotation;

	[SerializeField] private Transform PositionSpirit;
	[SerializeField] private GameObject _texture;
	[SerializeField] private GameObject _smoke;
	[SerializeField][Range(1.5F, 3.5F)] private float timeForFollowPlayer = 2f;
	[SerializeField][Range(5f, 20f)] private float distDetection = 15f;
	[SerializeField] private bool beTheOtherSpecter;
	[SerializeField] private GameObject forDestroyItSelf;

	private float timeFollowPlayer;
	private float timeToNotAppear;

	private enum State{NotAppear, Appear, FollowPlayer};
	private State _state;

	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_audioSource = GetComponent<AudioSource>();
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		agent = GetComponent<NavMeshAgent> ();
		if(PositionSpirit == null){
			Debug.Log("Spirit position null");
		} else {
			GetPaths();
			if(listPosition.Count > 0) GetNewPosition();
		}
		_state = State.Appear;
	}

	public void Update(){
		switch(_state){
		case State.NotAppear:	
			if (Time.time > timeToNotAppear)_state = State.Appear;
			CameraController.Instance.setVortexState (CameraController.VortexState.DEC);
			CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.DEC);
			GameController.Instance.stopVibration ();
			break;
		case State.Appear:
			if (_gazeAwareComponent.HasGaze && nearPlayer(distDetection)) {
				if (!_audioSource.isPlaying)_audioSource.Play ();
				CameraController.Instance.setVortexState (CameraController.VortexState.INC);
				CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.INC);
				GameController.Instance.startVibration (0.8f, 0.8f);
				faceTarget (targetPlayer.transform.position);
				StartCoroutine (CameraController.Instance.Shake (2.0f, 0.05f, 10.0f));
				StartCoroutine (Flashlight.Instance.Flicker ());
				_state = State.FollowPlayer;
				timeFollowPlayer = Time.time + Random.Range (timeForFollowPlayer - 0.5f, timeForFollowPlayer + 0.5f);
				_smoke.SetActive (true);
				_texture.SetActive (true);
				HeartBeat.playLoop();
				iniatFog.specterSeen();
			} 
			else {
					WalkAround ();
			}
			break;
		case State.FollowPlayer:
			faceTarget (targetPlayer.transform.position);
			StartWalk ();
			if (Time.time > timeFollowPlayer) {
				GetNewPosition ();
				switchPosition ();
				_smoke.SetActive (false);
				_texture.SetActive (false);
				timeToNotAppear = Time.time + Random.Range (4f, 6f);
				if(beTheOtherSpecter)
					Destroy(forDestroyItSelf);
				_state = State.NotAppear;
			}
			break;
		}
	}

	void GetPaths(){
		foreach(Transform temp in PositionSpirit){
			listPosition.Add(temp);
		}
	}
	
	void GetNewPosition(){
		currentTarget = listPosition[Random.Range (0, listPosition.Count)];
	}

	void switchPosition(){
		transform.position = currentTarget.position;
		GetNewPosition ();
	}

	void WalkAround(){
		if(currentTarget != null){
			agent.SetDestination(currentTarget.position);
			if(CheckDistance(currentTarget.position) <= 2f){
				GetNewPosition();
			}
		}
	}

	void StartWalk(){
		if(!nearPlayer(4f)){
			agent.SetDestination(targetPlayer.transform.position);
		}
	}

	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 1000 * Time.deltaTime);
	}
	private bool nearPlayer(float dist){
		if (CheckDistance (targetPlayer.transform.position) < dist) return true;
		return false;	
	}

	private float CheckDistance(Vector3 v){
		return Vector3.Distance(transform.position, v);
	}
}