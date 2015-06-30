using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent (typeof(GazePointDataComponent))]
public class ThrowObject : MonoBehaviour {
	
	private GazePointDataComponent _gazePointDataComponent;
	private string GlowStickPrefab = "Prefabs/Items/GlowStick";
	private static GameObject objectToThrow;
	private float force = 2000.0f;
	private Vector3 trajectory;
	private bool throwing;
	
	public void Start(){
		_gazePointDataComponent = GetComponent<GazePointDataComponent>();
	}
	
	public void Update(){
		
		var gazePoint = _gazePointDataComponent.LastGazePoint;
		
		if(gazePoint.IsValid && gazePoint.IsWithinScreenBounds){

			if(Input.GetButtonDown("Throw")){
				if(Inventory.Instance.glowStickStash > 0 && objectToThrow == null){
					objectToThrow = Instantiate(Resources.Load (GlowStickPrefab), transform.position, Quaternion.identity) as GameObject;
					objectToThrow.name = "Glowstick";
					Inventory.Instance.removeGlowStick(1);
				}
				if(objectToThrow != null){
					throwing = true;
				
					Ray ray = Camera.main.ScreenPointToRay(gazePoint.Screen);
					RaycastHit hit;
					
					if (Physics.Raycast (ray, out hit)){
						trajectory = hit.point - objectToThrow.transform.position;
						trajectory.y += 3.0f;
					}
				}
			}
		}
	}
	
	public void FixedUpdate(){
		if(objectToThrow && throwing){
			StartCoroutine(Vibration());
			Rigidbody rb = objectToThrow.GetComponent<Rigidbody>();
			if(rb.isKinematic) rb.isKinematic = false;
			rb.AddForce(trajectory * force * Time.fixedDeltaTime);
			objectToThrow = null;
			throwing = false;
		}
	}

	public static void setObjectToThrow(GameObject go){
		objectToThrow = go;
	}
	
	private IEnumerator Vibration(){
		GamePad.SetVibration(0, 0.5f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		GamePad.SetVibration(0, 0.0f, 0.0f);
	}
}