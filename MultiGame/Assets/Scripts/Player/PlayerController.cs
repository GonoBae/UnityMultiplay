using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject _camHolder;
	[SerializeField] Transform _characterNeck;
	[SerializeField] float _mouseSensitivity, _walkSpeed;
	private Rigidbody _rb;
	private float _verticalLookRotation;
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}
	
	private void Update()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * _mouseSensitivity);
		_verticalLookRotation += Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
		_verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -60, 45);
		_camHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
		Debug.Log(_characterNeck.localEulerAngles);
	}
}
