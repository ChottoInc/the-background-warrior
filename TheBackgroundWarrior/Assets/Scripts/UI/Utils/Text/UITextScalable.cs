using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UITextScalable : MonoBehaviour
{
    private TMP_Text _text;
    private float _baseSize;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        // save size in the editor

        if(_text != null)
            _baseSize = _text.fontSize;

        SettingsManager.Instance.OnFontSizeChange += Apply;
    }


    private void OnEnable()
    {
        Apply(SettingsManager.Instance.FontSize);
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.OnFontSizeChange -= Apply;
    }

    private void Apply(SettingsManager.AccessibilityFontSize fontSize)
    {
        if (_text == null) return;

        float scaleFactor = 1f;
        switch (fontSize)
        {
            case SettingsManager.AccessibilityFontSize.F100: scaleFactor = 1f; break;
            case SettingsManager.AccessibilityFontSize.F125: scaleFactor = 1.25f; break;
            case SettingsManager.AccessibilityFontSize.F150: scaleFactor = 1.5f; break;
        }

        //Debug.Log("scale factor: " + scaleFactor);
        //Debug.Log("base font: " + _baseSize);
        _text.fontSize =  _baseSize * scaleFactor;
        //Debug.Log("font: " + _text.fontSize);
    }
}
