using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AIStateMachine : MonoBehaviour
{
	private Dictionary<Type, AIBaseState> _availableStates;
	
	public AIBaseState _CurrentState { get; private set; }
	public event Action<AIBaseState> _OnStateChanged;
	
	public void SetStates(Dictionary<Type, AIBaseState> states)
	{
		_availableStates = states;
	}
	
	private void Update()
	{
		if(_CurrentState == null)
		{
			_CurrentState = _availableStates.Values.First();
		}
	}
	
	private void FixedUpdate()
	{
		var nextState = _CurrentState?.Tick();
		
		if(nextState != null && nextState != _CurrentState?.GetType())
		{
			SwitchToNewState(nextState);
		}
	}
	
	private void SwitchToNewState(Type nextState)
	{
		_CurrentState = _availableStates[nextState];
		_OnStateChanged?.Invoke(_CurrentState);
	}
}
