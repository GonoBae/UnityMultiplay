using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	public List<MyPlayer> _lstPlayer;
	public List<GameObject> _lstThief;
	public List<GameObject> _lstPolice;
	public List<Quest> _lstQuest;
	
	private PhotonView _pv;
	private bool _test;
	
	private static GameManager _instance = null;
	public static GameManager _Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<GameManager>();
			}
			return _instance;
		}
	}
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		if(_pv.IsMine)
		{
			StartCoroutine("GameReady");
		}
	}
	
	private void Start()
	{
		
	}
	
	public Quest GetQuest()
	{
		int rand = Random.Range(0, _lstQuest.Count);
		return _lstQuest[rand];
	}
	
	public void AddPlayer(MyPlayer player)
	{
		_lstPlayer.Add(player);
	}
	
	private IEnumerator GameReady()
	{
		while(PhotonNetwork.CurrentRoom.Players.Count != _lstPlayer.Count)
		{
			yield return null;
		}
		
		for(int i = 0; i < 1; i++)
		{
			var player = _lstPlayer[Random.Range(0, _lstPlayer.Count)];
			
			if(player._playerType != PlayerType.POLICE)
			{
				player.Change((int)PlayerType.POLICE);
			}
			else
			{
				i--;
				continue;
			}
		}
		
		_pv.RPC("TestSettings", RpcTarget.All);
	}
	
	[PunRPC]
	private void TestSettings()
	{
		for(int i = 0; i < _lstPlayer.Count; i++)
		{
			MyPlayer player = _lstPlayer[i];
			if(player._playerType == PlayerType.THIEF)
			{
				player.QuestTest();
				_lstThief.Add(player.gameObject);
			}
			else
			{
				_lstPolice.Add(player.gameObject);
			}
		}
	}
}
