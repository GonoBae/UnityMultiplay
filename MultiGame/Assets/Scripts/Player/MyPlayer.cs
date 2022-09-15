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
	
	public Renderer _body;
	public Renderer _head;
	
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
		_knife.ActiveCollider(true);
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
		if(_pv.IsMine)
		{
			this._body.material.SetOverrideTag("RenderType", "Transparent");
			this._body.material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
			this._body.material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			this._body.material.SetInt("_ZWrite", 0);
			this._body.material.DisableKeyword("_ALPHATEST_ON");
			this._body.material.EnableKeyword("_ALPHABLEND_ON");
			this._body.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			this._body.material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
			this._head.material.SetOverrideTag("RenderType", "Transparent");
			this._head.material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
			this._head.material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			this._head.material.SetInt("_ZWrite", 0);
			this._head.material.DisableKeyword("_ALPHATEST_ON");
			this._head.material.EnableKeyword("_ALPHABLEND_ON");
			this._head.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			this._head.material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
		}
		else
		{
			this.gameObject.SetActive(false);
		}
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
}
