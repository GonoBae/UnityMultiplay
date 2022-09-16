using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OptionUI : MonoBehaviour
{
	public void OutButton()
	{
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
		HelperManager._Instance._Pv.RPC("DestroyObject", RpcTarget.All);
		PhotonNetwork.LoadLevel(0);
	}
}
