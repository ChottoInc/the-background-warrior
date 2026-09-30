using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using static UtilsShop;

public class ShopManager : MonoBehaviour
{
    private IDataService saveService;



    // Store every shop item purchase info


    public Dictionary<string, ShopItemPurchaseInfo> DictItemPurchaseInfo { get; private set; }


    // List of all shop items
    public List<string> ShopItemsList { get; private set; }

    public long LastDailyCreationDate { get; private set; }


    // Redeem codes

    public bool HasRedeemedErisCode { get; private set; }



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
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load shop");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            SetupFromDefault();
            SaveShopData();
        }
    }

    #region DEFAULT

    private void SetupFromDefault()
    {
        /*
         * check on daily, if day changed reset purchase
         * */

        LastDailyCreationDate = DateTime.UtcNow.Ticks;

        InitializeAllItems();

        InitializeRedeemCodes();
    }

    private void InitializeAllItems()
    {
        // initialize dict and all items

        if(ShopItemsList == null)
            ShopItemsList = new List<string>();

        if(DictItemPurchaseInfo == null)
            DictItemPurchaseInfo = new Dictionary<string, ShopItemPurchaseInfo>();

        // create default for every item
        ShopItemSO[] allItems = GetAllItems().ToArray();
        for (int i = 0; i < allItems.Length; i++)
        {
            // get so
            ShopItemSO so = allItems[i];

            // init if item isn't in dict
            if (!DictItemPurchaseInfo.ContainsKey(so.UniqueId))
            {
                ShopItemPurchaseInfo purchaseInfo = new ShopItemPurchaseInfo();

                purchaseInfo.isPurchased = false;
                purchaseInfo.purchaseCount = 0;

                // save in dictionary
                DictItemPurchaseInfo.Add(so.UniqueId, purchaseInfo);
            }
        }
    }

    private void InitializeRedeemCodes()
    {
        HasRedeemedErisCode = false;
    }

    #endregion
    
    #region FROM FILE

    private void SetupFromFile(ShopSaveData saveData)
    {
        LastDailyCreationDate = saveData.lastDailyCreationDate;

        LoadShopItems(saveData.shopItemSaveDatas);

        // check for daily using date
        DateTime lastDailyDate = new DateTime(LastDailyCreationDate, DateTimeKind.Utc);
        if (DateTime.UtcNow.Date != lastDailyDate.Date)
        {
            //Debug.Log("Diffrent date shop");
            LastDailyCreationDate = DateTime.UtcNow.Ticks;
            ResetDailyItems();
        }

        HasRedeemedErisCode = saveData.hasRedeemedErisCode;

        SaveShopData();
    }

    private void LoadShopItems(List<ShopItemSaveData> datas)
    {
        if (ShopItemsList == null)
            ShopItemsList = new List<string>();

        if (DictItemPurchaseInfo == null)
            DictItemPurchaseInfo = new Dictionary<string, ShopItemPurchaseInfo>();

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
                DictItemPurchaseInfo[datas[i].shopItemId] = dataProgress;

                //Debug.Log("unique id: " + datas[i].shopItemId + ", ispurch: " + dataProgress.isPurchased);
                //Debug.Log("unique id: " + datas[i].shopItemId + ", ispurch: " + dictItemPurchaseInfo[datas[i].shopItemId].isPurchased);
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
                purchaseInfo.purchaseCount = DictItemPurchaseInfo[so.UniqueId].purchaseCount;


                dictToUpdate.Add(so.UniqueId, purchaseInfo);
            }
        }

        // update dictionary
        foreach (var pair in dictToUpdate)
        {
            DictItemPurchaseInfo[pair.Key] = pair.Value;
        }
    }

    #endregion


    public void UpdateShopItemPurchase(ShopItemSO itemSO)
    {
        ShopItemPurchaseInfo itemInfo = DictItemPurchaseInfo[itemSO.UniqueId];

        if (itemSO.IsDaily || itemSO.IsUnique)
        {
            itemInfo.isPurchased = true;
        }

        itemInfo.purchaseCount++;

        DictItemPurchaseInfo[itemSO.UniqueId] = itemInfo;
    }


    public void SetRedeemCode(int id)
    {
        switch (id)
        {
            default: Debug.Log("Invalid redeem code id: " + id); break;

            case ID_REDEEM_ERIS_CODE: HasRedeemedErisCode = true; break;
        }
    }



    public void SaveShopData()
    {
        ShopSaveData data = new ShopSaveData(this);
        saveService.SaveData(UtilsSave.GetShopFile(), data, SettingsManager.Instance.FileEncryption);
    }
}
