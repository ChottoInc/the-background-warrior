using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UtilsShop;

public class UIShopCardPack : UIShopItem
{
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textPrice;

    private UIPanelShopItems panelShopItems;
    private ShopItemSO itemSO;
    private int currentFilter;

    private bool isPurchased;

    public override void Setup(UIPanelShopItems panelShopItems, ShopItemSO itemSO, int currentFilter)
    {
        this.panelShopItems = panelShopItems;
        this.itemSO = itemSO;
        this.currentFilter = currentFilter;

        ShopItemPurchaseInfo purchaseInfo = ShopManager.Instance.DictItemPurchaseInfo[itemSO.UniqueId];

        isPurchased = false;
        if(itemSO.IsDaily && purchaseInfo.isPurchased)
        {
            isPurchased = true;
        }
        else if(itemSO.IsUnique && purchaseInfo.isPurchased)
        {
            isPurchased = true;
        }

        panelPrice.SetActive(!isPurchased);
        panelPurchased.SetActive(isPurchased);

        imageItem.sprite = itemSO.Sprite;
        textPrice.text = itemSO.Price.ToString();
    }

    public void OnButtonClick()
    {
        if (UITooltipManager.Instance.IsCallbackOpen || isPurchased) return;

        panelShopItems.ShowDetails(itemSO, currentFilter);
    }
}
