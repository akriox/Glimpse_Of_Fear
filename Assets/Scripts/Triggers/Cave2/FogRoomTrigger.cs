using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.ImageEffects
{
	public class FogRoomTrigger: MonoBehaviour {
		
		
		[SerializeField][Range(0, 5)]public int _case = 1;
		[SerializeField] private GameObject desactivAnObject;
		[SerializeField] private GameObject activAnObject;
		[SerializeField] private GameObject thisTrigger;
		[SerializeField] private GameObject otherTrigger;

		[SerializeField] private string str;
		[SerializeField] private AudioClip voice;

		public void OnTriggerEnter(Collider other) {
			if (other.gameObject.tag == "Player") {
				if (activAnObject != null)
					activAnObject.SetActive(true);
				switch (_case) {
				case 0:
					canDesactiveAnObject();
					if(!FollowPath.Instance.isReady()){
						FollowPath.Instance.setReady();
						FollowPath.Instance.resetBeginningRoom ();
					}
					break;
				case 1:
					//CameraController.Instance.setVortexState (CameraController.VortexState.DEC);
					CameraController.Instance.setNoiseAndScratches(CameraController.NoiseAndScratchesState.DEC);
					FollowPath.Instance.setFinish();
					canDesactiveAnObject();
					break;
				case 2:
					if(!FollowPath.Instance.isReady()){
						FollowPath.Instance.setReady();
						FollowPath.Instance.resetEndRoom ();
					}
					break;
				case 3:
					FollowPath.Instance.setFinish();
					CameraController.Instance.setNoiseAndScratches(CameraController.NoiseAndScratchesState.DEC);
					break;
				case 4:
					FollowPath.Instance.setReady();
					break;
				case 5:
					CameraController.Instance.setNoiseAndScratches(CameraController.NoiseAndScratchesState.DEC);
					FollowPath.Instance.setFinish();
					StartCoroutine(voiceFall());
					break;
				}
				if(thisTrigger!= null) thisTrigger.SetActive(false);
				if(otherTrigger!= null) otherTrigger.SetActive(true);
			}
		}

		public void canDesactiveAnObject(){
			if (desactivAnObject != null && desactivAnObject.activeSelf) {
				desactivAnObject.SetActive (false);
			}
		}

		IEnumerator voiceFall(){
			yield return new WaitForSeconds(1f);
			if(voice != null) VoiceOver.Talk(voice);
			if(Settings.subtitles) GameController.Instance.displayDebug(str);
			yield return new WaitForSeconds(1.5f);
			if(Settings.subtitles) GameController.Instance.displayDebug("");
		}
	}
}
