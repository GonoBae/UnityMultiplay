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
	[SerializeField]
	Menu[] menus;
	
	/***********************************
				Functions
	***********************************/
	public void OpenMenu(string menuName)
	{
		foreach(var menu in menus)
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
		foreach(var m in menus)
		{
			if(m._Open)
			{
				CloseMenu(menu);
			}
		}
		menu.Open();
	}
	
	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
}
