using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(UserPresenceComponent))]
[RequireComponent(typeof(GazeAwareComponent))]
public class ReaperController_V4 : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	private UserPresenceComponent _userPresenceComponent;
	private GameObject targetPlayer;
	
	private List<Transform> listPaths = new List<Transform>();
	private List<Vector3> playerPath = new List<Vector3>();
	private Vector3 direction;
	private Vector3 currentTarget;
	private Quaternion rotation;
	
	private float angleYOrigine;
	private float moveSpeed;
	private float stayCloseToThePlayer;
	private float delayToHaveSomething;
	private float delayPlayer = 0.0f;
	private float timeToReset;
	private int index = 1;
	
	private Renderer myRenderer;
	private float areaMinPlayerDetection = 3f;
	
	[SerializeField] private Transform pathToFollow;
	[SerializeField][Range(0.1F, 5.0F)] private float lumThreshold;
	[SerializeField][Range(1.0F, 10.0F)] private float initSpeedReaper;
	[SerializeField][Range(0.1F, 3.0F)] private float rotationSpeed;
	[SerializeField][Range(8.0F, 15.0F)] private float areaMaxPlayerDetection;
	[SerializeField][Range(0.1F, 5.0F)] private float minWaitTime;
	[SerializeField][Range(0.1F, 10.0F)] private float maxWaitTime;
	[SerializeField][Range(0.5F, 1.0F)] private float timeDropPathPlayer;
	[SerializeField][Range(5.0F, 10.0F)] private float timeStayCloseToThePlayer;
	[SerializeField][Range(0.01F, 0.1F)] private float increamentSpeedOfTheReaper;
	[SerializeField] private AudioSource m_LookSound;          // the sound when the player had look the reaper
	[SerializeField] private AudioSource m_ReaperClose;        // the sound when the reaper is close to the player
	[SerializeField] private AudioSource atmosphere;        
	[SerializeField] private GameObject jumpScare;
	[SerializeField] private bool randomMove;
	
	
	private enum State{moveWraith, FollowPlayer, PlayerInArea, PlayerClose, AreNothing, WraithCatchPlayer};
	private State _state;
	
	public void Start(){
		//get component for eye tracker
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_userPresenceComponent = GetComponent<UserPresenceComponent> ();
		//get the gameObject player
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		//save reaper Y angle and his original position
		angleYOrigine = transform.eulerAngles.y;
		myRenderer = GetComponent<Renderer> ();
		
		
		//test if the field is not empty
		if(pathToFollow == null){
			Debug.LogError("A GameObject 'Path' needed in 'FollowPath.cs'.");
		} else {
			GetPaths();
			if(listPaths.Count > 0) GetNewPosition();
			setInitialInformation ();
		}
	}
	
	public void Update(){
		switch(_state){
		case State.moveWraith:
			//Initial state of the wraith (patrol)
			if (_gazeAwareComponent.HasGaze) {
				setPlayerPath ();
				index = (index > 1 ) ? index - 1 : listPaths.Count;
				currentTarget = targetPlayer.transform.position;
				_state = State.FollowPlayer;
			}
			StartWalk (currentTarget);
			if (isPlayerFound())_state = State.PlayerInArea;
			break;

		case State.FollowPlayer:
			//State where the wraith is following the player
			PlayLookSound ();
			setNewPositionAroundPlayer (Vector3.Distance (transform.position, targetPlayer.transform.position));
			if(Time.time > delayPlayer) {
				setPlayerPath ();
			}
			StartWalk(currentTarget);
			//the player close his eyes so the wraith don't continue to get the current position of the player
			if (!_userPresenceComponent.IsUserPresent) {
				setPlayerPath();
				_state = State.moveWraith;
			}
			break;

		case State.PlayerInArea:
			//State where the player is in the wraith area detection
			faceTarget (targetPlayer.transform.position);
			PlayCloseSound ();
			if (closeToTheTarget (areaMinPlayerDetection, targetPlayer.transform.position)) {
				stayCloseToThePlayer = Time.time + timeStayCloseToThePlayer;
				_state = State.PlayerClose;
			} 
			setPlayerPath ();
			getPlayerPath ();
			//the wraith is near of the target (here the player)
			if (_userPresenceComponent.IsUserPresent && Flashlight.Instance.lum.intensity > lumThreshold) {
				playJumpScare();
			}
			break;

		case State.PlayerClose:
			//the wraith has detected the player so he stop and stay close to him during a amount of time
			faceTarget (targetPlayer.transform.position);
			turnAround ();
			//the wraith is near of the target (here the player)
			if (_userPresenceComponent.IsUserPresent && Flashlight.Instance.lum.intensity > lumThreshold) {
				playJumpScare();
			}
			if (Time.time > stayCloseToThePlayer) {
				setInitialInformation ();
				delayToHaveSomething = Time.time + 3.0f;
				_state = State.AreNothing;
			}
			break;

		case State.WraithCatchPlayer:
			//the wraith caught the player so after a certain amount of time reset the position and state of the wraith
			if(Time.time > timeToReset){
				transform.position = (listPaths.Single (p => p.name == "Path" + index)).position;
				transform.position = new Vector3(transform.position.x,0.0f,transform.position.z);
				myRenderer.enabled = true;
				setInitialInformation();
				_state = State.moveWraith;
			}
			break;

		case State.AreNothing:
			//the wraith found nothing so he come back to his initial walk
			StartWalk (currentTarget);
			if(Time.time > delayToHaveSomething)
				_state = State.moveWraith;
			break;
		}
	}

	private void playJumpScare(){
		//launch the jumpscare
		jumpScare.SetActive (true);
		timeToReset = Time.time + 2.0f;
		myRenderer.enabled = false;
		_state = State.WraithCatchPlayer;
	}

	private bool isPlayerFound(){
		//return true if the player is in the big area detection of the wraith
		if (closeToTheTarget (areaMaxPlayerDetection, targetPlayer.transform.position)) 
			return true;   
		return false;
	}


	private void turnAround(){
		//the wraith is turning around the player
		transform.RotateAround (targetPlayer.transform.position, Vector3.up, 30 * Time.deltaTime);
	}
	
	//put initial information for the reaper
	void setInitialInformation(){
		jumpScare.SetActive(false);
		moveSpeed = initSpeedReaper;
		playerPath.Clear ();
		GetNewPosition();
		StopCloseSound();
		StopLookSound();
	}
	
	//the reaper save the current position of the player
	void setPlayerPath(){
		playerPath.Add (targetPlayer.transform.position);
		delayPlayer = Time.time + timeDropPathPlayer;
	}
	
	//get the normal path of the reaper
	void GetPaths(){
		foreach(Transform temp in pathToFollow){
			listPaths.Add(temp);
		}
	}
	
	private void PlayLookSound(){
		if(!m_LookSound.isPlaying)
			m_LookSound.Play();
	}
	private void StopLookSound(){
		m_LookSound.Stop ();
	}
	private void PlayCloseSound(){
		if(!m_ReaperClose.isPlaying){
			m_ReaperClose.Play();
			m_LookSound.volume = 0.1f;
			atmosphere.volume = 0.2f;
		}
	}
	private void StopCloseSound(){
		m_ReaperClose.Stop ();
		m_LookSound.volume = 1f;
		atmosphere.volume = 1f;
	}
	
	//walk forward the target
	private void StartWalk(Vector3 targ){
		moveTowardTarget(targ);
		//the target is met
		if( closeToTheTarget(1f, targ)){
			GetNewPosition();
		}
	}
	
	//the monster can take time before walk to the next step
	IEnumerator Wait(){
		float time = Random.Range(minWaitTime, maxWaitTime);
		yield return new WaitForSeconds(time);
	}
	
	//set the reaper to a new position around the player
	private void setNewPositionAroundPlayer(float dist){
		float x = Random.Range (-(dist), (dist));
		float z = (Mathf.Sqrt (Mathf.Pow((dist),2) - Mathf.Pow(x,2)));
		if (Random.value > 0.5f)
			z = -z;
		transform.position = new Vector3 (x+targetPlayer.transform.position.x , transform.position.y,z+targetPlayer.transform.position.z);
	}
	
	private void getPlayerPath(){
		currentTarget = playerPath [0];
		playerPath.RemoveAt (0);
	}
	
	//get the next step of the path
	private void GetNewPosition(){
		//follow player path
		if (playerPath.Count > 0) {
			getPlayerPath();
			if (playerPath.Count == 0){
				stayCloseToThePlayer = Time.time + timeStayCloseToThePlayer;
				moveSpeed = initSpeedReaper;
			}
		} 
		else {
			//follow the normal path
			if (listPaths.Count > 0) {
				if(!randomMove){
					currentTarget = (listPaths.Single (p => p.name == "Path" + index)).position;
					index = (index < listPaths.Count) ? index + 1 : 1;
				}
				else{
					index = Random.Range(0, listPaths.Count);
					currentTarget = listPaths[index].position;
				}
			}
		}
		StartCoroutine (Wait ());
	}
	
	//return true if the monster is close to the target 
	private bool closeToTheTarget(float dist, Vector3 target){
		if (Vector3.Distance(transform.position, target) < dist) {
			return true;
		}
		else {
			return false;
		}
	}
	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, angleYOrigine, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}
	
	//move forward and face the position of the "vector3 to"
	private void moveTowardTarget(Vector3 to){
		faceTarget (to);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(to.x, to.y, to.z), moveSpeed* Time.deltaTime);
	}
}