using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("Cheats")]
    [SerializeField] bool enableCheats = true;
    [SerializeField] bool fileEncryption;


    public bool AreCheatsEnabled => enableCheats;
    public bool FileEncryption => fileEncryption;




    private IDataService saveService;

    // ---- TUTORIAL ----

    public bool HasSeenIntroTutorial { get; private set; }





    // --- LAST OPEN VARS ---

    private LastSceneSettings lastSceneSettings;

    
    public LastSceneSettings LastSceneSettings => lastSceneSettings;





    // --- SETTINGS ---

    public long LastLoginDate { get; private set; }
    public bool FirstOpen { get; private set; }


    // --------- GAMEPLAY

    // -- Battle
    public bool IsAutoBattleOn { get; private set; }

    // -- HUD
    public bool IsInvertedHudOn { get; private set; }

    // -- Floating HUD
    public bool IsDamageOn { get; private set; }
    public bool IsItemCollectionOn { get; private set; }
    public bool AreTooltipsOn { get; private set; }

    // -- Animations
    public bool AreLevelUpEquipmentOn { get; private set; }

    // -- Fisher
    public bool IsInvertedFishingSpot { get; private set; }
    public bool IsHiddenFishingBar { get; private set; }


    public event Action<bool> OnInvertedHUDChange;
    public event Action<bool> OnInvertedFishingSpotChange;
    public event Action<bool> OnIsHiddenFishingBarChange;




    // --------- VIDEO

    public bool IsAlwaysOnTop { get; private set; }
    public bool IsClickThrough { get; private set; }
    public bool Is60FPS { get; private set; }
    public int CurrentMonitorIndex { get; private set; }



    public event Action<bool> OnAlwaysOnTopChange;
    public event Action<bool> OnClickThroughChange;



    // --------- AUDIO

    public float MasterVolume { get; private set; }






    public static SettingsManager Instance { get; private set; }


    private void OnDestroy()
    {
        if(Instance == this)
        {
            Save();
        }
    }



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

    public void Setup(IDataService service)
    {
        saveService = service;

        try
        {
            SettingsSaveData saveData = saveService.LoadData<SettingsSaveData>(UtilsSave.GetSettingsFile(), FileEncryption);

            // call first default to ensure any new variables is updated by default
            SetupFromDefault();

            // then call from file so any saved variables is overwritten
            SetupFromFile(saveData);
        }
        catch
        {
            SetupFromDefault();
            FirstOpen = true;

            Save();
        }
    }

    private void SetupFromFile(SettingsSaveData saveData)
    {
        // tutorial

        HasSeenIntroTutorial = saveData.hasSeenIntroTutorial;


        // last scene

        lastSceneSettings = new LastSceneSettings();
        lastSceneSettings.lastSceneName = saveData.lastSceneName;
        lastSceneSettings.lastSceneType = (SceneLoaderManager.SceneType)saveData.lastSceneType;
        lastSceneSettings.lastCombatMapId = saveData.lastCombatMapId;

        // settings

        LastLoginDate = saveData.lastLoginDate;

        // --- gameplay
        SetIsAutoBattle(saveData.isAutoBattleOn, false);

        SetIsInvertedHUDOn(saveData.isInvertedHUDOn, false);

        SetIsDamageOn(saveData.isDamageOn, false);
        SetIsItemCollectionOn(saveData.isItemCollectionOn, false);
        SetAreTooltipsOn(saveData.areTooltipsOn, false);

        SetAreLevelUpEquipmentAnimationOn(saveData.areLevelUpEquipmentOn, false);

        SetIsInvertedFishingSpotOn(saveData.isInvertedFishingSpot, false);
        SetIsHiddenFishingSpot(saveData.isHiddenFishingBar, false);

        // --- video
        SetIsAlwaysOnTop(saveData.isAlwaysOnTop, false);
        SetIsClickThrough(saveData.isClickThrough, false);
        SetIs60FPS(saveData.is60FPS, false);
        SetCurrentMonitorIndex(saveData.currentMonitorIndex, false, false);

        // --- audio
        SetMasterVolume(saveData.masterVolume, false);
    }

    private void SetupFromDefault()
    {
        // tutorial
        HasSeenIntroTutorial = false;


        // last scene
        lastSceneSettings = new LastSceneSettings();
        lastSceneSettings.lastSceneName = "WoodsScene";
        lastSceneSettings.lastSceneType = SceneLoaderManager.SceneType.CombatMap;
        lastSceneSettings.lastCombatMapId = 0;

        RefreshMapSaveDatas();

        // settings

        LastLoginDate = DateTime.UtcNow.Ticks;

        // --- gameplay
        SetIsAutoBattle(true, false);

        SetIsInvertedHUDOn(false, false);

        SetIsDamageOn(true, false);
        SetIsItemCollectionOn(true, false);
        SetAreTooltipsOn(true, false);

        SetAreLevelUpEquipmentAnimationOn(true, false);

        SetIsInvertedFishingSpotOn(false, false);
        SetIsHiddenFishingSpot(false, false);

        // --- video
        SetIsAlwaysOnTop(false, false);
        SetIsClickThrough(true, false);
        SetIs60FPS(true, false);
        SetCurrentMonitorIndex(0, true, false);

        // --- audio
        SetMasterVolume(1f, false);
    }

    /// <summary>
    /// Refresh and check if map saves are updated
    /// </summary>
    private void RefreshMapSaveDatas()
    {
        CombatMapSO[] maps = UtilsCombatMap.GetAllMaps();
        for (int i = 0; i < maps.Length; i++)
        {
            try
            {
                // if save exists do nothing
                CombatMapSaveData mapData = saveService.LoadData<CombatMapSaveData>(UtilsSave.GetCombatMapFile(maps[i].MapName + i.ToString()), FileEncryption);
            }
            catch
            {
                // if it doesn't exists, create new save file for the map
                CombatMapSaveData mapData = new CombatMapSaveData(maps[i].IdMap, 1, 1, 0);
                saveService.SaveData(UtilsSave.GetCombatMapFile(maps[i].MapName + i.ToString()), mapData, FileEncryption);
            }
        }
    }

    #region TUTORIAL

    public void SetSeenTutorial(int idTutorial, bool save = true)
    {
        switch (idTutorial)
        {
            default:
            case UtilsGeneral.ID_INTRO_TUTORIAL: HasSeenIntroTutorial = true; break;
        }

        if (save)
            Save();
    }

    #endregion


    #region SCENE

    public void SetSceneSettings(LastSceneSettings settings, bool save = true)
    {
        lastSceneSettings = settings;

        if (save)
            Save();
    }

    public CombatMapSaveData GetCombatMapSaveData(CombatMapSO mapSO)
    {
        try
        {
            CombatMapSaveData saveData = saveService.LoadData<CombatMapSaveData>(UtilsSave.GetCombatMapFile(mapSO.MapName + mapSO.IdMap.ToString()), FileEncryption);
            return saveData;
        }
        catch
        {
            Debug.LogError("Can't load combat map data id: " + mapSO.IdMap);
            return null;
        }
    }

    public void SaveCombatMapData(CombatMapSO mapSO, int currentStage, int reachedStage, int reachedPrestige)
    {
        CombatMapSaveData mapData = new CombatMapSaveData(mapSO.IdMap, currentStage, reachedStage, reachedPrestige);
        saveService.SaveData(UtilsSave.GetCombatMapFile(mapSO.MapName + mapSO.IdMap.ToString()), mapData, FileEncryption);
    }

    public void SaveCombatMapData(CombatMapSO mapSO, CombatMapSaveData combatMapSaveData)
    {
        saveService.SaveData(UtilsSave.GetCombatMapFile(mapSO.MapName + mapSO.IdMap.ToString()), combatMapSaveData, FileEncryption);
    }

    #endregion


    #region GAMEPLAY

    public void SetIsAutoBattle(bool isOn, bool save = true)
    {
        IsAutoBattleOn = isOn;

        if(save)
            Save();
    }


    public void SetIsInvertedHUDOn(bool isOn, bool save = true)
    {
        IsInvertedHudOn = isOn;
        OnInvertedHUDChange?.Invoke(isOn);

        if (save)
            Save();
    }


    public void SetIsDamageOn(bool isOn, bool save = true)
    {
        IsDamageOn = isOn;

        if (save)
            Save();
    }

    public void SetIsItemCollectionOn(bool isOn, bool save = true)
    {
        IsItemCollectionOn = isOn;

        if (save)
            Save();
    }

    public void SetAreTooltipsOn(bool isOn, bool save = true)
    {
        AreTooltipsOn = isOn;

        if (save)
            Save();
    }


    public void SetAreLevelUpEquipmentAnimationOn(bool isOn, bool save = true)
    {
        AreLevelUpEquipmentOn = isOn;

        if (save)
            Save();
    }


    public void SetIsInvertedFishingSpotOn(bool isOn, bool save = true)
    {
        IsInvertedFishingSpot = isOn;
        OnInvertedFishingSpotChange?.Invoke(isOn);

        if (save)
            Save();
    }

    public void SetIsHiddenFishingSpot(bool isOn, bool save = true)
    {
        IsHiddenFishingBar = isOn;
        OnIsHiddenFishingBarChange?.Invoke(isOn);

        if (save)
            Save();
    }


    #endregion

    #region VIDEO

    public void SetIsAlwaysOnTop(bool isOn, bool save = true)
    {
        IsAlwaysOnTop = isOn;
        OnAlwaysOnTopChange?.Invoke(IsAlwaysOnTop);

        if (save)
            Save();
    }

    public void SetIsClickThrough(bool isOn, bool save = true)
    {
        IsClickThrough = isOn;
        OnClickThroughChange?.Invoke(IsClickThrough);

        if (save)
            Save();
    }

    public void SetIs60FPS(bool isOn, bool save = true)
    {
        Is60FPS = isOn;

        if (Is60FPS)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }

        if (save)
            Save();
    }

    public void SetCurrentMonitorIndex(int index, bool fromDefault, bool save = true)
    {
        CurrentMonitorIndex = index;

        int possibleIndexes = Display.displays.Length;

        // if the saved index is greater than the displays reset to 0
        if (CurrentMonitorIndex >= possibleIndexes)
        {
            CurrentMonitorIndex = 0;
        }

        // check if a setting has been changed from menu or is already saved
        if (!fromDefault)
        {
            /*
            // ensure the display is activated
            if (currentMonitorIndex != 0)
            {
                // check if the display is not active, activate it in case
                if (!Display.displays[currentMonitorIndex].active)
                {
                    Display.displays[currentMonitorIndex].Activate();
                }
            }
            */
            StartCoroutine(InitializerManager.Instance.CoChangeMonitor(CurrentMonitorIndex));
        }

        if (save)
            Save();
    }

    #endregion

    #region AUDIO

    public void SetMasterVolume(float value, bool save = true)
    {
        MasterVolume = value;

        AudioManager.Instance.SetMasterVolume(MasterVolume);

        if (save)
            Save();
    }

    #endregion


    public void Save()
    {
        LastLoginDate = DateTime.UtcNow.Ticks;

        SettingsSaveData data = new SettingsSaveData(this);
        saveService.SaveData(UtilsSave.GetSettingsFile(), data, FileEncryption);
    }
}
