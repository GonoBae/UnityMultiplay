using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private MyPlayer _player;
	public MyPlayer _Player {get{return _player;} set{_player = value;}}
	
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
		_instance = this;
	}
	
	public MyPlayer GetPlayer()
	{
		Debug.Log(_player);
		return _player;
	}
}
