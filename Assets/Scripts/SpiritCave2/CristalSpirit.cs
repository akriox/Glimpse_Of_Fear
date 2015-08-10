using UnityEngine;
using System.Collections;

public class CristalSpirit : MonoBehaviour {

	private Material activeMat;
	private Material inactiveMat;
	private GameObject targetSpirit;
	private float distActivationCrystal = 5f;

	// Use this for initialization
	void Start () {
		inactiveMat = (Material)Resources.Load("Materials/Caves_objects/Crystal", typeof(Material));
		activeMat = (Material)Resources.Load("Materials/Caves_objects/CrystalSpirit", typeof(Material));
		gazeEffects (inactiveMat);
		targetSpirit = GameObject.FindGameObjectWithTag("Specter");
	}
	
	// Update is called once per frame
	void Update () {
		if (closeToTheTarget (distActivationCrystal, targetSpirit)) {
			gazeEffects(activeMat);
		} 
		else {
			gazeEffects(inactiveMat);
		}
	}

	/** Change material color and enable/disable Halo */
	private void gazeEffects(Material mat) {
		GetComponent<Renderer>().material = mat;
	}

	//return true if the spirit is close to the crystal 
	private bool closeToTheTarget(float dist, GameObject target){
		if (Vector3.Distance(transform.position, target.transform.position) < dist) {
			return true;
		}
		else {
			return false;
		}
	}
}
