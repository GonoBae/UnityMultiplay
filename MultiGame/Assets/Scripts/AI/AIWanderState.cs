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

	public readonly LayerMask _layerMask;
	
	public AIWanderState(AI ai) : base(ai.gameObject)
	{
		_ai = ai;
	}
	
	public override Type Tick()
	{
		//CheckForAggro();
		Debug.DrawRay(_transform.position + new Vector3(0, 1, 0), _direction * _rayDistance, Color.green);
		
		if(_destination.HasValue == false || Vector3.Distance(_transform.position, _destination.Value) <= _stopDistance)
		{
			FindRandomDestination();
		}

		_transform.rotation = Quaternion.Slerp(_transform.rotation, _desiredRotation, Time.deltaTime * _turnSpeed);

		if(IsForwardBlocked())
        {
			FindRandomDestination();
        }
		else
        {

        }

		

		return null;
	}

	// 랜덤한 위치 설정
	private void FindRandomDestination()
    {
		Vector3 pos = (_transform.position + (_transform.forward * 4f))
			+ new Vector3(UnityEngine.Random.Range(-10f, 10), 0f, UnityEngine.Random.Range(-10f, 10));

		_destination = new Vector3(pos.x, 0f, pos.z);

		_direction = Vector3.Normalize(_destination.Value - _transform.position);
		_direction = new Vector3(_direction.x, 0f, _direction.z);
		_desiredRotation = Quaternion.LookRotation(_direction);
    }

	private bool IsForwardBlocked()
    {
		Ray ray = new Ray(_transform.position + new Vector3(0, 1, 0), _direction);
		return Physics.SphereCast(ray, 0.5f, _rayDistance, _layerMask);
    }

	Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);

	private void CheckForAggro()
    {
		RaycastHit hit;
		var angle = _transform.rotation * startingAngle;
		var dir = angle * Vector3.forward;
		var pos = _transform.position + new Vector3(0, 1, 0);
		for(var i = 0; i < 24; i++)
        {
			if(Physics.Raycast(pos, dir, out hit, AISettings.AggroRadius))
            {
				Debug.DrawRay(pos, dir * hit.distance, Color.green);
            }
        }
    }
}
