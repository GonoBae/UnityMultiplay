using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MoreMountains.Tools;

public class MyPlayer : MonoBehaviour
{
	public PlayerType _playerType = PlayerType.THIEF;
	
	private PlayerController _playerController;
	private PhotonView _pv;
	private Animator _ani;
	private Knife _knife;
	
	public List<Quest> _lstQuest;
	public List<Quest> _lstSucQuest;
	
	private bool _canAttack = true;
	public bool _CanAttack { get{return _canAttack;} }
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		GameManager._Instance.AddPlayer(this);
		if(_pv.IsMine)
		{
			_ani = GetComponent<Animator>();
			_playerController = GetComponent<PlayerController>();
			_knife = GetComponentInChildren<Knife>();
		}
	}
	
	private void Update()
	{
		if(_pv.IsMine)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				_lstSucQuest.Add(_lstQuest[0]);
				_lstQuest.RemoveAt(0);
				Debug.LogError("Successed Quest : " + _lstSucQuest[0]._questName);
			}
			
			if(_playerController._PlayerInput == Vector2.zero)
			{
				_ani.SetBool("Walk", false);
			}
			else
			{
				_ani.SetBool("Walk", true);
			}
		}
	}
	
	public void Attack()
	{
		_ani.SetTrigger("Attack");
		_knife.gameObject.SetActive(true);
		StartCoroutine("DelayAttack");
	}
	
	private IEnumerator DelayAttack()
	{
		_canAttack = false;
		yield return new WaitForSeconds(3f);
		_canAttack = true;
		_knife.gameObject.SetActive(false);
		_playerController._AttackButton.SetOpacity(1f);
	}
	
	[PunRPC]
	private void GetQuest(int index)
	{
		_lstQuest.Add(GameManager._Instance._lstQuest[index]);
	}
	
	[PunRPC]
	private void ChangePlayerType(int index)
	{
		_playerType = (PlayerType)index;
	}
	
	public void QuestTest()
	{
		if(_pv.IsMine) _pv.RPC("Quest", RpcTarget.All);
	}
	
	[PunRPC]
	public void Quest()
	{
		if(_pv.IsMine)
		{
			int questIndex = Random.Range(0, 5);
			_pv.RPC("GetQuest", RpcTarget.All, questIndex);
			UIManager._Instance.SetQuestUI(_lstQuest[0]._questName);
		}
	}
	
	
	public void Change(int index)
	{
		_pv.RPC("ChangePlayerType", RpcTarget.All, index);
	}
}
