using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GazePointDataComponent))]
public class EyeLook : MonoBehaviour {

	[SerializeField][Range(-60.0f, 60.0f)] public float minAngle;
	[SerializeField][Range(-60.0f, 60.0f)] public float maxAngle;

	public static bool isActive = true;

	private GazePointDataComponent _gazePointDataComponent;
	private GameObject player;
	private Vector3 gazePointScreen;
	private Vector3 gazePointWorld;
	private Quaternion targetRotation;
	private Quaternion lastRotation;
	private float velocity;

	private float w;
	private float h;
	private float ratio;
	private int offset = 80;
	private Rect[] r;

	public void Start () {
	
		_gazePointDataComponent = GetComponent<GazePointDataComponent>();
		player = GameObject.FindGameObjectWithTag("Player");

		w = Screen.width;
		h = Screen.height;
		ratio = w/h;
		r = new Rect[5];

		int i;
		for(i=0; i<r.Length; i++){
			r[i] = new Rect( (i+1)*offset*ratio, (i+1)*offset, w-ratio*offset*(i+1)*2, h-offset*(i+1)*2);
		}
	}

	public void Update(){
		lastRotation = player.transform.rotation;
	}
	
	public void LateUpdate(){
		if(isActive){
			player.transform.rotation = lastRotation;
			var gazePoint = _gazePointDataComponent.LastGazePoint;
			if(gazePoint.IsValid && gazePoint.IsWithinScreenBounds){
				centerCamera(gazePoint.Screen);
			}
		}
	}

	private void centerCamera(Vector2 point){
		velocity = getVelocity(point, 0);
		gazePointScreen = new Vector3(point.x, point.y, Camera.main.nearClipPlane);
		gazePointWorld = Camera.main.ScreenToWorldPoint(gazePointScreen);
		targetRotation = Quaternion.LookRotation(gazePointWorld - transform.position);
		player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * velocity);

		player.transform.rotation = ClampRotationXAxis(player.transform.rotation);
	}

	private float getVelocity(Vector2 point, int n){
		if(n < r.Length && r[n].Contains(point)){
			return getVelocity(point, n+1);
		}
		else{
			return Settings.EyeXSensitivity - (n*0.1f);
		}
	}

	private Quaternion ClampRotationXAxis(Quaternion q){
		
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;
		
		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
		angleX = Mathf.Clamp (angleX, minAngle, maxAngle);
		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
		
		return q;
	}

	/* DEBUG DISPLAY RECT
	public void OnGUI() {
		GUI.Box(r[4], "");
		GUI.Box(r[3], "");
		GUI.Box(r[2], "");
		GUI.Box(r[1], "");
		GUI.Box(r[0], velocity.ToString());
	}
	*/
}