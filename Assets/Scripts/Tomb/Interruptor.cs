using UnityEngine;
using System.Collections;

public class Interruptor : Collectible {

	public GameObject triggerWallExplode;
	public AudioClip clip;
	public GameObject sonMecanisme;

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
			sonMecanisme.SetActive(true);
			EventSound.playClip(clip, 0.5f);
			GetComponent<Animation>().Play();
			triggerWallExplode.SetActive(true);
			GetComponent<Renderer>().material.shader = defaultShader;
			GetComponent<ItemGlow>().enabled = false;
			this.enabled = false;
		}
	}
}
