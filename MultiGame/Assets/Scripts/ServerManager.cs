using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ServerManager : MonoBehaviourPunCallbacks
{
	/***********************************
				Singleton
	***********************************/
	private static ServerManager _instance;
	public static ServerManager _Instance
	{
		get
		{
			if(!_instance) _instance = FindObjectOfType(typeof(ServerManager)) as ServerManager;
			return _instance;
		}
	}
	
	/***********************************
				Fields
	***********************************/
	private PhotonView _pv;
	
	private string _gameVersion = "1";
	
	/***********************************
				Unity Events
	***********************************/
	private void Start()
	{
		PhotonNetwork.GameVersion = _gameVersion;
		if(PhotonNetwork.IsConnected)
		{
			
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
	}
	
	/***********************************
				Photon
	***********************************/
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}
	
	public override void OnDisconnected(DisconnectCause cause)
	{
		PhotonNetwork.ConnectUsingSettings();
	}
	
	public override void OnJoinedLobby()
	{
		Debug.Log("Lobby");
	}
}
