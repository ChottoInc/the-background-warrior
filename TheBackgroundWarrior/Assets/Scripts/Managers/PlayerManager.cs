using System;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private IDataService saveService;


    // --- INVENTORY
    public Inventory Inventory { get; private set; }


    // ------- JOBS -------------


    public PlayerJobsData PlayerJobsData { get; private set; }
    public PlayerBuffsData PlayerBuffsData { get; private set; }

    public PlayerFightData PlayerFightData { get; private set; }

    public PlayerMinerData PlayerMinerData { get; private set; }

    public PlayerBlacksmithData PlayerBlacksmithData { get; private set; }

    public PlayerFisherData PlayerFisherData { get; private set; }

    public PlayerFarmerData PlayerFarmerData { get; private set; }

    public PlayerMageData PlayerMageData { get; private set; }

    public PlayerAlchemistData PlayerAlchemistData { get; private set; }

    public PlayerNecromancerData PlayerNecromancerData { get; private set; }

    public PlayerBardData PlayerBardData { get; private set; }



    // TRIGGERS FOR QUESTS

    public event Action<int> OnItemAdd;
    public event Action<int> OnCompanionBefriended;
    public event Action<int> OnSpellRankUp;
    public event Action<int> OnSummon;







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
    public float FisherLifeSeriesMultiplier => PlayerFisherData.IsLifeSeriesCompleted ? UtilsFisher.FISHER_LIFE_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherPredatorSeriesMultiplier => PlayerFisherData.IsPredatorSeriesCompleted ? UtilsFisher.FISHER_PREDATOR_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherGuardianSeriesMultiplier => PlayerFisherData.IsGuardianSeriesCompleted ? UtilsFisher.FISHER_GUARDIAN_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherDartSeriesMultiplier => PlayerFisherData.IsDartSeriesCompleted ? UtilsFisher.FISHER_DART_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherSharpSeriesMultiplier => PlayerFisherData.IsSharpSeriesCompleted ? UtilsFisher.FISHER_SHARP_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherPiercingSeriesMultiplier => PlayerFisherData.IsPiercingSeriesCompleted ? UtilsFisher.FISHER_PIERCING_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherGoldenSeriesMultiplier => PlayerFisherData.IsGoldenSeriesCompleted ? UtilsFisher.FISHER_GOLDEN_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherElderSeriesMultiplier => PlayerFisherData.IsElderSeriesCompleted ? UtilsFisher.FISHER_ELDER_SERIES_COMPLETE_MULTIPLIER : 1f;
    public float FisherQuickSeriesMultiplier => PlayerFisherData.IsQuickSeriesCompleted ? UtilsFisher.FISHER_QUICK_SERIES_COMPLETE_MULTIPLIER : 1f;




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

        // ensure at close the inspiration buff is removed, it's added again on startup
        if(PlayerBuffsData != null)
        {
            Buff bardBuff = new Buff(UtilsBuffs.BuffType.Inspiration, 0);
            PlayerBuffsData.RemoveBuff(bardBuff);
        }
    }

    // Called after Settings Manager setup
    public void Setup(IDataService service)
    {
        saveService = service;

        try
        {

            LoadJobsData();
            LoadBuffsData();
            LoadInventoryData();

            LoadMinerData();
            LoadBlacksmithData();
            LoadFisherData();
            LoadFarmerData();
            LoadMageData();
            LoadAlchemistData();
            LoadNecromancerData();
            LoadBardData();

            LoadFightData();
        }
        catch(FatalLoadException e)
        {
            throw e;
        }
        catch(Exception e)
        {
            Debug.LogError("Different error from loading exception");
            throw e;
        }
    }

    #region JOBS DATA

    private void LoadJobsData()
    {
        try
        {
            PlayerJobsSaveData jobsSaveData = saveService.LoadData<PlayerJobsSaveData>(UtilsSave.GetPlayerJobsFile(), SettingsManager.Instance.FileEncryption);
            PlayerJobsData = new PlayerJobsData(jobsSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load jobs data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            PlayerJobsData = new PlayerJobsData();
            SaveJobsData();
        }
    }


    public void SaveJobsData()
    {
        PlayerJobsSaveData data = new PlayerJobsSaveData(PlayerJobsData);
        saveService.SaveData(UtilsSave.GetPlayerJobsFile(), data, SettingsManager.Instance.FileEncryption);
    }

    /// <summary>
    /// Check for jobs that player should have, and if has not add them to availables
    /// </summary>
    public void InitialJobChecks()
    {
        // Check blacksmith
        if (Inventory.HasItem(UtilsItem.ID_GOLD_ORE))
        {
            if (!PlayerJobsData.IsBlacksmithUnlocked)
            {
                PlayerJobsData.AddAvailableJob(UtilsPlayer.PlayerJob.Blacksmith);
            }
        }

        // Check mage
        if (Inventory.HasItem(UtilsItem.ID_CARD_ANCIENTTOME))
        {
            if (!PlayerJobsData.IsMageUnlocked)
            {
                PlayerJobsData.AddAvailableJob(UtilsPlayer.PlayerJob.Mage);
            }
        }
    }

    #endregion

    #region BUFFS DATA

    private void LoadBuffsData()
    {
        try
        {
            PlayerBuffsSaveData buffsSaveData = saveService.LoadData<PlayerBuffsSaveData>(UtilsSave.GetPlayerBuffsFile(), SettingsManager.Instance.FileEncryption);
            PlayerBuffsData = new PlayerBuffsData(buffsSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load buffs data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            PlayerBuffsData = new PlayerBuffsData();
            SaveBuffsData();
        }

    }

    
    public void UpdateBuffsData(PlayerBuffsData data)
    {
        PlayerBuffsData = data;
    }

    public void SaveBuffsData()
    {
        PlayerBuffsSaveData data = new PlayerBuffsSaveData(PlayerBuffsData);
        saveService.SaveData(UtilsSave.GetPlayerBuffsFile(), data, SettingsManager.Instance.FileEncryption);
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
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load inventory data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            Inventory = new Inventory();
            SaveInventoryData();
        }

        Inventory.OnItemAdd += ItemAdd;
    }

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

    #region WARRIOR DATA

    private void LoadFightData()
    {
        try
        {
            PlayerFightSaveData fightSaveData = saveService.LoadData<PlayerFightSaveData>(UtilsSave.GetPlayerFightFile(), SettingsManager.Instance.FileEncryption);
            PlayerFightData = new PlayerFightData(fightSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load warrior data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

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
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load miner data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

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
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load blacksmith data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

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
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load fisher data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

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
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load farmer data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

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

    public void OnBefriendedCompanionEvent(int companion)
    {
        OnCompanionBefriended?.Invoke(companion);
    }

    #endregion

    #region MAGE DATA

    private void LoadMageData()
    {
        try
        {
            PlayerMageSaveData mageSaveData = saveService.LoadData<PlayerMageSaveData>(UtilsSave.GetPlayerMageFile(), SettingsManager.Instance.FileEncryption);
            PlayerMageData = new PlayerMageData(mageSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load mage data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            PlayerMageData = new PlayerMageData();
            SaveMageData();
        }
    }

    public void UpdateMageData(PlayerMageData data)
    {
        PlayerMageData = data;
        SaveMageData();
    }

    public void SaveMageData()
    {
        PlayerMageSaveData data = new PlayerMageSaveData(PlayerMageData);
        saveService.SaveData(UtilsSave.GetPlayerMageFile(), data, SettingsManager.Instance.FileEncryption);
    }

    public void OnSpellRankUpEvent(int spell)
    {
        OnSpellRankUp?.Invoke(spell);
    }

    #endregion

    #region ALCHEMIST DATA

    private void LoadAlchemistData()
    {
        try
        {
            PlayerAlchemistSaveData alchemistSaveData = saveService.LoadData<PlayerAlchemistSaveData>(UtilsSave.GetPlayerAlchemistFile(), SettingsManager.Instance.FileEncryption);
            PlayerAlchemistData = new PlayerAlchemistData(alchemistSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load alchemist data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            PlayerAlchemistData = new PlayerAlchemistData();
            SaveAlchemistData();
        }
    }

    public void UpdateAlchemistData(PlayerAlchemistData data)
    {
        PlayerAlchemistData = data;
        SaveAlchemistData();
    }

    public void SaveAlchemistData()
    {
        PlayerAlchemistSaveData data = new PlayerAlchemistSaveData(PlayerAlchemistData);
        saveService.SaveData(UtilsSave.GetPlayerAlchemistFile(), data, SettingsManager.Instance.FileEncryption);
    }

    #endregion

    #region NECROMANCER DATA

    private void LoadNecromancerData()
    {
        try
        {
            PlayerNecromancerSaveData necromancerSaveData = saveService.LoadData<PlayerNecromancerSaveData>(UtilsSave.GetPlayerNecromancerFile(), SettingsManager.Instance.FileEncryption);
            PlayerNecromancerData = new PlayerNecromancerData(necromancerSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load necromancer data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            PlayerNecromancerData = new PlayerNecromancerData();
            SaveNecromancerData();
        }
    }

    public void UpdateNecromancerData(PlayerNecromancerData data)
    {
        PlayerNecromancerData = data;
        SaveNecromancerData();
    }

    public void SaveNecromancerData()
    {
        PlayerNecromancerSaveData data = new PlayerNecromancerSaveData(PlayerNecromancerData);
        saveService.SaveData(UtilsSave.GetPlayerNecromancerFile(), data, SettingsManager.Instance.FileEncryption);
    }

    public void OnSummonEvent(int amount)
    {
        OnSummon?.Invoke(amount);
    }

    #endregion

    #region BARD DATA

    private void LoadBardData()
    {
        try
        {
            PlayerBardSaveData bardSaveData = saveService.LoadData<PlayerBardSaveData>(UtilsSave.GetPlayerBardFile(), SettingsManager.Instance.FileEncryption);
            PlayerBardData = new PlayerBardData(bardSaveData);
        }
        catch (ConversionException e)
        {
            Debug.LogError(e.Message);
            throw new FatalLoadException("Cannot load bard data");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e.Message);

            PlayerBardData = new PlayerBardData();
            SaveBardData();
        }
    }

    public void UpdateBardData(PlayerBardData data)
    {
        PlayerBardData = data;
        SaveBardData();
    }

    public void SaveBardData()
    {
        PlayerBardSaveData data = new PlayerBardSaveData(PlayerBardData);
        saveService.SaveData(UtilsSave.GetPlayerBardFile(), data, SettingsManager.Instance.FileEncryption);
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
        SaveMageData();
        SaveAlchemistData();
        SaveNecromancerData();
        SaveBardData();

        SaveJobsData();
    }
}
