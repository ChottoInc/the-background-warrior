using UnityEngine;
using UnityEngine.UI;

public class UIShopFilterButton : MonoBehaviour
{
    [SerializeField] UITabShop tabShop;
    [SerializeField] int filterId;

    [Header("Highlight")]
    [SerializeField] Image imageSelected;
    [SerializeField] Color selectedColor;

    public void OnButtonClick()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        tabShop.OpenShopWindow(this, filterId);
    }

    public void SelectButton(bool selected)
    {
        if (selected)
            imageSelected.color = selectedColor;
        else
            imageSelected.color = Color.white;
    }
}
