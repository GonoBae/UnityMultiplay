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
	private PlayerInput _playerInput = new PlayerInput();
	private string _gameVersion = "1";

	private Menu _currentMenu;
	private RoomMenu _room;
	private FindRoomMenu _findRoom;
	
	public Menu _CurrentMenu { get{return _currentMenu;} set{_currentMenu = value;} }
	public RoomMenu _Room { get{return _room;} }
	public PlayerInput _PlayerInput { get{return _playerInput;} }
	
	[SerializeField] List<GameObject> objs;
	
	/***********************************
				Unity Events
	***********************************/
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		_room = FindObjectOfType<RoomMenu>(true);
		_findRoom = FindObjectOfType<FindRoomMenu>(true);
	}
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(4.5f);
		PhotonNetwork.GameVersion = _gameVersion;
		
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
		MenuManager._Instance.OpenMenu("LoadingMenu");
		PhotonNetwork.ConnectUsingSettings();
	}
	
	public override void OnJoinedLobby()
	{
		if(PhotonNetwork.NickName == "") MenuManager._Instance.OpenMenu("CreationNickNameMenu");
		else MenuManager._Instance.OpenMenu("TitleMenu");
	}
	
	/********** 방 리스트 업데이터 **********/
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		_findRoom.UpdateRoomList(roomList);
	}
	
	/********** 내가 방에 들어가고 나올 때 **********/
	public override void OnJoinedRoom()
	{
		MenuManager._Instance.OpenMenu("RoomMenu");
		_findRoom.ResetRoomList();
		
		StartCoroutine(_room.Co_ChatMethod());
		
		// 방에 있는 플레이어 체크
		_room.IEnterPlayerUpdate();
		
		_room._StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}
	
	public override void OnLeftRoom()
	{
		StopCoroutine(_room.Co_ChatMethod());
		
		// Player List 초기화
		_room.IOutPlayerUpdate();
		
		// Chatting List 초기화
		_room.IOutChatUpdate();
		
		MenuManager._Instance.OpenMenu("TitleMenu");
	}
	
	
	
	/********** 다른 플레이어가 방에 들어오고 나갈 때 **********/
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		_room.NewEnteredPlayerUpdate(newPlayer);
	}
	
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// Player List 갱신
		_room.OtherOutPlayerUpdate(otherPlayer);
	}
}
