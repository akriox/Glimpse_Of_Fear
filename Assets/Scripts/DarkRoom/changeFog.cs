using UnityEngine;
using System.Collections;

public class changeFog : MonoBehaviour {
	public static changeFog Instance {get; private set;}

	[SerializeField][Range(0.00F, 2.00F)] public float _fogDensity;
	[SerializeField][Range(0F, 20.0F)] public float _fogStartDistance;
	[SerializeField][Range(0F, 20.0F)] public float _fogEndDistance;

	private float _FogEndDist;

	public void Awake(){
		Instance = this;
	}

	void Start()
	{
		RenderSettings.fog = false;
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogDensity = _fogDensity;
		RenderSettings.fogStartDistance = _fogStartDistance;
		_FogEndDist = RenderSettings.fogEndDistance = _fogEndDistance;
	
	}
	
	void Update ()
	{
		RenderSettings.fogEndDistance = _FogEndDist;
		if (Input.GetKeyDown (KeyCode.F))
			_FogEndDist += 0.2f;
	}

	public void setMaxFogEndDistance(){
		_FogEndDist = 18.0f;
	}

	public void decrementFogEndDistance(){
		if(_FogEndDist>_fogEndDistance)
			_FogEndDist -= 0.08f;
	}

	public void desactiveFog(){
		RenderSettings.fog = false;
	}
	public void activeFog(){
		RenderSettings.fog = true;
	}
}
