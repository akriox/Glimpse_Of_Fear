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
		
		
		private List<Transform> listPaths = new List<Transform>();
		private int index = 1;
		private Transform currentTarget;
		private GameObject player;
		private GazeAwareComponent _gazeAwareComponent;
		private AudioSource audioSource;
		public AudioClip[] audioClip;
		private bool ready;
		
		private float _angleTwirl = 0;
		private float _saturationColorCorrection = 1f;
		private Color _selectiveFromColorCorrection = Color.white;
		private Color _selectiveToColorCorrection = Color.white;
		
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
				if(!audioSource.isPlaying) playASong(); 
				resetImageEffect();
			} 
			else {
				if(ready){
					pauseASong();
					if(_angleTwirl < 20.0f)
						_angleTwirl +=0.1f;
					if(_saturationColorCorrection >0f)
						_saturationColorCorrection -= 0.005f;
					_selectiveFromColorCorrection = new Color(152f,152f,152f,255f);
					_selectiveToColorCorrection = new Color(83f,83f,83f,255f);
				}
			}
			Walk();
			Twirl.angle = _angleTwirl;
			ColorCorrectionCurves.ccMaterial.SetFloat ("_Saturation", _saturationColorCorrection);
			ColorCorrectionCurves.selectiveCcMaterial.SetColor ("selColor", _selectiveFromColorCorrection);
			ColorCorrectionCurves.selectiveCcMaterial.SetColor ("targetColor", _selectiveToColorCorrection);
		}
		
		void GetPaths(){
			foreach(Transform temp in pathToFollow){
				listPaths.Add(temp);
			}
		}
		
		void GetNewPosition(){
			currentTarget = listPaths.Single(p => p.name == "Path" + index);
			index = (index < listPaths.Count) ? index +1 : 1;
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

		public void isReady(){
			ready = true;
		}
		
		public void isFinish(){
			ready = false;
			audioSource.Stop();
		}
		public void resetImageEffect(){
			_angleTwirl = 0f;
			_saturationColorCorrection = 1f;
			_selectiveFromColorCorrection = Color.white;
			_selectiveToColorCorrection = Color.white;
		}

		public void resetBeginningRoom(){
			index = 1;
			currentTarget = listPaths [index-1];
			transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
		}
	}
}