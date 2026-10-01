using UnityEngine;
using UnityEngine.UI;

public class UIButtonJobIcon : MonoBehaviour
{
    [Header("Notification")]
    [SerializeField] UIButtonJobTab[] _buttonJobs;
    [SerializeField] GameObject _notificationObj;


    [System.Serializable]
    public struct ButtonIconSettings
    {
        public string sceneName;
        public Sprite icon;
    }

    [Header("Scene")]
    [SerializeField] Image imageIcon;
    [SerializeField] ButtonIconSettings[] iconSettings;


    private void Start()
    {
        LastSceneSettings sceneSettings = SettingsManager.Instance.LastSceneSettings;

        foreach (var iconSetting in iconSettings)
        {
            if(iconSetting.sceneName == sceneSettings.lastSceneName)
            {
                imageIcon.sprite = iconSetting.icon;
            }
        }
    }

    private void Update()
    {
        bool isShowing = false;
        foreach (var button in _buttonJobs)
        {
            if (button.IsNotificationShowing)
            {
                isShowing = true;
                break;
            }
        }

        // if the state is different from what it should be, change it, just a performance check
        if (_notificationObj.activeSelf != isShowing)
            _notificationObj.SetActive(isShowing);
    }
}
