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
	// 방장이 정하도록 할 것인가? 아니면 총 인원의 20% 로 자동으로 정해줄 것인가?
	private const int _policeCount = 1;
	
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
	}
	
	private void Start()
	{
		if(_pv.IsMine)
		{
			StartCoroutine("GetCharacterType");
		}
	}
	
	// 역할 부여하기
	private IEnumerator GetCharacterType()
	{
		// 모든 플레이어 스폰 기다리기
		int n = PhotonNetwork.CurrentRoom.Players.Count;
		while(n != _lstPlayer.Count)
		{
			yield return new WaitForSeconds(0.03f);
		}
		
		// 역할 부여하기 기본 : 도둑 => 정해진 수만큼 경찰이 배치됨
		
		
		for(int i = 0; i < _policeCount; i++)
		{
			int ix = Random.Range(0, _lstPlayer.Count);
			var player = _lstPlayer[ix];
			
			if(player._playerType != PlayerType.POLICE)
			{
				player.ChangeType((int)PlayerType.POLICE);
			}
			else i--;
		}
		
		_pv.RPC("PlayerSettings", RpcTarget.All);
	}
	
	[PunRPC]
	private void PlayerSettings()
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
	
	public Quest GetQuest()
	{
		int rand = Random.Range(0, _lstQuest.Count);
		return _lstQuest[rand];
	}
	
	public void AddPlayer(MyPlayer player)
	{
		_lstPlayer.Add(player);
	}
	
	private void Update()
	{
		
	}
}
