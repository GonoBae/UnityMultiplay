using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIWanderState : AIBaseState
{
	private Vector3? _destination;
	
	public override Type Tick()
	{
		if(_destination.HasValue == false)
		{
			return typeof(AIWanderState);
		}
		return typeof(AIWanderState);
	}
}
