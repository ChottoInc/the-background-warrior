using UnityEngine;

public class UIShopFilterButton : MonoBehaviour
{
    [SerializeField] UITabShop tabShop;
    [SerializeField] int filterId;

    public void OnButtonClick()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        tabShop.OpenShopWindow(filterId);
    }
}
