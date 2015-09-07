using UnityEngine;
using System.Collections;

public class Note : Collectible {

	public string contentStr;
	public AudioClip contentAudio;
	private Shader defaultShader;
	private bool activ = true;

	public new void Start(){
		base.Start();
		defaultShader = GetRenderer().material.shader;
		GetRenderer().material.shader = (Shader) Resources.Load("Shaders/ItemGlow", typeof(Shader));
	}

	public new void Update(){
		base.Update();
		if(pickedUp && activ){
			activ = false;
			GetRenderer().material.shader = defaultShader;
			GetComponent<ItemGlow>().enabled = false;
			GameController.Instance.displayWidget(false);
			if(Settings.subtitles) StartCoroutine(GameController.Instance.displayTimedDebug(contentStr, 24.0f));
			if(contentAudio != null) VoiceOver.Talk(contentAudio);
		}
	}
}