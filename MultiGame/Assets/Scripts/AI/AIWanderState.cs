using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIWanderState : AIBaseState
{
	private Vector3? _destination;
	private AI _ai;

	private float _stopDistance = 1f;
	private float _turnSpeed = 2f;
	private float _rayDistance = 2f;
	private Quaternion _desiredRotation;
	private Vector3 _direction;

	public readonly LayerMask _layerMask = LayerMask.NameToLayer("Obstacle");
	
	public AIWanderState(AI ai) : base(ai.gameObject)
	{
		_ai = ai;
	}
	
	public override Type Tick()
	{
		//CheckForAggro();
		Debug.DrawRay(_transform.position + new Vector3(0, 1, 0), _direction * _rayDistance, Color.green);
		
		
		
		return null;
	}
}
