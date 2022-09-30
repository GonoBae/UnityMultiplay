using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	[SerializeField] private PlayerType _type = PlayerType.AI;
	
	private float _rayDistance = 5.0f;
	private float _stoppingDistance = 1.5f;
	private Vector3 _direction;
	
	private void Update()
	{
		Debug.DrawRay(transform.position + new Vector3(0, 1, 0), _direction * _rayDistance, Color.red);
	}
}
