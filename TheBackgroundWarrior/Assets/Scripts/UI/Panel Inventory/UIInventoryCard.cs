using UnityEngine;
using UnityEngine.UI;

public class UIInventoryCard : UIInventoryItem
{
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    private CardSO cardSO;

    public override void Setup(UIPanelItems panelItems, ItemGroup group, ItemSO itemSO)
    {
        BaseSetup(panelItems, group, itemSO);

        cardSO = itemSO as CardSO;

        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;
    }
}
