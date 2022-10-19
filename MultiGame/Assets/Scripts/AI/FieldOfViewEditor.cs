//using System.Collections;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(FieldOfView))]
//public class FieldOfViewEditor : Editor
//{
//	protected void OnSceneGUI()
//	{
//		FieldOfView fow = (FieldOfView) target;
//		Handles.color = Color.white;
//		Handles.DrawWireArc(fow.pos, Vector3.up, Vector3.forward, 360, fow.viewRadius);
		
//		Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
//		Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
		
//		Handles.DrawLine(fow.pos, fow.pos + viewAngleA * fow.viewRadius);
//		Handles.DrawLine(fow.pos, fow.pos + viewAngleB * fow.viewRadius);
		
//		Handles.color = Color.red;
//		foreach(Transform visibleAI in fow.visibleAI)
//		{
//			Handles.DrawLine(fow.pos, visibleAI.position);
//		}
//	}
//}
