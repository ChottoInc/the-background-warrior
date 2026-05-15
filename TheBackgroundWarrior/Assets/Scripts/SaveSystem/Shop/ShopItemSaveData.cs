
public class ShopItemSaveData
{
    public string shopItemId;

    public bool isPurchased;
    public int purchaseCount;


    public ShopItemSaveData() { }

    public ShopItemSaveData(string shopItemId, UtilsShop.ShopItemPurchaseInfo info)
    {
        this.shopItemId = shopItemId;

        isPurchased = info.isPurchased;
        purchaseCount = info.purchaseCount;
    }
}
