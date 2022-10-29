using UnityEngine;
using Photon.Pun;
using System.IO;

public class AIManager : MonoBehaviour
{
	private PhotonView _pv;
	private int _aiNumber = 40;
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		spawnObjLayer = LayerMask.GetMask("AI");
	}
	
	private void Start()
	{
		if(_pv.IsMine)
		{
			CreateController();
		}
	}
	private LayerMask spawnObjLayer;
	private float overlapBoxSize = 100f;
	
	// AI Spawn
	private void CreateController()
	{
		int num = 0;
		while(num < _aiNumber)
		{
			Vector3 spawnPos = new Vector3(Random.Range(-15, 15), 0f, Random.Range(-15, 15)) + transform.position;
			
			Vector3 overlapBoxScale = new Vector3(overlapBoxSize, overlapBoxSize, overlapBoxSize);
			Collider[] colliderInsideOverlapBox = new Collider[1];
			int numOfColliderFound = Physics.OverlapBoxNonAlloc(spawnPos, overlapBoxScale, colliderInsideOverlapBox, Quaternion.identity, spawnObjLayer);
			
			if(numOfColliderFound == 0)
			{
				GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AI"), spawnPos + Vector3.up, Quaternion.identity, 0, new object[] {_pv.ViewID});
			}
			
			num++;
		}
	}
}
