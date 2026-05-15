using UnityEngine;
using static UtilsItem;

[CreateAssetMenu(menuName = "Data/Inventory/Item Data", fileName = "ItemData_")]
public class ItemSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] ItemType itemType;

    [Space(10)]
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;

    public int Id => id;
    public ItemType ItemType => itemType;

    public Sprite Sprite => sprite;
    public string ItemName => itemName;



    public override bool Equals(object other)
    {
        ItemSO otherItem = other as ItemSO;
        return id == otherItem.id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
