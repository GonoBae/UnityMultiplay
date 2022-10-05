using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ObjectPooler : MonoBehaviour
{
	[SerializeField] private Transform _test;
	
	[System.Serializable]
	public class Pool
	{
		public string _tag;
		public ObjectType _type = ObjectType.NONE;
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
	private Dictionary<string, Queue<GameObject>> _dicPool = new Dictionary<string, Queue<GameObject>>();
	
	public void PrePoolInstantiate()
	{
		foreach(Pool pool in _lstPool)
		{
			Queue<GameObject> objPool = new Queue<GameObject>();
			for(int i = 0; i < pool._size; i++)
			{
				GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", pool._tag), Vector3.zero, Quaternion.identity);
				obj.GetComponent<PhotonView>().RPC("ActiveRPC", RpcTarget.All, false);
				if(pool._type == ObjectType.UI) obj.GetComponent<PhotonView>().RPC("SetParent", RpcTarget.All);
				
				objPool.Enqueue(obj);
			}
			_dicPool.Add(pool._tag, objPool);
		}
	}
	
	public GameObject PoolInstantiate(string tag)
	{
		if(!_dicPool.ContainsKey(tag))
		{
			Debug.LogError("Dic Doesn't have this Key");
			return null;
		}
		
		GameObject obj = _dicPool[tag].Dequeue();
		obj.GetComponent<PhotonView>().RPC("ActiveRPC", RpcTarget.All, true);
		_dicPool[tag].Enqueue(obj);
		
		return obj;
	}
	
	public void PoolDestroy(GameObject obj)
	{
		obj.GetComponent<PhotonView>().RPC("ActiveRPC", RpcTarget.All, false);
	}
}
