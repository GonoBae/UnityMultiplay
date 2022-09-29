using UnityEngine;

public class AISettings : MonoBehaviour
{
	[SerializeField] private float aiSpeed = 1f;
	
	public static float AISpeed => Instance.aiSpeed;
	
	private static AISettings _instance;
	public static AISettings Instance
	{
		get
		{
			if(_instance == null) _instance = FindObjectOfType<AISettings>();
			return _instance;
		}
	}
}
