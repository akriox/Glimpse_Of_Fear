using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
	[SerializeField][Range(0.0f, 10.0f)] private float duration;
	[SerializeField][Range(1.0f, 100.0f)] private float magnitude;
	[SerializeField][Range(1.0f, 100.0f)] private float speed;

	public void Update () {
		if(Input.GetKeyDown(KeyCode.F)){
			StartCoroutine(Shake());
		}
	}

	private IEnumerator Shake() {
		
		float elapsed = 0.0f;
		Quaternion initRot = Camera.main.transform.localRotation;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;          
			
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			float z = Random.value * 2.0f - 1.0f;
			x *= magnitude/100 * damper;
			y *= magnitude/100 * damper;
			z *= magnitude/100 * damper;

			Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, new Quaternion(x, y, z, initRot.w), speed * Time.deltaTime);
			
			yield return null;
		}
		
		Camera.main.transform.localRotation = initRot;
	}
}
