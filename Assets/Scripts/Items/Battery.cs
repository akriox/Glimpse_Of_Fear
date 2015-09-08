using UnityEngine;
using System.Collections;

public class Battery : Collectible {

	private AudioClip clip;

	public new void Start(){
		base.Start();
		clip = (AudioClip) Resources.Load ("Audio/VoiceOver/pickUpItem", typeof(AudioClip));
	}

	public new void Update(){
		base.Update();
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			if(!VoiceOver._audioSource.isPlaying) VoiceOver.Talk(clip);
			Flashlight.Instance.charge();
			Destroy(this.gameObject);
		}
	}
}