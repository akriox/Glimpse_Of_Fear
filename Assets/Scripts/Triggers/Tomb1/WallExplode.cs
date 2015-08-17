using UnityEngine;

public class WallExplode : MonoBehaviour {

	public GameObject wall;
	public GameObject blockingRocks;
	public GameObject smoke;
	private Collider[] rocks;
	
	public void Start(){
		rocks = wall.GetComponentsInChildren<Collider>();
		Physics.IgnoreLayerCollision (12, 13);
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			Explode();
			Destroy(blockingRocks);
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			Destroy(this.gameObject);
		}
	}

	private void Explode(){
		wall.GetComponent<AudioSource>().Play();
		smoke.SetActive(true);
		StartCoroutine(GameController.Instance.timedVibration(0.8f, 0.8f, 1.0f));
		StartCoroutine(CameraController.Instance.Shake(1.0f, 0.5f, 2.0f));
		foreach(Collider rock in rocks){
			rock.GetComponent<Rigidbody>().isKinematic = false;
			rock.GetComponent<Rigidbody>().AddForce(Vector3.left * 800.0f);
		}
	}
}