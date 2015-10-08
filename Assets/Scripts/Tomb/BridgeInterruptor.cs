using UnityEngine;
using System.Collections;

public class BridgeInterruptor : MonoBehaviour {

	public bool activ = false;
	public GameObject bridge;
	public GameObject blockingCollider;
	public BridgeInterruptor otherInterruptor;
	public GameObject smoke;
	private bool alreadyActive = false;

	private Material activMat;

	public void Start(){
		activMat = (Material) Resources.Load("Materials/CaveObjects/BridgePillar", typeof(Material));
	}

	public void OnCollisionEnter(Collision co){
		activ = true;
		GetComponent<Renderer>().material = activMat;
		GetComponent<ItemGlow>().enabled = false;
		if(otherInterruptor.activ){
			if(!alreadyActive){
				smoke.SetActive(true);
				StartCoroutine(removeCollider());
				bridge.GetComponent<Animation>().Play();
                bridge.GetComponent<AudioSource>().Play();
				alreadyActive = true;
			}
		}
	}

	public IEnumerator removeCollider(){
		yield return new WaitForSeconds(6);
		blockingCollider.SetActive(false);
		yield return new WaitForSeconds(4f);
		smoke.SetActive(false);
		Destroy (this.gameObject);
	}
}
