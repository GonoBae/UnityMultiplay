using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Cinemachine;

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

	[SerializeField] private Menu _currentMenu;
	private RoomMenu _room;
	private FindRoomMenu _findRoom;
	
	public Menu _CurrentMenu { get{return _currentMenu;} set{_currentMenu = value;} }
	public RoomMenu _Room { get{return _room;} }
	
	[SerializeField] List<GameObject> objs;
	
	[Header("Virtual Cam")]
	[SerializeField] private CinemachineVirtualCamera _vCam1;
	
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
		PhotonNetwork.GameVersion = _gameVersion;
		_vCam1.gameObject.SetActive(true);
		if(HelperManager._Instance._first) // 처음 시작할 때
		{
			yield return new WaitForSeconds(4.5f);
			
			foreach (var obj in objs)
			{
				obj.gameObject.SetActive(true);
			}
			yield return new WaitForSeconds(1.5f);
			
			MenuManager._Instance.OpenMenu("LoadingMenu");
		}
		else // 게임에서 나왔을 때
		{
			yield return null;
			foreach (var obj in objs)
			{
				obj.gameObject.SetActive(true);
			}
		}
		
		if(PhotonNetwork.IsConnected)
		{
			Cursor.visible = true;
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
		// 자동으로 모든 사람들의 Scene을 통일
		PhotonNetwork.AutomaticallySyncScene = true;
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
		if(_pv.IsMine) _room.EnterIntro(PhotonNetwork.NickName);
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
		if(_pv.IsMine) _room.EnterIntro(newPlayer.NickName);
	}
	
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// Player List 갱신
		_room.OtherOutPlayerUpdate(otherPlayer);
		if(_pv.IsMine) _room.LeftIntro(otherPlayer.NickName);
	}
	
	/********** 방장이 나가서 방장이 바뀌면 **********/
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		_room.OnMasterSwitched();
	}
}

