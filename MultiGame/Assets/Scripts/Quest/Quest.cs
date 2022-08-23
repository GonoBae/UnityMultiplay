using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	NONE,
	EASY,
	NORMAL,
	HARD,
}

[CreateAssetMenu(fileName = "New Quest", menuName = "New Quest")]
public class Quest : ScriptableObject
{
	public string _questName;
	public int _id;
	[TextArea]
	public string _description;
	public Difficulty _level = Difficulty.NONE;
	public bool _complete = false;
}
