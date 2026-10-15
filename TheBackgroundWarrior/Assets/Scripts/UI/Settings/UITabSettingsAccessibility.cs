using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsAccessibility : UITabWindow
{
    [SerializeField] Toggle toggleFont100;
    [SerializeField] Toggle toggleFont125;
    [SerializeField] Toggle toggleFont150;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        toggleFont100.SetIsOnWithoutNotify(SettingsManager.Instance.FontSize == SettingsManager.AccessibilityFontSize.F100);
        toggleFont125.SetIsOnWithoutNotify(SettingsManager.Instance.FontSize == SettingsManager.AccessibilityFontSize.F125);
        toggleFont150.SetIsOnWithoutNotify(SettingsManager.Instance.FontSize == SettingsManager.AccessibilityFontSize.F150);
    }

    public void OnToggleFontSize100(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        AudioManager.Instance.PlayClickUI();

        // set size
        SettingsManager.Instance.SetTargeFontSize(SettingsManager.AccessibilityFontSize.F100);
    }

    public void OnToggleFontSize125(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        AudioManager.Instance.PlayClickUI();

        // set size
        SettingsManager.Instance.SetTargeFontSize(SettingsManager.AccessibilityFontSize.F125);
    }

    public void OnToggleFontSize150(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        AudioManager.Instance.PlayClickUI();

        // set size
        SettingsManager.Instance.SetTargeFontSize(SettingsManager.AccessibilityFontSize.F150);
    }
}
