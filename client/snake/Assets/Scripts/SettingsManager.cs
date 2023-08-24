using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Manager<SettingsManager>
{
    [SerializeField] private Image _colorImage;

    private const string PARAM_PLAYER_COLOR = "color";

    private Color _playerColor = Color.magenta;
    public string PlayerColorString { get; private set; } = "#FF00FF";

    protected override void Awake()
    {
        base.Awake();
        LoadPlayerColor();
        _colorImage.color = _playerColor;
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
}
