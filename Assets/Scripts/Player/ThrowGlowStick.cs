using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent (typeof(GazePointDataComponent))]
public class ThrowGlowStick : MonoBehaviour {
	
	private GazePointDataComponent _gazePointDataComponent;
	private string prefab = "Prefabs/Items/GlowStick";
	private GameObject glowStick;
	private float force = 2000.0f;
	private Vector3 trajectory;

	public void Start(){
		_gazePointDataComponent = GetComponent<GazePointDataComponent>();
	}
	
	public void Update(){

		var gazePoint = _gazePointDataComponent.LastGazePoint;

		if(gazePoint.IsValid && gazePoint.IsWithinScreenBounds){

			if(Input.GetButtonDown("Throw") && Inventory.Instance.glowStickStash > 0){

				StartCoroutine(Vibration());

				glowStick = Instantiate(Resources.Load (prefab), transform.position, Quaternion.identity) as GameObject;
				glowStick.name = "Glowstick";

				Ray ray = Camera.main.ScreenPointToRay(gazePoint.Screen);
				RaycastHit hit;
				
				if (Physics.Raycast (ray, out hit)){
					trajectory = hit.point - glowStick.transform.position;
					trajectory.y += 3.0f;
				}

				Inventory.Instance.removeGlowStick(1);
			}
		}
	}

	public void FixedUpdate(){
		if(glowStick){
			glowStick.GetComponent<Rigidbody>().AddForce(trajectory * force * Time.fixedDeltaTime);
			glowStick = null;
		}
	}

	private IEnumerator Vibration(){
		GamePad.SetVibration(0, 0.5f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		GamePad.SetVibration(0, 0.0f, 0.0f);
	}
}