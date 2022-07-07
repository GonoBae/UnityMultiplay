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
	private MainUI _mainUI;
	public MainUI _MainUI { get{return _mainUI;} }
	
	[SerializeField] private Menu[] _menus;
	
	/***********************************
				Unity Events
	***********************************/
	private void Awake()
	{
		_mainUI = GetComponent<MainUI>();
	}
	
	/***********************************
				Functions
	***********************************/
	public void OpenMenu(string menuName)
	{
		foreach(var menu in _menus)
		{
			if(menu._MenuName == menuName)
			{
				menu.Open();
			}
			else if(menu._Open)
			{
				CloseMenu(menu);
			}
		}
	}
	
	public void OpenMenu(Menu menu)
	{
		foreach(var m in _menus)
		{
			if(m._Open)
			{
				CloseMenu(m);
			}
		}
		menu.Open();
		ServerManager._Instance._CurrentMenu = menu;
	}
	
	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
}
