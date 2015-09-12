using UnityEngine;
using System.Collections;

public class Note : Collectible {

	public string contentStr;
	public AudioClip contentAudio;
	private AudioClip pickUpNote;
	private Shader defaultShader;

	public new void Start(){
		base.Start();
		defaultShader = GetRenderer().material.shader;
		GetRenderer().material.shader = (Shader) Resources.Load("Shaders/ItemGlow", typeof(Shader));
		pickUpNote = (AudioClip) Resources.Load("Audio/Objects/pickUpNote", typeof(AudioClip));
	}

	public new void Update(){
		base.Update();
		if(pickedUp){
			EventSound.playClip(pickUpNote, 0.2f);
			GetRenderer().material.shader = defaultShader;
			GetComponent<ItemGlow>().enabled = false;
			GameController.Instance.displayWidget(false);
			if(Settings.subtitles) StartCoroutine(GameController.Instance.displayTimedDebug(contentStr, 24.0f));
			if(contentAudio != null) VoiceOver.Talk(contentAudio);
			this.enabled = false;
		}
	}
}