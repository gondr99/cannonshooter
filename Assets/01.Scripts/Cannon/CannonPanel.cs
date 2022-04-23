using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CannonPanel : MonoBehaviour
{
    private TextMeshProUGUI _textAngle;
    private Image _imageFill;

    private void Awake()
    {
        _textAngle = transform.Find("TextAngle").GetComponent<TextMeshProUGUI>();
        _imageFill = transform.Find("GaugeMask/ImageFill").GetComponent<Image>();
    }

    public void SetAngleText(float angle)
    {
        _textAngle.SetText($"{Mathf.FloorToInt(angle)}µµ");
    }

    public void SetPowerGauge(float powerNormal)
    {
        _imageFill.fillAmount = powerNormal;    
    }
}
