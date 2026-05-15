using System.Collections.Generic;
using UnityEngine;

public static class UtilsShop
{
    public const int ID_SHOP_FILTER_CARDPACKS = 0;
    public const int ID_SHOP_FILTER_JOBS = 1;
    public const int ID_SHOP_FILTER_REDEEM = 10;


    public const string REDEEM_ERIS_CODE = "85641";
    public const int ID_REDEEM_ERIS_CODE = 0;


    public enum ShopItemType { CardPack, Job }

    private static List<ShopItemSO> cardPacks;
    private static List<ShopItemSO> jobs;

    private static List<ShopItemSO> otherShopItems;

    public static void Initialize()
    {
        LoadAllItems();
    }

    private static void LoadAllItems()
    {
        ShopItemSO[] items = Resources.LoadAll<ShopItemSO>("Data/Shop");

        cardPacks = new List<ShopItemSO>();
        jobs = new List<ShopItemSO>();
        otherShopItems = new List<ShopItemSO>();

        foreach (ShopItemSO item in items)
        {
            switch (item.ShopItemType)
            {
                default: otherShopItems.Add(item); break;
                case ShopItemType.CardPack: cardPacks.Add(item); break;
                case ShopItemType.Job: jobs.Add(item); break;
            }
        }
    }

    public static List<ShopItemSO> GetAllItems()
    {
        List<ShopItemSO> result = new List<ShopItemSO>();
        result.AddRange(cardPacks);
        result.AddRange(jobs);
        result.AddRange(otherShopItems);
        return result;
    }

    public static ShopItemSO GetItemById(string id)
    {
        List<ShopItemSO> items = GetAllItems();

        foreach (ShopItemSO item in items)
        {
            if (item.UniqueId == id)
                return item;
        }
        return null;
    }


    #region CARD PACKS

    public static ShopItemSO GetCardPackById(string id)
    {
        foreach (ShopItemSO pack in cardPacks)
        {
            if (pack.UniqueId == id)
                return pack;
        }
        return null;
    }

    public static List<ShopItemSO> GetAllCardPacks()
    {
        return cardPacks;
    }

    #endregion


    #region CARD PACKS

    public static ShopItemSO GetShopJobById(string id)
    {
        foreach (ShopItemSO job in jobs)
        {
            if (job.UniqueId == id)
                return job;
        }
        return null;
    }

    public static List<ShopItemSO> GetAllShopJobs()
    {
        return jobs;
    }

    #endregion






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
