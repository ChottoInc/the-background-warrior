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
    [SerializeField] GameObject buttonRedeem;
    [SerializeField] GameObject panelRedeem;

    public override void Open()
    {
        base.Open();

        UpdateBitsUI();

        // Check for redeem filter to appear
        if( PlayerManager.Instance.Inventory.HasItem(ID_CARD_ERIS_1) &&
            PlayerManager.Instance.Inventory.HasItem(ID_CARD_ERIS_2) &&
            PlayerManager.Instance.Inventory.HasItem(ID_CARD_ERIS_3) &&
            PlayerManager.Instance.Inventory.HasItem(ID_CARD_ERIS_4) &&
            PlayerManager.Instance.Inventory.HasItem(ID_CARD_ERIS_5))
        {
            buttonRedeem.SetActive(true);
        }
        else
        {
            buttonRedeem.SetActive(false);
        }

        // By default open scroll shop, and panel redeem is hidden
        panelShopItems.gameObject.SetActive(true);
        panelRedeem.SetActive(false);

        // select first filter by default
        currentFilterButton = firstFilterButton;
        currentFilterButton.SelectButton(true);
        panelShopItems.Setup(UtilsShop.ID_SHOP_FILTER_CARDPACKS);
    }

    public void OpenShopWindow(UIShopFilterButton filterButton, int filter)
    {
        // deselect current button filter
        if(currentFilterButton != null)
        {
            currentFilterButton.SelectButton(false);
        }

        currentFilterButton = filterButton;

        // select new button filter
        if (currentFilterButton != null)
        {
            currentFilterButton.SelectButton(true);
        }

        if (filter == UtilsShop.ID_SHOP_FILTER_REDEEM)
        {
            panelRedeem.SetActive(true);
            panelShopItems.gameObject.SetActive(false);
        }
        else
        {
            panelShopItems.gameObject.SetActive(true);
            panelRedeem.SetActive(false);

            panelShopItems.Setup(filter);
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

        AudioManager.Instance.PlayClickUI();

        base.Close();
    }
}
