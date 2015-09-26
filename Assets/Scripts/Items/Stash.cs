using UnityEngine;
using System.Collections;

public class Stash : Collectible {

    private AudioClip clip;

    public new void Start()
    {
        base.Start();
        clip = (AudioClip)Resources.Load("Audio/Objets/pickup_glowstick", typeof(AudioClip));
    }

    public new void Update(){
		
		base.Update();
		
		if(pickedUp && Inventory.Instance.canTake()){
			GameController.Instance.displayWidget(false);
			Inventory.Instance.addFlareStick(4);
			TipsTracker.Instance.displayTip(TipsTracker.Tips.UseFlareStick);
            if (!VoiceOver._audioSource.isPlaying) VoiceOver.Talk(clip);
            Destroy(this.gameObject);
		}
	}
}