using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
	[SerializeField] private Text _chat;
	public Text _Chat { get{return _chat;} }
	
	public void SetUp(string msg, Color color)
	{
		_chat.text = msg;
		_chat.color = color;
		gameObject.SetActive(true);
	}
	
	public void UnSet()
	{
		_chat.text = "";
		_chat.color = Color.black;
		transform.SetAsLastSibling();
		gameObject.SetActive(false);
	}
}
