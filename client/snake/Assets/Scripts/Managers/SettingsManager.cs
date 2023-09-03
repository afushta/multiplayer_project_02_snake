using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Manager<SettingsManager>
{
    [SerializeField] private Image _colorImage;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Button _startButton;

    private const string PARAM_PLAYER_COLOR = "color";
    private const string PARAM_PLAYER_NAME = "name";

    private Color _playerColor = Color.magenta;
    public string PlayerColorString { get; private set; } = "#FF00FF";
    private string _playerName;

    protected override void Awake()
    {
        base.Awake();
        LoadPlayerColor();
        _colorImage.color = _playerColor;
        _playerName = PlayerPrefs.GetString(PARAM_PLAYER_NAME, "");
        _nameInput.text = _playerName;
        _nameInput.onValueChanged.AddListener((value) => PlayerName = value);

        _startButton.enabled = !string.IsNullOrEmpty(_playerName);
    }

    private Color LoadPlayerColor()
    {
        PlayerColorString = PlayerPrefs.GetString(PARAM_PLAYER_COLOR);
        if (ColorUtility.TryParseHtmlString(PlayerColorString, out Color color))
        {
            _playerColor = color;
        }

        return _playerColor;
    }
    
    public Color PlayerColor
    {
        get => _playerColor;
        set
        {
            _colorImage.color = value;
            _playerColor = value;
            PlayerColorString = "#" + ColorUtility.ToHtmlStringRGB(value);
            PlayerPrefs.SetString(PARAM_PLAYER_COLOR, PlayerColorString);
        }
    }

    public string PlayerName
    {
        get => _playerName;
        set
        {
            _playerName = value;
            PlayerPrefs.SetString(PARAM_PLAYER_NAME, _playerName);
            _startButton.enabled = !string.IsNullOrEmpty(_playerName);
        }
    }
}
