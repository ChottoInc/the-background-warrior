using UnityEngine;

public class UISelectedTabContent : MonoBehaviour
{
    [SerializeField] RectTransform[] tabs;

    private bool isInit;
    private bool lastInverted;

    public void SetInvertedTabs(bool isOn)
    {
        foreach (var tab in tabs)
        {
            Vector2 startTabPos = tab.localPosition;

            if (isOn)
            {
                tab.anchorMin = Vector2.zero;
                tab.anchorMax = Vector2.zero;

                if (!isInit)
                {
                    tab.localPosition = new Vector2(-startTabPos.x, startTabPos.y);
                }
            }
            else
            {
                tab.anchorMin = Vector2.right;
                tab.anchorMax = Vector2.right;
            }

            if (isInit && lastInverted != isOn)
            {
                tab.localPosition = new Vector2(-startTabPos.x, startTabPos.y);
            }
        }

        lastInverted = isOn;

        if (!isInit)
            isInit = true;
    }
}
