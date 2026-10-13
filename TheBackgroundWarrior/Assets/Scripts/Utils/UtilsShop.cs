using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsShop
{
    public const int ID_SHOP_FILTER_CARDPACKS = 0;
    public const int ID_SHOP_FILTER_JOBS = 1;
    public const int ID_SHOP_FILTER_BAITS = 2;

    public const int ID_SHOP_FILTER_REDEEM = 9;
    public const int ID_SHOP_FILTER_DEBUG = 10;
    public const int ID_SHOP_FILTER_ACHIEVEMENTS = 11;


    public const string REDEEM_ERIS_CODE = "85641";
    public const int ID_REDEEM_ERIS_CODE = 0;


    public enum ShopItemType { CardPack, Job, Baits }

    private static Dictionary<string, ShopItemSO> dictShopItems;

    public static void Initialize()
    {
        LoadItems();
    }

    private static void LoadItems()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Shop/ContainerGameData_ShopItems");
        dictShopItems = container.Entries.OfType<ShopItemSO>().ToDictionary(e => e.UniqueId);
    }

    public static List<ShopItemSO> GetAllItems()
    {
        return dictShopItems.Values.ToList();
    }

    public static ShopItemSO GetItemById(string id)
    {
        return UtilsGeneral.GetGameDataSO<ShopItemSO>(id, dictShopItems);
    }

    public static T[] GetAllTypeItem<T>() where T : ShopItemSO
    {
        return dictShopItems.Values.OfType<T>().OrderBy(item => item.Id).ToArray();
    }




    [System.Serializable]
    public struct ShopItemPurchaseInfo
    {
        public bool isPurchased;
        public int purchaseCount;

        public ShopItemPurchaseInfo(ShopItemSaveData saveData)
        {
            isPurchased = saveData.isPurchased;
            purchaseCount = saveData.purchaseCount;
        }
    }
}
