using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MoreMountains.Tools;
using System.IO;

public class MyPlayer : MonoBehaviour
{
	public PlayerType _playerType = PlayerType.THIEF;
	public PlayerLife _playerLife = PlayerLife.Alive;
	[SerializeField] private string _nickName;
	public string _NickName { get{return _nickName;} }
	
	private PlayerController _playerController;
	private PhotonView _pv;
	private Animator _ani;
	private Knife _knife;
	
	public Quest _quest;
	public Quest _sucQuest;
	
	public Renderer _body;
	public Renderer _head;
	public Material _bodyGhost;
	public Material _headGhost;
	
	private bool _canAttack = true;
	public bool _CanAttack { get{return _canAttack;} }
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		if(_pv.IsMine)
		{
			_playerController = GetComponent<PlayerController>();
			_knife = GetComponentInChildren<Knife>();
			
			_pv.RPC("SetNickName", RpcTarget.All, PhotonNetwork.NickName);
			UIManager._Instance.SetTypeUI((_playerType).ToString());
		}
	}
	
	private void Start()
	{
		GameManager._Instance.AddPlayer(this);
		_ani = GetComponent<Animator>();
		if(_pv.IsMine)
		{
			_bodyGhost = Resources.Load<Material>("BodyGhost");
			_headGhost = Resources.Load<Material>("HeadGhost");
			
			UIManager._Instance.SetmasterUI();
		}
	}
	bool _ismaster = false;
	private void Update()
	{
		if(_pv.IsMine)
		{
			// 테스트용 코드
			//if(Input.GetKeyDown(KeyCode.Space))
			//{
			//	_lstSucQuest.Add(_lstQuest[0]);
			//	_lstQuest.RemoveAt(0);
			//	Debug.LogError("Successed Quest : " + _lstSucQuest[0]._questName);
			//}
			
			if(_playerController._PlayerInput == Vector2.zero)
			{
				_ani.SetBool("Walk", false);
			}
			else
			{
				_ani.SetBool("Walk", true);
			}
			// 마스터 클라이언트는 바뀐다.
			if(PhotonNetwork.IsMasterClient && !_ismaster) {
				UIManager._Instance.SetmasterUI();
				AI[] ais = GameObject.FindObjectsOfType<AI>();
				for(int i = 0; i < ais.Length; i++)
				{
					ais[i].GetComponent<AI>()._owner = ais[i].GetComponent<PhotonView>().Owner.NickName;
				}
				_ismaster = true;
			}
		}
	}
	
	// 닉네임 저장
	[PunRPC]
	private void SetNickName(string nick)
	{
		_nickName = nick;
	}
	
	// 플레이어 역할 바꾸기
	public void ChangeType(int index)
	{
		_pv.RPC("ChangePlayerType", RpcTarget.All, index);
	}
	
	[PunRPC]
	private void ChangePlayerType(int index)
	{
		_playerType = (PlayerType)index;
		if(_pv.IsMine) UIManager._Instance.SetTypeUI((_playerType).ToString());
	}
	
	// 퀘스트 받기
	public void QuestTest()
	{
		if(_pv.IsMine) 
		{
			int questIndex = Random.Range(0, 5);
			_pv.RPC("GetQuest", RpcTarget.All, questIndex);
			UIManager._Instance.SetQuestUI(_quest._questName);
		}
	}
	
	[PunRPC]
	private void GetQuest(int index)
	{
		_quest = GameManager._Instance._lstQuest[index];
	}
	
	// 공격
	public void Attack()
	{
		if(_playerLife == PlayerLife.Alive) _knife.ActiveCollider(true);
		_ani.SetTrigger("Attack");
		StartCoroutine("DelayAttack");
	}
	
	public void TakeDamage(string attacker)
	{
		_pv.RPC("RPC_TakeDamage", RpcTarget.All);
		//UIManager._Instance._PV.RPC("ShowKillLog", RpcTarget.All, attacker, attacked);
		
		_pv.RPC("RPC_ShowKillLog", RpcTarget.All, attacker);
	}
	
	// 내 화면 : 고스트 , 다른 화면 : 시체
	[PunRPC]
	private void RPC_TakeDamage()
	{
		_playerLife = PlayerLife.Ghost;
		
		if (_pv.IsMine)
		{
			ChangeMaterial(_bodyGhost, _headGhost);
			RemoveGravityCollider();
		}
		else
		{
			_ani.SetTrigger("Hit");
			GetComponent<PhotonTransformView>().enabled = false;
			GetComponent<CapsuleCollider>().isTrigger = true;
		}
		GetComponent<PhotonAnimatorView>().enabled = false;
	}
	
	// 모두의 화면 : 킬로그 ( ㅇㅇㅇ -> ㅇㅇㅇ )
	[PunRPC]
	private void RPC_ShowKillLog(string attacker)
	{
		if(_pv.IsMine) 
		{
			ObjectPooler._Instance.PoolInstantiate("KillLog")
				.GetComponent<PhotonView>()
				.RPC("SetUp", RpcTarget.All, attacker, _nickName);
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
	
	private void ChangeMaterial(Material body, Material head)
	{
		_body.material = body;
		_head.material = head;
	}
	
	private void RemoveGravityCollider()
	{
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<CapsuleCollider>().isTrigger = true;
	}
}
