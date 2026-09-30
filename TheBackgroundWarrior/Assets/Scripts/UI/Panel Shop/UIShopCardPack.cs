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

    public bool IsPurchased { get; private set; }

    public override void Setup(UIPanelShopItems panelShopItems, ShopItemSO itemSO, int currentFilter)
    {
        this.panelShopItems = panelShopItems;
        this.itemSO = itemSO;
        this.currentFilter = currentFilter;

        ShopItemPurchaseInfo purchaseInfo = ShopManager.Instance.DictItemPurchaseInfo[itemSO.UniqueId];

        IsPurchased = false;
        if(itemSO.IsDaily && purchaseInfo.isPurchased)
        {
            IsPurchased = true;
        }
        else if(itemSO.IsUnique && purchaseInfo.isPurchased)
        {
            IsPurchased = true;
        }

        //Debug.Log("id: " + itemSO.UniqueId + ", purchase info file: " + purchaseInfo.isPurchased);
        //Debug.Log("Purchase info game: " + IsPurchased);

        panelPrice.SetActive(!IsPurchased);
        panelPurchased.SetActive(IsPurchased);

        imageItem.sprite = itemSO.Sprite;
        textPrice.text = itemSO.Price.ToString();
    }

    public void OnButtonClick()
    {
        if (UITooltipManager.Instance.IsCallbackOpen || IsPurchased) return;

        panelShopItems.ShowDetails(itemSO, currentFilter);
    }
}
