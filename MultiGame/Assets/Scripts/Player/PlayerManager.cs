using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	private PhotonView _pv;
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		if(_pv.IsMine)
		{
			CreateController();
		}
	}
	
	private void CreateController()
	{
		GameManager._Instance._Player = 
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)) + Vector3.up, Quaternion.identity, 0, new object[] {_pv.ViewID})
			.GetComponent<MyPlayer>();
	}
}
