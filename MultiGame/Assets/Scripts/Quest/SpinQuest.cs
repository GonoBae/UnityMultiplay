using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinQuest : MonoBehaviour
{
	protected void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 3);
	}
}
