using UnityEngine;

public enum Difficulty
{
	NONE = -1,
	EASY,
	NORMAL,
	HARD,
}

public enum QuestType : short
{
	NONE = -1,
	SPIN,
	FLAP,
	RUB,
	STOP
}

[CreateAssetMenu(fileName = "New Quest", menuName = "New Quest")]
public class Quest : ScriptableObject
{
	public QuestType _questType = QuestType.NONE;
	public string _questName;
	public int _id;
	[TextArea]
	public string _description;
	public Difficulty _level = Difficulty.NONE;
	public bool _complete = false;
}
