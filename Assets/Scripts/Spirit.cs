using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[RequireComponent (typeof (GazeAwareComponent))]
public class Spirit : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private AudioSource _audioSource;
	private MeshRenderer _meshRenderer;

	private bool activ;
	public Transform PositionSpirit;
	
	private List<Transform> listPosition = new List<Transform>();
	private Transform currentTarget;
	private NavMeshAgent agent;
	private bool followPlayer;
	private bool canSwitchPosition;
	private GameObject targetPlayer;

	[SerializeField][Range(0.0F, 10.0F)] public float minWaitTime;
	[SerializeField][Range(0.0F, 10.0F)] public float maxWaitTime;
	[SerializeField][Range(8.0F, 12.0F)] public float timeForFollowPlayer;
	private float timeFollowPlayer;

	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_audioSource = GetComponent<AudioSource>();
		_meshRenderer = GetComponent<MeshRenderer> ();
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		followPlayer = false;
		agent = GetComponent<NavMeshAgent> ();
		if(PositionSpirit == null){
			Debug.Log("Spirit position null");
		} else {
			GetPaths();
			if(listPosition.Count > 0) GetNewPosition();
		}
		canSwitchPosition = true;
	}

	public void Update(){
		if(_gazeAwareComponent.HasGaze){
			if(!_audioSource.isPlaying)_audioSource.Play();
			CameraController.Instance.setVortexState(CameraController.VortexState.INC);
			CameraController.Instance.setNoiseAndScratches(true);
			GameController.Instance.startVibration(0.8f, 0.8f);
			if(activ == false){
				StartCoroutine(CameraController.Instance.Shake(2.0f, 0.05f, 10.0f));
				StartCoroutine(Flashlight.Instance.Flicker());
				activ = true;
				followPlayer = true;
				timeFollowPlayer = Time.time + timeForFollowPlayer;
				canSwitchPosition = false;
			}
			_meshRenderer.enabled = false;

		}
		else{
			activ = false;
			CameraController.Instance.setVortexState(CameraController.VortexState.DEC);
			CameraController.Instance.setNoiseAndScratches(false);
			GameController.Instance.stopVibration();
			if(followPlayer){
				_meshRenderer.enabled = true;
				StartWalk();
				if(Time.time > timeFollowPlayer){
					followPlayer = false;
					canSwitchPosition = true;
					GetNewPosition();
					switchPosition();
				}
			}
			else{
				if(canSwitchPosition) WalkAround();
			}
		}
	}

	void GetPaths(){
		foreach(Transform temp in PositionSpirit){
			listPosition.Add(temp);
		}
	}
	
	IEnumerator Wait(){
		float time = Random.Range(minWaitTime, maxWaitTime);
		yield return new WaitForSeconds(time);
		canSwitchPosition = true;
	}
	
	void GetNewPosition(){
		currentTarget = listPosition[Random.Range (0, listPosition.Count)];
		StartCoroutine(Wait());
	}

	void switchPosition(){
		transform.position = currentTarget.position;
		canSwitchPosition = false;
		GetNewPosition ();
	}

	void WalkAround(){
		if(currentTarget != null){
			agent.SetDestination(currentTarget.position);
			if(CheckDistance(currentTarget.position) <= 2f){
				canSwitchPosition = false;
				GetNewPosition();
			}
		}
	}

	void StartWalk(){
		if(CheckDistance(targetPlayer.transform.position)>= 4f){
				agent.SetDestination(targetPlayer.transform.position);
			}
	}

	float CheckDistance(Vector3 v){
		return Vector3.Distance(transform.position, v);
	}
}