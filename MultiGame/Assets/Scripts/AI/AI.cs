using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
	private Rigidbody _rb;
	public Rigidbody _Rb { get{return _rb;} }
	
	[SerializeField] private PlayerType _type = PlayerType.AI;
	[SerializeField] private bool _showGizmos;
	private Vector3 _direction;
	
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		
		InitializeStateMachine();
	}
	
	private void InitializeStateMachine()
	{
		var states = new Dictionary<Type, AIBaseState>()
		{
			{ typeof(AIWanderState), new AIWanderState(this) }
		};
		
		GetComponent<AIStateMachine>().SetStates(states);
	}
}
