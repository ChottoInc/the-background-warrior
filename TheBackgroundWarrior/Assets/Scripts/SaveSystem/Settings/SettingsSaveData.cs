

public class SettingsSaveData
{
    // ---- TUTORIAL ----

    public bool hasSeenIntroTutorial;


    // ---- LAST SCENE ----

    public string lastSceneName;

    public int lastSceneType;

    // combat map
    public int lastCombatMapId;


    // ---- SETTINGS ----

    public long lastLoginDate;

    // ------------ GAMEPLAY

    // -- Battle
    public bool isAutoBattleOn;

    // -- HUD
    public bool isInvertedHUDOn;
    public bool isAutocloseHUDOn;
    public int autocloseTimer;
    public bool isShowAdvancedStatsOn;

    // -- Floating HUD
    public bool isDamageOn;
    public bool isItemCollectionOn;
    public bool areTooltipsOn;

    // -- Animations
    public bool areLevelUpEquipmentOn;

    // -- Fisher
    public bool isInvertedFishingSpot;
    public bool isHiddenFishingBar;

    // -- Mage

    public bool isCurrentSpellShowing;

    // ------------ VIDEO

    public bool isAlwaysOnTop;
    public bool isClickThrough;
    public bool is60FPS;
    public int currentMonitorIndex;


    // ------------ GENERAL

    public float masterVolume;
    public int currentLanguage;






    public SettingsSaveData() { }

    public SettingsSaveData(SettingsManager manager)
    {
        hasSeenIntroTutorial = manager.HasSeenIntroTutorial;



        lastSceneName = manager.LastSceneSettings.lastSceneName;
        lastSceneType = (int)manager.LastSceneSettings.lastSceneType;
        lastCombatMapId = manager.LastSceneSettings.lastCombatMapId;



        lastLoginDate = manager.LastLoginDate;



        isAutoBattleOn = manager.IsAutoBattleOn;

        isInvertedHUDOn = manager.IsInvertedHudOn;
        isAutocloseHUDOn = manager.IsAutocloseHudOn;
        autocloseTimer = (int)manager.CurrentAutocloseTimerType;
        isShowAdvancedStatsOn = manager.IsShowAdvancedStatOn;

        isDamageOn = manager.IsDamageOn;
        isItemCollectionOn = manager.IsItemCollectionOn;
        areTooltipsOn = manager.AreTooltipsOn;

        areLevelUpEquipmentOn = manager.AreLevelUpEquipmentOn;

        isInvertedFishingSpot = manager.IsInvertedFishingSpot;
        isHiddenFishingBar = manager.IsHiddenFishingBar;

        isCurrentSpellShowing = manager.IsCurrentSpellShowing;



        isAlwaysOnTop = manager.IsAlwaysOnTop;
        isClickThrough = manager.IsClickThrough;
        is60FPS = manager.Is60FPS;
        currentMonitorIndex = manager.CurrentMonitorIndex;



        masterVolume = manager.MasterVolume;
        currentLanguage = (int)manager.CurrentLanguage;
    }
}
