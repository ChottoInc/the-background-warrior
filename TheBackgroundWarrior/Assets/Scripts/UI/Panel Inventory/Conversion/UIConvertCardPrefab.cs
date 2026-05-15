using UnityEngine;
using UnityEngine.UI;

public class UIConvertCardPrefab : MonoBehaviour
{
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;
    [SerializeField] Image imageSelected;

    private UIPanelConversionList panelConversionList;

    private CardSO cardSO;
    private int index;

    private UIConversionSlot assignedSlot;


    public CardSO CardSO => cardSO;

    public bool IsSelected => assignedSlot != null;


    public void Setup(UIPanelConversionList panelConversionList, CardSO cardSO, int index)
    {
        this.panelConversionList = panelConversionList;
        this.cardSO = cardSO;
        this.index = index;

        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;

        imageSelected.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        AudioManager.Instance.PlayClickUI();

        if (!IsSelected)
            Select();
        else
            Deselect();
    }

    /// <summary>
    /// Check if are there any empty slot for this card
    /// </summary>
    public void Select()
    {
        panelConversionList.OnCardSelected(this);
    }

    public void Deselect()
    {
        // Clear the slot
        assignedSlot.Setup(null);
        assignedSlot = null;

        imageSelected.gameObject.SetActive(false);
    }

    public void SetAsSelected(UIConversionSlot slot)
    {
        assignedSlot = slot;

        imageSelected.gameObject.SetActive(true);
    }
}
