using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GazePointDataComponent))]
public class ThrowObject : MonoBehaviour {

	private GazeAwareComponent _gazeAwareComponent;
	private GazePointDataComponent _gazePointDataComponent;
	private string FlareStickPrefab = "Prefabs/CollectibleItems/FlareStick";
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

			if(Input.GetButtonUp("Throw")){
				if(Inventory.Instance.FlareStickStash > 0 && objectToThrow == null){
					objectToThrow = Instantiate(Resources.Load (FlareStickPrefab), transform.position, Quaternion.identity) as GameObject;
					objectToThrow.name = "FlareStick";
					Inventory.Instance.removeFlareStick(1);
				}
				if(objectToThrow != null){
					throwing = true;
				
					Ray ray = Camera.main.ScreenPointToRay(gazePoint.Screen);
					RaycastHit hit;
					
					if (Physics.Raycast (ray, out hit, 500.0f, 11)){
						trajectory = hit.point - objectToThrow.transform.position;
						trajectory.y += 3.0f;
					}
				}
			}
		}
	}
	
	public void FixedUpdate(){
		if(objectToThrow && throwing){
			StartCoroutine(GameController.Instance.timedVibration(0.3f, 0.3f, 0.5f));
			Rigidbody rb = objectToThrow.GetComponent<Rigidbody>();
			if(rb.isKinematic) rb.isKinematic = false;
			rb.AddForce(trajectory * force * Time.fixedDeltaTime);
			objectToThrow = null;
			throwing = false;
		}
	}

	public static bool setObjectToThrow(GameObject go){
		if(objectToThrow == null){
			objectToThrow = go;
			return true;
		}
		return false;	
	}
}