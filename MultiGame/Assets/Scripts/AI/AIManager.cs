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
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI"), new Vector3(Random.Range(-20, 30), 0, Random.Range(0, 100)) + Vector3.up, Quaternion.identity, 0, new object[] {_pv.ViewID});
	}
}
