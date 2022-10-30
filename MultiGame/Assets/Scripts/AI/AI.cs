﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public AIState _aiState = AIState.NONE;
	private Rigidbody _rb;
	private Animator _ani;
	public Rigidbody _Rb { get{return _rb;} }
	
	[SerializeField] private PlayerType _type = PlayerType.AI;
	[SerializeField] private bool _showGizmos;
	
	private FieldOfView _fow;
	private Quaternion _target;
	private float _rotSpeed = 30;
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_ani = GetComponent<Animator>();
		_fow = GetComponent<FieldOfView>();
	}
	
	private void Start()
	{
		ChangeState(AIState.Idle);
	}
	
	public void ChangeState(AIState newState)
	{
		if(_aiState == newState) return;
		StopCoroutine(_aiState.ToString());
		_aiState = newState;
		StartCoroutine(_aiState.ToString());
	}
	
	private IEnumerator Idle()
	{
		// 애니메이션 Idle
		_ani.SetBool("Walk", false);
		_rb.angularVelocity = Vector3.zero;
		int changeTime = Random.Range(3, 5);
		yield return new WaitForSeconds(changeTime);
		if(_fow._hitPoints.Count > 10) _target = GetRandomBackRot();
		else _target = GetRandomRot();
		ChangeState(AIState.Wander);
	}
	
	Quaternion GetRandomRot()
	{
		return Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
	}
	
	Quaternion GetRandomBackRot()
	{
		return Quaternion.AngleAxis(transform.eulerAngles.y + Random.Range(120, 240), Vector3.up);
	}
	
	private IEnumerator Wander()
	{
		// 애니메이션 Walk
		_ani.SetBool("Walk", true);
		float randomTime = 0;
		float limitTime = Random.Range(4, 25);
		//Debug.Log("방향 틀기");
		while(true)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, _target, Time.fixedDeltaTime * 30);
			// 앞으로 걷기
			_rb.MovePosition(transform.position + 
				transform.forward * AISettings.AISpeed * Time.fixedDeltaTime * 4);
				
			if(randomTime > limitTime) {
				//Debug.Log("ran : " + randomTime + "lim : " + limitTime + "Count : " + _fow._hitPoints.Count);
				ChangeState(AIState.Idle);
			}
			
			randomTime += Time.fixedDeltaTime;
			//Debug.Log("ran : " + randomTime + "lim : " + limitTime);
			yield return null;			
		}
	}
}
