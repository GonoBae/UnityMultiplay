using UnityEngine;

public class PlayerInput
{
	public KeyCode _Enter { get{return KeyCode.Return;}}
	public bool _PressEnter { get{return Input.GetKeyDown(KeyCode.Return);} }
}
