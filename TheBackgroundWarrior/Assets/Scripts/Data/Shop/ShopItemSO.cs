using UnityEngine;

public class ShopItemSO : ScriptableObject
{
    [SerializeField] protected string uniqueId;

    [Space(10)]
    [SerializeField] protected bool isUnique;
    [SerializeField] protected bool isDaily;

    [Space(10)]
    [SerializeField] protected UtilsShop.ShopItemType shopItemType;

    [Space(10)]
    [SerializeField] protected string itemName;

    [TextArea]
    [SerializeField] protected string itemDesc;

    [Space(10)]
    [SerializeField] protected int price;

    [Space(10)]
    [SerializeField] protected Sprite sprite;


    public string UniqueId => uniqueId;

    public bool IsUnique => isUnique;
    public bool IsDaily => isDaily;

    public UtilsShop.ShopItemType ShopItemType => shopItemType;

    public string ItemName => itemName; 
    public string ItemDesc => itemDesc;

    public int Price => price;

    public Sprite Sprite => sprite;
}
