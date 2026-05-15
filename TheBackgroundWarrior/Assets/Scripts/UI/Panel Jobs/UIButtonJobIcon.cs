using UnityEngine;
using UnityEngine.UI;

public class UIButtonJobIcon : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonIconSettings
    {
        public string sceneName;
        public Sprite icon;
    }

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
}
