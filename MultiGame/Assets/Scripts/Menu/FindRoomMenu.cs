using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class FindRoomMenu : Menu
{
	[SerializeField] Transform _roomListContent;
	[SerializeField] GameObject _roomListItemPrefab;
	// 오버젝트 풀을 위한 Queue
	private Queue<RoomListItem> _waitingRoom = new Queue<RoomListItem>();
	[SerializeField] private List<RoomListItem> _lstRoom = new List<RoomListItem>();
	
	public Transform _RoomListContent { get{return _roomListContent;} }
	public GameObject _RoomListItemPrefab { get{return _roomListItemPrefab;} }
	
	/********** 방 리스트 업데이터 **********/
	public void UpdateRoomList(List<RoomInfo> roomList)
	{
		foreach(RoomInfo info in roomList)
		{
			int index = _lstRoom.FindIndex(x => x._Info.Name == info.Name);
			// 사라진 방이 존재한다면
			if(info.RemovedFromList)
			{
				
				// 있으면 해당 방을 리스트에서 제거
				if(index != -1)
				{
					RoomListItem room = _lstRoom[index];
					RoomItemUnSet(room);
					_lstRoom.RemoveAt(index);
				}
			}
			else
			{
				// 사라진 방이 없으면
				if(index == -1)
				{
					RoomListItem item;
					// 현재 풀링할 수 있는 룸 목록이 있다면 세팅 후 활성화
					if(_waitingRoom.Count > 0)
					{
						item = _waitingRoom.Dequeue();
						item.SetUp(info);
						item.gameObject.SetActive(true);
					}
					// 없다면 프리팹을 가져온 후 세팅 후 리스트에 담아준다.
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
