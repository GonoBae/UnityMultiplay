using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MakeRoomMenu : Menu
{
	private const int _minimumPlayer = 1;
	private const int _maximumPlayer = 6;
	private int _playerCountInt = 1;
	
	[SerializeField] InputField _roomNameInputField;
	[SerializeField] Slider _playerCountSlider;
	[SerializeField] Text _playerCount;
	
	private void Start()
	{
		SetPlayerCountSlider();
	}
	
	private void OnDisable()
	{
		ResetCreateRoom();
	}
	
	// 슬라이더 세팅
	private void SetPlayerCountSlider()
	{
		_playerCountSlider.minValue = _minimumPlayer;
		_playerCountSlider.maxValue = _maximumPlayer;
		_playerCountSlider.value = _minimumPlayer;
		_playerCountSlider.onValueChanged.AddListener((v) => {
			_playerCountInt = (int) Mathf.Round(v);
			_playerCount.text = _playerCountInt.ToString("0");
			_playerCountSlider.value = _playerCountInt;
		});
	}
	
	// 버튼
	public void MakeRoomButton()
	{
		if(string.IsNullOrEmpty(_roomNameInputField.text)) return;
		else
		{
			RoomOptions ro = new RoomOptions
			{
				MaxPlayers = (byte)_playerCountInt,
				IsVisible = true,
				IsOpen = true,
				CleanupCacheOnLeave = true,
				PublishUserId = true
			};
			PhotonNetwork.CreateRoom(_roomNameInputField.text, ro);
			MenuManager._Instance.OpenMenu("LoadingMenu");
		}
	}
	
	// 룸 정보 초기화
	public void ResetCreateRoom()
	{
		_roomNameInputField.text = "";
		_playerCountSlider.value = _minimumPlayer;
		_playerCount.text = "1";
	}
}
