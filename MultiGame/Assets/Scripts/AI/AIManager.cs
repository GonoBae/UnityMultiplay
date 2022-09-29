using UnityEngine;
using Photon.Pun;
using System.IO;

public class AIManager : MonoBehaviour
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
		for(int i = 0; i < 50; i++)
		{
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI"), new Vector3(Random.Range(-17, 17), 0, Random.Range(-18, 18)) + Vector3.up, Quaternion.identity, 0, new object[] {_pv.ViewID});
		}
	}
}
