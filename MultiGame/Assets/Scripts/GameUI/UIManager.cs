using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] Text _quest;
	
	private static UIManager _instance = null;
	public static UIManager _Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<UIManager>();
			}
			return _instance;
		}
	}
	
	public void SetQuestUI(string quest)
	{
		_quest.text = quest;
	}
}
