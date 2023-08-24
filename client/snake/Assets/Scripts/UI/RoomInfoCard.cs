using Colyseus;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoCard : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomNameLabel;
    [SerializeField] private TMP_Text _roomPlayersLabel;
    [SerializeField] private TMP_Text _roomPingLabel;
    [SerializeField] private Button _connectButton;

    private string _roomId;

    public void Setup(ColyseusRoomAvailable roomData, Action<string> connectCallback)
    {
        _roomId = roomData.name;
        _roomNameLabel.text = roomData.name;
        _roomPlayersLabel.text = $"{roomData.clients} / {roomData.maxClients}";
        _roomPingLabel.text = "unknown";
        _connectButton.onClick.AddListener(() => connectCallback?.Invoke(_roomId));
    }
}
