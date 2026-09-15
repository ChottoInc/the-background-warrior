using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmerPanelCompanionInfoPrefab : MonoBehaviour
{
    [SerializeField] Image imageIconCompanion;

    [Header("Info")]
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textLevel;
    [SerializeField] GenericBar barExp;

    [Header("Ability")]
    [SerializeField] TMP_Text textAbility;

    [Header("Equip")]
    [SerializeField] GameObject panelEquipped;
    [SerializeField] Button buttonEquip;
    [SerializeField] GameObject buttonUnequip;

    private UIFarmerPanelCompanions panelCompanions;
    private CompanionData companionData;

    public void Setup(UIFarmerPanelCompanions panelCompanions, CompanionData companionData)
    {
        this.panelCompanions = panelCompanions;
        this.companionData = companionData;

        imageIconCompanion.sprite = companionData.CompanionSO.IconCompanion;

        textName.text = companionData.CompanionSO.CompanionName;
        textLevel.text = string.Format(UtilsText.AllText[UtilsText.text_job_current_stat_level], companionData.CurrentLevel);
        barExp.Setup(companionData.ExpToNextLevel, companionData.CurrentExp);

        float valueStat = companionData.CompanionSO.StatModifier.BaseModifierValue + (companionData.CompanionSO.StatModifier.IncreasePerLevelValue * (companionData.CurrentLevel - 1));
        textAbility.text = string.Format(companionData.CompanionSO.CompanionDesc, valueStat * 100f);

        if(companionData.CurrentSlot != -1)
        {
            panelEquipped.SetActive(true);
            buttonEquip.gameObject.SetActive(false);
            buttonUnequip.SetActive(true);
        }
        else
        {
            panelEquipped.SetActive(false);
            buttonEquip.gameObject.SetActive(true);
            buttonUnequip.SetActive(false);
        }

        UpdateEquipButtonUI();
    }

    private void UpdateEquipButtonUI()
    {
        buttonEquip.interactable = !PlayerManager.Instance.PlayerFarmerData.AreEquippedCompanionsFull();
    }

    public void OnButtonEquip()
    {
        panelCompanions.OnButtonEquip(companionData);
    }

    public void OnButtonUnequip()
    {
        panelCompanions.OnButtonUnequip(companionData);
    }
}
