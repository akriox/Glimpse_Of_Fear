using UnityEngine;
using System.Collections;

public class moveWraithCave2 : MonoBehaviour {

	//public static moveWraithCave2 Instance {get; private set;}

	private Vector3 direction;
	private Vector3 currentTarget;
	private Quaternion rotation;
	private float angleYOrigine;

 	private float rotationSpeed = 3.0f;

	private static Vector3 positionStart;
	private static Vector3 positionEnd;

	private static bool ready;
	private bool walk;
	private Renderer _renderer;

	public static float moveSpeed = 1.0f;

	public void Awake(){
		//Instance = this;
	}

	// Use this for initialization
	void Start () {
		//save wraith Y angle and his original position
		angleYOrigine = transform.eulerAngles.y;
		ready = false;
		walk = false;
		_renderer = GetComponent<Renderer> ();
		_renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (ready) {
			_renderer.enabled = true;
			transform.position = positionStart;
			walk = true;
			ready = false;
		}
		if (walk) {
			moveTowardTarget (positionEnd);
		}
	}

	public static void setPosition(Vector3 s, Vector3 e){
		positionStart = s;
		positionEnd = e;
		ready = true;
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
		if (Vector3.Distance(transform.position, to) <= 0.5f) {
			walk = false;
			_renderer.enabled = false;
		}
	}

	public static void setSpeed(float speed){
		moveSpeed = speed;
	}
}
