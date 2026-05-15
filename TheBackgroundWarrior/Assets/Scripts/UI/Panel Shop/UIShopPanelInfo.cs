using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPanelInfo : MonoBehaviour
{
    [SerializeField] UITabShop tabShop;
    [SerializeField] UIPanelShopItems panelItems;

    [Space(10)]
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textDesc;

    [Space(10)]
    [SerializeField] Transform confirmBuyPosition;

    private ShopItemSO itemSO;
    private int currentFilter;

    public void Setup(ShopItemSO itemSO, int currentFilter)
    {
        this.itemSO = itemSO;
        this.currentFilter = currentFilter;
        
        imageItem.sprite = itemSO.Sprite;
        textName.text = itemSO.ItemName;
        textDesc.text = itemSO.ItemDesc;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public async void OnButtonBuy()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        if (PlayerManager.Instance.Inventory.CurrentBits < itemSO.Price) return;

        string question = $"Do you want to buy {itemSO.ItemName} for {itemSO.Price} bits?";

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
        tooltipData.text = question;

        bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, confirmBuyPosition.position, true);

        if (confirm)
        {
            bool needClose = false;

            // remove bits
            PlayerManager.Instance.Inventory.RemoveBits(itemSO.Price);

            // update shop data
            ShopManager.Instance.UpdateShopItemPurchase(itemSO);
            ShopManager.Instance.SaveShopData();

            // update shop, auto hide panel info
            panelItems.Setup(currentFilter);
            tabShop.UpdateBitsUI();

            // check if need the shop to close
            switch (itemSO.ShopItemType)
            {
                case UtilsShop.ShopItemType.CardPack: needClose = true; break;
                case UtilsShop.ShopItemType.Job: needClose = false; break;
            }

            if (needClose)
            {
                tabShop.ForceClose();
            }

            // handle item purchase if add to inventory or something else
            switch (itemSO.ShopItemType)
            {
                case UtilsShop.ShopItemType.CardPack: HandleCardPack(itemSO as ShopCardPackSO); break;
                case UtilsShop.ShopItemType.Job: HandleShopJob(itemSO as ShopJobSO); break;
            }
        }
    }

    private void HandleCardPack(ShopCardPackSO cardPack)
    {
        int totalCards = cardPack.Size;

        List<CardSO> result = new List<CardSO>();

        // first fill all cards
        for (int i = 0; i < totalCards; i++)
        {
            UtilsItem.CardRarity rarity = UtilsGeneral.GetRandomValueFromGeneralChanches(cardPack.RarityChances);
            CardSO card = UtilsItem.GetRandomCardByRarity(rarity);
            result.Add(card);
        }

        // check for guaranteed
        if (cardPack.IsGuaranteed)
        {
            // check if not contains guaranteed
            if(!UtilsItem.DoesCardListContainRarity(result, cardPack.GuaranteedRarity))
            {
                // get random of guarateed rarity
                CardSO cardToAdd = UtilsItem.GetRandomCardByRarity(cardPack.GuaranteedRarity);

                // switch with guaranteed
                int indexToSub = UtilsItem.GetRandomIndexLowestRarityCard(result);
                result[indexToSub] = cardToAdd;
            }
        }

        // add to inventory
        foreach (var card in result)
        {
            PlayerManager.Instance.Inventory.AddItem(card.Id, 1);
        }

        // save only when added new cards and removed bits
        PlayerManager.Instance.SaveInventoryData();

        TooltipManagerData tooltipData = new TooltipManagerData
        {
            idTooltip = UITooltipManager.ID_SHOW_CARDOPENING,
            openingCards = result
        };

        UITooltipManager.Instance.Show(tooltipData, Vector2.zero, true);
    }

    private void HandleShopJob(ShopJobSO jobSO)
    {
        PlayerManager.Instance.PlayerJobsData.AddAvailableJob(jobSO.ShoppingJob);

        // save only when added new cards and removed bits
        PlayerManager.Instance.SaveInventoryData();
    }
}
