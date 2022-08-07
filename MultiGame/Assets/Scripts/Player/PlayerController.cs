using UnityEngine;
using Photon.Pun;
using MoreMountains.Tools;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject _camHolder;
	[SerializeField] Transform _characterNeck;
	[SerializeField] float _mouseSensitivity, _walkSpeed;
	private Rigidbody _rb;
	private Animator _ani;
	private PhotonView _pv;
	private MMTouchJoystick _joyStick;
	
	private float _aniTime;
	private float _horizontal;
	private float _vertical;
	private float mx;
	private float my;
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_ani = GetComponent<Animator>();
		_pv = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		if(!_pv.IsMine)
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(_rb);
		}
		else _joyStick = FindObjectOfType<MMTouchJoystick>();
	}
	
	private void Update()
	{
		if(_pv.IsMine)
		{
			//mx = Input.GetAxisRaw("Mouse X");
		
			//if(mx != 0)
			//{
			//	transform.Rotate(Vector3.up * mx * _mouseSensitivity);
			//}
			
			if(Input.GetKey(KeyCode.Space))
			{
				_ani.SetBool("Walk", true);
			}
			else if(Input.GetKeyUp(KeyCode.Space))
			{
				_ani.SetBool("Walk", false);
			}
			if(_joyStick._joystickValue != Vector2.zero) Movement(_joyStick._joystickValue);
			else Movement(Vector2.zero);
		}
	}
	
	private void FixedUpdate()
	{
		if(_pv.IsMine)
		{
			Vector3 movement = new Vector3(_horizontal, 0, _vertical) * _walkSpeed * Time.fixedDeltaTime;
			if(movement.sqrMagnitude == 0)
			{
				_ani.SetBool("Walk", false);
				return;
			}
			_rb.MovePosition(_rb.position + movement);
			_ani.SetBool("Walk", true);
		}
	}
	
	public void Movement(Vector2 move)
	{
		_horizontal = move.x;
		_vertical = move.y;
	}
}
