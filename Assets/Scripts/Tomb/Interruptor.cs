using UnityEngine;
using System.Collections;

public class Interruptor : Collectible {

	public GameObject triggerWallExplode;
	public AudioClip clip;

	private Shader defaultShader;
	
	public new void Start(){
		base.Start();
		defaultShader = GetComponent<Renderer>().material.shader;
		GetComponent<Renderer>().material.shader = (Shader) Resources.Load("Shaders/ItemGlow", typeof(Shader));
	}
	
	public new void Update(){
		base.Update();
		if(pickedUp){
			GameController.Instance.displayWidget(false);
			EventSound.playClip(clip);
			GetComponent<Animation>().Play();
			triggerWallExplode.SetActive(true);
			GetComponent<Renderer>().material.shader = defaultShader;
			GetComponent<ItemGlow>().enabled = false;
			this.enabled = false;
		}
	}
}
