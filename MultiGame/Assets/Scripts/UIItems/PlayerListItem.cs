using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviour
{
	[SerializeField] private Text _playerName;
	private PlayerItemHelp _ui;
	private Player _player;
	private RectTransform _rect;
	public Player _Player { get{return _player;} }
	public RectTransform _Rect { get{return _rect;} }
	
	/***********************************
				Unity Events
	***********************************/
	private void Start()
	{
		_ui = FindObjectOfType<PlayerItemHelp>(true);
		_rect = GetComponent<RectTransform>();
	}
	
	/***********************************
				Functions
	***********************************/
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
	
	// 버튼을 누르면 UI 가 뜨도록
	public void ShowUI()
	{
		if(PhotonNetwork.IsMasterClient)
		{
			if(_player.IsMasterClient) return;
			_ui.SetUp(this);
		}
	}
}
