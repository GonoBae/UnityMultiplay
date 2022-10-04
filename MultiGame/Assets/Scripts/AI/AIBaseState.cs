using UnityEngine;
using System;

public abstract class AIBaseState
{
	protected GameObject _gameObject;
	protected Transform _transform;
	
	public AIBaseState(GameObject gameObject)
	{
		this._gameObject = gameObject;
		this._transform = gameObject.transform;
	}
	
	public abstract Type Tick();
}
