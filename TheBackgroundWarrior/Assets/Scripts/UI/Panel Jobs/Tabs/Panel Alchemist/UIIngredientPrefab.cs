using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngredientPrefab : MonoBehaviour
{
    [SerializeField] Image _imageIngredient;
    [SerializeField] TMP_Text _textName;
    [SerializeField] TMP_Text _textQuantity;

    [Space(10)]
    [SerializeField] Transform _tooltipPosition;

    private int _quantityInventory;

    private GenericRequirement _requirement;

    public void Setup(GenericRequirement requirement)
    {
        _requirement = requirement;

        _imageIngredient.sprite = requirement.item.Sprite;

        _textName.text = requirement.item.ItemName;

        if (PlayerManager.Instance.Inventory.HasItem(requirement.item.Id))
        {
            int index = PlayerManager.Instance.Inventory.GetGroupIndex(requirement.item.Id);
            _quantityInventory = PlayerManager.Instance.Inventory.ItemGroups[index].Quantity;
        }

        string colorTagOpen = "<color=#FFFFFF>";
        string colorTagClose = "</color>";

        if (_quantityInventory < requirement.quantity)
            colorTagOpen = "<color=#878787>";

        string finalRequirement = string.Format("{0}{1}{2}/{3}", colorTagOpen, _quantityInventory, colorTagClose, requirement.quantity);
        _textQuantity.text = finalRequirement;
    }

    public void OnPointerEnter()
    {
        string itemName = "N/A";
        if (_requirement.item != null)
        {
            switch(_requirement.item.ItemType)
            {
                case UtilsItem.ItemType.Fish: itemName = UtilsGeneral.GetFishTooltip(_requirement.item as FishSO); break;

                default:
                    itemName = _requirement.item.ItemName;
                    break;
            }
        }

        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = itemName;
        UITooltipManager.Instance.Show(tooltipData, _tooltipPosition.position, true);
    }

    public void OnPointerExit()
    {
        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }
}
