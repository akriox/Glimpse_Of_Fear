using UnityEngine;
using System.Collections;

public class ItemGlow : MonoBehaviour {

	public float minValue = 1.0f;
	public float maxValue = 6.0f;
	public float speed = 4.0f;

	private Renderer _renderer;
	private enum State {INC, DEC};
	private State _state;
	
	public void Start () {
		_renderer = GetComponent<Renderer>();
		_state = State.INC;
	}

	public void Update () {
		float value = _renderer.material.GetFloat("_RimPower");

		if(value >= maxValue) _state = State.DEC;
		else if(value <= minValue) _state = State.INC;

		switch(_state){
			case State.INC: value += speed * Time.deltaTime; break;
			case State.DEC: value -= speed * Time.deltaTime; break;
		}

		_renderer.material.SetFloat("_RimPower", value);
	}
}