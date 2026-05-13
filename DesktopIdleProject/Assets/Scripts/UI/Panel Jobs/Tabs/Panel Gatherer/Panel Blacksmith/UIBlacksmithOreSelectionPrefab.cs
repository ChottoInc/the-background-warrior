using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBlacksmithOreSelectionPrefab : MonoBehaviour
{
    [SerializeField] Image imageSelectBorder;
    [SerializeField] Image imageOre;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textAmount;

    private UIBlacksmithPanelSelectOre panelSelection;
    private ItemGroup itemGroup;

    private ItemSO itemSO;

    public void Setup(UIBlacksmithPanelSelectOre panelSelection, ItemGroup itemGroup)
    {
        this.panelSelection = panelSelection;
        this.itemGroup = itemGroup;

        Deselect();

        itemSO = UtilsItem.GetItemById(itemGroup.IdItem);

        imageOre.sprite = itemSO.Sprite;
        textName.text = itemSO.ItemName;
        textAmount.text = itemGroup.Quantity.ToString();
    }

    public void OnButtonSelect()
    {
        AudioManager.Instance.PlayClickUI();

        panelSelection.OnSelectOre(this, itemSO);
    }

    public void Select()
    {
        imageSelectBorder.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        imageSelectBorder.gameObject.SetActive(false);
    }
}
