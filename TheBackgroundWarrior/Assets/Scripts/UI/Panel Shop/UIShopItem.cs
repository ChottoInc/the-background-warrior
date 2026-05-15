using UnityEngine;

public abstract class UIShopItem : MonoBehaviour
{
    [SerializeField] protected GameObject panelPrice;
    [SerializeField] protected GameObject panelPurchased;

    public abstract void Setup(UIPanelShopItems panelShopItems, ShopItemSO itemSO, int currentFilter);
}
