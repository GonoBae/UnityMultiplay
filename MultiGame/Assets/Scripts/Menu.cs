using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] private string _menuName;
	[SerializeField] private bool _open;
	
	public string _MenuName { get{return _menuName;} set{_menuName = value;} }
	public bool _Open { get{return _open;} set{_open = value;} }
	
	public void Open()
	{
		_open = true;
		this.gameObject.SetActive(true);
	}
	
	public void Close()
	{
		_open = false;
		this.gameObject.SetActive(false);
	}
}
