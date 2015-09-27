using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(UserPresenceComponent))]
[RequireComponent(typeof(GazeAwareComponent))]
public class ReaperController_V4 : MonoBehaviour {
	
	private GazeAwareComponent _gazeAwareComponent;
	private UserPresenceComponent _userPresenceComponent;
	private GameObject targetPlayer;
	private GameObject _parent;

	private List<Transform> listPaths = new List<Transform>();
	private Vector3 direction;
	private Vector3 direction2;
	private Vector3 currentTarget;

	private Vector3 posPlayer;
	private Quaternion rotation;
	private Quaternion rotation2;

	private float stayCloseToThePlayer;
	private float delayToHaveSomething;
	private float timeToReset;
	private int index = 1;

	private bool isPlayerInArea = false;
	private bool _playerOpenEyes;
	private bool _jumpScare = false;
	
	private Renderer myRenderer;

	private NavMeshAgent _wraith;
	private Animator _animator;

	private int angleLookWraith;
	private int positionPlayerWraith;

	private Vector3 _positionPlayerEntrance = new Vector3 (-122.23f, -14.6f, 2.78f);
	private Vector3 _positionPlayerTablet = new Vector3 (-195.46f, -6.586f, 2.78f);
	
	[SerializeField] private Transform pathToFollow;
	[SerializeField] private Transform pathToFollow2;
	[SerializeField][Range(0.1F, 5.0F)] private float lumThreshold;
	[SerializeField][Range(1.0F, 10.0F)] private float initSpeedReaper;
	[SerializeField][Range(8.0F, 15.0F)] private float areaMaxPlayerDetection;
	[SerializeField][Range(0.1F, 5.0F)] private float minWaitTime;
	[SerializeField][Range(0.1F, 10.0F)] private float maxWaitTime;
	[SerializeField][Range(0.01F, 0.1F)] private float increamentSpeedOfTheReaper;      
	[SerializeField] private bool randomMove;

	private float rotationSpeed = 6f;

	private AudioSource _audio; 
	private GameObject jumpScare;
	
	private enum State{moveWraith, PlayerIsLooking, FollowPlayer, PlayerInArea, PlayerClose, BeNothing, WraithCatchPlayer, PlayerLost, comeBack, Wait, No};
	private State _state;
	
	private int _IdleDiff = Animator.StringToHash("IdleDiff");
	private int _CatchDiff = Animator.StringToHash("CatchDiff");
	private int _LookDiff = Animator.StringToHash("LookDiff");
	private int _LookBehindDiff = Animator.StringToHash("LookBehindDiff");
	private int _RunDiff = Animator.StringToHash("RunDiff");
	
	private int _Walk = Animator.StringToHash("Walk");
	private int _Run = Animator.StringToHash("Run");
	private int _Catch = Animator.StringToHash("Catch");
	private int _CatchFinal1 = Animator.StringToHash("CatchFinal1");
	private int _CatchFinal2 = Animator.StringToHash("CatchFinal2");
	private int _Look = Animator.StringToHash("Look");
	private int _LookBehind = Animator.StringToHash("LookBehind");
	private int _Crawl = Animator.StringToHash("Crawl");
	private int _WalkAround = Animator.StringToHash("WalkAround");
	
	private AudioClip _walkSound;
	private AudioClip _runSound;
	private AudioClip _turnAroundSound;
	private AudioClip _goAwaySound;
	private AudioClip _lookSound;

	public void Start(){
		//get component for eye tracker
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_userPresenceComponent = GetComponent<UserPresenceComponent> ();
		//get the gameObject player
		targetPlayer = GameObject.FindGameObjectWithTag("Player");
		myRenderer = GetComponent<Renderer> ();
		_wraith = GetComponentInParent<NavMeshAgent> ();
		_parent = transform.parent.gameObject;
		_wraith.speed = initSpeedReaper;
		_animator = GetComponentInParent<Animator> ();
		jumpScare = GameObject.FindGameObjectWithTag("WraithJumpScare");
		jumpScare.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;
		jumpScare.SetActive (false);
		_audio = GetComponent<AudioSource> ();

		_walkSound = (AudioClip)Resources.Load ("Audio/Wraith/Reaper_faraway", typeof(AudioClip));
		_runSound = (AudioClip)Resources.Load ("Audio/Wraith/Reaper_catch02", typeof(AudioClip));
		_turnAroundSound = (AudioClip)Resources.Load ("Audio/Wraith/Reaper_turnaround01_loop", typeof(AudioClip));
		_goAwaySound = (AudioClip)Resources.Load ("Audio/Wraith/Reaper_disappear01", typeof(AudioClip));
		_lookSound = (AudioClip)Resources.Load ("Audio/Wraith/Giant_SE17", typeof(AudioClip));

		
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

		_playerOpenEyes = _userPresenceComponent.GazeTracking == EyeXGazeTracking.GazeTracked ? true : false;

		if (_playerOpenEyes && AreaJumpScare.isPlayerInAreaForJumpScare && !_jumpScare) {
			_jumpScare = true; 
			StopAllCoroutines ();
			_state = State.No;
			playJumpScare();
		}
		/*
		if (_playerOpenEyes && AreaJumpScare.isPlayerInAreaForJumpScare && Flashlight.Instance.lum.intensity > lumThreshold) {
			playJumpScare();
		}
		*/
		switch(_state){
		case State.moveWraith:
			    //Initial state of the wraith (patrol)
			    if (_gazeAwareComponent.HasGaze) {
				    _wraith.Stop();
				    chooseTheCorrectLook();
				    StartCoroutine(WaitBeforeAttak());
				    EventSound.playClip(_lookSound);
				    _state = State.PlayerIsLooking;
			    }
			    StartWalk (currentTarget);
                if (isPlayerInArea)
                {
                    Walk();
                    _wraith.enabled = false;
                    _state = State.PlayerInArea;
                }
			break;

		case State.PlayerIsLooking:
			    if (!_playerOpenEyes) {
				    _wraith.Resume();
				    StopAllCoroutines();
				    Walk ();
				    _state = State.moveWraith;
			    }
			break;


		case State.PlayerLost:
			flyTowardTarget(currentTarget, true);
			if( closeToTheTarget(4f, currentTarget)){
				StartCoroutine(Wait());
				ChooseIdle(1f);
				_state = State.Wait;
			}
			if (_gazeAwareComponent.HasGaze) {
				ChooseRun();
				_state = State.FollowPlayer;
			}

		break;

		case State.Wait:
			if (_gazeAwareComponent.HasGaze) {
				StopAllCoroutines();
				ChooseRun();
				_state = State.FollowPlayer;
			}
			break;

		case State.comeBack:
			flyTowardTarget(currentTarget, false);
			if( closeToTheTarget(1f, currentTarget)){
				_wraith.transform.position = currentTarget;
				GetNewPosition();
				_wraith.enabled = true;
				Walk();
				_state = State.moveWraith;
			}
			if (_gazeAwareComponent.HasGaze) {
				ChooseRun();
				_state = State.FollowPlayer;
			}
			break;

		case State.FollowPlayer:
			//State where the wraith is following the player
			if (_gazeAwareComponent.HasGaze) {
				_wraith.speed += increamentSpeedOfTheReaper;
			}
			//PlayLookSound ();
			//the player close his eyes so the wraith don't continue to get the current position of the player
			if (!_playerOpenEyes) {
				_wraith.speed = initSpeedReaper;
				currentTarget = targetPlayer.transform.position;
				Walk ();
				_state = State.PlayerLost;
			}
			flyTowardTarget(targetPlayer.transform.position, true);

			break;

		case State.PlayerInArea:
			//State where the player is in the wraith area detection
			if (_gazeAwareComponent.HasGaze) {
				_state = State.FollowPlayer;
			}
            flyTowardTarget(targetPlayer.transform.position, true);
            //PlayCloseSound ();
            if (AreaJumpScare.isPlayerInAreaForJumpScare) {
		        posPlayer = targetPlayer.transform.position;
                StartCoroutine(Wait());
			    _wraith.enabled = false;
			    _state = State.PlayerClose;
			} 


			break;

		case State.PlayerClose:
			//the wraith has detected the player so he stop and stay close to him during a amount of time
			//faceTarget (targetPlayer.transform.position);
			turnAround ();
			if (!AreaJumpScare.isPlayerInAreaForJumpScare) {
                StopAllCoroutines();
				_state = State.PlayerInArea;
			}
			break;

		case State.WraithCatchPlayer:
			//the wraith caught the player so after a certain amount of time reset the position and state of the wraith

			//CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.INC);
			jumpScare.SetActive (false);
			jumpScare.GetComponent<AudioSource> ().Stop ();
			if(Inventory.Instance.hasTablet)
				setPlayerTransform(_positionPlayerTablet);
			else{
				setPlayerTransform(_positionPlayerEntrance);
			}
			_state = State.BeNothing;
			break;

		case State.BeNothing:
			//the wraith do nothing
			GetNewPosition();
			_wraith.transform.position = currentTarget;
			_wraith.enabled = true;
			myRenderer.enabled = true;
			_jumpScare = false; 
			_wraith.speed = initSpeedReaper;
			EyeLook.isActive = true;
			FirstPersonController.canMove = true;
			CameraController.Instance.fadeToClear(2.0f);
			Walk();
			_state = State.moveWraith;
			break;

		case State.No:
			break;
		}
	}

	public void setPlayerTransform(Vector3 position){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = position;
	}

	private void playJumpScare(){
		//launch the jumpscare
		EyeLook.isActive = false;
		FirstPersonController.canMove = false;
		jumpScare.SetActive (true);
		jumpScare.GetComponent<AudioSource> ().Play ();
		ChooseCatch ();
		StartCoroutine (WaitAfterCatch ());
		myRenderer.enabled = false;
	}


	private void turnAround(){
        //the wraith is turning around the player
        WalkAround();
        _parent.transform.RotateAround (posPlayer, Vector3.down, 90 * Time.deltaTime);
	}
	
	//put initial information for the reaper
	void setInitialInformation(){
		jumpScare.SetActive(false);
		isPlayerInArea = false;
		GetNewPosition();
		Walk();
		//StopCloseSound();
		//StopLookSound();
	}
	
	//get the normal path of the reaper
	void GetPaths(){
		foreach(Transform temp in pathToFollow){
			listPaths.Add(temp);
		}
	}


	private void playSound(AudioClip _clip, bool _loop){
		if (_audio.isPlaying)
			StopSound ();
		_audio.clip = _clip;
		_audio.loop = _loop;
		_audio.Play ();
	}

	private void StopSound(){
		_audio.Stop ();
	}

	//walk forward the target
	private void StartWalk(Vector3 targ){
		moveTowardTarget(targ);
		//the target is met
		if( closeToTheTarget(3f, targ)){
            GetNewPosition();
        }
	}

	//the monster can take time before walk to the next step
	IEnumerator WaitAfterCatch(){
		StartCoroutine (CameraController.Instance.Shake (1.0f, 0.5f, 1.5f));
		StartCoroutine (GameController.Instance.timedVibration (0.6f, 0.6f, 1.0f));
		yield return new WaitForSeconds(0.5f);
		CameraController.Instance.fadeToBlack(2.0f);
		yield return new WaitForSeconds(1.2f);
		_state = State.WraithCatchPlayer;
	}

	//the monster can take time before walk to the next step
	IEnumerator WaitBeforeAttak(){
		yield return new WaitForSeconds(0.7f);
		//_wraith.Resume();
		_wraith.enabled = false;
		ChooseRun();
		_state = State.FollowPlayer;
	}

	//the monster can take time before walk to the next step
	IEnumerator Wait(){
		yield return new WaitForSeconds(3.2f);
		Walk ();
		GetNewPosition();
		EventSound.playClip (_goAwaySound);
		_state = State.comeBack;
	}
	
	//get the next step of the path
	private void GetNewPosition(){
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

	public void chooseTheCorrectLook(){
		angleLookWraith = Mathf.FloorToInt(rotation.eulerAngles.y);
		direction2 = (_wraith.transform.position -targetPlayer.transform.position).normalized;
		rotation2 = Quaternion.LookRotation (direction2);
		positionPlayerWraith = Mathf.FloorToInt((rotation2.eulerAngles.y +180f));
		positionPlayerWraith %= 360;
		positionPlayerWraith += (360 -angleLookWraith);
		positionPlayerWraith %= 360;
		if (positionPlayerWraith > 30) {
			if(positionPlayerWraith > 120){
				if(positionPlayerWraith > 180){
					if(positionPlayerWraith > 240){
						if(positionPlayerWraith > 330){
							ChooseIdle(0f);
							return;
						}
						ChooseLook(0f);
						return;
					}
					ChooseLookBehind(0f);
					return;
				}
				ChooseLookBehind(1f);
				return;
			}
			ChooseLook(1f);
			return;
		}
		ChooseIdle(0f);
	}
	
	//return true if the monster is close to the target 
	private bool closeToTheTarget(float dist, Vector3 target){
		if (Vector3.Distance(_wraith.transform.position, target) < dist) {
			return true;
		}
		else {
			return false;
		}
	}
	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - _wraith.transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		_wraith.transform.rotation = Quaternion.Slerp (_wraith.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}
	
	//move forward and face the position of the "vector3 to"
	private void moveTowardTarget(Vector3 to){
		faceTarget (to);
		//_parent.transform.position = Vector3.MoveTowards(_wraith.transform.position, new Vector3(to.x, to.y, to.z), moveSpeed* Time.deltaTime);
		_wraith.SetDestination(to);
	}
	//move forward and face the position of the "vector3 to"
	private void flyTowardTarget(Vector3 to, bool up){
		faceTarget (to);
		if (up)
			_parent.transform.position = Vector3.MoveTowards (_parent.transform.position, new Vector3 (to.x, to.y - 2f, to.z), _wraith.speed * Time.deltaTime);
		else {
			_parent.transform.position = Vector3.MoveTowards (_parent.transform.position, new Vector3 (to.x, to.y, to.z), _wraith.speed * Time.deltaTime);
		}
	}


	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			isPlayerInArea = true;
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			isPlayerInArea = false;
		}
	}

	public void Idle(){
		_animator.SetBool(_Walk, false);
	}
	
	public void Walk(){
		playSound (_walkSound, true);
		_animator.SetBool(_Walk, true);
		_animator.SetBool(_Run, false);
		_animator.SetBool(_WalkAround, false);
	}
	
	public void WalkAround(){
		playSound (_turnAroundSound, true);
		_animator.SetBool(_WalkAround, true);
		_animator.SetBool(_Run, false);
		
	}
	
	public void Run(){
		playSound (_runSound, true);
		_animator.SetBool(_Walk, true);
		_animator.SetBool(_Run, true);
	}
	
	public void Crawl(){
		_animator.SetBool(_Walk, true);
		_animator.SetBool(_Crawl, true);
	}
	
	public void LookBehind(){
		_animator.SetBool(_Walk, false);
		_animator.SetTrigger(_LookBehind);
	}
	
	public void Look(){
		_animator.SetBool(_Walk, false);
		_animator.SetTrigger(_Look);
	}
	
	public void Catch(){
		jumpScare.GetComponent<Animator> ().SetTrigger(_Catch);
	}
	
	public void CatchFinal(){
		_animator.SetTrigger(_CatchFinal1);
	}
	
	public void CatchFinalEnd(){
		_animator.SetTrigger(_CatchFinal2);
	}
	
	public void ChooseIdle(float f){
		_animator.SetFloat(_IdleDiff, f);
		Idle ();
	}

	public void ChooseCatch(){
		jumpScare.GetComponent<Animator> ().SetFloat(_CatchDiff, giveRandomAnim());
		Catch ();
	}
	
	public void ChooseLook(float f){
		_animator.SetFloat(_LookDiff, f);
		Look ();
	}
	
	public void ChooseLookBehind(float f){
		_animator.SetFloat(_LookBehindDiff, f);
		LookBehind ();
	}
	
	public void ChooseRun(){
		_wraith.speed = 7f;
		_animator.SetFloat(_RunDiff, giveRandomAnim());
		Run ();
	}

	public float giveRandomAnim(){
		float f = 0.0f;
		if (Random.Range (0, 2) == 1)
			f = 1.0f;
		return f;
	}
}