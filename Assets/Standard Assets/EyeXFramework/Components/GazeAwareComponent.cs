//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that encapsulates the <see cref="EyeXGazeAware"/> behavior.
/// </summary>
[AddComponentMenu("Tobii EyeX/Gaze Aware")]
public class GazeAwareComponent : EyeXGameObjectInteractorBase
{
	// Delay between first glance and gaze aware event response
	public int delayInMilliseconds;
	
	/// <summary>
	/// Gets a value indicating whether the user's eye-gaze is within the bounds of the interactor.
	/// </summary>
	public bool HasGaze { get; private set; }
	private int layerMask =  1 << 8;
	
	public void Start(){
		layerMask = ~layerMask;
	}
	
	protected override void Update()
	{
		base.Update();
		
		HasGaze = GameObjectInteractor.HasGaze() && checkVisibility();
	}
	
	protected override IList<IEyeXBehavior> GetEyeXBehaviorsForGameObjectInteractor()
	{
		return new List<IEyeXBehavior>(new[] { new EyeXGazeAware() { DelayTime = delayInMilliseconds }});
	}
	
	protected bool checkVisibility()
	{
		Debug.DrawLine(transform.position, Camera.main.transform.position, Color.white);
		return Physics.Linecast(transform.position, Camera.main.transform.position, layerMask) ? false : true;
	}
}