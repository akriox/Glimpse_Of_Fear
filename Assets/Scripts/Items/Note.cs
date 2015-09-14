using UnityEngine;
using System.Collections;

public class Note : Collectible {

	public string contentStr;
	public AudioClip contentAudio;
	private AudioClip pickUpNote;
	private Shader defaultShader;

	public new void Start(){
		base.Start();
		defaultShader = GetComponent<Renderer>().material.shader;
		GetComponent<Renderer>().material.shader = (Shader) Resources.Load("Shaders/ItemGlow", typeof(Shader));
		pickUpNote = (AudioClip) Resources.Load("Audio/Objects/pickUpNote", typeof(AudioClip));
	}

	public new void Update(){
		base.Update();
		if(pickedUp){
			EventSound.playClip(pickUpNote, 1.0f);
			GetComponent<Renderer>().material.shader = defaultShader;
			GetComponent<ItemGlow>().enabled = false;
			GetComponent<Collider>().enabled = false;
			GameController.Instance.displayWidget(false);
			StartCoroutine(ReadNote(1.0f));
			this.enabled = false;
		}
	}

	private IEnumerator ReadNote(float delay){
		yield return new WaitForSeconds(delay);
		if(Settings.subtitles) StartCoroutine(GameController.Instance.displayTimedDebug(contentStr, 24.0f));
		if(contentAudio != null) VoiceOver.Talk(contentAudio);

	}
}