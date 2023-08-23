using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    [SerializeField] private Snake _snake;
    private PlayerNO _player;

    public void Init(PlayerNO player)
    {
        _player = player;
        _player.OnChange += OnChange;

        _snake.Init(_player.d);
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
                case "d":
                    _snake.SetSegmentsCount((byte)change.Value);
                    break;
                default:
                    Debug.LogWarning($"Change of field {change.Field} is not supported");
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
