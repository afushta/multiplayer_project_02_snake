using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    [SerializeField] private Snake _snake;
    private PlayerNO _player;
    private string _playerId;

    public void Init(PlayerNO player, string playerId, bool isPlayer = false)
    {
        _player = player;
        _playerId = playerId;
        _player.OnChange += OnChange;
        _snake.Init(_player.size, player.color, playerId, isPlayer);
    }

    private void OnChange(List<DataChange> changes)
    {
        if (!_snake) return;

        Vector3 position = _snake.transform.position;

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    position.x = (float)change.Value;
                    break;
                case "z":
                    position.z = (float)change.Value;
                    break;
                case "size":
                    _snake.SetSegmentsCount((byte)change.Value);
                    break;
                case "score":
                    int score = (ushort)change.Value;
                    Leaderboard.Instance.UpdateScore(_playerId, score);
                    break;
                default:
                    Debug.LogWarning($"Change of field {change.Field} is not supported in PlayerNO");
                    break;
            }
        }

        _snake.LookAt(position);
    }

    public void Destroy()
    {
        if(_player != null) _player.OnChange -= OnChange;
        _snake.Destroy();
    }
}
