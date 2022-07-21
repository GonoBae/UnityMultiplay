using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreationNickName : Menu
{
	[SerializeField] InputField _nickNameInputField;
	
	public InputField _NickNameInputField { get{return _nickNameInputField;} }
	
	// 버튼
	public void CreateNickNameButton()
	{
		string nick = _nickNameInputField.text;
		if(string.IsNullOrEmpty(nick)) return;
		else
		{
			PhotonNetwork.NickName = nick;
			MenuManager._Instance.OpenMenu("TitleMenu");
		}
	}
}
