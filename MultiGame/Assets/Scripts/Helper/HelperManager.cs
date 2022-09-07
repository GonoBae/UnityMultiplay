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
			if(PhotonNetwork.CurrentRoom.Name != "")
			{
				
			}
		}
		else if(scene.buildIndex == 1)
		{
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
	}
	
	private IEnumerator Room()
	{
		while(ServerManager._Instance._CurrentMenu._MenuName != "RoomMenu")
		{
			yield return null;
		}
		
		RoomMenu menu = ServerManager._Instance._CurrentMenu as RoomMenu;
		
	}
	
	#endregion
}
