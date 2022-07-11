using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

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
	private PlayerInput _playerInput = new PlayerInput();
	
	private string _gameVersion = "1";
	
	private Transform _roomListContent;
	private GameObject _roomListItemPrefab;
	private List<RoomListItem> _lstRoom = new List<RoomListItem>();
	private Queue<RoomListItem> _waitingRoom = new Queue<RoomListItem>();
	private List<PlayerListItem> _lstPlayer = new List<PlayerListItem>();
	
	[SerializeField] private Menu _currentMenu;
	
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
	
	public override void OnJoinedLobby()
	{
		if(PhotonNetwork.NickName == "") MenuManager._Instance.OpenMenu("CreationNickNameMenu");
		else MenuManager._Instance.OpenMenu("TitleMenu");
	}
	
	/********** 내가 방에 들어가고 나올 때 **********/
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
		
		StartCoroutine("Co_ChatMethod");
		
		// 방에 있는 플레이어 체크
		Player[] players = PhotonNetwork.PlayerList;
		
		if(_lstPlayer.Count() >= players.Count())
		{
			// 그대로 사용
			for(int i = 0; i < players.Count(); i++)
			{
				_lstPlayer[i].SetUp(players[i]);
				_lstPlayer[i].gameObject.SetActive(true);
			}
		}
		else
		{
			// Instantiate
			for(int i = 0; i < players.Count(); i++)
			{
				if(i <_lstPlayer.Count() && _lstPlayer.Count() != 0)
				{
					_lstPlayer[i].SetUp(players[i]);
					_lstPlayer[i].gameObject.SetActive(true);
				}
				else
				{
					PlayerListItem item = Instantiate
						(_mainUI._PlayerListItemPrefab, _mainUI._PlayerListContent)
						.GetComponent<PlayerListItem>();
					item.SetUp(players[i]);
					_lstPlayer.Add(item);
				}
			}
		}
		_mainUI._StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}
	
	public override void OnLeftRoom()
	{
		StopCoroutine("Co_ChatMethod");
		_mainUI.ResetRoom();
		
		// PlayerList 초기화
		foreach(Transform child in _mainUI._PlayerListContent)
		{
			// 풀 적용
		}
		
		// Chatting List 초기화
		foreach(Transform child in _mainUI._ChatBoxContent)
		{
			// 풀 적용
		}
		
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
		// Player List 갱신
		_mainUI.CurrentPlayerUpdate(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
		
		PlayerListItem item;
		if(_lstPlayer.Count() < PhotonNetwork.CurrentRoom.PlayerCount)
		{
			item = Instantiate(_mainUI._PlayerListItemPrefab, _mainUI._PlayerListContent).GetComponent<PlayerListItem>();
			_lstPlayer.Add(item);
		}
		else
		{
			item = _lstPlayer[PhotonNetwork.CurrentRoom.PlayerCount - 1];
			item.gameObject.SetActive(true);
		}
		item.SetUp(newPlayer);
	}
	
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// Player List 갱신
		_mainUI.CurrentPlayerUpdate(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
		
		int index = _lstPlayer.FindIndex(x => x._Player == otherPlayer);
		PlayerListItem item = _lstPlayer[index];
		item.UnSet();
		item.transform.SetAsLastSibling();
		item.gameObject.SetActive(false);
		_lstPlayer.RemoveAt(index);
		_lstPlayer.Add(item);
		
		// Chat List 갱신
		
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
	
	public void JoinRoom(RoomInfo info)
	{
		if(info.PlayerCount < info.MaxPlayers)
		{
			PhotonNetwork.JoinRoom(info.Name);
			MenuManager._Instance.OpenMenu("LoadingMenu");
		}
		else
		{
			// 에러 (정원초과. 입장할 수 없습니다.)
		}
	}
	
	// 룸 입장하면 채팅 메소드 처리하기
	private IEnumerator Co_ChatMethod()
	{
		while(_currentMenu._MenuName == "RoomMenu")
		{
			if (_playerInput._PressEnter)
			{
				if (_mainUI._ChatInputField.text != "")
				{
					Send();
					_mainUI._ChatInputField.ActivateInputField();
				}
                else
                {
					if(!_mainUI._ChatInputField.isFocused)
                    {
						_mainUI._ChatInputField.ActivateInputField();
					}
                }
			}
			yield return null;
		}
	}
	
	private void Send()
	{
		if(_mainUI._ChatInputField.text == "") return;
		string msg = PhotonNetwork.NickName + " : " + _mainUI._ChatInputField.text;
		_pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId + '\t' + PhotonNetwork.NickName + " : " + _mainUI._ChatInputField.text);
		_mainUI._ChatInputField.text = "";
	}
	
	[PunRPC]
	private void ChatRPC(string msg)
	{
		bool isFull = false;
		string[] words = msg.Split('\t');
		for(int i = 0; i < _mainUI._ChatText.Length; i++)
		{
			if(_mainUI._ChatText[i].text == "")
			{
				isFull = true;
				_mainUI._ChatText[i].text = words[1];
				_mainUI._ChatText[i].color = 
					words[0].StartsWith(PhotonNetwork.LocalPlayer.UserId)
					&& words[1].Contains(PhotonNetwork.NickName)
					? Color.green : Color.white;
				break;
			}
		}
		if(!isFull)
		{
			UnityEngine.UI.Text box = Instantiate(_mainUI._ChatBoxPrefab, _mainUI._ChatBoxContent).GetComponentInChildren<UnityEngine.UI.Text>();
			box.transform.SetAsLastSibling();
			box.text = words[1];
			box.color = words[0].StartsWith(PhotonNetwork.LocalPlayer.UserId) && words[1].Contains(PhotonNetwork.NickName)
				? Color.green : Color.white;
			Invoke("ScrollDelay", 0.03f);
		}
	}
	
	void ScrollDelay() => _mainUI._Scroll.verticalScrollbar.value = 0;
}
