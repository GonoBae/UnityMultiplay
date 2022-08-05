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
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero + Vector3.up, Quaternion.identity);
	}
}
