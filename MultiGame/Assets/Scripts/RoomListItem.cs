using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
	[SerializeField] Text _text;
	private RoomInfo _info;
	
	public RoomInfo _Info { get{return _info;} }
	
	public void SetUp(RoomInfo info)
	{
		_info = info;
		_text.text = _info.Name;
	}
	
	public void UnSet()
	{
		_info = null;
		_text.text = "";
	}
	
	public void OnClick()
	{
		ServerManager._Instance.JoinRoom(_info);
	}
}
