using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(UserPresenceComponent))]
[RequireComponent(typeof(GazeAwareComponent))]
public class ReaperController : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	private UserPresenceComponent _userPresenceComponent;
	private GameObject targetPlayer;

	private Vector3 dir;
	private Vector3 pos;
	private Vector3 temp;
	private Quaternion rotation;
	private float angleYOrigine;
	private float delayReaper ;
	private float delaySpeedReaper = 0.0F; 
	private float delayPlayer ; 

	private List<Transform> listPaths = new List<Transform>();
	private List<Vector3> reaperPath = new List<Vector3>();
	private List<Vector3> playerPath = new List<Vector3>();

	private bool canReturnLastPos = false;
	private int index = 1;
	private bool walk = false;
	private bool followPlayer;
	private Vector3 currentTarget;

	public Transform pathToFollow;
	[SerializeField][Range(0.1F, 5.0F)] public float lumThreshold;
	[SerializeField][Range(0.1F, 10.0F)] public float moveSpeed;
	[SerializeField][Range(0.0F, 10.0F)] public float detection;
	[SerializeField][Range(0.1F, 3.0F)] public float rotationSpeed;
	[SerializeField][Range(0.0F, 60.0F)] public float minWaitTime;
	[SerializeField][Range(0.0F, 60.0F)] public float maxWaitTime;
	[SerializeField][Range(1.0F, 5.0F)] public float timeDropPathReaper;
	[SerializeField][Range(1.0F, 5.0F)] public float timeDropPathPlayer;
	[SerializeField][Range(0.5F, 2.0F)] public float timeToIncreamentSpeedOfTheReaper;


	public void Start(){

		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_userPresenceComponent = GetComponent<UserPresenceComponent> ();
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		angleYOrigine = transform.eulerAngles.y;

		delayReaper = delayPlayer = Time.time;
		followPlayer = false;

		//test if the field is not empty
		if(pathToFollow == null){
			Debug.LogError("A GameObject 'Path' needed in 'FollowPath.cs'.");
		} else {
			GetPaths();
			if(listPaths.Count > 0) GetNewPosition();
		}
	}
	

	public void Update(){
		//the player is looking the reaper
		if (_gazeAwareComponent.HasGaze) {
			delaySpeedReaper +=0.05F;
			followPlayer = true;
			playerPath.Clear ();
			canReturnLastPos = true;
			faceTargetAt (targetPlayer.transform.position);
			//the reaper continues to get closer to the player
			moveTowardTarget (targetPlayer.transform.position);
			//the player take to long for change his focus
			if (delaySpeedReaper>timeToIncreamentSpeedOfTheReaper) {
				print ("je t'ai eu");
			}
			//save the first position of the reaper before the player look at him
			if (reaperPath.Count == 0) {
				reaperPath.Add (currentTarget);
				setReaperPath ();
			}
			//save the player position
			if (playerPath.Count == 0) {
				setPlayerPath ();
			}
		} 
		//follow the path or the steps of the reaper
		else {
			delaySpeedReaper = 0.0F;
			if (canReturnLastPos) {
				GetNewPosition ();
				canReturnLastPos = false;
			}
			if (walk) {
				StartWalk ();
			}
		}
		if (_userPresenceComponent.IsUserPresent && followPlayer) {
			if (Time.time > delayPlayer) {
				setPlayerPath ();
				delayPlayer = Time.time + timeDropPathPlayer;
			}
		}
		if(Time.time > delayReaper && playerPath.Count >0){
			setReaperPath();
			delayReaper = Time.time + timeDropPathReaper;
		}
		//the reaper is near of the target (here the player)
		if (closeToTheTarget (detection,targetPlayer.transform.position) && Flashlight.Instance.lum.intensity > lumThreshold) {
			jumpToPos(Camera.main);
		} 
	}

	private void jumpToPos(Camera cam){
		Vector3 pos = cam.transform.position;
		pos.z += cam.nearClipPlane + 5.0f;
		transform.position = pos;
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

	//walk forward the target
	void StartWalk(){
		moveTowardTarget(currentTarget);
		faceTargetAt(currentTarget);
		//the target is met
		if( closeToTheTarget(2f, currentTarget)){
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

	//get the next step of the path
	void GetNewPosition(){
		//follow player path
		if (playerPath.Count > 0) {
			currentTarget = playerPath [0];
			playerPath.RemoveAt (0);
		} 
		else {
			followPlayer = false;
			//or come back on his steps
			if (reaperPath.Count > 0) {
				currentTarget = reaperPath [reaperPath.Count - 1];
				reaperPath.RemoveAt (reaperPath.Count - 1);
			} 
			else {
				//follow the normal path
				if (listPaths.Count > 0) {
					currentTarget = (listPaths.Single (p => p.name == "Path" + index)).position;
					index = (index < listPaths.Count) ? index + 1 : 1;
				}
			}
		}
		StartCoroutine (Wait ());
	}

	//look the target (player or next step of the path)
	private void faceTargetAt(Vector3 at){
		dir = (at - transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (dir.x, angleYOrigine, dir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
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
	//increase speed speed of the monster
	private void increaseSpeed(){
		
	}

	//move forward the position of the "vector3 to"
	private void moveTowardTarget(Vector3 to){
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(to.x, 0.0f, to.z), moveSpeed* Time.deltaTime);
	}
}