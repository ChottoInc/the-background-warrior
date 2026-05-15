using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private IDataService saveService;


    // --- INVENTORY
    public Inventory Inventory { get; private set; }

    // Trigger used for quests
    public event Action<int> OnItemAdd;


    // ------- JOBS -------------


    public PlayerJobsData PlayerJobsData { get; private set; }

    public PlayerFightData PlayerFightData { get; private set; }

    public PlayerMinerData PlayerMinerData { get; private set; }

    public PlayerBlacksmithData PlayerBlacksmithData { get; private set; }

    public PlayerFisherData PlayerFisherData { get; private set; }

    public PlayerFarmerData PlayerFarmerData { get; private set; }


    // --- COMPANIONS

    // Trigger used for quests
    public event Action<int> OnCompanionBefriended;





    



    




    // ---- PLAYER GLOBAL VARIABLES ----

    // Miner
    public float WeaponMinerMultiplier => UtilsMiner.GetMinerWeaponMultiplier(PlayerMinerData.WeaponLevel);

    // Blacksmith
    public float HelmetMaxHpBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithHelmetMaxHpMultiplier(PlayerBlacksmithData.HelmetLevel);
    public float ArmorDefBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithArmorDefMultiplier(PlayerBlacksmithData.ArmorLevel);
    public float GlovesAtkSpdBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithGlovesAtkSpdMultiplier(PlayerBlacksmithData.GlovesLevel);
    public float GlovesCritDmgBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithGlovesCritDmgMultiplier(PlayerBlacksmithData.GlovesLevel);
    public float BootsDefBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithBootsDefMultiplier(PlayerBlacksmithData.BootsLevel);
    public float BootsCritRateBlacksmithMultiplier => UtilsBlacksmith.GetBlacksmithBootsCritRateMultiplier(PlayerBlacksmithData.BootsLevel);

    //Fisher
    public float FisherLifeSeriesMultiplier => PlayerFisherData.IsLifeSeriesCompleted ? UtilsGather.FISHER_LIFE_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherPredatorSeriesMultiplier => PlayerFisherData.IsPredatorSeriesCompleted ? UtilsGather.FISHER_PREDATOR_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherGuardianSeriesMultiplier => PlayerFisherData.IsGuardianSeriesCompleted ? UtilsGather.FISHER_GUARDIAN_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherDartSeriesMultiplier => PlayerFisherData.IsDartSeriesCompleted ? UtilsGather.FISHER_DART_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherSharpSeriesMultiplier => PlayerFisherData.IsSharpSeriesCompleted ? UtilsGather.FISHER_SHARP_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherPiercingSeriesMultiplier => PlayerFisherData.IsPiercingSeriesCompleted ? UtilsGather.FISHER_PIERCING_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherGoldenSeriesMultiplier => PlayerFisherData.IsGoldenSeriesCompleted ? UtilsGather.FISHER_GOLDEN_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherElderSeriesMultiplier => PlayerFisherData.IsElderSeriesCompleted ? UtilsGather.FISHER_ELDER_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherQuickSeriesMultiplier => PlayerFisherData.IsQuickSeriesCompleted ? UtilsGather.FISHER_QUICK_SERIES_COMPLETE_MULTIPLIER : 1f;




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

        if (Inventory != null)
        {
            Inventory.OnItemAdd -= ItemAdd;
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
            PlayerJobsData = new PlayerJobsData(jobsSaveData);
        }
        catch
        {
            PlayerJobsData = new PlayerJobsData();
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
        PlayerJobsSaveData data = new PlayerJobsSaveData(PlayerJobsData);
        saveService.SaveData(UtilsSave.GetPlayerJobsFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region INVENTORY DATA

    private void LoadInventoryData()
    {
        try
        {
            InventorySaveData inventorySaveData = saveService.LoadData<InventorySaveData>(UtilsSave.GetPlayerInventoryFile(), SettingsManager.Instance.FileEncryption);
            Inventory = new Inventory(inventorySaveData);
        }
        catch
        {
            Inventory = new Inventory();
            SaveInventoryData();
        }

        Inventory.OnItemAdd += ItemAdd;
    }

    /*
    public void UpdateInventoryData(Inventory data)
    {
        inventory = data;
    }*/

    public void SaveInventoryData()
    {
        InventorySaveData data = new InventorySaveData(Inventory);
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
            PlayerFightData = new PlayerFightData(fightSaveData);
        }
        catch
        {
            PlayerFightData = new PlayerFightData();
            SaveFightData();
        }

    }

    public void UpdateFightData(PlayerFightData data)
    {
        PlayerFightData = data;
        SaveFightData();
    }

    public void SaveFightData()
    {
        PlayerFightSaveData data = new PlayerFightSaveData(PlayerFightData);
        saveService.SaveData(UtilsSave.GetPlayerFightFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region MINER DATA

    private void LoadMinerData()
    {
        try
        {
            PlayerMinerSaveData minerSaveData = saveService.LoadData<PlayerMinerSaveData>(UtilsSave.GetPlayerMinerFile(), SettingsManager.Instance.FileEncryption);
            PlayerMinerData = new PlayerMinerData(minerSaveData);
        }
        catch
        {
            PlayerMinerData = new PlayerMinerData();
            SaveMinerData();
        }

    }

    public void UpdateMinerData(PlayerMinerData data)
    {
        PlayerMinerData = data;
        SaveMinerData();
    }

    public void SaveMinerData()
    {
        PlayerMinerSaveData data = new PlayerMinerSaveData(PlayerMinerData);
        saveService.SaveData(UtilsSave.GetPlayerMinerFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region BLACKSMITH

    private void LoadBlacksmithData()
    {
        try
        {
            PlayerBlacksmithSaveData blacksmithSaveData = saveService.LoadData<PlayerBlacksmithSaveData>(UtilsSave.GetPlayerBlacksmithFile(), SettingsManager.Instance.FileEncryption);
            PlayerBlacksmithData = new PlayerBlacksmithData(blacksmithSaveData);
        }
        catch
        {
            PlayerBlacksmithData = new PlayerBlacksmithData();
            SaveBlacksmithData();
        }

    }

    public void UpdateBlacksmithData(PlayerBlacksmithData data)
    {
        PlayerBlacksmithData = data;
        SaveBlacksmithData();
    }

    public void SaveBlacksmithData()
    {
        PlayerBlacksmithSaveData data = new PlayerBlacksmithSaveData(PlayerBlacksmithData);
        saveService.SaveData(UtilsSave.GetPlayerBlacksmithFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region FISHER DATA

    private void LoadFisherData()
    {
        try
        {
            PlayerFisherSaveData fisherSaveData = saveService.LoadData<PlayerFisherSaveData>(UtilsSave.GetPlayerFisherFile(), SettingsManager.Instance.FileEncryption);
            PlayerFisherData = new PlayerFisherData(fisherSaveData);
        }
        catch
        {
            PlayerFisherData = new PlayerFisherData();
            SaveFisherData();
        }

    }

    public void UpdateFisherData(PlayerFisherData data)
    {
        PlayerFisherData = data;
        SaveFisherData();
    }

    public void SaveFisherData()
    {
        PlayerFisherSaveData data = new PlayerFisherSaveData(PlayerFisherData);
        saveService.SaveData(UtilsSave.GetPlayerFisherFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region FARMER DATA

    private void LoadFarmerData()
    {
        try
        {
            PlayerFarmerSaveData farmerSaveData = saveService.LoadData<PlayerFarmerSaveData>(UtilsSave.GetPlayerFarmerFile(), SettingsManager.Instance.FileEncryption);
            PlayerFarmerData = new PlayerFarmerData(farmerSaveData);
        }
        catch
        {
            PlayerFarmerData = new PlayerFarmerData();
            SaveFarmerData();
        }

    }

    public void UpdateFarmerData(PlayerFarmerData data)
    {
        PlayerFarmerData = data;
        SaveFarmerData();
    }

    public void SaveFarmerData()
    {
        PlayerFarmerSaveData data = new PlayerFarmerSaveData(PlayerFarmerData);
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
