using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Menu Item", menuName = "Menu Item")]
public class MenuItem : ScriptableObject
{
	
	public string _name;
	[Header("InputFields")]
	[SerializeField] InputField _inputFields;
	public void Generate()
	{
		
	}
}
