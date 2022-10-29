using UnityEngine;

public class AISettings : MonoBehaviour
{
	// rigidbody.moveposition?
	
	[SerializeField] private float aiSpeed = 10f;
	[SerializeField] private float aggroRadius = 4f;
	
	public static float AISpeed => Instance.aiSpeed;
	public static float AggroRadius => Instance.aggroRadius;
	
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
