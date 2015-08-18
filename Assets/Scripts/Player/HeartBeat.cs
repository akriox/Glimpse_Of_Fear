using UnityEngine;
using System.Collections;

public class HeartBeat : MonoBehaviour {
	
	public static AudioSource audioSource;
	[SerializeField][Range(3.0F, 10.0F)] private static float timeHeartBeat = 7.0f;
	private static float _time;
	private static bool loop;
	
	public void Start () {
		audioSource = GetComponent<AudioSource>();
		loop = false;
	}
	
	public void Update () {
		if (loop && Time.time > _time)
			stopLoop ();
	}
	
	public static void playLoop(){
		audioSource.loop = true;
		loop = true;
		_time = Time.time + timeHeartBeat;
		audioSource.Play();
	}
	public static void stopLoop(){
		audioSource.loop = false;
		loop = false;
	}
}
