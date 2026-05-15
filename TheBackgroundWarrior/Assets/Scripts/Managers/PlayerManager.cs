using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private IDataService saveService;


    // --- INVENTORY
    private Inventory inventory;

    // Trigger used for quests
    public event Action<int> OnItemAdd;


    // ------- JOBS -------------

    private PlayerJobsData playerJobsData;


    private PlayerFightData playerFightData;

    private PlayerMinerData playerMinerData;

    private PlayerBlacksmithData playerBlacksmithData;

    private PlayerFisherData playerFisherData;

    private PlayerFarmerData playerFarmerData;


    // --- COMPANIONS

    // Trigger used for quests
    public event Action<int> OnCompanionBefriended;





    public Inventory Inventory => inventory;



    public PlayerJobsData PlayerJobsData => playerJobsData;

    public PlayerFightData PlayerFightData => playerFightData;

    public PlayerMinerData PlayerMinerData => playerMinerData;

    public PlayerBlacksmithData PlayerBlacksmithData => playerBlacksmithData;

    public PlayerFisherData PlayerFisherData => playerFisherData;

    public PlayerFarmerData PlayerFarmerData => playerFarmerData;




    // ---- PLAYER GLOBAL VARIABLES ----

    // Miner
    public float WeaponMinerMultiplier => UtilsMiner.GetMinerWeaponMultiplier(playerMinerData.WeaponLevel);

    // Blacksmith
    public float HelmetMaxHpBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithHelmetMaxHpMultiplier(playerBlacksmithData.HelmetLevel);
    public float ArmorDefBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithArmorDefMultiplier(playerBlacksmithData.ArmorLevel);
    public float GlovesAtkSpdBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithGlovesAtkSpdMultiplier(playerBlacksmithData.GlovesLevel);
    public float GlovesCritDmgBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithGlovesCritDmgMultiplier(playerBlacksmithData.GlovesLevel);
    public float BootsDefBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithBootsDefMultiplier(playerBlacksmithData.BootsLevel);
    public float BootsCritRateBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithBootsCritRateMultiplier(playerBlacksmithData.BootsLevel);

    //Fisher
    public float FisherLifeSeriesMultiplier => playerFisherData.IsLifeSeriesCompleted ? UtilsGather.FISHER_LIFE_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherPredatorSeriesMultiplier => playerFisherData.IsPredatorSeriesCompleted ? UtilsGather.FISHER_PREDATOR_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherGuardianSeriesMultiplier => playerFisherData.IsGuardianSeriesCompleted ? UtilsGather.FISHER_GUARDIAN_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherDartSeriesMultiplier => playerFisherData.IsDartSeriesCompleted ? UtilsGather.FISHER_DART_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherSharpSeriesMultiplier => playerFisherData.IsSharpSeriesCompleted ? UtilsGather.FISHER_SHARP_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherPiercingSeriesMultiplier => playerFisherData.IsPiercingSeriesCompleted ? UtilsGather.FISHER_PIERCING_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherGoldenSeriesMultiplier => playerFisherData.IsGoldenSeriesCompleted ? UtilsGather.FISHER_GOLDEN_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherElderSeriesMultiplier => playerFisherData.IsElderSeriesCompleted ? UtilsGather.FISHER_ELDER_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherQuickSeriesMultiplier => playerFisherData.IsQuickSeriesCompleted ? UtilsGather.FISHER_QUICK_SERIES_COMPLETE_MULTIPLIER : 1f;




    public static PlayerManager Instance { get; private set; }

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

    private void OnDestroy()
    {
        if (Instance != this) return;

        if (inventory != null)
        {
            inventory.OnItemAdd -= ItemAdd;
        }
    }

    // Called after Settings Manager setup
    public void Setup(IDataService service)
    {
        saveService = service;

        LoadJobsData();
        LoadInventoryData();

        LoadMinerData();
        LoadBlacksmithData();
        LoadFisherData();
        LoadFarmerData();

        LoadFightData();
    }

    #region JOBS DATA

    private void LoadJobsData()
    {
        try
        {
            PlayerJobsSaveData jobsSaveData = saveService.LoadData<PlayerJobsSaveData>(UtilsSave.GetPlayerJobsFile(), SettingsManager.Instance.FileEncryption);
            playerJobsData = new PlayerJobsData(jobsSaveData);
        }
        catch
        {
            playerJobsData = new PlayerJobsData();
            SaveJobsData();
        }
    }

    /*
    public void UpdateInventoryData(Inventory data)
    {
        inventory = data;
    }*/

    public void SaveJobsData()
    {
        PlayerJobsSaveData data = new PlayerJobsSaveData(playerJobsData);
        saveService.SaveData(UtilsSave.GetPlayerJobsFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region INVENTORY DATA

    private void LoadInventoryData()
    {
        try
        {
            InventorySaveData inventorySaveData = saveService.LoadData<InventorySaveData>(UtilsSave.GetPlayerInventoryFile(), SettingsManager.Instance.FileEncryption);
            inventory = new Inventory(inventorySaveData);
        }
        catch
        {
            inventory = new Inventory();
            SaveInventoryData();
        }

        inventory.OnItemAdd += ItemAdd;
    }

    /*
    public void UpdateInventoryData(Inventory data)
    {
        inventory = data;
    }*/

    public void SaveInventoryData()
    {
        InventorySaveData data = new InventorySaveData(inventory);
        saveService.SaveData(UtilsSave.GetPlayerInventoryFile(), data, SettingsManager.Instance.FileEncryption);
    }

    private void ItemAdd(int id)
    {
        OnItemAdd?.Invoke(id);
    }

    #endregion

    #region FIGHT DATA

    private void LoadFightData()
    {
        try
        {
            PlayerFightSaveData fightSaveData = saveService.LoadData<PlayerFightSaveData>(UtilsSave.GetPlayerFightFile(), SettingsManager.Instance.FileEncryption);
            playerFightData = new PlayerFightData(fightSaveData);
        }
        catch
        {
            playerFightData = new PlayerFightData();
            SaveFightData();
        }

    }

    public void UpdateFightData(PlayerFightData data)
    {
        playerFightData = data;
        SaveFightData();
    }

    public void SaveFightData()
    {
        PlayerFightSaveData data = new PlayerFightSaveData(playerFightData);
        saveService.SaveData(UtilsSave.GetPlayerFightFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region MINER DATA

    private void LoadMinerData()
    {
        try
        {
            PlayerMinerSaveData minerSaveData = saveService.LoadData<PlayerMinerSaveData>(UtilsSave.GetPlayerMinerFile(), SettingsManager.Instance.FileEncryption);
            playerMinerData = new PlayerMinerData(minerSaveData);
        }
        catch
        {
            playerMinerData = new PlayerMinerData();
            SaveMinerData();
        }

    }

    public void UpdateMinerData(PlayerMinerData data)
    {
        playerMinerData = data;
        SaveMinerData();
    }

    public void SaveMinerData()
    {
        PlayerMinerSaveData data = new PlayerMinerSaveData(playerMinerData);
        saveService.SaveData(UtilsSave.GetPlayerMinerFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region BLACKSMITH

    private void LoadBlacksmithData()
    {
        try
        {
            PlayerBlacksmithSaveData blacksmithSaveData = saveService.LoadData<PlayerBlacksmithSaveData>(UtilsSave.GetPlayerBlacksmithFile(), SettingsManager.Instance.FileEncryption);
            playerBlacksmithData = new PlayerBlacksmithData(blacksmithSaveData);
        }
        catch
        {
            playerBlacksmithData = new PlayerBlacksmithData();
            SaveBlacksmithData();
        }

    }

    public void UpdateBlacksmithData(PlayerBlacksmithData data)
    {
        playerBlacksmithData = data;
        SaveBlacksmithData();
    }

    public void SaveBlacksmithData()
    {
        PlayerBlacksmithSaveData data = new PlayerBlacksmithSaveData(playerBlacksmithData);
        saveService.SaveData(UtilsSave.GetPlayerBlacksmithFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region FISHER DATA

    private void LoadFisherData()
    {
        try
        {
            PlayerFisherSaveData fisherSaveData = saveService.LoadData<PlayerFisherSaveData>(UtilsSave.GetPlayerFisherFile(), SettingsManager.Instance.FileEncryption);
            playerFisherData = new PlayerFisherData(fisherSaveData);
        }
        catch
        {
            playerFisherData = new PlayerFisherData();
            SaveFisherData();
        }

    }

    public void UpdateFisherData(PlayerFisherData data)
    {
        playerFisherData = data;
        SaveFisherData();
    }

    public void SaveFisherData()
    {
        PlayerFisherSaveData data = new PlayerFisherSaveData(playerFisherData);
        saveService.SaveData(UtilsSave.GetPlayerFisherFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region FARMER DATA

    private void LoadFarmerData()
    {
        try
        {
            PlayerFarmerSaveData farmerSaveData = saveService.LoadData<PlayerFarmerSaveData>(UtilsSave.GetPlayerFarmerFile(), SettingsManager.Instance.FileEncryption);
            playerFarmerData = new PlayerFarmerData(farmerSaveData);
        }
        catch
        {
            playerFarmerData = new PlayerFarmerData();
            SaveFarmerData();
        }

    }

    public void UpdateFarmerData(PlayerFarmerData data)
    {
        playerFarmerData = data;
        SaveFarmerData();
    }

    public void SaveFarmerData()
    {
        PlayerFarmerSaveData data = new PlayerFarmerSaveData(playerFarmerData);
        saveService.SaveData(UtilsSave.GetPlayerFarmerFile(), data, SettingsManager.Instance.FileEncryption);
    }

    public void OnBefriendedCompanion(int companion)
    {
        OnCompanionBefriended?.Invoke(companion);
    }

    #endregion

    public void SaveAll()
    {
        SaveInventoryData();

        SaveFightData();
        SaveMinerData();
        SaveBlacksmithData();
        SaveFisherData();
        SaveFarmerData();

        SaveJobsData();
    }
}
