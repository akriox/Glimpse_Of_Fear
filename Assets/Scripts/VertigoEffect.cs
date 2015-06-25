using UnityEngine;
using System.Collections;

public class VertigoEffect : MonoBehaviour {

	public Transform target;

	private bool vertigoEnabled;
	private Vector3 initPos;
	private float initFOV;
	private float initHeightAtDist;
	
	// Calculate the frustum height at a given distance from the camera.
	float FrustumHeightAtDistance(float distance) {
		return 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
	}
	
	// Calculate the FOV needed to get a given frustum height at a given distance.
	float FOVForHeightAndDistance(float height, float distance) {
		return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
	}

	void StartVertigo() {
		var distance = Vector3.Distance(transform.position, target.position);
		initHeightAtDist = FrustumHeightAtDistance(distance);
		vertigoEnabled = true;
	}

	void StopVertigo() {
		Camera.main.fieldOfView = initFOV;
		Camera.main.transform.position = initPos;
		vertigoEnabled = false;
	}
	
	public void Start() {
		initPos = Camera.main.transform.position;
		initFOV = Camera.main.fieldOfView;
	}
	
	public void Update () {
		if (vertigoEnabled){
			// Measure the new distance and readjust the FOV accordingly.
			var currDistance = Vector3.Distance(transform.position, target.position);
			Camera.main.fieldOfView = FOVForHeightAndDistance(initHeightAtDist, currDistance);
		}
	}
}