using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform hud;
    [SerializeField] RectTransform rightButtonsContainer;
    [SerializeField] UISelectedTabContent selectedTabContent;

    protected VerticalLayoutGroup vLayoutGroup;


    public virtual void OnDestroy()
    {
        SettingsManager.Instance.OnInvertedHUDChange -= OnInvertedHUD;
    }


    public virtual void Awake()
    {
        SettingsManager.Instance.OnInvertedHUDChange += OnInvertedHUD;

        if(hud != null)
        {
            vLayoutGroup = hud.GetComponent<VerticalLayoutGroup>(); 
        }

        OnInvertedHUD(SettingsManager.Instance.IsInvertedHudOn);
    }


    public virtual void Setup()
    {
        // Handle every ui at the start
    }

    protected virtual void OnInvertedHUD(bool isOn)
    {
        if (isOn)
        {
            hud.SetAsFirstSibling();
            vLayoutGroup.childAlignment = TextAnchor.LowerLeft;
            rightButtonsContainer.SetAsFirstSibling();
        }
        else
        {
            hud.SetAsLastSibling();
            vLayoutGroup.childAlignment = TextAnchor.LowerRight;
            rightButtonsContainer.SetAsLastSibling();
        }

        selectedTabContent.SetInvertedTabs(isOn);
    }
}
