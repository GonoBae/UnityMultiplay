using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
	private BoxCollider _knifeCollider;
	
	private void Start()
	{
		_knifeCollider = GetComponent<BoxCollider>();
	}
	
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			MyPlayer player = other.GetComponent<MyPlayer>();
			player.TakeDamage();
		}
	}
	
	public void ActiveCollider(bool active)
	{
		_knifeCollider.enabled = active;
	}
}
