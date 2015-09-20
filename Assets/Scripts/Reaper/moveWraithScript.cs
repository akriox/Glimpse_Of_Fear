using UnityEngine;
using System.Collections;

public class moveWraithScript : MonoBehaviour {

	//public static moveWraithScripte Instance {get; private set;}

	private Vector3 direction;
	private Vector3 currentTarget;
	private Quaternion rotation;

 	private float rotationSpeed = 3.0f;

	private static Vector3 positionStart;
	private static Vector3 positionEnd;

	private static bool ready;
	private Renderer _renderer;
	private GameObject wraith;

	public static float moveSpeed = 1.0f;

	private GameObject _player;
	private bool walk;

	public void Awake(){
		//Instance = this;
	}

	// Use this for initialization
	void Start () {
		ready = false;
		_renderer = GetComponent<Renderer> ();
		_renderer.enabled = false;
		wraith = this.gameObject.transform.parent.gameObject;
		_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (ready) {
			_renderer.enabled = true;
			wraith.transform.position = positionStart;
			moveTowardTarget (positionEnd);
			walk = true;
			ready = false;

		}
		if (walk) {
			moveTowardTarget (positionEnd);
			if(nearPlayer(2.5f)){
				_renderer.enabled = false;
			}
		}

	}

	private bool nearPlayer(float dist){
		if (Vector3.Distance(transform.position,_player.transform.position) < dist) return true;
		return false;	
	}

	public static void setPosition(Vector3 s, Vector3 e){
		positionStart = s;
		positionEnd = e;
		ready = true;
	}

	//face the target
	private void faceTarget(Vector3 to){
		direction = (to - wraith.transform.position).normalized;
		rotation = Quaternion.LookRotation (new Vector3 (direction.x, 0, direction.z));
		wraith.transform.rotation = Quaternion.Slerp (wraith.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}

	//move forward and face the position of the "vector3 to"
	private void moveTowardTarget(Vector3 to){
		faceTarget (to);
		wraith.transform.position = Vector3.MoveTowards(wraith.transform.position, new Vector3(to.x, to.y, to.z), moveSpeed* Time.deltaTime);
		if (Vector3.Distance(wraith.transform.position, to) <= 0.5f) {
			_renderer.enabled = false;
			walk = false;
		}
	}

	public static void setSpeed(float speed){
		moveSpeed = speed;
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Flashlight.Instance.drainBattery();
		}
	}
}