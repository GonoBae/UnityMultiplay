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
	
	[SerializeField] RawImage _toggleScreteIcon;
	private bool _toggleScrete;
	
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
			string roomName = _roomNameInputField.text;
			bool screte = _toggleScrete;
			RoomOptions ro = new RoomOptions
			{
				MaxPlayers = (byte)_playerCountInt,
				IsVisible = true,
				IsOpen = true,
				CleanupCacheOnLeave = true,
				PublishUserId = true,
				//CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
				//{
				//	{ "Screte", screte}
				//},
				//CustomRoomPropertiesForLobby = new string[] {
				//	"Screte"
				//}
			};
			PhotonNetwork.CreateRoom(roomName, ro);
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
	
	// Screte Room
	public void ToggleScreteButton()
	{
		_toggleScrete = !_toggleScrete;
		if(_toggleScrete)
		{
			_toggleScreteIcon.gameObject.SetActive(true);
		}
		else
		{
			_toggleScreteIcon.gameObject.SetActive(false);
		}
	}
}
