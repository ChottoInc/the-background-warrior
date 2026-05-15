using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsVideo : UITabWindow
{
    [SerializeField] Toggle toggleAlwaysOnTop;

    [Space(10)]
    [SerializeField] Toggle toggleClickThrough;

    [Space(10)]
    [SerializeField] Toggle toggle30FPS;
    [SerializeField] Toggle toggle60FPS;

    private int currentMonitorIndex;
    private int possibleIndexes;

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        toggleAlwaysOnTop.SetIsOnWithoutNotify(SettingsManager.Instance.IsAlwaysOnTop);
        toggleClickThrough.SetIsOnWithoutNotify(SettingsManager.Instance.IsClickThrough);

        toggle30FPS.SetIsOnWithoutNotify(!SettingsManager.Instance.Is60FPS);
        toggle60FPS.SetIsOnWithoutNotify(SettingsManager.Instance.Is60FPS);

        currentMonitorIndex = SettingsManager.Instance.CurrentMonitorIndex;

        possibleIndexes = Display.displays.Length;

        // if the saved index is greater than the displays reset to 0
        if(currentMonitorIndex >= possibleIndexes )
        {
            currentMonitorIndex = 0;
        }
    }


    public void OnToggleAlwaysOnTop(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsAlwaysOnTop(isOn);
    }

    public void OnToggleClickThrough(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsClickThrough(isOn);
    }



    public void OnToggle30FPS(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        AudioManager.Instance.PlayClickUI();

        // set to 30 fps if on
        SettingsManager.Instance.SetIs60FPS(!isOn);
    }

    public void OnToggle60FPS(bool isOn)
    {
        // do something only if isOn, so only one of the group make changes
        if (!isOn) return;

        AudioManager.Instance.PlayClickUI();

        SettingsManager.Instance.SetIs60FPS(isOn);
    }


    public void OnButtonChangeMonitor()
    {
        // check if more than 1 display is available
        if (possibleIndexes <= 1) return;

        currentMonitorIndex++;

        // if next display is not available go back to start
        if(currentMonitorIndex >= possibleIndexes)
        {
            currentMonitorIndex = 0;
        }

        // tell settings manager to swap
        SettingsManager.Instance.SetCurrentMonitorIndex(currentMonitorIndex, false, true);
    }
}
