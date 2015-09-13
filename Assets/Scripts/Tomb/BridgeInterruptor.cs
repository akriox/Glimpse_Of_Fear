using UnityEngine;
using System.Collections;

public class BridgeInterruptor : MonoBehaviour {

	public bool activ = false;
	public GameObject bridge;
	public GameObject blockingCollider;
	public BridgeInterruptor otherInterruptor;

	private Material activMat;

	public void Start(){
		activMat = (Material) Resources.Load("Materials/CaveObjects/Pillar2", typeof(Material));
	}

	public void OnCollisionEnter(Collision co){
		activ = true;
		GetComponent<Renderer>().material = activMat;
		GetComponent<ItemGlow>().enabled = false;
		if(otherInterruptor.activ){
			blockingCollider.SetActive(false);
			bridge.GetComponent<Animation>().Play();
		}
	}
}
