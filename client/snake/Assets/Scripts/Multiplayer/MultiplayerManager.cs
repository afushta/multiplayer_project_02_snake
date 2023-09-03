using System.Collections.Generic;
using Colyseus;
using UnityEngine;


public enum ConnectionMode { Create, Join, JoinOrCreate }


public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private const string ROOM_NAME = "snake_room";

    private ColyseusRoom<StateNO> _room;
    public string SessionId => _room?.SessionId;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeClient();
    }

    private Dictionary<string, object> GetOptions()
    {
        return new() {
            { "name", SettingsManager.Instance.PlayerName },
            { "color", SettingsManager.Instance.PlayerColorString },
        };
    }

    public async void JoinOrCreateRoom()
    {
        _room = await Instance.client.JoinOrCreate<StateNO>(ROOM_NAME, GetOptions());
        _room.OnStateChange += OnChange;
    }

    public async void JoinRoom(string roomId)
    {
        _room = await Instance.client.Join<StateNO>(roomId, GetOptions());
        _room.OnStateChange += OnChange;
    }

    public async void CreateRoom(string roomName)
    {
        var options = GetOptions();
        options.Add("roomName", roomName);
        _room = await Instance.client.Create<StateNO>(ROOM_NAME, options);
        _room.OnStateChange += OnChange;
    }

    public void LeaveRoom()
    {
        _room?.Leave();
    }

    private void OnChange(StateNO state, bool isFirstState)
    {
        if (!isFirstState) return;
        _room.OnStateChange -= OnChange;

        state.players.ForEach(
            (playerId, player) =>
            {
                if (playerId == SessionId) GameManager.Instance.CreatePlayer(player);
                else GameManager.Instance.CreateEnemy(playerId, player);
            }
        );

        state.players.OnAdd += GameManager.Instance.CreateEnemy;
        state.players.OnRemove += GameManager.Instance.RemoveEnemy;

        state.apples.ForEach(GameManager.Instance.CreateApple);
        state.apples.OnAdd += GameManager.Instance.CreateApple;
        state.apples.OnRemove += GameManager.Instance.RemoveApple;
    }

    public async IAsyncEnumerable<ColyseusRoomAvailable> GetAvailableRooms()
    {
        foreach (ColyseusRoomAvailable room in await Instance.client.GetAvailableRooms())
        {
            yield return room;
        }
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public new void SendMessage(string key)
    {
        _room.Send(key);
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }
}
