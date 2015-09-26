using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlareStick : Collectible {

	private Vector3 rotation;
    private AudioClip clip;

    public new void Awake(){
		base.Awake();
		// Ignore collisions with guardrail colliders
		Physics.IgnoreLayerCollision(10, 11);
		// Ignore collisions with water
		Physics.IgnoreLayerCollision(10, 4);
	}

	public new void Start(){
		base.Start ();
        clip = (AudioClip)Resources.Load("Audio/Objets/pickup_glowstick", typeof(AudioClip));
        rotation = new Vector3(Random.Range(100,360),Random.Range(100,360),Random.Range(100,360));
		transform.Rotate(rotation);
	}

	public new void Update(){

		base.Update();

		if(pickedUp && Inventory.Instance.canTake()){
			GameController.Instance.displayWidget(false);
			Inventory.Instance.addFlareStick(1);
            if (!VoiceOver._audioSource.isPlaying) VoiceOver.Talk(clip);
            Destroy(this.gameObject);
		}
	}
}