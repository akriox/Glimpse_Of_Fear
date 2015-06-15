using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Framerate : MonoBehaviour
{
	private Text framerate;
	private float deltaTime = 0.0f;
	private float ms = 0.0f;
	private float fps = 0.0f;
	
	public void Start(){
		framerate = GetComponent<Text>();
	}

	public void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		ms = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		framerate.text = string.Format("{0:0.0} ms ({1:0.} fps)", ms, fps);
	}
}