

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

    // -- Floating HUD
    public bool isDamageOn;
    public bool isItemCollectionOn;
    public bool areTooltipsOn;

    // -- Animations
    public bool areLevelUpEquipmentOn;

    // -- Fisher
    public bool isInvertedFishingSpot;
    public bool isHiddenFishingBar;

    // ------------ VIDEO

    public bool isAlwaysOnTop;
    public bool isClickThrough;
    public bool is60FPS;
    public int currentMonitorIndex;


    // ------------ AUDIO

    public float masterVolume;






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

        isDamageOn = manager.IsDamageOn;
        isItemCollectionOn = manager.IsItemCollectionOn;
        areTooltipsOn = manager.AreTooltipsOn;

        areLevelUpEquipmentOn = manager.AreLevelUpEquipmentOn;

        isInvertedFishingSpot = manager.IsInvertedFishingSpot;
        isHiddenFishingBar = manager.IsHiddenFishingBar;



        isAlwaysOnTop = manager.IsAlwaysOnTop;
        isClickThrough = manager.IsClickThrough;
        is60FPS = manager.Is60FPS;
        currentMonitorIndex = manager.CurrentMonitorIndex;



        masterVolume = manager.MasterVolume;
    }
}
