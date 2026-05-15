using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] protected Image imageItem;
    [SerializeField] protected TMP_Text textAmount;

    protected UIPanelItems panelItems;
    protected ItemGroup group;
    protected ItemSO itemSO;

    public virtual void Setup(UIPanelItems panelItems, ItemGroup group, ItemSO itemSO)
    {
        BaseSetup(panelItems, group, itemSO);

        imageItem.sprite = UtilsItem.GetItemById(group.IdItem).Sprite;
    }

    protected virtual void BaseSetup(UIPanelItems panelItems, ItemGroup group, ItemSO itemSO)
    {
        this.panelItems = panelItems;
        this.group = group;
        this.itemSO = itemSO;

        textAmount.text = group.Quantity.ToString();
    }

    public void OnItemClick()
    {
        panelItems.ShowDetails(group);
    }
}
