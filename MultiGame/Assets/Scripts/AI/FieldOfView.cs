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
	private List<Vector3> viewPoints = new List<Vector3>();
	public List<Vector3> _hitPoints = new List<Vector3>();
	
	public float meshResolution;
	
	public MeshFilter viewMeshFilter;
	private Mesh viewMesh;
	
	public bool _isView = false;
	
	private void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
		
		StartCoroutine("FindObstacleWithDelay", .1f);
	}
	
	private void Update()
	{
		DrawFieldOfView();
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
			Transform ai = aiInViewRadius[i].transform.GetChild(4).transform;
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
	
	private void DrawFieldOfView()
	{
		viewPoints.Clear();
		_hitPoints.Clear();
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		
		for(int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			//Debug.DrawLine(pos, pos + DirFromAngle(angle, true) * viewRadius, Color.red);
			ViewCastInfo newViewCast = ViewCast(angle);
			if(newViewCast.hit) _hitPoints.Add(newViewCast.point);
			viewPoints.Add(newViewCast.point);
		}
		
		if(_isView)
		{
			int vertexCount = viewPoints.Count + 1;
			Vector3[] vertices = new Vector3[vertexCount];
			int[] triangles = new int[(vertexCount - 2) * 3];
		
			vertices[0] = new Vector3(0, pos.y, 0);
			for(int i = 0; i < vertexCount - 1; i++)
			{
				vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
				if(i < vertexCount - 2)
				{
					triangles[i * 3] = 0;
					triangles[i * 3 + 1] = i + 1;
					triangles[i * 3 + 2] = i + 2;
				}
			}
			viewMesh.Clear();
			viewMesh.vertices = vertices;
			viewMesh.triangles = triangles;
			viewMesh.RecalculateNormals();
		}
		else viewMesh.Clear();
	}
	
	private ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;
		
		if(Physics.Raycast(pos, dir, out hit, viewRadius, obstacleMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, pos + dir * viewRadius, viewRadius, globalAngle);
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
	
	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;
		
		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}
}
