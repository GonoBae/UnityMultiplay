using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TitleMenu : Menu
{
	[SerializeField] Text _userNickName;
	private bool screte;
	
	private void OnEnable()
	{
		_userNickName.text = PhotonNetwork.NickName;
	}
	
	public void JoinRandomRoom()
	{
		if(PhotonNetwork.CountOfRooms > 0) PhotonNetwork.JoinRandomRoom();
		else Debug.Log("방 없음");
		// UI 작업 해야함 ( 방 없음 )
	}
}
