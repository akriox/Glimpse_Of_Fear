using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

[RequireComponent (typeof (GazeAwareComponent))]
public class Spirit : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private AudioSource _audioSource;
	private List<Transform> listPosition = new List<Transform>();
	private Transform currentTarget;
	private NavMeshAgent agent;
	
	[SerializeField] private Transform PositionSpirit;
	[SerializeField] private Transform PositionSpirit2;

	private float timeToAppear = 0.9f;

	private enum State{NotAppear, Appear};
	private State _state;
	private SkinnedMeshRenderer jumpScare;

	private static bool notSeen = false;

	public void Start(){
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_audioSource = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent> ();

		jumpScare = GameObject.FindGameObjectWithTag("GhostGirl").GetComponentInChildren<SkinnedMeshRenderer>();
        //jumpScare.SetActive (false);

        if (PositionSpirit == null){
			Debug.Log("Spirit position null");
		} else {
			GetPaths(PositionSpirit);
			if(listPosition.Count > 0) GetNewPosition();
		}
		_state = State.Appear;
	}

	public void Update(){
		if (notSeen) {
			agent.enabled =false;
			transform.position = new Vector3(110.77f,7f,-22.84f);
			GetPaths(PositionSpirit2);
			GetNewPosition();
			notSeen = false;
		}
		switch(_state){
		case State.NotAppear:	

			break;
		case State.Appear:
			if (_gazeAwareComponent.HasGaze) {
				if (!_audioSource.isPlaying)_audioSource.Play ();
				CameraController.Instance.setVortexState (CameraController.VortexState.INC);
				//CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.INC);
				GameController.Instance.startVibration (0.8f, 0.8f);
				StartCoroutine (CameraController.Instance.Shake (2.0f, 0.05f, 10.0f));
				StartCoroutine (Appear());
				jumpScare.enabled = true;
				HeartBeat.playLoop();
				_state = State.NotAppear;
			} 
			else {
				WalkAround ();
			}
			break;
	
		}
	}

	IEnumerator Appear(){
		yield return new WaitForSeconds(timeToAppear);
		CameraController.Instance.setVortexState (CameraController.VortexState.DEC);
		//CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.DEC);
		GameController.Instance.stopVibration ();
		jumpScare.enabled = false;
	}

	void GetPaths(Transform _PositionSpirit){
		listPosition.Clear ();
		foreach(Transform temp in _PositionSpirit){
			listPosition.Add(temp);
		}
		agent.enabled = true;
	}
	
	void GetNewPosition(){
		currentTarget = listPosition[Random.Range (0, listPosition.Count)];
	}

	void WalkAround(){
		if(currentTarget != null){
			agent.SetDestination(currentTarget.position);
			if(CheckDistance(currentTarget.position) <= 2f){
				GetNewPosition();
			}
		}
	}

	public static void setNewPosition(){
		notSeen = true;
	}

	private float CheckDistance(Vector3 v){
		return Vector3.Distance(transform.position, v);
	}
}