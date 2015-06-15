using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(UserPresenceComponent))]
[RequireComponent(typeof(GazeAwareComponent))]
public class ReaperController_V2 : MonoBehaviour {
		
	private GazeAwareComponent _gazeAwareComponent;
	private UserPresenceComponent _userPresenceComponent;
	private GameObject targetPlayer;
	
	private List<Transform> listPaths = new List<Transform>();
	private List<Vector3> reaperPath = new List<Vector3>();
	private List<Vector3> playerPath = new List<Vector3>();
	private Vector3 direction;
	private Vector3 currentTarget;
	private Vector3 positionOrigine;
	private Quaternion rotation;
	
	private float angleYOrigine;
	private float moveSpeed;
	private float stayCloseToThePlayer;
	private float delayPlayer ;
	private float timeToReset;
	private int index = 1;
	
	private bool walk; 
	private bool followPlayer;
	private bool playerClose;
	private bool canReturnLastPos;
	private bool haveJumpScare;

	public Transform pathToFollow;
	[SerializeField][Range(0.1F, 5.0F)] public float lumThreshold;
	[SerializeField][Range(1.0F, 10.0F)] public float initSpeedReaper;
	[SerializeField][Range(0.1F, 3.0F)] public float rotationSpeed;
	[SerializeField][Range(1.0F, 15.0F)] public float areaPlayerDetection;
	[SerializeField][Range(0.0F, 5.0F)] public float minWaitTime;
	[SerializeField][Range(0.0F, 10.0F)] public float maxWaitTime;
	[SerializeField][Range(1.0F, 5.0F)] public float timeDropPathPlayer;
	[SerializeField][Range(5.0F, 10.0F)] public float timeStayCloseToThePlayer;
	[SerializeField][Range(0.01F, 0.1F)] public float increamentSpeedOfTheReaper;
	[SerializeField] private AudioSource m_LookSound;          // the sound when the player had look the reaper
	[SerializeField] private AudioSource m_ReaperClose;        // the sound when the reaper is close to the player
	
	public GameObject jumpScare;
	
	
	public void Start(){
		//get component for eye tracker
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_userPresenceComponent = GetComponent<UserPresenceComponent> ();
		//get the gameObject player
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		//save reaper Y angle and his original position
		angleYOrigine = transform.eulerAngles.y;
		positionOrigine = transform.position;

		setInitialInformation ();
	
		//test if the field is not empty
		if(pathToFollow == null){
			Debug.LogError("A GameObject 'Path' needed in 'FollowPath.cs'.");
		} else {
			GetPaths();
			if(listPaths.Count > 0) GetNewPosition();
		}
	}
	
	public void Update(){

		moveReaper ();
		//the player is near of the reaper. The reaper is moving towards the player
		if (closeToTheTarget (areaPlayerDetection, targetPlayer.transform.position)&& !closeToTheTarget(3f, targetPlayer.transform.position)) {
			PlayCloseSound();
			setPlayerPath();
			playerClose = true;
			canReturnLastPos = false;
			stayCloseToThePlayer = Time.time + timeStayCloseToThePlayer;
		}
		//the reaper is near of the target (here the player)
		if (_userPresenceComponent.IsUserPresent && closeToTheTarget (areaPlayerDetection,targetPlayer.transform.position) && Flashlight.Instance.lum.intensity > lumThreshold && !haveJumpScare) {
			jumpScare.SetActive(true);
			haveJumpScare = true;
			timeToReset = Time.time+2.0f;
		}
		//the reaper caught the player
		if (haveJumpScare) {
			if(Time.time > timeToReset){
				transform.position = positionOrigine;
				setInitialInformation();
			}
		}
		//the player close his eyes so the reaper don't continue to get the current position of the player
		if (!_userPresenceComponent.IsUserPresent && !canReturnLastPos) {
			followPlayer = false;
			moveSpeed = initSpeedReaper;
		}
		//the reaper has detected the player so he stop and stay close to him during a amount of time
		if(playerClose && Time.time>stayCloseToThePlayer){
			setInitialInformation();
			canReturnLastPos = true;
		}
	}

	private void moveReaper(){
		//the player is looking the reaper
		if (_gazeAwareComponent.HasGaze) {
			PlayLookSound ();
			//setNewPosition (Vector3.Distance (transform.position, targetPlayer.transform.position));
			followPlayer = true;
			canReturnLastPos = true;
			//save the current target of the reaper before the player look at him
			if (reaperPath.Count == 0) {
				reaperPath.Add (currentTarget);
			}
			//save the player position
			if (playerPath.Count == 0) {
				setPlayerPath ();
				delayPlayer = Time.time;
			}
			currentTarget = targetPlayer.transform.position;
		}
		//follow the path or the steps of the reaper
		else {
			if(followPlayer){
				moveSpeed += increamentSpeedOfTheReaper;
				//currentTarget = targetPlayer.transform.position;
				if (Time.time > delayPlayer) {
					setPlayerPath ();
					delayPlayer = Time.time + timeDropPathPlayer;
				}
			}
			if (walk) {
				//StartWalk (currentTarget);
			}
		}
	}

	void setInitialInformation(){
		index = 1;
		jumpScare.SetActive(false);
		haveJumpScare = false;
		followPlayer = false;
		playerClose = false;
		canReturnLastPos = true;
		walk = false;
		moveSpeed = initSpeedReaper;
		playerPath.Clear ();
		reaperPath.Clear ();
		GetNewPosition();
		StopCloseSound();
		StopLookSound();
	}
	
	//the reaper save is current position
	void setReaperPath(){
		reaperPath.Add (transform.position);
	}
	//the reaper save the current position of the player
	void setPlayerPath(){
		playerPath.Add (targetPlayer.transform.position);
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
			m_LookSound.volume = 0.2f;
		}
	}
	private void StopCloseSound(){
		m_ReaperClose.Stop ();
	}

	//walk forward the target
	private void StartWalk(Vector3 targ){
		moveTowardTarget(targ);
		//the target is met
		if( closeToTheTarget(2f, targ)){
			walk = false;
			//if(!playerClose)
				GetNewPosition();
		}
	}
	
	//the monster can take time before walk to the next step
	IEnumerator Wait(){
		float time = Random.Range(minWaitTime, maxWaitTime);
		yield return new WaitForSeconds(time);
		walk = true;
	}

	//set the reaper to a new position around the player
	private void setNewPosition(float dist){
		float x = Random.Range (-(dist), (dist));
		float z = (Mathf.Sqrt (Mathf.Pow((dist),2) - Mathf.Pow(x,2)));
		if (Random.value > 0.5f)
			z = -z;
		transform.position = new Vector3 (x+targetPlayer.transform.position.x , transform.position.y,z+targetPlayer.transform.position.z);
	}

	//get the next step of the path
	private void GetNewPosition(){
		//follow player path
		if (playerPath.Count > 0) {
			currentTarget = playerPath [0];
			playerPath.RemoveAt (0);
			if (playerPath.Count == 0)
				stayCloseToThePlayer = Time.time + timeStayCloseToThePlayer;
		} 
		else {
			if(canReturnLastPos){
				//or come back on his steps
				if (reaperPath.Count > 0) {
					currentTarget = reaperPath [reaperPath.Count - 1];
					reaperPath.RemoveAt (reaperPath.Count - 1);
				} else {
					//follow the normal path
					if (listPaths.Count > 0) {
						currentTarget = (listPaths.Single (p => p.name == "Path" + index)).position;
						index = (index < listPaths.Count) ? index + 1 : 1;
					}
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
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(to.x, 0.0f, to.z), moveSpeed* Time.deltaTime);
	}
}