using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TitleMenu : Menu
{
	[SerializeField] Text _userNickName;
	
	private void OnEnable()
	{
		_userNickName.text = PhotonNetwork.NickName;
	}
}
