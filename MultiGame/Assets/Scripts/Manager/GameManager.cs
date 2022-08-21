using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public List<Quest> _lstQuest;
	
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
	
	public Quest GetQuest()
	{
		int rand = Random.Range(0, _lstQuest.Count);
		return _lstQuest[rand];
	}
}
