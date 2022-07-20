using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HelperManager : MonoBehaviour
{
	#region Singleton
	
	private static HelperManager _instance;
	public static HelperManager _Instance
	{
		get
		{
			if(!_instance) _instance = FindObjectOfType(typeof(HelperManager)) as HelperManager;
			return _instance;
		}
	}
	
	#endregion
	
	private PhotonView _pv;
	public PhotonView _Pv { get{return _pv;} }
	
	private void Start()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	[PunRPC]
	public void KickPlayer()
	{
		PhotonNetwork.LeaveRoom();
	}
}
