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
		
		// ProtoType 코드 참고하여 제작할 것
		// ProtoType 은 뒤로가면 방으로 가지지만 이 게임은 게임도중 나가면 그냥 로비로 나가야함
		// Loading -> TitleMenu
		
		// Title Menu 에서 캐릭터가 누운 상태로 시작해야함
	}
	
	#endregion
}
