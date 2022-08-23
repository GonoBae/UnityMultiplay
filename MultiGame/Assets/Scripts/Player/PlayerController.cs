using UnityEngine;
using Photon.Pun;
using MoreMountains.Tools;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float _mouseSensitivity, _walkSpeed;
	private Rigidbody _rb;
	
	private PhotonView _pv;
	private MMTouchJoystick _joyStick;
	
	private float _horizontal;
	private float _vertical;
	private float _angle;
	
	[SerializeField] private LayerMask _groundLayer;
	
	public Vector2 _PlayerInput {get{return new Vector2(_horizontal, _vertical);}}
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_pv = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		if(!_pv.IsMine)
		{
			Destroy(_rb);
		}
		else _joyStick = FindObjectOfType<MMTouchJoystick>();
	}
	
	private void Update()
	{
		if(_pv.IsMine)
		{
			if(_joyStick._joystickValue != Vector2.zero)
			{
				Movement(_joyStick._joystickValue);
			}
			else
			{
				Movement(Vector2.zero);
			}
		}
	}
	
	private void FixedUpdate()
	{
		if(_pv.IsMine)
		{
			Vector3 movement = new Vector3(_horizontal, 0, _vertical) * _walkSpeed * Time.fixedDeltaTime;
			
			if(_angle != 0)
			{
				float cur = this.transform.eulerAngles.y;
				float tar = _angle;
				float yrot = Mathf.Lerp(cur, tar, 10) % 360;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, yrot, transform.eulerAngles.z);
			}
			
			if(movement.sqrMagnitude != 0)
			{
				_rb.MovePosition(_rb.position + movement);
			}
		}
	}
	
	public void Movement(Vector2 move)
	{
		_horizontal = move.x;
		_vertical = move.y;
		// 현재 조이스틱의 각도 (앞 : 0 , 좌 : -90 , 뒤 : -180 , 우 : 90)
		_angle = Mathf.Atan2(_horizontal, _vertical) * Mathf.Rad2Deg;
		if(_angle < 0)
		{
			_angle += 360;
		}
	}
	
	private void OnCollisionExit(Collision collisionInfo)
	{
		if(_pv.IsMine)
		{
			_rb.velocity = Vector3.zero;
			_rb.angularVelocity = Vector3.zero;
			_rb.ResetCenterOfMass();
		}
	}
}
