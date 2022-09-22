using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLog : MonoBehaviour
{
	[SerializeField] private Text _attacker;
	[SerializeField] private Text _attacked;
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(3);
		UnSet();
		gameObject.SetActive(false);
	}
	
	[Photon.Pun.PunRPC]
	public void SetUp(string attacker, string attacked)
	{
		_attacker.text = attacker;
		_attacked.text = attacked;
	}
	
	private void UnSet()
	{
		_attacker.text = "";
		_attacked.text = "";
	}
	
	[Photon.Pun.PunRPC]
	public void SetParent()
	{
		this.transform.SetParent(GameObject.Find("KillLogBox").transform.GetChild(0).transform);
		this.transform.localScale = Vector3.one;
	}
	
	[Photon.Pun.PunRPC]
	public void ActiveRPC(bool active)
	{
		gameObject.SetActive(active);
	}
}
