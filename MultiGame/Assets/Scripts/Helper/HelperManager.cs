using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class HelperManager : MonoBehaviour
{
	#region Singleton
	
	private static HelperManager _instance;
	public static HelperManager _Instance
	{
		get
		{
			if(!_instance) _instance = FindObjectOfType(typeof(HelperManager)) as HelperManager;
			return _instance;
		}
	}
	
	#endregion
	
	#region Fields
	
	private PhotonView _pv;
	public bool _first = true;
	
	#endregion
	
	#region Properties
	
	public PhotonView _Pv { get{return _pv;} }
	
	#endregion
	
	#region Unity Events
	
	private void Awake()
	{
		if(_instance != null && _instance != this) Destroy(gameObject);
		else _instance = this;
		DontDestroyOnLoad(gameObject);
		
		_pv = GetComponent<PhotonView>();
	}
	
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	
	#endregion
	
	#region Custom Methods
	
	[PunRPC]
	public void KickPlayer()
	{
		PhotonNetwork.LeaveRoom();
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode load)
	{
		if(scene.buildIndex == 0 && PhotonNetwork.InRoom)
		{
			_first = false;
			
			StartCoroutine("OutRoom");
		}
		else if(scene.buildIndex == 1)
		{
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
	}
	
	private IEnumerator OutRoom()
	{
		yield return null;
		
		PhotonNetwork.LeaveRoom();
	}
	
	[PunRPC]
	public void DestroyObject()
	{
		Destroy(HelperManager._Instance.gameObject);
	}
	#endregion
}
