using System.Collections.Generic;
using UnityEngine;


public class GameManager : Manager<GameManager>
{
    [SerializeField] private NetworkController _snakePrefab;
    [SerializeField] private Controller _controllerPrefab;
    [SerializeField] private Apple _applePrefab;

    private Dictionary<string, NetworkController> _enemies = new Dictionary<string, NetworkController>();
    private Dictionary<string, Apple> _apples = new Dictionary<string, Apple>();

    public void CreatePlayer(PlayerNO player)
    {
        Vector3 position = new(player.x, 0f, player.z);
        NetworkController snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player, MultiplayerManager.Instance.SessionId, isPlayer: true);

        Controller controller = Instantiate(_controllerPrefab, position, Quaternion.identity);
        controller.Init(snake);

        Leaderboard.Instance.AddPlayer(MultiplayerManager.Instance.SessionId, player);
    }

    public void CreateEnemy(string playerId, PlayerNO player)
    {
        Vector3 position = new(player.x, 0f, player.z);
        NetworkController snake = Instantiate(_snakePrefab, position, Quaternion.identity);
        snake.Init(player, playerId);
        
        _enemies.Add(playerId, snake);

        Leaderboard.Instance.AddPlayer(playerId, player);
    }

    public void RemoveEnemy(string playerId, PlayerNO player)
    {
        if (!_enemies.ContainsKey(playerId)) return;

        NetworkController enemy = _enemies[playerId];
        _enemies.Remove(playerId);
        enemy.Destroy();

        Leaderboard.Instance.RemovePlayer(playerId);
    }

    public void CreateApple(string appleId, AppleNO appleNO)
    {
        Vector3 position = new Vector3(appleNO.x, 0f, appleNO.z);
        Apple apple = Instantiate(_applePrefab, position, Quaternion.identity);
        apple.Init(appleId, appleNO);
        _apples.Add(appleId, apple);
    }

    public void RemoveApple(string appleId, AppleNO appleNO)
    {
        if (!_apples.ContainsKey(appleId)) return;

        Apple apple = _apples[appleId];
        _apples.Remove(appleId);
        apple.Destroy();
    }

    public void GameOver()
    {
        FindObjectOfType<Controller>().Destroy();
    }
}
