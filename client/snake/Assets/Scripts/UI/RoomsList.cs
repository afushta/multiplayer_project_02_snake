using Colyseus;
using UnityEngine;

public class RoomsList : MonoBehaviour
{
    [SerializeField] private RoomInfoCard _roomInfoPrefab;
    [SerializeField] private Transform _containerRoot;

    private async void Setup()
    {
        for (int i = 1; i < _containerRoot.childCount; i++)
        {
            Destroy(_containerRoot.GetChild(1).gameObject);
        }

        await foreach (ColyseusRoomAvailable room in MultiplayerManager.Instance.GetAvailableRooms())
        {
            RoomInfoCard roomInfo = Instantiate(_roomInfoPrefab, _containerRoot);
            roomInfo.Setup(room, ConnectToRoom);
        }
    }

    public void ConnectToRoom(string roomId)
    {
        MultiplayerManager.Instance.JoinRoom(roomId);
        Hide();
    }

    public void CreateNewRoom(string roomName)
    {
        MultiplayerManager.Instance.CreateRoom(roomName);
        Hide();
    }

    public void QuickConnect()
    {
        MultiplayerManager.Instance.JoinOrCreateRoom();
        Hide();
    }

    public void Refresh()
    {
        Setup();
    }

    public void Show()
    {
        Setup();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
