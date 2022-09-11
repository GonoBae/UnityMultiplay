using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OptionUI : MonoBehaviour
{
	public void OutButton()
	{
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
		Destroy(HelperManager._Instance.gameObject);
		
		
		// 마스터 클라이언트인 경우에만 호출되어야 한다.
		// 예제에서는 PhotonNetwork.automaticallySyncScene 을 사용하도록 해놓았기 때문에
		// 룸 안의 모든 접속한 클라이언트에 대해 이 레벨 로드를 유니티가 직접 하는 것이 아닌
		// Photon 이 하도록 하였습니다.
		
		// 이 게임에서 게임을 나가는 것은 개인의 자유이므로 SyncScene 에 대해 생각해보자.
		
		PhotonNetwork.LoadLevel(0);
	}
}
