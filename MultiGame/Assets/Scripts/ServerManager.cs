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
	private MainUI _mainUI;
	
	private string _gameVersion = "1";
	private List<RoomListItem> _lstRoom = new List<RoomListItem>();
	
	/***********************************
				Unity Events
	***********************************/
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		PhotonNetwork.GameVersion = _gameVersion;
		_mainUI = MenuManager._Instance._MainUI;
		
		if(PhotonNetwork.IsConnected)
		{
			Cursor.visible = true;
			MenuManager._Instance.OpenMenu("LoadingMenu");
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
		if(PhotonNetwork.NickName == "") MenuManager._Instance.OpenMenu("CreationNickNameMenu");
		else MenuManager._Instance.OpenMenu("TitleMenu");
	}
	
	public override void OnJoinedRoom()
	{
		_mainUI.ResetCreateRoom();
		MenuManager._Instance.OpenMenu("RoomMenu");
		_mainUI.SetRoom();
		
		Player[] players = PhotonNetwork.PlayerList;
	}
	
	public override void OnLeftRoom()
	{
		_mainUI.ResetRoom();
		MenuManager._Instance.OpenMenu("TitleMenu");
	}
	
	//public override void OnRoomListUpdate(List<RoomInfo> roomList)
	//{
	//	foreach(RoomInfo info in roomList)
	//	{
	//		// 제거
	//		if(info.RemovedFromList)
	//		{
				
	//		}
	//		// 추가
	//		else
	//		{
	//			int index = _lstRoom.FindIndex(x => x._Info.Name == info.Name);
	//			if(index == -1)
	//			{
	//				RoomListItem item = Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>();
	//				if(item != null)
	//				{
	//					item.SetUp(info);
	//					_lstRoom.Add(item);
	//				}
	//			}
	//		}
	//	}
	//}
	
	/***********************************
				Functions
	***********************************/
	
}
