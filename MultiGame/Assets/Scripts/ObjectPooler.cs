using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPooler : MonoBehaviour
{
	[System.Serializable]
	public class Pool
	{
		public string _tag;
		public GameObject _prefab;
		public int _size;
	}
	
	private static ObjectPooler _instance;
	public static ObjectPooler _Instance
	{
		get
		{
			if(_instance == null) _instance = FindObjectOfType<ObjectPooler>();
			return _instance;
		}
	}
	
	[SerializeField] private List<Pool> _lstPool;
}
