using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
	private AudioSource song;

	public void Awake(){
		Instance = this;
	}

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		song = GetComponent<AudioSource> ();
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
		if (_gazeAwareComponent.HasGaze) {
			if (!song.isPlaying)
				song.Play ();
		} else {
			song.Stop ();
		}
		Walk();
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

	public void resetFirstRoom(){
		print ("coucou");
		index = 1;
		currentTarget = listPaths [index-1];
		transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
	}
	public void resetSecondRoom(){
		index = 6;
		currentTarget = listPaths [index-1];
		transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
	}
	public void resetThirdRoom(){
		index = 13;
		currentTarget = listPaths [index-1];
		transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
	}
	public void resetFourthRoom(){
		index = 20;
		currentTarget = listPaths [index-1];
		transform.position = new Vector3 (currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z);
	}
}