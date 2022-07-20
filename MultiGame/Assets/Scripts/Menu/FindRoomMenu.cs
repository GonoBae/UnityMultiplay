using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class FindRoomMenu : Menu
{
	[SerializeField] Transform _roomListContent;
	[SerializeField] GameObject _roomListItemPrefab;
	private Queue<RoomListItem> _waitingRoom = new Queue<RoomListItem>();
	[SerializeField] private List<RoomListItem> _lstRoom = new List<RoomListItem>();
	
	public Transform _RoomListContent { get{return _roomListContent;} }
	public GameObject _RoomListItemPrefab { get{return _roomListItemPrefab;} }
	
	/********** 방 리스트 업데이터 **********/
	public void UpdateRoomList(List<RoomInfo> roomList)
	{
		foreach(RoomInfo info in roomList)
		{
			// 제거
			if(info.RemovedFromList)
			{
				int index = _lstRoom.FindIndex(x => x._Info.Name == info.Name);
				// 있으면
				if(index != -1)
				{
					RoomListItem room = _lstRoom[index];
					RoomItemUnSet(room);
					_lstRoom.RemoveAt(index);
				}
			}
			// 추가
			else
			{
				int index = _lstRoom.FindIndex(x => x._Info.Name == info.Name);
				// 없으면
				if(index == -1)
				{
					RoomListItem item;
					if(_waitingRoom.Count > 0)
					{
						item = _waitingRoom.Dequeue();
						item.SetUp(info);
						item.gameObject.SetActive(true);
					}
					else
					{
						item = Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>();
						if(item != null)
						{
							item.SetUp(info);
						}
					}
					_lstRoom.Add(item);
				}
			}
		}
	}
	
	public void ResetRoomList()
	{
		_lstRoom.Clear();
	}
	
	private void OnDisable()
	{
		if(ServerManager._Instance._CurrentMenu._MenuName == "LoadingMenu")
		{
			// 방에 입장하면 룸 목록 모두 초기화
			RoomListItem[] allRoom = _roomListContent.GetComponentsInChildren<RoomListItem>();
			foreach(var room in allRoom)
			{
				RoomItemUnSet(room);
			}
		}
	}
	
	// 룸 목록 수정
	private void RoomItemUnSet(RoomListItem room)
	{
		room.UnSet();
		_waitingRoom.Enqueue(room);
		room.transform.SetAsLastSibling();
		room.gameObject.SetActive(false);
	}
}
