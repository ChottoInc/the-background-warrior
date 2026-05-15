using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UtilsShop;

public class ShopManager : MonoBehaviour
{
    private IDataService saveService;



    // Store every shop item purchase info
    private Dictionary<string, ShopItemPurchaseInfo> dictItemPurchaseInfo;

    // List of all shop items
    private List<string> shopItemsList;


    private long lastDailyCreationDate;


    // Redeem codes

    private bool hasRedeemedErisCode;



    public Dictionary<string, ShopItemPurchaseInfo> DictItemPurchaseInfo => dictItemPurchaseInfo;

    public List<string> ShopItemsList => shopItemsList;

    public long LastDailyCreationDate => lastDailyCreationDate;


    public bool HasRedeemedErisCode => hasRedeemedErisCode;



    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    //Called after player manager
    public void Setup(IDataService service)
    {
        saveService = service;

        try
        {
            ShopSaveData saveData = saveService.LoadData<ShopSaveData>(UtilsSave.GetShopFile(), SettingsManager.Instance.FileEncryption);

            // set items progress default values
            InitializeAllItems();

            // set redeem codes default values
            InitializeRedeemCodes();

            // update from file
            SetupFromFile(saveData);
        }
        catch
        {
            SetupFromDefault();
            SaveShopData();

            //Debug.Log("Datas quest: " + dictQuestsStoryProgress.Count);
            //Debug.Log("Datas quest active: " + activeStoryQuests.Count);
        }
    }

    #region DEFAULT

    private void SetupFromDefault()
    {
        /*
         * check on daily, if day changed reset purchase
         * */

        lastDailyCreationDate = DateTime.UtcNow.Ticks;

        InitializeAllItems();

        InitializeRedeemCodes();
    }

    private void InitializeAllItems()
    {
        // initialize dict and all items

        if(shopItemsList == null)
            shopItemsList = new List<string>();

        if(dictItemPurchaseInfo == null)
            dictItemPurchaseInfo = new Dictionary<string, ShopItemPurchaseInfo>();

        // create default for every item
        ShopItemSO[] allItems = GetAllItems().ToArray();
        for (int i = 0; i < allItems.Length; i++)
        {
            // get so
            ShopItemSO so = allItems[i];

            // init if item isn't in dict
            if (!dictItemPurchaseInfo.ContainsKey(so.UniqueId))
            {
                ShopItemPurchaseInfo purchaseInfo = new ShopItemPurchaseInfo();

                purchaseInfo.isPurchased = false;
                purchaseInfo.purchaseCount = 0;

                // save in dictionary
                dictItemPurchaseInfo.Add(so.UniqueId, purchaseInfo);
            }
        }
    }

    private void InitializeRedeemCodes()
    {
        hasRedeemedErisCode = false;
    }

    #endregion
    
    #region FROM FILE

    private void SetupFromFile(ShopSaveData saveData)
    {
        lastDailyCreationDate = saveData.lastDailyCreationDate;

        LoadShopItems(saveData.shopItemSaveDatas);

        // check for daily using date
        DateTime lastDailyDate = new DateTime(lastDailyCreationDate, DateTimeKind.Utc);
        if (DateTime.UtcNow.Date != lastDailyDate)
        {
            ResetDailyItems();
        }


        hasRedeemedErisCode = saveData.hasRedeemedErisCode;
    }

    private void LoadShopItems(List<ShopItemSaveData> datas)
    {
        if (shopItemsList == null)
            shopItemsList = new List<string>();

        if (dictItemPurchaseInfo == null)
            dictItemPurchaseInfo = new Dictionary<string, ShopItemPurchaseInfo>();

        // used for debug infos
        int exceptionIndex = 0;

        try
        {
            // for every save found in file update the progress
            for (int i = 0; i < datas.Count; i++)
            {
                exceptionIndex = i;

                // save in dictionary
                ShopItemPurchaseInfo dataProgress = new ShopItemPurchaseInfo(datas[i]);
                //dictItemPurchaseInfo.Add(datas[i].shopItemId, dataProgress);
                dictItemPurchaseInfo[datas[i].shopItemId] = dataProgress;
            }
        }
        catch
        {
            Debug.LogError("Can't load shop item data id: " + datas[exceptionIndex].shopItemId);
        }

        //Debug.Log("Dictionary quests counter: " + dictQuestsStoryProgress.Count);
    }



    private void ResetDailyItems()
    {
        Dictionary<string, ShopItemPurchaseInfo> dictToUpdate = new Dictionary<string, ShopItemPurchaseInfo>();

        // create default for every item
        ShopItemSO[] allItems = GetAllItems().ToArray();
        for (int i = 0; i < allItems.Length; i++)
        {
            ShopItemSO so = allItems[i];
            if (so.IsDaily)
            {
                ShopItemPurchaseInfo purchaseInfo = new ShopItemPurchaseInfo();

                purchaseInfo.isPurchased = false;

                // copy informations
                purchaseInfo.purchaseCount = dictItemPurchaseInfo[so.UniqueId].purchaseCount;


                dictToUpdate.Add(so.UniqueId, purchaseInfo);
            }
        }

        // update dictionary
        foreach (var pair in dictToUpdate)
        {
            dictItemPurchaseInfo[pair.Key] = pair.Value;
        }
    }

    #endregion


    public void UpdateShopItemPurchase(ShopItemSO itemSO)
    {
        ShopItemPurchaseInfo itemInfo = dictItemPurchaseInfo[itemSO.UniqueId];

        if (itemSO.IsDaily || itemSO.IsUnique)
        {
            itemInfo.isPurchased = true;
        }

        itemInfo.purchaseCount++;

        dictItemPurchaseInfo[itemSO.UniqueId] = itemInfo;
    }


    public void SetRedeemCode(int id)
    {
        switch (id)
        {
            default: Debug.Log("Invalid redeem code id: " + id); break;

            case ID_REDEEM_ERIS_CODE: hasRedeemedErisCode = true; break;
        }
    }



    public void SaveShopData()
    {
        ShopSaveData data = new ShopSaveData(this);
        saveService.SaveData(UtilsSave.GetShopFile(), data, SettingsManager.Instance.FileEncryption);
    }
}
