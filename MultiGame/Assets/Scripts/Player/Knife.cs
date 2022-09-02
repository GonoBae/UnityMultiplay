using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Debug.LogError(other.name);
		}
	}
}
