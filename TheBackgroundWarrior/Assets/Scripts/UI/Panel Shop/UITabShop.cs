using TMPro;
using UnityEngine;

public class UITabShop : UITabWindow
{
    private const int ID_CARD_ERIS_1 = 83;
    private const int ID_CARD_ERIS_2 = 84;
    private const int ID_CARD_ERIS_3 = 85;
    private const int ID_CARD_ERIS_4 = 86;
    private const int ID_CARD_ERIS_5 = 87;

    [Header("Currencies")]
    [SerializeField] TMP_Text textBits;

    [Header("Items")]
    [SerializeField] UIPanelShopItems panelShopItems;
    [SerializeField] UIShopFilterButton firstFilterButton;

    private UIShopFilterButton currentFilterButton;

    [Header("Redeem")]
    [SerializeField] GameObject panelRedeem;

    [Header("Debug")]
    [SerializeField] GameObject panelDebug;

    public override void Open()
    {
        base.Open();

        UpdateBitsUI();

        // By default open scroll shop, and panel debug is hidden
        panelShopItems.gameObject.SetActive(true);
        panelDebug.SetActive(false);
        panelRedeem.SetActive(false);

        // disable last filter

        // select first filter by default
        currentFilterButton = firstFilterButton;
        panelShopItems.Setup(UtilsShop.ID_SHOP_FILTER_CARDPACKS);
    }

    public void OpenShopWindow(UIShopFilterButton filterButton, int filter)
    {
        currentFilterButton = filterButton;

        if (filter == UtilsShop.ID_SHOP_FILTER_DEBUG)
        {
            panelDebug.SetActive(true);

            panelShopItems.Hide();
            panelRedeem.SetActive(false);
        }
        else if(filter == UtilsShop.ID_SHOP_FILTER_REDEEM)
        {
            panelRedeem.SetActive(true);

            panelDebug.SetActive(false);
            panelShopItems.Hide();
        }
        else
        {
            panelShopItems.gameObject.SetActive(true);
            panelShopItems.Setup(filter);

            panelRedeem.SetActive(false);
            panelDebug.SetActive(false);
        }
    }


    public void UpdateBitsUI()
    {
        textBits.text = $"x{PlayerManager.Instance.Inventory.CurrentBits}";
    }


    public override void Close()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        base.Close();
    }

    public void ForceClose()
    {
        base.Close();
    }


    public void OnButtonClose()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        base.Close();
    }
}
