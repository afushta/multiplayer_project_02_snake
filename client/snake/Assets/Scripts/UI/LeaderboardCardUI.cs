using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private Image _playerColor;

    public int Score { get; private set; }

    public void Init(string name, int score, string colorString)
    {
        _playerName.text = name;
        _playerScore.text = score.ToString();
        Score = score;

        if (ColorUtility.TryParseHtmlString(colorString, out Color color))
        {
            _playerColor.color = color;
        }
    }

    public void UpdateScore(int score)
    {
        _playerScore.text = score.ToString();
        Score = score;
    }
}
