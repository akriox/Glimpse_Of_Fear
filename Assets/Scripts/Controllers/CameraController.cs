using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {

	public static CameraController Instance {get; private set;}

	private Camera _camera;
	private Quaternion initCamera;

	private Animator _animator;
	private int gettingUp = Animator.StringToHash("GettingUp");

	private VertigoEffect _vertigoScript;
	private Vortex _vortexScript;
	private NoiseAndScratches _noiseAndScratchesScript;

	public Image fade;
	public enum FadeState {IN, OUT, OFF};
	private FadeState _fadeState = FadeState.OFF;
	private float _fadeSpeed = 2.0f;

	public enum VortexState {INC, DEC, MAX, OFF};
	private VortexState _vortexState = VortexState.OFF;
	[SerializeField][Range(0.0f, 10.0f)] private float _vortexSpeed = 1.0f;
	[SerializeField][Range(0.0f, 1.0f)] private float _vortexMaxRadius = 0.4f;

	public enum NoiseAndScratchesState {INC, DEC, MAX, OFF};
	private NoiseAndScratchesState _noiseAndScratchesState = NoiseAndScratchesState.OFF;
	[SerializeField][Range(0.0f, 2.5f)] private float _noiseAndScratchesMaxValue = 1.5f;
	[SerializeField][Range(0.0f, 1.0f)] private float _noiseAndScratchesMaxValueSpeed = 0.5f;


	public void Awake(){
		Instance = this;
	}

	public void Start(){
		_camera = Camera.main;
		initCamera = Camera.main.transform.localRotation;
		_animator = _camera.GetComponent<Animator>();

		_vortexScript = _camera.GetComponent<Vortex>();
		_noiseAndScratchesScript = _camera.GetComponent<NoiseAndScratches>();
		_vertigoScript = _camera.GetComponent<VertigoEffect>();
	}

	public void Update(){
		FadeEffect();
		VortexEffect();
		NoiseAndScratchesEffect();
	}
	
	public void setVortexState(VortexState state){
		_vortexState = state;
	}

	public void setNoiseAndScratches(NoiseAndScratchesState state){
		_noiseAndScratchesState = state;
	}

	public void setFadeState(FadeState state, float speed){
		_fadeState = state;
		_fadeSpeed = speed;
	}

	public void setVertigoEffect(bool b){
		if(b) _vertigoScript.StartVertigo();
		else _vertigoScript.StopVertigo();
	}

	public void setVertigoTarget(Transform t){
		_vertigoScript.target = t;
	}

	private void NoiseAndScratchesEffect(){

		switch (_noiseAndScratchesState) {
			case NoiseAndScratchesState.INC:
				_noiseAndScratchesScript.enabled = true;
				if(_noiseAndScratchesScript.grainIntensityMax < _noiseAndScratchesMaxValue){
					_noiseAndScratchesScript.grainIntensityMax += _noiseAndScratchesMaxValueSpeed * Time.deltaTime;
				}
				else{
					_noiseAndScratchesScript.grainIntensityMax = _noiseAndScratchesMaxValue;
					_noiseAndScratchesState = NoiseAndScratchesState.MAX;
				}
				break;
			case NoiseAndScratchesState.DEC:
				if(_noiseAndScratchesScript.grainIntensityMax >0.0f){
					_noiseAndScratchesScript.grainIntensityMax -= _noiseAndScratchesMaxValueSpeed * 2 * Time.deltaTime;
				}
				else{
					_noiseAndScratchesScript.grainIntensityMax = 0.0f;
					_noiseAndScratchesScript.enabled = false;
					_noiseAndScratchesState = NoiseAndScratchesState.OFF;
				}
				break;
			case NoiseAndScratchesState.MAX:
				break;
			case NoiseAndScratchesState.OFF:
				_noiseAndScratchesScript.enabled = false;
				break;
		}
	}

	private void VortexEffect(){

		switch(_vortexState){
			case VortexState.INC:
				_vortexScript.enabled = true;
				if(_vortexScript.radius.x < _vortexMaxRadius && _vortexScript.radius.y < _vortexMaxRadius){
					_vortexScript.radius.x = _vortexScript.radius.y += _vortexSpeed * Time.deltaTime;
				}
				else{
					_vortexScript.radius.x = _vortexScript.radius.y = _vortexMaxRadius;
					_vortexState = VortexState.MAX;
				}
				break;
			case VortexState.DEC:
				if(_vortexScript.radius.x > 0.0f && _vortexScript.radius.y > 0.0f){
					_vortexScript.radius.x = _vortexScript.radius.y -= _vortexSpeed * Time.deltaTime;
				}
				else{
					_vortexScript.radius.x = _vortexScript.radius.y = 0.0f;
					_vortexState = VortexState.OFF;
				}
				break;
			case VortexState.MAX:
				break;
			case VortexState.OFF: 
			    _vortexScript.enabled = false;
				break;
		}
	}

	private void FadeEffect(){

		switch(_fadeState){
			case FadeState.IN:
				if(fade.color.a >= 0.05f){ 
					fade.color = Color.Lerp(fade.color, Color.clear, _fadeSpeed * Time.deltaTime);
				}
				else{
					fade.color = Color.clear;
					_fadeState = FadeState.OFF;
					fade.enabled = false;
				}
				break;
			case FadeState.OUT:
				if(fade.color.a <= 0.95f){ 
					fade.color = Color.Lerp(fade.color, Color.black, _fadeSpeed * Time.deltaTime);
				}
				else{
					fade.color = Color.black;
					_fadeState = FadeState.OFF;
					fade.enabled = false;
				}
				break;
			case FadeState.OFF:
				break;
		}
	}

	public IEnumerator Shake(float duration, float magnitude, float speed) {
		
		float elapsed = 0.0f;
		Quaternion initRot = Camera.main.transform.localRotation;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;          
			
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			float z = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			z *= magnitude * damper;
			
			Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, new Quaternion(x, y, z, initRot.w), speed * Time.deltaTime);
			
			yield return null;
		}
		
		Camera.main.transform.localRotation = initRot;
	}

	public void resetShake(){
		Camera.main.transform.localRotation = initCamera;
	}

	public void GettingUpAnimation(){
		lockPlayer(1);
		_animator.SetTrigger(gettingUp);
	}

	public void lockPlayer(int value){
		bool b = value == 1 ? true : false;
		FirstPersonController.ableToMove = !b;
		EyeLook.isActive = !b;
		_animator.applyRootMotion = !b;
	}
}