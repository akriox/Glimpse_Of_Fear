using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[RequireComponent (typeof (GazeAwareComponent))]
public class Spirit : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private AudioSource _audioSource;

	private bool activ;
	public Transform PositionSpirit;
	
	private List<Transform> listPosition = new List<Transform>();
	private Transform currentTarget;
	private NavMeshAgent agent;
	private bool followPlayer;
	private bool canSwitchPosition;
	private bool activeTexture;
	private GameObject targetPlayer;

	private Vector3 direction;
	private Quaternion rotation;

	[SerializeField][Range(0.0F, 10.0F)] public float minWaitTime;
	[SerializeField][Range(0.0F, 10.0F)] public float maxWaitTime;
	[SerializeField][Range(8.0F, 12.0F)] public float timeForFollowPlayer;
	[SerializeField][Range(0.5F, 1.5F)] public float timeAppear;
	[SerializeField] private GameObject _texture;
	[SerializeField] private GameObject _smoke;
	private float timeFollowPlayer;
	private float timeForSwitchAppear;

	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_audioSource = GetComponent<AudioSource>();
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
			faceTarget(targetPlayer.transform.position);
			if(activ == false){
				StartCoroutine(CameraController.Instance.Shake(2.0f, 0.05f, 10.0f));
				StartCoroutine(Flashlight.Instance.Flicker());
				activ = true;
				followPlayer = true;
				timeFollowPlayer = Time.time + timeForFollowPlayer;
				timeForSwitchAppear = Time.time + timeAppear;
				canSwitchPosition = false;
				_smoke.SetActive(true);
				activeTexture = true;
				_texture.SetActive(activeTexture);

			}

		}
		else{
			_texture.SetActive(activeTexture);
			activ = false;
			CameraController.Instance.setVortexState(CameraController.VortexState.DEC);
			CameraController.Instance.setNoiseAndScratches(false);
			GameController.Instance.stopVibration();
			if(followPlayer){
				StartWalk();
				if(Time.time > timeForSwitchAppear){
					timeForSwitchAppear = Time.time + Random.Range(timeAppear-0.5f, timeAppear+1.5f);
					activeTexture = !activeTexture;
				}
				if(Time.time > timeFollowPlayer){
					followPlayer = false;
					canSwitchPosition = true;
					GetNewPosition();
					switchPosition();
					_smoke.SetActive(false);
					activeTexture = false;
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

	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 1000 * Time.deltaTime);
	}

	float CheckDistance(Vector3 v){
		return Vector3.Distance(transform.position, v);
	}
}