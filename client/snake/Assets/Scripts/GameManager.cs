using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private NetworkController _snakePrefab;
    [SerializeField] private Controller _controllerPrefab;

    private Dictionary<string, NetworkController> _enemies = new Dictionary<string, NetworkController>();

    public void CreatePlayer(PlayerNO player)
    {
        Vector3 position = new(player.x, 0f, player.z);
        NetworkController snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player);

        AttachCameraToPlayer(snake.gameObject);
        Controller controller = Instantiate(_controllerPrefab, position, Quaternion.identity);
        controller.Init(snake.transform);
    }

    private void AttachCameraToPlayer(GameObject player)
    {
        player.AddComponent<CameraController>();
    }

    public void CreateEnemy(string playerId, PlayerNO player)
    {
        Vector3 position = new(player.x, 0f, player.z);
        NetworkController snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player);
        
        _enemies.Add(playerId, snake);
    }

    public void RemoveEnemy(string playerId, PlayerNO player)
    {
        NetworkController enemy = _enemies[playerId];
        _enemies.Remove(playerId);
        enemy.Destroy();
    }
}
