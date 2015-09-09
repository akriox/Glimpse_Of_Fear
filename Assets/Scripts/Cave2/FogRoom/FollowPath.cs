using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace UnityStandardAssets.ImageEffects
{
	[RequireComponent(typeof(GazeAwareComponent))]
	public class FollowPath : MonoBehaviour {
		public static FollowPath Instance {get; private set;}

		[SerializeField] private Transform pathToFollow;
		[SerializeField][Range(2F, 10.0F)] private float speed;
		[SerializeField][Range(2F, 5.0F)] private float DistanceMinForMove;
		[SerializeField][Range(5F, 10.0F)] private float DistanceMaxBeforeComeBack;
		[SerializeField] private AudioClip whisper;

		[SerializeField] private AudioClip voice;
		[SerializeField] private AudioClip voice2;

		private enum MovementTypes { Follow, Reverse };
		private MovementTypes type = MovementTypes.Follow;
		private MovementTypes contraryType = MovementTypes.Reverse;
		
		private List<Transform> listPaths = new List<Transform>();
		private int index = 1;
		private Transform currentTarget;
		private GameObject player;
		private GazeAwareComponent _gazeAwareComponent;
		private bool ready = false;

		private AudioClip nothing;
		private FadingAudioSource _fadingAudioSource;

		private bool correctWay;

		public void Awake(){
			Instance = this;
		}
		
		private void Start () {
			nothing = (AudioClip) Resources.Load("Audio/blank_sound", typeof(AudioClip));
			player = GameObject.FindGameObjectWithTag("Player");
			_gazeAwareComponent = GetComponent<GazeAwareComponent>();
			_fadingAudioSource = GetComponent<FadingAudioSource> ();
			type = MovementTypes.Follow;
			contraryType = MovementTypes.Reverse;
			_fadingAudioSource.Fade (nothing, 0.0f, false);
			correctWay = true;
			if(pathToFollow == null){
				Debug.LogError("Un GameObject 'Path' doit etre renseigné dans le script 'FollowPath.cs'.");
			} 
			else {
				GetPaths();
				index = 5;
				if(listPaths.Count > 0) GetNewPosition(type);
			}
		}
		
		private void Update () {
			if (_gazeAwareComponent.HasGaze && ready) {
				_fadingAudioSource.Fade (nothing, 0.0f, false);
				//CameraController.Instance.setVortexState(CameraController.VortexState.DEC);
				CameraController.Instance.setNoiseAndScratches (CameraController.NoiseAndScratchesState.DEC);
				//if(!correctWay)talk (voice);
			} 
			else {
				if(ready){
					_fadingAudioSource.Fade (whisper, 1.0f, true);
					//CameraController.Instance.setVortexState(CameraController.VortexState.INC);
					CameraController.Instance.setNoiseAndScratches(CameraController.NoiseAndScratchesState.INC);
					//if(correctWay)talk(voice2);
				}
			}
			Walk();
		}

		void talk(AudioClip v){
			correctWay = !correctWay;
			VoiceOver.stop ();
			if (UnityEngine.Random.Range (0.0f, 1.0F) > 0.8f) {
				VoiceOver.Talk (v);
			}
		}

		
		private void GetPaths(){
			foreach(Transform temp in pathToFollow){
				listPaths.Add(temp);
			}
		}
		
		private void GetNewPosition(MovementTypes type){
			switch (type) {
				case MovementTypes.Follow:
					index = (index < listPaths.Count) ? index + 1 : 1;
					currentTarget = listPaths.Single (p => p.name == "Path" + index);
				break;
				case MovementTypes.Reverse:
					index = (index > 1) ? index - 1 : listPaths.Count;
					currentTarget = listPaths.Single (p => p.name == "Path" + index);
				break;
			}
		}

		private void Walk(){
			if(currentTarget != null){
				transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
				if(CheckDistance(currentTarget.transform.position) <= 0.5f &&CheckDistance(player.transform.position)>=DistanceMaxBeforeComeBack){
					GetNewPosition(contraryType);
				}
				else {
					if(CheckDistance(currentTarget.transform.position) <= 0.5f && CheckDistance(player.transform.position)<=DistanceMinForMove){
						GetNewPosition(type);
					}
				}
			}
		}

		private float CheckDistance(Vector3 to){
			return Vector3.Distance(transform.position, to);
		}

		public void setReady(){
			ready = true;
		}

		public bool isReady(){
			return ready;
		}

		public void setFinish(){
			ready = false;
			_fadingAudioSource.Fade (nothing, 0.0f, false);
		}

		public void resetBeginningRoom(){
			type = MovementTypes.Follow;
			contraryType = MovementTypes.Reverse;
			index = 5;
			if (listPaths.Count >= index) {
				currentTarget = listPaths [index - 1];
				transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
			}
		}

		public void resetEndRoom(){
			type = MovementTypes.Reverse;
			contraryType = MovementTypes.Follow;
			index = listPaths.Count-5;
			currentTarget = listPaths [index-1];
			transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
		}
	}
}

