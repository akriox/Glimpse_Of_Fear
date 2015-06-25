
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class navCharScript : MonoBehaviour {
	

	public Transform pathToFollow;
	
	private List<Transform> listPaths = new List<Transform>();
	private Transform currentTarget;

	NavMeshAgent agent;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		if(pathToFollow == null){
			Debug.LogError("Un GameObject 'Path' doit etre renseigné dans le script 'FollowPath.cs'.");
		} else {
			GetPaths();
			if(listPaths.Count > 0) GetNewPosition();
		}
	}

	
	void Update () {
		StartWalk();
		/*if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast (ray,out hit,100)){
				agent.SetDestination(hit.point);
			}
		}*/
	}

	void GetPaths(){
		foreach(Transform temp in pathToFollow){
			listPaths.Add(temp);
		}
	}
	


	void GetNewPosition(){
		currentTarget = listPaths[Random.Range(0, listPaths.Count)];
	}
	

	void StartWalk(){
		if(currentTarget != null){
			agent.SetDestination(currentTarget.position);
			if(CheckDistance() <= 0.5f){
				GetNewPosition();
			}
		}
	}

	float CheckDistance(){
		return Vector3.Distance(transform.position, currentTarget.position);
	}
} 
