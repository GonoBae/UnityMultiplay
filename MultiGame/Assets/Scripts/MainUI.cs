using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainUI : MonoBehaviour
{
	/***********************************
				Fields
	***********************************/
	[Header("Make Nick & Room")]
	private const int _minimumPlayer = 1;
	private const int _maximumPlayer = 6;
	private int _playerCountInt = 1;
	
	[Header("NickName")]
	[SerializeField] InputField _nickNameInputField;
	[SerializeField] Text _userNickName;
	
	[Header("Create Room")]
	[SerializeField] InputField _roomNameInputField;
	[SerializeField] Slider _playerCountSlider;
	[SerializeField] Text _playerCount;
	
	[Header("Room List")]
	[SerializeField] Transform _roomListContent;
	[SerializeField] GameObject _roomListItemPrefab;
	
	[Header("In Room")]
	[SerializeField] Text _nickName;
	[SerializeField] Text _roomName;
	[SerializeField] Text _roomMaxPlayer;
	[SerializeField] Text _roomCurrentPlayer;
	[SerializeField] Transform _playerListContent;
	[SerializeField] GameObject _playerListItemPrefab;
	
	[Header("Chat")]
	[SerializeField] InputField _chatInputField;
	[SerializeField] Text[] _chatText;
	[SerializeField] GameObject _chatBoxPrefab;
	[SerializeField] Transform _chatBoxContent;
	[SerializeField] ScrollRect _scroll;
	
	[Header("Start Game Button")]
	[SerializeField] GameObject _startGameButton;
	
	/***********************************
				Property
	***********************************/
	public InputField _NickNameInputField { get{return _nickNameInputField;} }
	
	public InputField _ChatInputField { get{return _chatInputField;} }
	public Text[] _ChatText { get{return _chatText;} }
	public GameObject _ChatBoxPrefab { get{return _chatBoxPrefab;} }
	public Transform _ChatBoxContent { get{return _chatBoxContent;} }
	public ScrollRect _Scroll { get{return _scroll;} }
	
	public Transform _RoomListContent { get{return _roomListContent;} }
	public GameObject _RoomListItemPrefab { get{return _roomListItemPrefab;} }
	
	public Transform _PlayerListContent { get{return _playerListContent;}}
	public GameObject _PlayerListItemPrefab { get{return _playerListItemPrefab;}}
	public Text _RoomCurrentPlayer { get{return _roomCurrentPlayer;} set{_roomCurrentPlayer = value;} }
	
	public GameObject _StartGameButton { get{return _startGameButton;} }
	
	/***********************************
				Unity Events
	***********************************/
	private void Start()
	{
		SetPlayerCountSlider();
	}
	
	/***********************************
				Functions
	***********************************/
	private void SetPlayerCountSlider()
	{
		_playerCountSlider.minValue = _minimumPlayer;
		_playerCountSlider.maxValue = _maximumPlayer;
		_playerCountSlider.value = 1;
		_playerCountSlider.onValueChanged.AddListener((v) => {
			_playerCountInt = (int) Mathf.Round(v);
			_playerCount.text = _playerCountInt.ToString("0");
			_playerCountSlider.value = _playerCountInt;
		});
	}
	
	public void CreateNickNameButton()
	{
		if(string.IsNullOrEmpty(_nickNameInputField.text)) return;
		else
		{
			PhotonNetwork.NickName = _nickNameInputField.text;
			MenuManager._Instance.OpenMenu("TitleMenu");
			_userNickName.text = PhotonNetwork.NickName;
		}
	}
	
	public void CreateRoomButton()
	{
		if(string.IsNullOrEmpty(_roomNameInputField.text)) return;
		else
		{
			RoomOptions ro = new RoomOptions
			{
				MaxPlayers = (byte)_playerCountInt,
				IsVisible = true,
				IsOpen = true,
				CleanupCacheOnLeave = true
			};
			PhotonNetwork.CreateRoom(_roomNameInputField.text, ro);
			MenuManager._Instance.OpenMenu("LoadingMenu");
		}
	}
	
	public void JoinRoom(RoomInfo info)
	{
		
	}
	
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager._Instance.OpenMenu("LoadingMenu");
	}
	
	public void ResetCreateRoom()
	{
		_roomNameInputField.text = "";
		_playerCountSlider.value = 1;
		_playerCount.text = "1";
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
		_chatInputField.text = "";
	}
	
	public void CurrentPlayerUpdate(string count)
	{
		_roomCurrentPlayer.text = count;
	}
	
	// Quit Game
	public void QuitGame()
	{
		Application.Quit();
	}
}
