using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MainUI : MonoBehaviour
{
	/***********************************
				Fields
	***********************************/
	[Header("Make Nick & Room")]
	[SerializeField] InputField _nickNameInputField;
	[SerializeField] InputField _roomNameInputField;
	[SerializeField] Text _playerCountSetting;
	
	[Header("In Room")]
	[SerializeField] Text _roomName;
	[SerializeField] Text _roomMaxPlayer;
	[SerializeField] Text _roomCurrentPlayer;
	
	[Header("Room List")]
	[SerializeField] Transform _roomListContent;
	[SerializeField] GameObject _roomListItemPrefab;
	
	[Header("Chat")]
	[SerializeField] InputField _chatInputField;
	[SerializeField] Text[] _chatText;
	[SerializeField] Text _chatBoxPrefab;
	[SerializeField] Transform _chatBoxContent;
	[SerializeField] ScrollRect _scroll;
	
	[Header("Start Game Button")]
	[SerializeField] GameObject _startGameButton;
	
	/***********************************
				Property
	***********************************/
	public InputField _NickNameInputField { get{return _nickNameInputField;} }
	
	/***********************************
				Functions
	***********************************/
	public void CreateNickNameButton()
	{
		if(string.IsNullOrEmpty(_nickNameInputField.text)) return;
		else
		{
			PhotonNetwork.NickName = _nickNameInputField.text;
			MenuManager._Instance.OpenMenu("TitleMenu");
		}
	}
}
