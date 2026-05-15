using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] UITab[] tabs;

    private UITab currentTab;

    public void ChangeCurrentTab(UITab selected)
    {
        // check if the game is expecting a callback
        if (UITooltipManager.Instance.IsCallbackOpen) return;


        // Since on deselect is called before opening the new tab, if the tab stops time, it will resume for an instant
        // and stop again when called on select on the next
        // just ensure the tabs all share stops time variable

        if (currentTab != null)
        {
            currentTab.OnDeselect();
        }

        currentTab = selected;

        if (currentTab != null)
        {
            currentTab.OnSelect();
        }
    }

    public void SelectFirstTab()
    {
        if(tabs != null)
        {
            if(tabs.Length > 0)
            {
                tabs[0].Select();
            }
        }
    }

    public void CloseCurrentTab()
    {
        if (currentTab != null)
        {
            currentTab.OnDeselect();
        }

        currentTab = null;
    }
}
