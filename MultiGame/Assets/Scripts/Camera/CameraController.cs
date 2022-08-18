using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	private CinemachineVirtualCamera _mainCam;
	private float _maxDistance = 10f;
	private RaycastHit hit;
	
	private void Start()
	{
		_mainCam = FindObjectOfType<CinemachineVirtualCamera>();
		_mainCam.Follow = this.transform;
		_mainCam.LookAt = this.transform;
	}
	
	private void Update()
	{
		Debug.DrawRay(transform.position, (_mainCam.transform.position - transform.position).normalized * _maxDistance, Color.red, 0.1f);
		if(Physics.Raycast(transform.position, (_mainCam.transform.position - transform.position).normalized, out hit, _maxDistance))
		{
			if(hit.transform.tag != "Player")
			{
				Debug.Log("Not");
				_mainCam.transform.position = hit.point;
			}
		}
		else
		{
			Debug.Log("Yes");
		}
	}
}
