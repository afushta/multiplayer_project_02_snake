using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private LeaderboardCardUI _leaderboardCardPrefab;

    Dictionary<string, LeaderboardCardUI> playerCards = new Dictionary<string, LeaderboardCardUI>();

    public void OnPlayerAdd(string playerId, PlayerNO player)
    {
        if (playerCards.ContainsKey(playerId)) return;

        LeaderboardCardUI card = Instantiate(_leaderboardCardPrefab, transform);
        card.Init(player.name, player.score, player.color);
        playerCards.Add(playerId, card);
        Rearrange();
    }

    public void OnPlayerRemove(string playerId)
    {
        if (!playerCards.ContainsKey(playerId)) return;

        LeaderboardCardUI card = playerCards[playerId];
        playerCards.Remove(playerId);
        Destroy(card.gameObject);
        Rearrange();
    }

    public void OnScoreChange(string playerId, int score)
    {
        if (!playerCards.ContainsKey(playerId)) return;

        playerCards[playerId].UpdateScore(score);
        Rearrange();
    }

    private void Rearrange()
    {
        int i = 1;
        foreach (var item in playerCards.OrderByDescending(pair => pair.Value.Score))
        {
            LeaderboardCardUI card = item.Value;
            card.transform.SetSiblingIndex(i);
            card.gameObject.SetActive(i <= 5);
            i++;
        }
    }
}
