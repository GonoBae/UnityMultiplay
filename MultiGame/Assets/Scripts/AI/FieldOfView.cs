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
	public List<Transform> visibleObstacle;
	
	private void Start()
	{
		StartCoroutine("FindObstacleWithDelay", .1f);
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
			Transform ai = aiInViewRadius[i].transform.GetChild(3).transform;
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
		
		visibleObstacle.Clear();
		Collider[] obstacleInViewRadius = Physics.OverlapSphere(pos, viewRadius, obstacleMask);
		RaycastHit hit;
		for(int i = 0; i < obstacleInViewRadius.Length; i++)
		{
			Collider ob = obstacleInViewRadius[i];
			//Transform ob = obstacleInViewRadius[i].transform;
			if(Physics.Raycast(pos, transform.forward, out hit, viewRadius))
			{
				if(hit.collider == ob) {
					Vector3 dirToObstacle = (hit.point - pos).normalized;
					if(Vector3.Angle(transform.forward, dirToObstacle) < viewAngle / 2)
					{
						Debug.Log(Vector3.Angle(transform.forward, dirToObstacle));
						visibleObstacle.Add(hit.transform);
					}
				}
			}
			//Vector3 dirToObstacle = (ob.position - pos).normalized;
			//if(Vector3.Angle(transform.forward, dirToObstacle) < viewAngle / 2)
			//{
			//	Debug.Log(Vector3.Angle(transform.forward, dirToObstacle));
			//	visibleObstacle.Add(ob);
			//}
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
