using System.Collections.Generic;

public class ShopSaveData
{
    public List<ShopItemSaveData> shopItemSaveDatas;


    public long lastDailyCreationDate;


    public bool hasRedeemedErisCode;



    public ShopSaveData() { }

    public ShopSaveData(ShopManager manager)
    {
        lastDailyCreationDate = manager.LastDailyCreationDate;

        SaveShopItems(manager.DictItemPurchaseInfo);


        hasRedeemedErisCode = manager.HasRedeemedErisCode;
    }


    private void SaveShopItems(Dictionary<string, UtilsShop.ShopItemPurchaseInfo> dict)
    {
        shopItemSaveDatas = new List<ShopItemSaveData>();

        // first save for each story quest
        foreach (var pair in dict)
        {
            // save single progress fro every quest
            ShopItemSaveData shopItemData = new ShopItemSaveData(pair.Key, pair.Value);
            shopItemSaveDatas.Add(shopItemData);
        }
    }
}
