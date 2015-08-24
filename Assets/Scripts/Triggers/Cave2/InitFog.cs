using UnityEngine;
using System.Collections;

public class InitFog : MonoBehaviour {

	public GameObject activFog;
	public GameObject otherSpecter;
	public GameObject desactivRealSpecter;

	private static bool jumpScare;

	public void start(){
		jumpScare = false;
	}
	
	public IEnumerator OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Player") {
			CameraController.Instance.setNoiseAndScratches(CameraController.NoiseAndScratchesState.INC);
			activFog.SetActive (true);
			if (!jumpScare){
				otherSpecter.SetActive(true);
				desactivRealSpecter.SetActive(false);
			}
			yield return new WaitForSeconds(1f);
			this.enabled = false;
		}
	}

	public static void specterSeen(){
		jumpScare = true;
	}
}
