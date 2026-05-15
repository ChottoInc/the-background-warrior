using Kirurobo;
using UnityEngine;
using static Kirurobo.UniWindowController;

[RequireComponent(typeof(UniWindowController))]
public class WindowSettingsController : MonoBehaviour
{
    private UniWindowController windowController;

    private bool isInitialized;

    private void Awake()
    {
        windowController = GetComponent<UniWindowController>();


        SettingsManager.Instance.OnAlwaysOnTopChange += SetAlwaysOnTop;
        SettingsManager.Instance.OnClickThroughChange += SetClickThrough;

        windowController.OnStateChanged += Setup;
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.OnAlwaysOnTopChange -= SetAlwaysOnTop;
        SettingsManager.Instance.OnClickThroughChange -= SetClickThrough;
    }

    private void Setup(WindowStateEventType type)
    {
        if (isInitialized) return;

        // ensure is called after the start
        isInitialized = true;

        windowController.isTopmost = SettingsManager.Instance.IsAlwaysOnTop;
        windowController.isHitTestEnabled = SettingsManager.Instance.IsClickThrough;
    }


    private void SetAlwaysOnTop(bool isOn)
    {
        windowController.isTopmost = isOn;
    }

    private void SetClickThrough(bool isOn)
    {
        windowController.isHitTestEnabled = isOn;
    }
}
