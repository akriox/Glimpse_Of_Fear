using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GazeAwareComponent))]
public class GazeLight : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private Renderer _renderer;
	private Color currentColor;
	private Color skyColor = new Color(0.21f, 0.26f, 0.30f, 1.0f);
	private float speed = 2.0f;

	public void Start () {
		_gazeAwareComponent = GetComponent<GazeAwareComponent>();
		_renderer = GetComponent<Renderer>();
	}
	
	public void Update () {
		currentColor = _renderer.material.GetColor("_EmissionColor");
		if(_gazeAwareComponent.HasGaze){
			currentColor = Color.Lerp (currentColor, skyColor, Time.deltaTime * speed);
		}
		else{
			currentColor = Color.Lerp (currentColor, Color.white, Time.deltaTime * speed);
		}
		_renderer.material.SetColor("_EmissionColor", currentColor);
	}
}
