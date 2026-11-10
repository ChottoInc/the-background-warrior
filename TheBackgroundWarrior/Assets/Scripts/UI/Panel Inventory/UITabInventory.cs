using TMPro;
using UnityEngine;

public class UITabInventory : UITabWindow
{
    public enum InventoryItemType { All, Ores, Metals, Fishes, Crops, Cards }

    [Header("Currencies")]
    [SerializeField] TMP_Text textBits;

    [Header("Filters")]
    [SerializeField]
    UIInventoryFilterButton[] filterButtons;

    private UIInventoryFilterButton currentFilterButton;
    private InventoryItemType _currentFilter;

    [Header("Window Center")]
    [SerializeField] UIPanelItems panelItems;
    [SerializeField] UIPanelConversion panelConvert;

    [Header("Window Right")]
    [SerializeField] UIPanelConversionList panelConvertList;

    public override void Open()
    {
        base.Open();

        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";

        UpdateFilters();

        panelItems.ShowPanelInfo(false);

        currentFilterButton = filterButtons[0];
        panelItems.Setup(InventoryItemType.All);
        _currentFilter = InventoryItemType.All;
    }

    private void UpdateFilters()
    {
        foreach (var filter in filterButtons)
        {
            filter.Refresh();
        }
    }

    public void RefreshInventory()
    {
        base.Open();

        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";

        UpdateFilters();

        panelItems.ShowPanelInfo(false);

        panelItems.Setup(_currentFilter);
    }

    public void OpenInventory(UIInventoryFilterButton filterButton, InventoryItemType filter)
    {
        if(filterButton != null)
        {
            currentFilterButton = filterButton;
        }

        panelItems.Setup(filter);
        _currentFilter = filter;

        ClosePanelConvert();
    }


    public void OpenPanelConvert()
    {
        // Hide inventory
        panelItems.ShowPanelInfo(false);
        panelItems.gameObject.SetActive(false);

        // Show Panel Conversion
        panelConvert.Setup();
        panelConvertList.Setup();
    }

    public void ClosePanelConvert()
    {
        // Hide inventory
        panelItems.ShowPanelInfo(false);
        panelItems.gameObject.SetActive(true);

        // Show Panel Conversion
        panelConvert.Close();
    }


    public void OnButtonClose()
    {
        Close();
    }

    public void OnButtonAddBits()
    {
        if (!SettingsManager.Instance.AreCheatsEnabled) return;

        PlayerManager.Instance.Inventory.AddBits(500);
        PlayerManager.Instance.SaveInventoryData();
        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";
    }
}
