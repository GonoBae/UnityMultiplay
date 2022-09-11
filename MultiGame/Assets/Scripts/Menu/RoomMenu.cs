using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class RoomMenu : Menu
{
	[Header("Room Info")]
	[SerializeField] Text _nickName;
	[SerializeField] Text _roomName;
	[SerializeField] Text _roomMaxPlayer;
	[SerializeField] Text _roomCurrentPlayer;
	[SerializeField] Transform _playerListContent;
	[SerializeField] GameObject _playerListItemPrefab;
	[SerializeField] GameObject _startGameButton;
	private List<PlayerListItem> _lstPlayer = new List<PlayerListItem>();
	
	[Header("Chat")]
	private const int _maximumChatCount = 30;
	[SerializeField] InputField _chatInputField;
	[SerializeField] List<ChatItem> _lstChat;
	[SerializeField] GameObject _chatBoxPrefab;
	[SerializeField] Transform _chatBoxContent;
	[SerializeField] ScrollRect _scroll;
	
	private PhotonView _pv;
	
	/***********************************
				Property
	***********************************/
	public Text _RoomCurrentPlayer { get{return _roomCurrentPlayer;} set{_roomCurrentPlayer = value;} }
	public Transform _PlayerListContent { get{return _playerListContent;}}
	public GameObject _PlayerListItemPrefab { get{return _playerListItemPrefab;}}
	
	public GameObject _StartGameButton { get{return _startGameButton;} }
	
	public InputField _ChatInputField { get{return _chatInputField;} }
	public List<ChatItem> _LstChat { get{return _lstChat;} }
	public GameObject _ChatBoxPrefab { get{return _chatBoxPrefab;} }
	public Transform _ChatBoxContent { get{return _chatBoxContent;} }
	public ScrollRect _Scroll { get{return _scroll;} }
	
	/***********************************
				Unity Events
	***********************************/
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	private void OnEnable()
	{
		SetRoom();
	}
	
	private void OnDisable()
	{
		ResetRoom();
	}
	
	// Room Settings
	public void SetRoom()
	{
		_nickName.text = PhotonNetwork.NickName;
		_roomName.text = PhotonNetwork.CurrentRoom.Name;
		_roomMaxPlayer.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
		_roomCurrentPlayer.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
	}
	
	public void ResetRoom()
	{
		_nickName.text = "";
		_roomName.text = "";
		_roomMaxPlayer.text = "";
		_roomCurrentPlayer.text = "";
	}
	
	// 내가 방에 들어갔을 때 플레이어 리스트 업데이트
	public void IEnterPlayerUpdate()
	{
		Player[] players = PhotonNetwork.PlayerList;
		if(_lstPlayer.Count() >= players.Count())
		{
			// Pool
			for(int i = 0; i < players.Count(); i++)
			{
				_lstPlayer[i].SetUp(players[i]);
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
				}
				else
				{
					PlayerListItem item = Instantiate
					(_playerListItemPrefab, _playerListContent)
						.GetComponent<PlayerListItem>();
					item.SetUp(players[i]);
					_lstPlayer.Add(item);
				}
			}
		}
	}
	
	// 내가 방에서 나갔을 때 플레이어 리스트 업데이트
	public void IOutPlayerUpdate()
	{
		foreach(Transform child in _playerListContent)
		{
			PlayerListItem item = child.GetComponent<PlayerListItem>();
			item.UnSet();
		}
	}
	
	// 내가 방에서 나갔을 떄 채팅 리스트 업데이트
	public void IOutChatUpdate()
	{
		foreach(var item in _lstChat)
		{
			item.UnSet();
		}
	}
	
	// 다른 플레이어가 방에 들어왔을 때 플레이어 리스트 업데이트
	public void NewEnteredPlayerUpdate(Player newPlayer)
	{
		CurrentPlayerUpdate(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
		PlayerListItem item;
		if(_lstPlayer.Count() < PhotonNetwork.CurrentRoom.PlayerCount)
		{
			item = Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>();
			_lstPlayer.Add(item);
		}
		else
		{
			item = _lstPlayer[PhotonNetwork.CurrentRoom.PlayerCount - 1];
			item.gameObject.SetActive(true);
		}
		item.SetUp(newPlayer);
	}
	
	// 다른 플레이어가 방에서 나갔을 떄 플레이어 리스트 업데이트
	public void OtherOutPlayerUpdate(Player otherPlayer)
	{
		CurrentPlayerUpdate(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
		
		int index = _lstPlayer.FindIndex(x => x._Player == otherPlayer);
		PlayerListItem item = _lstPlayer[index];
		item.UnSet();
		item.transform.SetAsLastSibling();
		item.gameObject.SetActive(false);
		_lstPlayer.RemoveAt(index);
		_lstPlayer.Add(item);
	}
	
	public void CurrentPlayerUpdate(string count)
	{
		_roomCurrentPlayer.text = count;
	}
	
	// 룸 입장하면 채팅 메소드 처리하기
	public IEnumerator Co_ChatMethod()
	{
		while(ServerManager._Instance._CurrentMenu._MenuName == "RoomMenu")
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				if (_chatInputField.text != "")
				{
					Send();
					_chatInputField.ActivateInputField();
				}
				else
				{
					if(!_chatInputField.isFocused)
					{
						_chatInputField.ActivateInputField();
					}
				}
			}
			yield return null;
		}
	}
	
	public void EnterIntro(string playerID)
	{
		_pv.RPC("IntroRPC", RpcTarget.All, playerID + "님이 참가하셨습니다.");
	}
	
	public void LeftIntro(string playerID)
	{
		_pv.RPC("IntroRPC", RpcTarget.All, playerID + "님이 떠나셨습니다.");
	}
	
	[PunRPC]
	private void IntroRPC(string msg)
	{
		if(_lstChat.Count == _maximumChatCount && _lstChat[_lstChat.Count - 1].gameObject.activeSelf)
		{
			// 0번 인덱스 삭제
			ChatItem item = _lstChat[0];
			item.UnSet();
			_lstChat.RemoveAt(0);
			_lstChat.Add(item);
		}
		bool isFull = false;
		foreach(var item in _lstChat)
		{
			if(item._Chat.text == "")
			{
				isFull = true;
				Color color = Color.yellow;
				item.SetUp(msg, color);
				break;
			}
		}
		if(!isFull) // 꽉 찼으면
		{
			ChatItem item = Instantiate(_chatBoxPrefab, _chatBoxContent).GetComponent<ChatItem>();
			Color color = Color.yellow;
			item.SetUp(msg, color);
			_lstChat.Add(item);
		}
	}
	
	public void Send()
	{
		if(_chatInputField.text == "") return;
		string msg = PhotonNetwork.NickName + " : " + _chatInputField.text;
		_pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId + '\t' + PhotonNetwork.NickName + " : " + _chatInputField.text);
		_chatInputField.text = "";
	}
	
	[PunRPC]
	private void ChatRPC(string msg)
	{
		if(_lstChat.Count == _maximumChatCount && _lstChat[_lstChat.Count - 1].gameObject.activeSelf)
		{
			// 0번 인덱스 삭제
			ChatItem item = _lstChat[0];
			item.UnSet();
			_lstChat.RemoveAt(0);
			_lstChat.Add(item);
		}
		bool isFull = false;
		string[] words = msg.Split('\t');
		foreach(var item in _lstChat)
		{
			if(item._Chat.text == "")
			{
				isFull = true;
				Color color = 
					words[0].StartsWith(PhotonNetwork.LocalPlayer.UserId)
					? Color.green : Color.white;
				item.SetUp(words[1], color);
				break;
			}
		}
		if(!isFull)
		{
			ChatItem item = Instantiate(_chatBoxPrefab, _chatBoxContent).GetComponent<ChatItem>();
			Color color = 
				item._Chat.color = words[0].StartsWith(PhotonNetwork.LocalPlayer.UserId)
				? Color.green : Color.white;
			item.SetUp(words[1], color);
			_lstChat.Add(item);
		}
		Invoke("ScrollDelay", 0.03f);
	}
	
	void ScrollDelay() => _scroll.verticalScrollbar.value = 0;
	
	// 방장이 바뀌게 되는 경우
	public void OnMasterSwitched()
	{
		_startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}
	
	// 시작 버튼
	public void StartGame()
	{
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.LoadLevel(1);
	}
	
	// 나가기 버튼
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager._Instance.OpenMenu("LoadingMenu");
	}
}
