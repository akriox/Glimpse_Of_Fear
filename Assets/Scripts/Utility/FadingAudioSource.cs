using UnityEngine;

/// <summary>
///   Audio source that fades between clips instead of playing them immediately.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FadingAudioSource : MonoBehaviour{

	///   Volume to end the previous clip at.
	public float FadeOutThreshold = 0.05f;
	///   Volume change per second when fading.
	public float FadeSpeed = 0.05f;
	///   Actual audio source.
	private AudioSource audioSource;
	///   Whether the audio source is currently fading, in or out.
	private FadeState fadeState = FadeState.None;
	///   Next clip to fade to.
	private AudioClip nextClip;
	///   Whether to loop the next clip.
	private bool nextClipLoop;
	///   Target volume to fade the next clip to.
	private float nextClipVolume;
	
	public enum FadeState{None,FadingOut,FadingIn}
	
	///   Current clip of the audio source.
	public AudioClip Clip{get{return this.audioSource.clip;}}

	///   Whether the audio source is currently playing a clip.
	public bool IsPlaying{get{return this.audioSource.isPlaying;}}

	///   Whether the audio source is looping the current clip.
	public bool Loop{get{return this.audioSource.loop;}}

	///   Current volume of the audio source.
	public float Volume{get{return this.audioSource.volume;}}

	
	///   If the audio source is enabled and playing, fades out the current clip and fades in the specified one, after.
	///   If the audio source is enabled and not playing, fades in the specified clip immediately.
	///   If the audio source is not enalbed, fades in the specified clip as soon as it gets enabled.
	/// <param name="clip">Clip to fade in.</param>
	/// <param name="volume">Volume to fade to.</param>
	/// <param name="loop">Whether to loop the new clip, or not.</param>
	public void Fade(AudioClip clip, float volume, bool loop){
		if (clip == null || clip == this.audioSource.clip){
			return;
		}
		this.nextClip = clip;
		this.nextClipVolume = volume;
		this.nextClipLoop = loop;
		if (this.audioSource.enabled){
			if (this.IsPlaying){
				this.fadeState = FadeState.FadingOut;
			}
			else{
				this.FadeToNextClip();
			}
		}
		else{
			this.FadeToNextClip();
		}
	}

	///   Continues fading in the current audio clip.
	public void Play(){
		this.fadeState = FadeState.FadingIn;
		this.audioSource.Play();
	}

	///   Stop playing the current audio clip immediately.
	public void Stop(){
		this.audioSource.Stop();
		this.fadeState = FadeState.None;
	}
	
	private void Awake(){
		this.audioSource = this.GetComponent<AudioSource>();
		this.audioSource.volume = 0f;
	}
	
	private void FadeToNextClip(){
		this.audioSource.clip = this.nextClip;
		this.audioSource.loop = this.nextClipLoop;

		this.fadeState = FadeState.FadingIn;
		
		if (this.audioSource.enabled){
			this.audioSource.Play();
		}
	}
	
	private void OnDisable()
	{
		this.audioSource.enabled = false;
		this.Stop();
	}
	
	private void OnEnable(){
		this.audioSource.enabled = true;
		this.Play();
	}
	
	private void Update(){
		if (!this.audioSource.enabled){
			return;
		}
		
		if (this.fadeState == FadeState.FadingOut){
			if (this.audioSource.volume > this.FadeOutThreshold){
				// Fade out current clip.
				this.audioSource.volume -= this.FadeSpeed * Time.deltaTime;
			}
			else{
				// Start fading in next clip.
				this.FadeToNextClip();
			}
		}
		else if (this.fadeState == FadeState.FadingIn){
			if (this.audioSource.volume < this.nextClipVolume){
				// Fade in next clip.
				this.audioSource.volume += this.FadeSpeed * Time.deltaTime;
			}
			else{
				// Stop fading in.
				this.fadeState = FadeState.None;
			}
		}
	}
}