using UnityEngine;
using System.Collections;

public class ThrowingRock : Collectible {

	private Rigidbody rb;
	private float lifespan = 5.0f;

	private AudioSource _audioSource;
	private AudioClip pickUpClip;
	private AudioClip hit;
	
	public new void Start(){
		base.Start();
		rb = GetComponentInChildren<Rigidbody>();
		//Ignore collisions with GuardRails(11)
		Physics.IgnoreLayerCollision(14, 11);
		
		_audioSource = GetComponent<AudioSource>();
		pickUpClip = (AudioClip) Resources.Load("Audio/Objects/pickup_itemrock", typeof(AudioClip));
		hit = (AudioClip) Resources.Load("Audio/Tomb/T2/FX_throwingRock_collision", typeof(AudioClip));
	}
	
	public new void Update(){
		base.Update ();
		if(pickedUp && ThrowObject.objectToThrow == null){
			_audioSource.clip = pickUpClip;
			_audioSource.Play();

			//Duplicate rock so there is always one available in the pile of rocks
			GameObject newRock = (GameObject) Instantiate(this.gameObject, this.transform.position , this.transform.rotation);
			newRock.name = "ThrowingRock";
			newRock.transform.parent = this.transform.parent;

			GetComponent<Renderer>().enabled = true;
			GameController.Instance.displayWidget(false);
			if(ThrowObject.setObjectToThrow(rb.gameObject)){
				rb.isKinematic = true;
				this.gameObject.transform.parent = Camera.main.transform;
			}
		}
		
		/* Rock throwed */
		if(rb.isKinematic == false){
			this.gameObject.transform.parent = null;
			lifespan -= Time.deltaTime;
		}

		if(lifespan <= 0.0f) Destroy(this.gameObject);
	}

	public void OnCollisionEnter(Collision co){
		_audioSource.clip = hit;
		_audioSource.Play();
	}
}
