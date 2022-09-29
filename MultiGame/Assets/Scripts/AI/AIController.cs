using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
	[SerializeField] private PlayerType _type = PlayerType.AI;
	
	private float _rayDistance = 5.0f;
	private float _stoppingDistance = 1.5f;
	private Vector3 _direction;
	
	private void Update()
	{
		Debug.DrawRay(transform.position, _direction * _rayDistance, Color.red);
	}
}
