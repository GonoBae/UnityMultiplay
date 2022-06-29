using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	/***********************************
				Singleton
	***********************************/
	private static MenuManager _instance;
	public static MenuManager _Instance
	{
		get
		{
			if(!_instance) _instance = FindObjectOfType(typeof(MenuManager)) as MenuManager;
			return _instance;
		}
	}
	
	/***********************************
				Fields
	***********************************/
}
