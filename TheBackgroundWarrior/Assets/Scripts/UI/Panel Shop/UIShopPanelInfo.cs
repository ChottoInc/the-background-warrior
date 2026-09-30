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

    private ShopItemSO _itemSO;
    private int _currentFilter;

    public void Setup(ShopItemSO itemSO, int currentFilter)
    {
        _itemSO = itemSO;
        _currentFilter = currentFilter;
        
        imageItem.sprite = itemSO.Sprite;
        textName.text = itemSO.ItemName;
       
        HandleDesc();
    }

    private void HandleDesc()
    {
        string resultText = string.Empty;

        switch (_itemSO.ShopItemType)
        {
            default: resultText = _itemSO.ItemDesc; break;
            case UtilsShop.ShopItemType.Baits: resultText = HandleBaitDesc(); break;
        }

        if (_itemSO.IsDaily)
        {
            resultText += string.Format("<br>{0}", UtilsText.AllText[UtilsText.text_shop_isdaily]);
        }
        else if(_itemSO.IsUnique)
        {
            resultText += string.Format("<br>{0}", UtilsText.AllText[UtilsText.text_shop_isunique]);
        }

        textDesc.text = resultText;
    }

    private string HandleBaitDesc()
    {
        ShopBaitSO shopBaitSO = _itemSO as ShopBaitSO;
        return string.Format(shopBaitSO.ItemDesc, shopBaitSO.BaitSO.ItemDesc);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public async void OnButtonBuy()
    {
        if (UITooltipManager.Instance.IsCallbackOpen) return;

        if (PlayerManager.Instance.Inventory.CurrentBits < _itemSO.Price) return;

        string question = string.Format(UtilsText.AllText[UtilsText.text_yesno_question_buy], _itemSO.ItemName, _itemSO.Price);

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_YESNO;
        tooltipData.text = question;

        bool confirm = await UITooltipManager.Instance.ShowPanelYesNoCallback(tooltipData, confirmBuyPosition.position, true);

        if (confirm)
        {
            bool needClose = false;

            // remove bits
            PlayerManager.Instance.Inventory.RemoveBits(_itemSO.Price);

            // update shop data
            ShopManager.Instance.UpdateShopItemPurchase(_itemSO);
            ShopManager.Instance.SaveShopData();

            // update shop, auto hide panel info
            panelItems.Setup(_currentFilter);
            tabShop.UpdateBitsUI();

            // check if need the shop to close
            switch (_itemSO.ShopItemType)
            {
                case UtilsShop.ShopItemType.CardPack: needClose = true; break;
                case UtilsShop.ShopItemType.Job: needClose = false; break;
                case UtilsShop.ShopItemType.Baits: needClose = false; break;
            }

            if (needClose)
            {
                tabShop.ForceClose();
            }

            // handle item purchase if add to inventory or something else
            switch (_itemSO.ShopItemType)
            {
                case UtilsShop.ShopItemType.CardPack: HandleCardPack(_itemSO as ShopCardPackSO); break;
                case UtilsShop.ShopItemType.Job: HandleShopJob(_itemSO as ShopJobSO); break;
                case UtilsShop.ShopItemType.Baits: HandleShopBait(_itemSO as ShopBaitSO); break;
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
            CardSO card = UtilsCard.GetRandomCardByRarity(rarity);
            result.Add(card);
        }

        // check for guaranteed
        if (cardPack.IsGuaranteed)
        {
            // check if not contains guaranteed
            if(!UtilsCard.DoesCardListContainRarity(result, cardPack.GuaranteedRarity))
            {
                // get random of guarateed rarity
                CardSO cardToAdd = UtilsCard.GetRandomCardByRarity(cardPack.GuaranteedRarity);

                // switch with guaranteed
                int indexToSub = UtilsCard.GetRandomIndexLowestRarityCard(result);
                result[indexToSub] = cardToAdd;
            }
        }

        // add to inventory
        foreach (var card in result)
        {
            PlayerManager.Instance.Inventory.AddItem(card.Id, 1);
        }

        // save
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

        // save
        PlayerManager.Instance.SaveInventoryData();
    }

    private void HandleShopBait(ShopBaitSO shopBaitSO)
    {
        PlayerManager.Instance.Inventory.AddItem(shopBaitSO.BaitSO.Id, shopBaitSO.Quantity);

        // save
        PlayerManager.Instance.SaveInventoryData();
    }
}
