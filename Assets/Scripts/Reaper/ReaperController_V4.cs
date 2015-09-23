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
	public GameObject pointToLook;
	public GameObject _test;
	private float angleYOrigine;
	
	private List<Transform> listPaths = new List<Transform>();
	private Vector3 direction;
	private Vector3 currentTarget;

	private Vector3 posPlayer;
	private Quaternion rotation;

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
	
	[SerializeField] private Transform pathToFollow;
	[SerializeField][Range(0.1F, 5.0F)] private float lumThreshold;
	[SerializeField][Range(1.0F, 10.0F)] private float initSpeedReaper;
	[SerializeField][Range(0.1F, 3.0F)] private float rotationSpeed;
	[SerializeField][Range(8.0F, 15.0F)] private float areaMaxPlayerDetection;
	[SerializeField][Range(0.1F, 5.0F)] private float minWaitTime;
	[SerializeField][Range(0.1F, 10.0F)] private float maxWaitTime;
	[SerializeField][Range(0.5F, 1.0F)] private float timeDropPathPlayer;
	private float timeStayCloseToThePlayer = 12f;
	[SerializeField][Range(0.01F, 0.1F)] private float increamentSpeedOfTheReaper;
	[SerializeField] private AudioSource m_LookSound;          // the sound when the player had look the reaper
	[SerializeField] private AudioSource m_ReaperClose;        // the sound when the reaper is close to the player
	[SerializeField] private AudioSource atmosphere;        
	[SerializeField] private GameObject jumpScare;
	[SerializeField] private bool randomMove;
	[SerializeField] private GameObject newPositionPlayer;
	
	
	private enum State{moveWraith, PlayerIsLooking, FollowPlayer, PlayerInArea, PlayerClose, BeNothing, WraithCatchPlayer};
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
		angleYOrigine = _test.transform.eulerAngles.y;
		_animator = GetComponentInParent<Animator> ();
		LoadingScreen.Instance.fadeToClear ();
		
		
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
			Walk();
			if (_gazeAwareComponent.HasGaze) {
				_wraith.Stop();
				StartCoroutine(WaitBeforeAttak());
				_state = State.PlayerIsLooking;
			}
			StartWalk (currentTarget);
			if (isPlayerInArea)_state = State.PlayerInArea;
			break;

		case State.PlayerIsLooking:
			//Initial state of the wraith (patrol)
			Look();
			if (!_gazeAwareComponent.HasGaze) {
				_wraith.Resume();
				StopAllCoroutines();
				_state = State.moveWraith;
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
				_state = State.moveWraith;
			}
			moveTowardTarget(targetPlayer.transform.position);

			break;

		case State.PlayerInArea:
			//State where the player is in the wraith area detection
			if (_gazeAwareComponent.HasGaze) {
				_state = State.FollowPlayer;
			}
			Walk();
			moveTowardTarget(targetPlayer.transform.position);
			//PlayCloseSound ();
			if (AreaJumpScare.isPlayerInAreaForJumpScare) {
				posPlayer = targetPlayer.transform.position;
				stayCloseToThePlayer = Time.time + timeStayCloseToThePlayer;
				_wraith.enabled = false;
				_state = State.PlayerClose;
			} 


			break;

		case State.PlayerClose:
			//the wraith has detected the player so he stop and stay close to him during a amount of time
			//faceTarget (targetPlayer.transform.position);
			WalkAround();
			turnAround ();
			if (!AreaJumpScare.isPlayerInAreaForJumpScare) {
				_wraith.enabled = true;
				_state = State.PlayerInArea;
			}

			if (Time.time > stayCloseToThePlayer) {
				_wraith.enabled = true;
				setInitialInformation ();
				_state = State.moveWraith;
			}
			break;

		case State.WraithCatchPlayer:
			//the wraith caught the player so after a certain amount of time reset the position and state of the wraith

			//CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.INC);
			jumpScare.SetActive (false);
			setPlayerTransform(newPositionPlayer.transform.position);
				
			_state = State.BeNothing;
			/*
			_wraith.transform.position = (listPaths.Single (p => p.name == "Path" + index)).position;
			_wraith.transform.position = new Vector3(_wraith.transform.position.x,0.0f,_wraith.transform.position.z);
			myRenderer.enabled = true;
			setInitialInformation();
			_state = State.moveWraith;
			*/

			break;

		case State.BeNothing:
			//the wraith do nothing
			GetNewPosition();
			_wraith.transform.position = currentTarget;
			_wraith.enabled = true;
			myRenderer.enabled = true;
			_jumpScare = false; 
			_wraith.speed = initSpeedReaper;
			LoadingScreen.Instance.fadeToClear();
			_state = State.moveWraith;
			break;

		}
	}

	public void setPlayerTransform(Vector3 position){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = position;
	}

	private void playJumpScare(){
		//launch the jumpscare
		jumpScare.SetActive (true);
		ChooseCatch ();
		StartCoroutine (WaitAfterCatch ());
		myRenderer.enabled = false;
	}


	private void turnAround(){
		//the wraith is turning around the player
		_parent.transform.RotateAround (posPlayer, Vector3.down, 30 * Time.deltaTime);
	}
	
	//put initial information for the reaper
	void setInitialInformation(){
		jumpScare.SetActive(false);
		isPlayerInArea = false;
		GetNewPosition();
		//StopCloseSound();
		//StopLookSound();
	}
	
	//get the normal path of the reaper
	void GetPaths(){
		foreach(Transform temp in pathToFollow){
			listPaths.Add(temp);
		}
	}


	/*
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

	*/

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
		LoadingScreen.Instance.fadeBlack ();
		//CameraController.Instance.setFadeState (CameraController.FadeState.OUT, 10f);
		yield return new WaitForSeconds(1.2f);
		_state = State.WraithCatchPlayer;
	}

	//the monster can take time before walk to the next step
	IEnumerator WaitBeforeAttak(){
		yield return new WaitForSeconds(0.5f);
		_wraith.Resume();
		ChooseRun();
		_state = State.FollowPlayer;
	}

	//the monster can take time before walk to the next step
	IEnumerator Wait(){
		float time = Random.Range(minWaitTime, maxWaitTime);
		yield return new WaitForSeconds(time);
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
		StartCoroutine (Wait ());
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
		_animator.SetBool(_Walk, true);
		_animator.SetBool(_Run, false);
		_animator.SetBool(_WalkAround, false);
	}
	
	public void WalkAround(){
		_animator.SetBool(_WalkAround, true);
		_animator.SetBool(_Run, false);
		
	}
	
	public void Run(){
		print ("coucou");
		_animator.SetBool(_Walk, true);
		_animator.SetBool(_Run, true);
	}
	
	public void Crawl(){
		_animator.SetBool(_Walk, true);
		_animator.SetBool(_Crawl, true);
	}
	
	public void LookBehind(){
		_animator.SetTrigger(_LookBehind);
	}
	
	public void Look(){
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