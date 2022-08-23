using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MoreMountains.Tools;

public class MyPlayer : MonoBehaviour
{
	private PlayerController _playerController;
	private PhotonView _pv;
	private Animator _ani;
	private MMTouchButton _touchButton;
	
	public List<Quest> _lstQuest;
	public List<Quest> _lstSucQuest;
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		_ani = GetComponent<Animator>();
		_playerController = GetComponent<PlayerController>();
	}
	
	private void Start()
	{
		if(_pv.IsMine)
		{
			int questIndex = Random.Range(0, 5);
			_pv.RPC("GetQuest", RpcTarget.All, questIndex);
			
			_touchButton = FindObjectOfType<MMTouchButton>();
			_touchButton.ButtonPressedFirstTime.AddListener(AttackPressed);
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
	
	public void AttackPressed()
	{
		_ani.SetTrigger("Attack");
	}
	
	[PunRPC]
	private void GetQuest(int index)
	{
		_lstQuest.Add(GameManager._Instance._lstQuest[index]);
	}
}
