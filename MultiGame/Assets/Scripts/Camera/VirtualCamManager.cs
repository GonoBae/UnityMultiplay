using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamManager : MonoBehaviour
{
	private CinemachineVirtualCamera _vCharCam;
	private CinemachineVirtualCamera _vBackCam;
	
	public CinemachineVirtualCamera _VCharCam { get{return _vCharCam;} }
	public CinemachineVirtualCamera _VBackCam { get{return _vBackCam;} }
	
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
		
		_vCharCam = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
		_vBackCam = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
	}
}
