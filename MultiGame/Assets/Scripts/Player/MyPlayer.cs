using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MoreMountains.Tools;

public class MyPlayer : MonoBehaviour
{
	public PlayerType _playerType = PlayerType.THIEF;
	public PlayerLife _playerLife = PlayerLife.Alive;
	
	private PlayerController _playerController;
	private PhotonView _pv;
	private Animator _ani;
	private Knife _knife;
	
	public List<Quest> _lstQuest;
	public List<Quest> _lstSucQuest;
	
	public Renderer _body;
	public Renderer _head;
	public List<Material> _lstthiefMaterials;
	public List<Material> _lstPoliceMaterials;
	public Material _bodyGhost;
	public Material _headGhost;
	
	private bool _canAttack = true;
	public bool _CanAttack { get{return _canAttack;} }
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		_bodyGhost = Resources.Load<Material>("BodyGhost");
		_headGhost = Resources.Load<Material>("HeadGhost");
	}
	
	private void Start()
	{
		GameManager._Instance.AddPlayer(this);
		_ani = GetComponent<Animator>();
		if(_pv.IsMine)
		{
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
		if(_playerLife == PlayerLife.Alive) _knife.ActiveCollider(true);
		_ani.SetTrigger("Attack");
		StartCoroutine("DelayAttack");
	}
	
	public void TakeDamage()
	{
		_pv.RPC("RPC_TakeDamage", RpcTarget.All);
	}
	
	[PunRPC]
	private void RPC_TakeDamage()
	{
		_playerLife = PlayerLife.Ghost;
		if(_pv.IsMine)
		{
			ChangeMaterial(_bodyGhost, _headGhost);
		}
		else
		{
			_ani.SetTrigger("Hit");
			GetComponent<PhotonTransformView>().enabled = false;
			GetComponent<CapsuleCollider>().enabled = false;
		}
		GetComponent<PhotonAnimatorView>().enabled = false;
	}
	
	private IEnumerator DelayAttack()
	{
		_canAttack = false;
		yield return new WaitForSeconds(3f);
		_canAttack = true;
		_knife.ActiveCollider(false);
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
	
	private void ChangeMaterial(Material body, Material head)
	{
		_body.material = body;
		_head.material = head;
	}
}
