using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITabSettingsGameplay : UITabWindow
{
    [Header("Battle")]
    [SerializeField] UIPanelAutoBattle panelAutoBattleSettings;
    [SerializeField] UIPanelAutoBattle panelAutoBattleWorld;

    [Header("HUD")]
    [SerializeField] Toggle toggleInvertedHUD;
    [SerializeField] Toggle toggleAutocloseHUD;
    [SerializeField] TMP_Dropdown dropdownMinutes;
    [SerializeField] Toggle toggleAdvancedStats;

    [Header("Floating HUD")]
    [SerializeField] Toggle toggleDamage;
    [SerializeField] Toggle toggleItemCollection;
    [SerializeField] Toggle toggleTooltips;

    [Header("Animations")]
    [SerializeField] Toggle toggleLevelUpEquipmentAnimation;

    [Header("Fisher")]
    [SerializeField] Toggle toggleInvertedFishingSpot;
    [SerializeField] Toggle toggleHideFishingBar;

    [Header("Mage")]
    [SerializeField] Toggle toggleShowCurrentSpell;

    private void Awake()
    {
        panelAutoBattleSettings.OnSet += OnToggleAutoBattleSettings;

        if(panelAutoBattleWorld != null)
            panelAutoBattleWorld.OnSet += OnToggleAutoBattleWorld;
    }

    private void OnDestroy()
    {
        panelAutoBattleSettings.OnSet -= OnToggleAutoBattleSettings;

        if (panelAutoBattleWorld != null)
            panelAutoBattleWorld.OnSet -= OnToggleAutoBattleWorld;
    }

    public override void Open()
    {
        base.Open();

        Setup();
    }

    private void Setup()
    {
        panelAutoBattleSettings.Setup();


        if (toggleInvertedHUD != null)
            toggleInvertedHUD.SetIsOnWithoutNotify(SettingsManager.Instance.IsInvertedHudOn);

        if (toggleAutocloseHUD != null)
            toggleAutocloseHUD.SetIsOnWithoutNotify(SettingsManager.Instance.IsAutocloseHudOn);

        dropdownMinutes.ClearOptions();
        List<TMP_Dropdown.OptionData> listLanguage = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData(UtilsGeneral.GetAutocloseTimerText(UtilsGeneral.AutocloseTimerType.MINS5)),
            new TMP_Dropdown.OptionData(UtilsGeneral.GetAutocloseTimerText(UtilsGeneral.AutocloseTimerType.MINS10)),
            new TMP_Dropdown.OptionData(UtilsGeneral.GetAutocloseTimerText(UtilsGeneral.AutocloseTimerType.MINS20)),
            new TMP_Dropdown.OptionData(UtilsGeneral.GetAutocloseTimerText(UtilsGeneral.AutocloseTimerType.MINS30)),
            new TMP_Dropdown.OptionData(UtilsGeneral.GetAutocloseTimerText(UtilsGeneral.AutocloseTimerType.MINS60)),
            new TMP_Dropdown.OptionData(UtilsGeneral.GetAutocloseTimerText(UtilsGeneral.AutocloseTimerType.MINS120)),
        };

        dropdownMinutes.AddOptions(listLanguage);

        dropdownMinutes.SetValueWithoutNotify((int)SettingsManager.Instance.CurrentAutocloseTimerType);


        if (toggleAdvancedStats != null)
            toggleAdvancedStats.SetIsOnWithoutNotify(SettingsManager.Instance.IsShowAdvancedStatOn);



        if (toggleDamage != null)
            toggleDamage.SetIsOnWithoutNotify(SettingsManager.Instance.IsDamageOn);

        if (toggleItemCollection != null)
            toggleItemCollection.SetIsOnWithoutNotify(SettingsManager.Instance.IsItemCollectionOn);

        if (toggleTooltips != null)
            toggleTooltips.SetIsOnWithoutNotify(SettingsManager.Instance.AreTooltipsOn);



        if (toggleLevelUpEquipmentAnimation != null)
            toggleLevelUpEquipmentAnimation.SetIsOnWithoutNotify(SettingsManager.Instance.AreLevelUpEquipmentOn);



        if (toggleInvertedFishingSpot != null)
            toggleInvertedFishingSpot.SetIsOnWithoutNotify(SettingsManager.Instance.IsInvertedFishingSpot);

        if (toggleHideFishingBar != null)
            toggleHideFishingBar.SetIsOnWithoutNotify(SettingsManager.Instance.IsHiddenFishingBar);



        if (toggleShowCurrentSpell != null)
            toggleShowCurrentSpell.SetIsOnWithoutNotify(SettingsManager.Instance.IsCurrentSpellShowing);
    }


    private void OnToggleAutoBattleSettings(bool isOn)
    {
        if (panelAutoBattleWorld != null)
            panelAutoBattleWorld.SetToggleWithoutNotify(isOn);
    }

    private void OnToggleAutoBattleWorld(bool isOn)
    {
        panelAutoBattleSettings.SetToggleWithoutNotify(isOn);
    }



    public void OnToggleInvertedHUD(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsInvertedHUDOn(isOn);
    }

    public void OnToggleAutocloseHUD(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsAutocloseHUDOn(isOn);
    }

    public void OnAutocloseTimerChange(int index)
    {
        SettingsManager.Instance.SetAutocloseTimer((UtilsGeneral.AutocloseTimerType)index);
    }

    public void OnToggleShowAdvancedStats(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsShowAdvancedStatsOn(isOn);
    }



    public void OnToggleDamage(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsDamageOn(isOn);
    }

    public void OnToggleItemCollection(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsItemCollectionOn(isOn);
    }

    public void OnToggleTooltips(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetAreTooltipsOn(isOn);
    }


    public void OnToggleLevelUpEquipmentAnimation(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetAreLevelUpEquipmentAnimationOn(isOn);
    }


    public void OnToggleInvertedFishingSpot(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsInvertedFishingSpotOn(isOn);
    }

    public void OnToggleHiddenFishingBar(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsHiddenFishingSpot(isOn);
    }


    public void OnToggleShowCurrentSpell(bool isOn)
    {
        AudioManager.Instance.PlayClickUI();
        SettingsManager.Instance.SetIsCurrentSpellShowing(isOn);
    }
}
