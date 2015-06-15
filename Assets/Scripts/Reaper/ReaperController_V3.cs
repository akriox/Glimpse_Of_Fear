using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(UserPresenceComponent))]
[RequireComponent(typeof(GazeAwareComponent))]
public class ReaperController_V3 : MonoBehaviour {
	
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
	
	private bool walk; 
	private bool followPlayer;
	private bool playerClose;
	private bool haveJumpScare;
	private bool canBeHaveSomething = true;
	private bool playerHasBeenSeen;
	
	private Renderer myRenderer;
	private float areaMinPlayerDetection = 3f;

	[SerializeField] private Transform pathToFollow;
	[SerializeField][Range(0.1F, 5.0F)] public float lumThreshold;
	[SerializeField][Range(1.0F, 10.0F)] public float initSpeedReaper;
	[SerializeField][Range(0.1F, 3.0F)] public float rotationSpeed;
	[SerializeField][Range(8.0F, 15.0F)] public float areaMaxPlayerDetection;
	[SerializeField][Range(0.1F, 5.0F)] public float minWaitTime;
	[SerializeField][Range(0.1F, 10.0F)] public float maxWaitTime;
	[SerializeField][Range(0.5F, 1.0F)] public float timeDropPathPlayer;
	[SerializeField][Range(5.0F, 10.0F)] public float timeStayCloseToThePlayer;
	[SerializeField][Range(0.01F, 0.1F)] public float increamentSpeedOfTheReaper;
	[SerializeField] private AudioSource m_LookSound;          // the sound when the player had look the reaper
	[SerializeField] private AudioSource m_ReaperClose;        // the sound when the reaper is close to the player
	[SerializeField] private AudioSource atmosphere;        
	[SerializeField] private GameObject jumpScare;
	public bool randomMove;
	
	
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
		moveReaper ();
		//the reaper caught the player
		if (haveJumpScare) {
			if(Time.time > timeToReset){
				transform.position = (listPaths.Single (p => p.name == "Path" + index)).position;
				transform.position = new Vector3(transform.position.x,0.0f,transform.position.z);
				myRenderer.enabled = true;
				canBeHaveSomething = false;
				delayToHaveSomething = Time.time + 3.0f;
				setInitialInformation();
			}
		}
		if(!canBeHaveSomething && Time.time > delayToHaveSomething)
			canBeHaveSomething = true;
	}

	private void detectPlayer(){
		//the player is near of the reaper. The reaper is moving towards the player
		if (closeToTheTarget (areaMaxPlayerDetection, targetPlayer.transform.position) && canBeHaveSomething) {
			if (closeToTheTarget (areaMinPlayerDetection, targetPlayer.transform.position)) {
				if (!playerClose) {
					playerClose = true;
					stayCloseToThePlayer = Time.time + timeStayCloseToThePlayer;
					walk = false;
				}
				moveAround ();
				//the reaper has detected the player so he stop and stay close to him during a amount of time
				if (playerClose && Time.time > stayCloseToThePlayer) {
					setInitialInformation ();
					canBeHaveSomething = false;
					delayToHaveSomething = Time.time + 3.0f;
				}
			} 
			else {
				setPlayerPath ();
				getPlayerPath ();
				playerClose = false;
				walk = true;
			}
			faceTarget (targetPlayer.transform.position);
			PlayCloseSound ();
			//the reaper is near of the target (here the player)
			if (_userPresenceComponent.IsUserPresent && Flashlight.Instance.lum.intensity > lumThreshold && !haveJumpScare) {
				jumpScare.SetActive (true);
				haveJumpScare = true;
				timeToReset = Time.time + 2.0f;
				myRenderer.enabled = false;
			}
		}
	}

	private void moveReaper(){
		//the player is looking the reaper
		if (_gazeAwareComponent.HasGaze) {
			PlayLookSound ();
			setNewPositionAroundPlayer (Vector3.Distance (transform.position, targetPlayer.transform.position));
			if(!playerHasBeenSeen){
				playerHasBeenSeen = true;
				followPlayer = true;
				setPlayerPath ();
				index = (index > 1 ) ? index - 1 : listPaths.Count;
			}
			currentTarget = targetPlayer.transform.position;
		}
		//follow the path save (playerPath and normalPath)
		else {
			if(followPlayer && Time.time > delayPlayer) {
				setPlayerPath ();
			}
			if (walk) {
				StartWalk (currentTarget);
			}
		}
		detectPlayer ();
		//the player close his eyes so the reaper don't continue to get the current position of the player
		if (!_userPresenceComponent.IsUserPresent && followPlayer) {
			followPlayer = false;
			setPlayerPath();
		}
	}

	//the reaper is moving around the player
	private void moveAround(){
		transform.RotateAround (targetPlayer.transform.position, Vector3.up, 30 * Time.deltaTime);
	}

	//put initial information for the reaper
	void setInitialInformation(){
		jumpScare.SetActive(false);
		haveJumpScare = false;
		followPlayer = false;
		playerClose = false;
		playerHasBeenSeen = false;
		walk = false;
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
		if(playerHasBeenSeen)
			moveSpeed += increamentSpeedOfTheReaper;
		//the target is met
		if( closeToTheTarget(1f, targ)){
			walk = false;
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