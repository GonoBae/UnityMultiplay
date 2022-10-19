using UnityEngine;

// 모든 메뉴들이 상속받음
public class Menu : MonoBehaviour
{
	// 메뉴의 이름 및 열림 여부
	[SerializeField] private string _menuName;
	[SerializeField] private bool _open;
	
	public string _MenuName { get{return _menuName;} }
	public bool _Open { get{return _open;} }
	
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
