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
		if (loop) {
			StartCoroutine(Wait (0.5f));
		}
		if (audioSource.isPlaying && Time.time > _time)
			stopLoop ();
	}
	
	public static void playLoop(){
		loop = true;
	}
	public static void stopLoop(){
		audioSource.loop = false;
		loop = false;
	}
	
	public static IEnumerator Wait (float s){
		yield return new WaitForSeconds(s);
		audioSource.loop = true;
		audioSource.Play();
		_time = Time.time + timeHeartBeat;
		loop = false;
	}
}
