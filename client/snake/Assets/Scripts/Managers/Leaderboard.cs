using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Leaderboard : Manager<Leaderboard>
{
    [SerializeField] private LeaderboardUI _leaderboardUI;

    private void Update()
    {
        _leaderboardUI.gameObject.SetActive(Input.GetKey(KeyCode.Tab));
    }

    private class NameScorePair
    {
        public string name;
        public int score;
    }

    private Dictionary<string, NameScorePair> leaderboard = new Dictionary<string, NameScorePair>();

    public void AddPlayer(string playerId, PlayerNO player)
    {
        if (leaderboard.ContainsKey(playerId)) return;

        leaderboard.Add(playerId, new () { name = player.name, score = player.score });

        _leaderboardUI.OnPlayerAdd(playerId, player);
    }

    public void RemovePlayer(string playerId)
    {
        if (!leaderboard.ContainsKey(playerId)) return;

        leaderboard.Remove(playerId);

        _leaderboardUI.OnPlayerRemove(playerId);
    }

    public void UpdateScore(string playerId, int score)
    {
        if (!leaderboard.ContainsKey(playerId)) return;

        leaderboard[playerId].score = score;

        _leaderboardUI.OnScoreChange(playerId, score);
    }
}
