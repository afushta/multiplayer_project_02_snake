using System.Collections.Generic;
using Colyseus;
using UnityEditor;
using UnityEngine;

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

    public void Connect()
    {
        Connection();
    }

    public void LeaveRoom()
    {
        _room?.Leave();
    }

    private async void Connection()
    {
        Dictionary<string, object> options = new() {
            { "c", SettingsManager.Instance.PlayerColorString },
        };

        _room = await Instance.client.JoinOrCreate<StateNO>(ROOM_NAME, options);
        _room.OnStateChange += OnChange;
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
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        LeaveRoom();
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }
}
