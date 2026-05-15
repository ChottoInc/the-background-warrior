using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelDismantle : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;

    [Space(10)]
    [SerializeField] UIInventoryPanelInfo panelInfo;

    [Space(10)]
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textAmount;
    [SerializeField] TMP_Text textName;

    [Space(10)]
    [SerializeField] TMP_InputField inputAmount;

    private int selectedAmount;


    private bool isOpen;

    public bool IsOpen => isOpen;


    private ItemGroup group;

    private ItemSO itemSO;

    public void Setup(ItemGroup group)
    {
        this.group = group;

        itemSO = UtilsItem.GetItemById(group.IdItem);

        imageItem.sprite = itemSO.Sprite;
        textAmount.text = group.Quantity.ToString();
        textName.text = itemSO.ItemName;

        inputAmount.text = "1";
        selectedAmount = 1;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
        isOpen = show;
    }

    public void OnButtonCancel()
    {
        AudioManager.Instance.PlayClickUI();

        Show(false);
        panelInfo.Show(true);

        // refresh and pass the last group selected
        panelInfo.Setup(PlayerManager.Instance.Inventory.ItemGroups[PlayerManager.Instance.Inventory.GetGroupIndex(group.IdItem)]);
    }

    public void OnButtonLeast()
    {
        AudioManager.Instance.PlayClickUI();

        selectedAmount = 1;
        RefreshInputAmountUI();
    }

    public void OnButtonLess()
    {
        AudioManager.Instance.PlayClickUI();

        if (selectedAmount > 1)
        {
            selectedAmount--;
            RefreshInputAmountUI();
        }
    }

    public void OnButtonMore()
    {
        AudioManager.Instance.PlayClickUI();

        if (selectedAmount < group.Quantity)
        {
            selectedAmount++;
            RefreshInputAmountUI();
        }
    }

    public void OnButtonMost()
    {
        AudioManager.Instance.PlayClickUI();

        selectedAmount = group.Quantity;
        RefreshInputAmountUI();
    }

    public void OnInputChange(string text)
    {
        int parsed = int.Parse(text);

        if (parsed < 1)
            parsed = 1;

        if(parsed > group.Quantity)
            parsed = group.Quantity;    

        selectedAmount = parsed;
        RefreshInputAmountUI();
    }

    private void RefreshInputAmountUI()
    {
        inputAmount.text = selectedAmount.ToString();
    }

    public void OnButtonDismantle()
    {
        // Update selected amount with input
        OnInputChange(inputAmount.text);

        CardSO selectedCard = itemSO as CardSO;

        // Calculate bits
        int totalBits = 0;

        for (int i = 0; i < selectedAmount; i++)
        {
            totalBits += UtilsItem.GetDismantleValueFromCard(selectedCard);
        }

        // Remove cards and add bits
        PlayerManager.Instance.Inventory.RemoveItem(selectedCard.Id, selectedAmount);
        PlayerManager.Instance.Inventory.AddBits(totalBits);

        PlayerManager.Instance.SaveInventoryData();

        Show(false);

        // Refresh tab inventory
        tabInventory.RefreshInventory();
    }
}
