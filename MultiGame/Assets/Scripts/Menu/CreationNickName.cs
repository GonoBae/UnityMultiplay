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
		if(string.IsNullOrEmpty(_nickNameInputField.text)) return;
		else
		{
			PhotonNetwork.NickName = _nickNameInputField.text;
			MenuManager._Instance.OpenMenu("TitleMenu");
		}
	}
}
