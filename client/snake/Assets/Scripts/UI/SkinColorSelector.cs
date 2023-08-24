using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinColorSelector : MonoBehaviour
{
    [SerializeField] private Image _newColorImage;
    [SerializeField] private Slider _rSlider;
    [SerializeField] private TMP_Text _rValue;
    [SerializeField] private Slider _gSlider;
    [SerializeField] private TMP_Text _gValue;
    [SerializeField] private Slider _bSlider;
    [SerializeField] private TMP_Text _bValue;

    private float _currentRValue;
    private float _currentGValue;
    private float _currentBValue;
    private Color _currentColor;

    private void Awake()
    {
        _rSlider.onValueChanged.AddListener((value) => UpdateColor(value, _currentGValue, _currentBValue));
        _gSlider.onValueChanged.AddListener((value) => UpdateColor(_currentRValue, value, _currentBValue));
        _bSlider.onValueChanged.AddListener((value) => UpdateColor(_currentRValue, _currentGValue, value));
    }

    private void UpdateColor(float r, float g, float b)
    {
        _currentRValue = r;
        _rValue.text = Mathf.Floor(r * 255).ToString();
        _currentGValue = g;
        _gValue.text = Mathf.Floor(g * 255).ToString();
        _currentBValue = b;
        _bValue.text = Mathf.Floor(b * 255).ToString();
        _currentColor = new Color(r, g, b);
        _newColorImage.color = _currentColor;
    }

    public void Show()
    {
        Color color = SettingsManager.Instance.PlayerColor;
        _rSlider.value = color.r;
        _gSlider.value = color.g;
        _bSlider.value = color.b;
        UpdateColor(color.r, color.g, color.b);

        gameObject.SetActive(true);
    }

    public void Save()
    {
        SettingsManager.Instance.PlayerColor = _currentColor;
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
