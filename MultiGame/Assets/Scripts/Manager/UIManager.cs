using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
	private PhotonView _pv;
	public PhotonView _PV { get{return _pv;} }
	
	[SerializeField] Text _type;
	[SerializeField] Text _quest;
	[SerializeField] Text _master;
	
	[Header("KillLog")]
	[SerializeField] GameObject _killLogPrefab;
	[SerializeField] Transform _killLogContent;
	
	private static UIManager _instance = null;
	public static UIManager _Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<UIManager>();
			}
			return _instance;
		}
	}
	
	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		if(_pv.IsMine) _pv.RPC("Test", RpcTarget.All);
	}
	
	public void SetQuestUI(string quest)
	{
		_quest.text = quest;
	}
	
	public void SetTypeUI(string type)
	{
		_type.text = type;
	}
	
	public void SetmasterUI()
	{
		_master.text = PhotonNetwork.MasterClient.NickName.ToString();
	}
	
	//[PunRPC]
	//public void ShowKillLog(string attacker, string attacked)
	//{
	//	if(_pv.IsMine)
	//	{
	//		ObjectPooler._Instance.PoolInstantiate("KillLog").GetComponent<KillLog>().SetUp(attacker, attacked);
	//	}
	//}
	
	[PunRPC]
	private void Test()
	{
		ObjectPooler._Instance.PrePoolInstantiate();
	}
}
