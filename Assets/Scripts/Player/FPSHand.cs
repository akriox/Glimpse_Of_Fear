using UnityEngine;
using System.Collections;

public class FPSHand : MonoBehaviour {

	public bool smooth;
	public float smoothTime = 5.0f;
	private float min = -30.0f;
	private float max = 30.0f;
	private Quaternion targetRotation;
	private float initRotZ;

	public void Start(){
		targetRotation = transform.localRotation;
		initRotZ = targetRotation.z;
	}

	public void Update(){
		
		float yRot = Input.GetAxis("RStickH") + Input.GetAxis("RStickV") != 0.0f ? Input.GetAxis("RStickH") : Input.GetAxis("Mouse X");
		float xRot = Input.GetAxis("RStickH") + Input.GetAxis("RStickV") != 0.0f ? -Input.GetAxis("RStickV") : Input.GetAxis("Mouse Y");
		
		yRot *= Settings.MousePadXYSensitivity;
		xRot *= Settings.MousePadXYSensitivity;
		
		targetRotation *= Quaternion.Euler (-yRot, -xRot, 0.0f);
		targetRotation = ClampRotationXYAxis(targetRotation);
		targetRotation.z = initRotZ;

		if(smooth){
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotation, smoothTime * Time.deltaTime);
		}
		else
		{
			transform.localRotation = targetRotation;
		}
	}

	private Quaternion ClampRotationXYAxis(Quaternion q){
		
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;
		
		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
		angleX = Mathf.Clamp (angleX, min, max);
		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
		
		float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.y);
		angleY = Mathf.Clamp (angleY, min, max);
		q.y = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleY);
		
		return q;
	}
}