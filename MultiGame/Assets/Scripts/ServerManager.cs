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
	
	private Transform _roomListContent;
	private GameObject _roomListItemPrefab;
	[SerializeField] List<RoomListItem> _lstRoom = new List<RoomListItem>();
	private Queue<RoomListItem> _waitingRoom = new Queue<RoomListItem>();
	private Menu _currentMenu;
	
	public Menu _CurrentMenu { get{return _currentMenu;} set{_currentMenu = value;} }
	
	[SerializeField] List<GameObject> objs;
	
	
	/***********************************
				Unity Events
	***********************************/
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(4.5f);
		PhotonNetwork.GameVersion = _gameVersion;
		_mainUI = MenuManager._Instance._MainUI;
		_roomListContent = _mainUI._RoomListContent;
		_roomListItemPrefab = _mainUI._RoomListItemPrefab;
		
		foreach (var obj in objs)
		{
			obj.gameObject.SetActive(true);
		}
		
		yield return new WaitForSeconds(1.5f);
		
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
	
	/********** 내가 방에 들어가고 나올 때 **********/
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
		
		// 방에 입장하면 룸 목록 모두 초기화
		RoomListItem[] allRoom = _roomListContent.GetComponentsInChildren<RoomListItem>();
		foreach(var room in allRoom)
		{
			RoomItemUnSet(room);
		}
		_lstRoom.Clear();
		
		
		Player[] players = PhotonNetwork.PlayerList;
		
		_mainUI._StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}
	
	public override void OnLeftRoom()
	{
		_mainUI.ResetRoom();
		MenuManager._Instance.OpenMenu("TitleMenu");
	}
	
	/********** 방 리스트 업데이터 **********/
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach(RoomInfo info in roomList)
		{
			// 제거
			if(info.RemovedFromList)
			{
				int index = _lstRoom.FindIndex(x => x._Info.Name == info.Name);
				// 있으면
				if(index != -1)
				{
					RoomListItem room = _lstRoom[index];
					RoomItemUnSet(room);
					_lstRoom.RemoveAt(index);
				}
			}
			// 추가
			else
			{
				int index = _lstRoom.FindIndex(x => x._Info.Name == info.Name);
				// 없으면
				if(index == -1)
				{
					RoomListItem item;
					if(_waitingRoom.Count > 0)
					{
						item = _waitingRoom.Dequeue();
						item.SetUp(info);
						item.gameObject.SetActive(true);
					}
					else
					{
						item = Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>();
						if(item != null)
						{
							item.SetUp(info);
						}
					}
					_lstRoom.Add(item);
				}
			}
		}
	}
	
	/********** 다른 플레이어가 방에 들어오고 나갈 때 **********/
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		
	}
	
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		
	}
	
	/***********************************
				Functions
	***********************************/
	// 룸 목록 수정
	private void RoomItemUnSet(RoomListItem room)
	{
		room.UnSet();
		_waitingRoom.Enqueue(room);
		room.transform.SetAsLastSibling();
		room.gameObject.SetActive(false);
	}
}
