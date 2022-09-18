using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class OptionUI : MonoBehaviour
{
	public void OutButton()
	{
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
		Destroy(HelperManager._Instance.gameObject);
		PhotonNetwork.AutomaticallySyncScene = false;
		SceneManager.LoadScene("Main");
	}
}
