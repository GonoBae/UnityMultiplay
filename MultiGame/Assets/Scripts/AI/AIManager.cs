using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		else {
			Transform parent = FindObjectOfType<AIManager>().transform;
			AI[] ais = GameObject.FindObjectsOfType<AI>();
			for(int i = 0; i < ais.Length; i++) {
				ais[i].transform.SetParent(parent);
			}
		}
	}
	private LayerMask spawnObjLayer;
	private float overlapBoxSize = 100f;
	
	// AI Spawn
	private void CreateController()
	{
		Transform parent = FindObjectOfType<AIManager>().transform;
		int num = 0;
		while(num < _aiNumber)
		{
			Vector3 spawnPos = new Vector3(Random.Range(-15, 15), 0f, Random.Range(-15, 15)) + transform.position;
			
			Vector3 overlapBoxScale = new Vector3(overlapBoxSize, overlapBoxSize, overlapBoxSize);
			Collider[] colliderInsideOverlapBox = new Collider[1];
			int numOfColliderFound = Physics.OverlapBoxNonAlloc(spawnPos, overlapBoxScale, colliderInsideOverlapBox, Quaternion.identity, spawnObjLayer);
			
			if(numOfColliderFound == 0)
			{
				GameObject obj = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "AI"), spawnPos + Vector3.up, Quaternion.identity, 0, new object[] {_pv.ViewID});
				if(parent != null)
				{
					obj.transform.SetParent(parent);
				}
			}
			
			num++;
		}
	}
}
