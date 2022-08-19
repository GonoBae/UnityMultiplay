using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamManager : MonoBehaviour
{
	private CinemachineVirtualCamera _vCam1;
	private CinemachineVirtualCamera _vCam2;
	private CinemachineVirtualCamera _vCam3;
	
	public CinemachineVirtualCamera _VCam1 {get{return _vCam1;}}
	public CinemachineVirtualCamera _VCam2 {get{return _vCam2;}}
	public CinemachineVirtualCamera _VCam3 {get{return _vCam3;}}
	
	private static VirtualCamManager _instance = null;
	public static VirtualCamManager _Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<VirtualCamManager>();
			}
			return _instance;
		}
	}
	
	private void Awake()
	{
		_instance = this;
		
		_vCam1 = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
		_vCam2 = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
		_vCam3 = transform.GetChild(2).GetComponent<CinemachineVirtualCamera>();
	}
}
