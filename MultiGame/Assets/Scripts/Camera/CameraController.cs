using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
	private PhotonView _pv;
	private CinemachineVirtualCamera _mainCam;
	//private CinemachineVirtualCamera _closerCam;
	//private CinemachineVirtualCamera _mostCloseCam;
	//private float _maxDistance = 10f;
	//private float _middleDistance = 5f;
	//private RaycastHit hit;
	//private RaycastHit otherHit;
	
	private void Start()
	{
		_pv = GetComponent<PhotonView>();
		if(_pv.IsMine)
		{
			_mainCam = VirtualCamManager._Instance._VCharCam;
			_mainCam.Follow = this.transform.GetChild(transform.childCount - 1);
			_mainCam.LookAt = this.transform.GetChild(transform.childCount - 1);
		
			//_closerCam = VirtualCamManager._Instance._VCam2;
			//_closerCam.Follow = this.transform;
			//_closerCam.LookAt = this.transform;
		
			//_mostCloseCam = VirtualCamManager._Instance._VCam3;
			//_mostCloseCam.Follow = this.transform;
			//_mostCloseCam.LookAt = this.transform;
		}
	}
	
	//private void Update()
	//{
	//	if(_pv.IsMine)
	//	{
	//		Debug.DrawRay(transform.position, (_mainCam.transform.position - transform.position).normalized * _maxDistance, Color.red, 0.1f);
	//		if(Physics.Raycast(transform.position, (_mainCam.transform.position - transform.position).normalized, out hit, _maxDistance))
	//		{
	//			if(hit.transform.tag != "Player")
	//			{
	//				Debug.Log("Not");
	//				_mainCam.Priority = 0;
	//				_closerCam.Priority = 1;
	//				_mostCloseCam.Priority = 0;
				
	//				if(Physics.Raycast(transform.position, (_closerCam.transform.position - transform.position).normalized, out otherHit, _middleDistance))
	//				{
	//					if(hit.transform.tag != "Player")
	//					{
	//						Debug.Log("Not");
	//						_mainCam.Priority = 0;
	//						_closerCam.Priority = 0;
	//						_mostCloseCam.Priority = 1;
	//					}
	//				}
	//			}
	//		}
	//		else
	//		{
	//			_mainCam.Priority = 1;
	//			_closerCam.Priority = 0;
	//			_mostCloseCam.Priority = 0;
	//			Debug.Log("Yes");
	//		}
	//	}
	//}
}
