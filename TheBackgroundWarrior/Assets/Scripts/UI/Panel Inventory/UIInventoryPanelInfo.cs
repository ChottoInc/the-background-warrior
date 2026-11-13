using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryPanelInfo : MonoBehaviour
{
    [SerializeField] UITabInventory tabInventory;
    [SerializeField] UIPanelItems _panelItems;

    [Space(10)]
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textAmount;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textDesc;

    [Header("Buttons")]
    [SerializeField] GameObject _panelCardButtons;
    [SerializeField] GameObject _panelConsumableButtons;
    [SerializeField] UIPanelDismantle panelDismantle;


    private ItemGroup group;

    private ItemSO itemSO;

    public void Setup(ItemGroup group)
    {
        this.group = group;

        itemSO = UtilsItem.GetItemById(group.IdItem);

        imageItem.sprite = itemSO.Sprite;
        textAmount.text = "x" + group.Quantity.ToString();
        textName.text = itemSO.ItemName;

        textDesc.text = itemSO.ItemDesc;

        // Set panel buttons to active if the selected item is a card
        _panelCardButtons.SetActive(itemSO.ItemType == UtilsItem.ItemType.Card);

        if( itemSO.ItemType == UtilsItem.ItemType.Bait ||
            itemSO.ItemType == UtilsItem.ItemType.Concoction)
        {
            _panelConsumableButtons.SetActive(true);
        }
        else
        {
            _panelConsumableButtons.SetActive(false);
        }
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }



    public void OnButtonConvert()
    {
        tabInventory.OpenPanelConvert();
    }

    public void OnButtonDismantle()
    {
        panelDismantle.Setup(group);
        panelDismantle.Show(true);
        Show(false);
    }



    public void OnButtonUse()
    {
        if (UtilsItem.UseConsumable(itemSO))
        {
            PlayerManager.Instance.Inventory.RemoveItem(itemSO.Id, 1);
            PlayerManager.Instance.SaveInventoryData();

            _panelItems.RefreshItemAfterUse();

            Setup(group);
        }
    }

}
