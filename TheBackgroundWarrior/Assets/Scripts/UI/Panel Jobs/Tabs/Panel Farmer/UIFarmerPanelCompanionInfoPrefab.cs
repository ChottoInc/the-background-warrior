using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmerPanelCompanionInfoPrefab : MonoBehaviour
{
    [SerializeField] Image imageIconCompanion;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textLevel;

    [Space(10)]
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
        textLevel.text = companionData.CurrentLevel.ToString();

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
