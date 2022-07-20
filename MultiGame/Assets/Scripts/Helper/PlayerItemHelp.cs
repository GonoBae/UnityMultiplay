using UnityEngine;
using Photon.Pun;

public class PlayerItemHelp : MonoBehaviour
{
	private RectTransform _rect;
	
	private Photon.Realtime.Player _player;
	public Photon.Realtime.Player _Player { get{return _player;} }
	
	/***********************************
				Functions
	***********************************/
	public void SetUp(PlayerListItem item)
	{
		if(_rect == null) _rect = GetComponent<RectTransform>();
		_rect.position = item._Rect.position;
		_player = item._Player;
		
		transform.parent.gameObject.SetActive(true);
		gameObject.SetActive(true);
	}
	
	public void SendKickPlayer()
	{
		foreach(var p in PhotonNetwork.CurrentRoom.Players)
		{
			if(p.Value.UserId == _player.UserId && p.Value.UserId != null)
			{
				HelperManager._Instance._Pv.RPC("KickPlayer", p.Value);
				break;
			}
		}
		CloseUI();
	}
	
	public void CloseUI()
	{
		transform.parent.gameObject.SetActive(false);
		gameObject.SetActive(false);
		UnSet();
	}
	
	private void UnSet()
	{
		_rect.position = transform.parent.position;
		_player = null;
	}
}
