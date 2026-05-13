using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelConversion : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;

    [Space(10)]
    [SerializeField] UIPanelConversionList panelConversionList;

    [Space(10)]
    [SerializeField] UIConversionSlot[] slots;

    [Space(10)]
    [SerializeField] GameObject panelEmpty;
    [SerializeField] GameObject panelCard;
    [SerializeField] UICardConverted cardConvertedPrefab;

    private void Awake()
    {
        cardConvertedPrefab.OnRevealed += OnCardRevealed;
    }

    private void OnDestroy()
    {
        cardConvertedPrefab.OnRevealed -= OnCardRevealed;
    }

    public void Setup()
    {
        ResetConvertedCardUI();

        ResetSlots();

        gameObject.SetActive(true);
    }

    private void ResetConvertedCardUI()
    {
        panelEmpty.SetActive(true);
        panelCard.SetActive(false);
    }

    private void ResetSlots()
    {
        foreach (var slot in slots)
        {
            ResetSlot(slot);
        }
    }

    private void ResetSlot(UIConversionSlot slot)
    {
        slot.Setup(null);
    }


    public UIConversionSlot GetFirstEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsFilled)
                return slot;
        }
        return null;
    }

    public bool CanConvert()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsFilled)
                return false;
        }
        return true;
    }

    public void OnButtonConvert()
    {
        if (!CanConvert())
        {
            Debug.Log("Not all slots are filled");
            return;
        }

        HandleConversion();
    }

    private void HandleConversion()
    {
        List<CardSO> cards = new List<CardSO>();
        foreach (var slot in slots)
        {
            cards.Add(slot.CardSO);
        }

        foreach (var card in cards)
        {
            PlayerManager.Instance.Inventory.RemoveItem(card.Id, 1);
        }

        CardSO convertedCard = convertedCard = UtilsItem.GetConvertedCard(cards);

        if(convertedCard == null)
        {
            Debug.Log("Something went wrong when converting the card");
            return;
        }

        PlayerManager.Instance.Inventory.AddItem(convertedCard.Id, 1);

        PlayerManager.Instance.SaveInventoryData();

        // Active panel
        panelCard.SetActive(true);

        // Start reveal card
        cardConvertedPrefab.Setup(convertedCard);


        // Reset slots and cards in list
        foreach (var slot in slots)
        {
            slot.DeselectCardPrefab();
        }

        // All refreshedd are handled once the revel is over
    }

    private void OnCardRevealed()
    {
        Debug.Log("Converted");

        // Refresh this panel
        Setup();

        // Reset to hide the card
        cardConvertedPrefab.ResetUI();

        // Refresh card list
        panelConversionList.Setup();
    }



    public void OnButtonClose()
    {
        AudioManager.Instance.PlayClickUI();

        Close();

        tabInventory.OpenInventory(null, UITabInventory.ID_INVENTORY_FILTER_CARDS);
    }

    public void Close()
    {
        gameObject.SetActive(false);

        panelConversionList.Close();
    }
}
