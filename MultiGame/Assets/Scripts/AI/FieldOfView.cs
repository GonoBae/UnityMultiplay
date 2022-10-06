using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	public float viewRadius;
	public Vector3 pos => transform.position + new Vector3(0, 1.4f, 0);
	[Range(0, 360)]
	public float viewAngle;
	
	public LayerMask aiMask;
	public LayerMask obstacleMask;
	
	public List<Transform> visibleAI;
	
	private void Start()
	{
		StartCoroutine("FindObstacleWithDelay", .2f);
	}
	
	IEnumerator FindObstacleWithDelay(float delay)
	{
		while(true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleObstacles();
		}
	}
	
	private void FindVisibleObstacles()
	{
		visibleAI.Clear();
		Collider[] aiInViewRadius = Physics.OverlapSphere(pos, viewRadius, aiMask);
	
		for(int i = 0; i < aiInViewRadius.Length; i++)
		{
			Transform ai = aiInViewRadius[i].transform;
			Vector3 dirToAI = (ai.position - pos).normalized;
			if(Vector3.Angle(transform.forward, dirToAI) < viewAngle / 2)
			{
				float dstToAI = Vector3.Distance(pos, ai.position);
				if(!Physics.Raycast(pos, dirToAI, dstToAI, obstacleMask))
				{
					visibleAI.Add(ai);
				}
			}
		}
	}
	
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if(!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}
