using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
	[SerializeField] Text _text;
	private RoomInfo _info;
	
	public RoomInfo _Info { get{return _info;} }
	
	public void SetUp(RoomInfo info)
	{
		_info = info;
		_text.text = _info.Name;
	}
	
	public void UnSet()
	{
		_info = null;
		_text.text = "";
	}
	
	public void OnClick()
	{
		JoinRoom(_info);
	}
	
	private void JoinRoom(RoomInfo info)
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
}
