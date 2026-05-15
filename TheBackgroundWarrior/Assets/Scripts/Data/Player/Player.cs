using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action<ItemSO> OnItemAdd;

    public event Action OnLevelUp;

    
    public virtual void AddItem(int id, int quantity)
    {
        PlayerManager.Instance.Inventory.AddItem(id, quantity);
        PlayerManager.Instance.SaveInventoryData();

        ItemSO itemSO = UtilsItem.GetItemById(id);
        AddItemEvent(itemSO);
    }

    public virtual void AddItemEvent(ItemSO itemSO)
    {
        OnItemAdd?.Invoke(itemSO);
    }

    protected virtual void LevelUp()
    {
        OnLevelUp?.Invoke();
    }
}
