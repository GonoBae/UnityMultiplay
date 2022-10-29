using System.Collections;
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
	
	private Vector3 _direction;
	
	private FieldOfView _fow;
	private Quaternion _target;
	private float _rotSpeed;
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_ani = GetComponent<Animator>();
		_fow = GetComponent<FieldOfView>();
	}
	
	private void Start()
	{
		if(Mathf.Approximately(0.11f, 0.11f)) {
			ChangeState(AIState.Idle);
		}
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
		int changeTime = Random.Range(1, 5);
		yield return new WaitForSeconds(changeTime);
		
		if(_fow.visibleObstacle.Count >= 3) _target = GetRandomBackRot();
		else _target = GetRandomRot();
		ChangeState(AIState.Wander);
		//Quaternion rot = GetRandomRot();
		
		//while(true)
		//{
			// 다른 방향으로 회전
			//transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * 5f);
			
			//float a = Mathf.Abs((float)System.Math.Truncate(transform.rotation.y * 100) / 100);
			//float b = (float)System.Math.Truncate(rot.y * 100) / 100;
			
			//if(Mathf.Approximately(a, b))
			//{
			//	if(_fow.visibleObstacle.Count <= 0) ChangeState(AIState.Wander);
			//	else 
			//	{
			//		rot = GetRandomRot();
			//		changeTime = Random.Range(1, 3);
			//		yield return new WaitForSeconds(changeTime);
			//	}
			//}
			
		//	yield return null;
		//}
	}
	
	Quaternion GetRandomRot()
	{
		_rotSpeed = 0.3f;
		return Quaternion.Euler(0, Random.Range(0, 360) + transform.rotation.y, 0);
	}
	
	Quaternion GetRandomBackRot()
	{
		_rotSpeed = 1f;
		return Quaternion.Euler(0, Random.Range(90, 270) + transform.rotation.y, 0);
	}
	
	private IEnumerator Wander()
	{
		// 애니메이션 Walk
		_ani.SetBool("Walk", true);
		float randomTime = 0;
		float limitTime = Random.Range(2, 15);
		
		while(true)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, _target, Time.fixedDeltaTime * _rotSpeed);
			// 앞으로 걷기
			_rb.MovePosition(transform.position + 
				transform.forward * AISettings.AISpeed * Time.fixedDeltaTime * 4);
				
			if(randomTime > limitTime) {
				ChangeState(AIState.Idle);
			}
			randomTime += Time.fixedDeltaTime;
			yield return null;
		}
	}
}
