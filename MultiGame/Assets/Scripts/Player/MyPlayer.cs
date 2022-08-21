using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPlayer : MonoBehaviour
{
	private PhotonView _pv;
	
	public List<Quest> _lstQuest;
	public List<Quest> _lstSucQuest;
	
	protected void Start()
	{
		_pv = GetComponent<PhotonView>();
		if(_pv.IsMine)
		{
			_lstQuest.Add(GameManager._Instance.GetQuest());
			Debug.LogError("My Quest : " + _lstQuest[0]._questName);
		}
	}
	
	protected void Update()
	{
		if(_pv.IsMine)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				_lstSucQuest.Add(_lstQuest[0]);
				_lstQuest.RemoveAt(0);
				Debug.LogError("Successed Quest : " + _lstSucQuest[0]._questName);
			}
		}
	}
}
