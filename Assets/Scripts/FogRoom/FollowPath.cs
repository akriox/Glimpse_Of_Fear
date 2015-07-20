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
		
		public Transform pathToFollow;
		[SerializeField][Range(2F, 10.0F)] public float speed;
		[SerializeField][Range(2F, 5.0F)] public float DistanceMinForMove;
		private MovementTypes type = MovementTypes.Follow;
		
		private List<Transform> listPaths = new List<Transform>();
		private int index = 1;
		private Transform currentTarget;
		private GameObject player;
		private GazeAwareComponent _gazeAwareComponent;
		private AudioSource audioSource;
		public AudioClip[] audioClip;
		private bool ready;
		
		public void Awake(){
			Instance = this;
		}
		
		void Start () {
			ready = false;
			audioSource = GetComponent<AudioSource>();
			player = GameObject.FindGameObjectWithTag("Player");
			_gazeAwareComponent = GetComponent<GazeAwareComponent>();
			if(pathToFollow == null){
				Debug.LogError("Un GameObject 'Path' doit etre renseigné dans le script 'FollowPath.cs'.");
			} 
			else {
				GetPaths();
				index = 1;
				if(listPaths.Count > 0) GetNewPosition();
			}
		}
		
		void Update () {
			if (_gazeAwareComponent.HasGaze && ready) {
				if(audioSource.isPlaying) pauseASong(); 
				CameraController.Instance.setVortexState(CameraController.VortexState.DEC);
				CameraController.Instance.setNoiseAndScratches(false);
			} 
			else {
				if(ready){
					if(!audioSource.isPlaying) playASong(); 
					CameraController.Instance.setVortexState(CameraController.VortexState.INC);
					CameraController.Instance.setNoiseAndScratches(true);
				}
			}
			Walk();
		}
		
		void GetPaths(){
			foreach(Transform temp in pathToFollow){
				listPaths.Add(temp);
			}
		}
		
		void GetNewPosition(){
			switch (type) {
				case MovementTypes.Follow:
					currentTarget = listPaths.Single (p => p.name == "Path" + index);
					index = (index < listPaths.Count) ? index + 1 : 1;
				break;
				case MovementTypes.Reverse:
					currentTarget = listPaths.Single (p => p.name == "Path" + index);
					index = (index > 1) ? index - 1 : listPaths.Count;
				break;
			}
		}
		
		
		void Walk(){
			if(currentTarget != null){
				transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
				
				if(CheckDistance() <= 0.5f && CheckPlayerDistance()<=DistanceMinForMove){
					GetNewPosition();
				}
			}
		}
		float CheckPlayerDistance(){
			return Vector3.Distance(transform.position, player.transform.position);
		}
		
		float CheckDistance(){
			return Vector3.Distance(transform.position, currentTarget.position);
		}

		private void playASong(){
			if (UnityEngine.Random.value > 0.5f) {
				audioSource.clip = audioClip[0];
				audioSource.Play();
			}
			else {
				audioSource.clip = audioClip[1];
				audioSource.Play();
			}
		}

		private void pauseASong(){
			audioSource.Pause();
		}

		public void setReady(){
			ready = true;
		}

		public bool isReday(){
			return ready;
		}

		public void setFinish(){
			ready = false;
			audioSource.Stop();
		}
		public void resetImageEffect(){
			CameraController.Instance.setVortexState (CameraController.VortexState.DEC);
		}

		public void resetBeginningRoom(){
			type = MovementTypes.Follow;
			index = 1;
			currentTarget = listPaths [index-1];
			transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
		}
		public void resetEndRoom(){
			type = MovementTypes.Reverse;
			index = listPaths.Count-1;
			currentTarget = listPaths [index-1];
			transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
		}
	}
}

public enum MovementTypes { Follow, Reverse }