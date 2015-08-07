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
		[SerializeField][Range(2F, 10.0F)] private float speed;
		[SerializeField][Range(2F, 5.0F)] private float DistanceMinForMove;
		[SerializeField][Range(5F, 10.0F)] private float DistanceMaxBeforeComeBack;
		private MovementTypes type = MovementTypes.Follow;
		private MovementTypes contraryType = MovementTypes.Reverse;
		
		private List<Transform> listPaths = new List<Transform>();
		private int index = 1;
		private Transform currentTarget;
		private GameObject player;
		private GazeAwareComponent _gazeAwareComponent;
		private AudioSource audioSource;
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
				index = 5;
				if(listPaths.Count > 0) GetNewPosition(type);
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
		
		void GetNewPosition(MovementTypes t){
			switch (t) {
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
		
		
		void Walk(){
			if(currentTarget != null){
				transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
				if(CheckDistance() <= 0.5f &&CheckPlayerDistance()>=DistanceMaxBeforeComeBack){
					GetNewPosition(contraryType);
				}
				else {
					if(CheckDistance() <= 0.5f && CheckPlayerDistance()<=DistanceMinForMove){
						GetNewPosition(type);
					}
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
			audioSource.Play();
		}

		private void pauseASong(){
			audioSource.Pause();
		}

		public void setReady(){
			ready = true;
		}

		public bool isReady(){
			return ready;
		}

		public void setFinish(){
			ready = false;
			audioSource.Stop();
		}
		public void resetImageEffect(){
			CameraController.Instance.setVortexState (CameraController.VortexState.DEC);
			CameraController.Instance.setNoiseAndScratches(false);
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

public enum MovementTypes { Follow, Reverse }