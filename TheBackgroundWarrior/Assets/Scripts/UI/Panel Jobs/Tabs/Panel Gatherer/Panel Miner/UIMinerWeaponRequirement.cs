using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMinerWeaponRequirement : MonoBehaviour
{
    [SerializeField] Image imageItem;
    [SerializeField] TMP_Text textRequirement;

    [Space(10)]
    [SerializeField] Transform tooltipPosition;


    private int quantityInventory;
    private ItemGroup requirement;

    private ItemSO itemSO;

    public void Setup(ItemGroup requirement)
    {
        this.requirement = requirement;

        itemSO = UtilsItem.GetItemById(requirement.IdItem);
        imageItem.sprite = itemSO.Sprite;

        if (PlayerManager.Instance.Inventory.HasItem(requirement.IdItem))
        {
            int index = PlayerManager.Instance.Inventory.GetGroupIndex(requirement.IdItem);
            quantityInventory = PlayerManager.Instance.Inventory.ItemGroups[index].Quantity;
        }

        string colorTagOpen = "<color=#FFFFFF>";
        string colorTagClose = "</color>";

        if (quantityInventory < requirement.Quantity)
            colorTagOpen = "<color=#878787>";

        //textRequirement.text = $"{quantityInventory}/{requirement.Quantity}";

        string finalRequirement = string.Format("{0}{1}{2}/{3}", colorTagOpen, quantityInventory, colorTagClose, requirement.Quantity);
        textRequirement.text = finalRequirement;
    }

    public void OnPointerEnter()
    {
        string itemName = "N/A";
        if(itemSO != null)
        {
            itemName = itemSO.ItemName;
        }

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = itemName;
        UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true);
    }

    public void OnPointerExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }
}
