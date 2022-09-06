//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(Quest))]
//public class E_Quest : Editor
//{
//	Quest _quest;
	
//	private void OnEnable()
//	{
//		_quest = (Quest)target;
//	}
	
//	public override void OnInspectorGUI()
//	{
//		base.OnInspectorGUI();
		
//		if(_quest._questType == QuestType.SPIN)
//		{
//			EditorGUILayout.Space();
//			Rect rect = EditorGUILayout.GetControlRect(true, 4);
//			EditorGUI.DrawRect(rect , new Color(0.5f, 0.5f, 0.5f, 1));
//			EditorGUILayout.Space();
			
			
//		}
//	}
//}
