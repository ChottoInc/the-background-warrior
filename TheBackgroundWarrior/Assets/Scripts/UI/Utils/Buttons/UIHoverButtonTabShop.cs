using UnityEngine.EventSystems;

public class UIHoverButtonTabShop : UIHoverButtonTab
{
    private UIShopCardPack _item;

    private void Awake()
    {
        _item = GetComponent<UIShopCardPack>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_item.IsPurchased) return;

        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_item.IsPurchased) return;

        base.OnPointerExit(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (_item.IsPurchased) return;

        base.OnPointerClick(eventData);
    }
}
