using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviour
{
	[SerializeField] private Text _playerName;
	private Player _player;
	public Player _Player { get{return _player;} }
	
	public void SetUp(Player player)
	{
		_playerName.text = player.NickName;
		_player = player;
		gameObject.SetActive(true);
	}
	
	public void UnSet()
	{
		_playerName.text = "";
		_player = null;
		gameObject.SetActive(false);
	}
}
