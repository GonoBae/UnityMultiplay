using UnityEngine;
using KeyType = System.String;

[DisallowMultipleComponent]
public class PoolObject : MonoBehaviour
{
	public KeyType _key;
	public PoolObject Clone(Transform parent)
	{
		GameObject obj = Instantiate(gameObject, parent);
		if(!obj.TryGetComponent(out PoolObject po))
		{
			po = obj.AddComponent<PoolObject>();
		}
		obj.SetActive(false);
		return po;
	}
	
	public void Activate()
	{
		gameObject.SetActive(true);
	}
	
	public void DeActivate()
	{
		gameObject.SetActive(false);
	}
}
