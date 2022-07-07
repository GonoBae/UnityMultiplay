using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;

public class ObjectPoolMgr : MonoBehaviour
{
	private static ObjectPoolMgr _instance;
	public static ObjectPoolMgr _Instance
	{
		get
		{
			if(_instance == null) _instance = FindObjectOfType<ObjectPoolMgr>();
			return _instance;
		}
	}
	
	[SerializeField] List<ObjectPoolData> _lstPoolObjData = new List<ObjectPoolData>();
	
	private Dictionary<KeyType, PoolObject> _dictObj;
	private Dictionary<KeyType, ObjectPoolData> _dictData;
	private Dictionary<KeyType, Stack<PoolObject>> _dictPool;
	
	private void Start()
	{
		Init();
	}
	
	private void Init()
	{
		int len = _lstPoolObjData.Count;
		if(len == 0) return;
		
		_dictObj = new Dictionary<string, PoolObject>(len);
		_dictData = new Dictionary<string, ObjectPoolData>(len);
		_dictPool = new Dictionary<string, Stack<PoolObject>>(len);
		
		foreach(var data in _lstPoolObjData)
		{
			Register(data);
		}
	}
	
	private void Register(ObjectPoolData data)
	{
		if(_dictPool.ContainsKey(data._key) || (data._objectType != ObjectType.ALL))
		{
			return;
		}
		
		string name = data._key;
		GameObject parentOfObj = GameObject.Find(name);
		if(parentOfObj == null) parentOfObj = new GameObject(name);
		
		GameObject obj = Instantiate(data._prefab, parentOfObj.transform);
		if(!obj.TryGetComponent(out PoolObject po))
		{
			po = obj.AddComponent<PoolObject>();
			po._key = data._key;
		}
		obj.SetActive(false);
		
		Stack<PoolObject> pool = new Stack<PoolObject>(data._maxObjectCount);
		for(int i = 0; i < data._initialObjectCount; i++)
		{
			PoolObject clone = po.Clone(parentOfObj.transform);
			pool.Push(clone);
		}
		
		_dictObj.Add(data._key, po);
		_dictData.Add(data._key, data);
		_dictPool.Add(data._key, pool);
	}
	
	public PoolObject Spawn(KeyType key)
	{
		if(!_dictPool.TryGetValue(key, out var pool))
		{
			return null;
		}
		PoolObject po;
		if(pool.Count > 0)
		{
			po = pool.Pop();
		}
		else
		{
			po = _dictObj[key];
			GameObject parentObj = GameObject.Find(po._key);
			po = _dictObj[key].Clone(parentObj.transform);
		}
		po.Activate();
		return po;
	}
	
	public void DeSpawn(PoolObject po)
	{
		if(!_dictPool.TryGetValue(po._key, out var pool))
		{
			return;
		}
		KeyType key = po._key;
		if(pool.Count < _dictData[key]._maxObjectCount)
		{
			pool.Push(po);
			po.DeActivate();
		}
		else
		{
			Destroy(po.gameObject);
		}
		
	}
}
