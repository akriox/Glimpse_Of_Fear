using UnityEngine;
using System.Collections;

public class moveReaperBigRoom : MonoBehaviour {

	public static moveReaperBigRoom Instance {get; private set;}

	private Vector3 direction;
	private Vector3 currentTarget;
	private Quaternion rotation;
	private float angleYOrigine;

 	private float rotationSpeed = 3.0f;

	private Vector3 positionStart;
	private Vector3 positionEnd;

	private bool ready;
	private bool walk;
	private Renderer _renderer;

	[SerializeField][Range(1.0F, 15.0F)] public float moveSpeed = 10.0f;

	public void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		//save reaper Y angle and his original position
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

	float CheckDistance(Vector3 to){
		return Vector3.Distance(transform.position, to);
	}

	public void setPosition(Vector3 s, Vector3 e){
		this.positionStart = s;
		this.positionEnd = e;
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
		if (CheckDistance (to) <= 0.5f) {
			walk = false;
			_renderer.enabled = false;
		}
	}
}
