using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {

	public static CameraController Instance {get; private set;}

	private Camera _camera;

	private Vortex _vortexScript;
	//private VertigoEffect _vertigoScript;
	private ColorCorrectionCurves _colorCorrectionCurvesScript;
	private NoiseAndScratches _noiseAndScratches;
	
	private bool _vertigo;

	public enum VortexState {INC, DEC, OFF};
	private VortexState _vortexState = VortexState.OFF;
	[SerializeField][Range(0.0f, 1.0f)] private float _vortexMaxRadius = 0.6f;
	[SerializeField][Range(0.0f, 1.0f)] private float _vortexSpeed = 1.0f;

	public void Awake(){
		Instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	public void Start(){
		_camera = Camera.main;
		_vortexScript = _camera.GetComponent<Vortex>();
		//_vertigoScript = _camera.GetComponent<VertigoEffect>();
		_colorCorrectionCurvesScript = _camera.GetComponent<ColorCorrectionCurves>();
		_noiseAndScratches = _camera.GetComponent<NoiseAndScratches>();
	}

	public void Update(){
		VortexEffect();
	}
	
	public void setVortexState(VortexState state){
		_vortexState = state;
	}

	public void setColorCorrectionCurves(bool b){
		_colorCorrectionCurvesScript.enabled = b;
	}

	public void setNoiseAndScratches(bool b){
		_noiseAndScratches.enabled = b;
	}

	private void VortexEffect(){

		switch(_vortexState){
			case VortexState.INC:
				if(_vortexScript.radius.x < _vortexMaxRadius && _vortexScript.radius.y < _vortexMaxRadius){
					_vortexScript.radius.x = _vortexScript.radius.y += _vortexSpeed * Time.deltaTime;
				}
				else{
					_vortexScript.radius.x = _vortexScript.radius.y = _vortexMaxRadius;
					_vortexState = VortexState.OFF;
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

			case VortexState.OFF: break;
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
}