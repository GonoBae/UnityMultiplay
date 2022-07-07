using UnityEngine;
using KeyType = System.String;

[System.Serializable]
public class ObjectPoolData
{
	private const int _INITIAL_COUNT = 1;
	private const int _MAX_COUNT = 10;
	
	public ObjectType _objectType = ObjectType.NONE;
	public KeyType _key;
	public GameObject _prefab;
	public int _initialObjectCount = _INITIAL_COUNT;
	public int _maxObjectCount = _MAX_COUNT;
}
