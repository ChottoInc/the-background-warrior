using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // base currency
    private int currentBits;

    private List<ItemGroup> itemGroups;


    public int CurrentBits => currentBits;

    public List<ItemGroup> ItemGroups => itemGroups;



    // Trigger used for quests
    public event Action<int> OnItemAdd;



    public Inventory()
    {
        //currentBits = 100;
        currentBits = 0;
        itemGroups = new List<ItemGroup>();
        /*
        itemGroups.Add(new ItemGroup(0, 2000));
        itemGroups.Add(new ItemGroup(1, 2000));
        itemGroups.Add(new ItemGroup(2, 2000));
        itemGroups.Add(new ItemGroup(3, 2000));
        itemGroups.Add(new ItemGroup(4, 2000));
        itemGroups.Add(new ItemGroup(50, 100));*/
    }

    public Inventory(InventorySaveData saveData)
    {
        currentBits = saveData.currentBits;

        itemGroups = new List<ItemGroup>();

        foreach (var group in saveData.groupSaves)
        {
            itemGroups.Add(new ItemGroup(group));
        }
    }

    #region CURRENCIES

    public void AddBits(int amount)
    {
        currentBits += amount;
    }

    public bool RemoveBits(int amount)
    {
        if(currentBits < amount)
        {
            Debug.Log("Insufficient bits");
            return false;
        }

        currentBits -= amount;
        return true;
    }

    #endregion

    #region ITEMS

    public void AddItem(int id, int quantity)
    {
        //Debug.Log("id: " + id);
        OnItemAdd?.Invoke(id);

        if (!HasItem(id))
        {
            ItemGroup group = new ItemGroup(id, quantity);
            itemGroups.Add(group);
        }
        else
        {
            ItemSO itemSO = UtilsItem.GetItemById(id);
            if(itemSO.ItemType != UtilsItem.ItemType.Fish)
            {
                int index = GetGroupIndex(id);
                itemGroups[index].AddQuantity(quantity);
            }
        }

        itemGroups.Sort();
    }

    public bool RemoveItem(int id, int quantity)
    {
        if (!HasItem(id)) return false;

        int index = GetGroupIndex(id);

        bool result = itemGroups[index].RemoveQuantity(quantity);

        if(result)
        {
            if (itemGroups[index].Quantity <= 0)
            {
                itemGroups.RemoveAt(index);
            }
        }

        return result;
    }

    public bool HasItem(int id)
    {
        foreach (var group in itemGroups)
        {
            if (group.IdItem == id)
                return true;
        }
        return false;
    }

    public bool HasEnough(int id, int amount)
    {
        if (!HasItem(id)) return false;

        int index = GetGroupIndex(id);

        if (index > -1)
        {
            if (itemGroups[index].Quantity >= amount)
            {
                return true;
            }
        }

        return false;
    }

    public int GetGroupIndex(int id)
    {
        for (int i = 0; i < itemGroups.Count; i++)
        {
            if (itemGroups[i].IdItem == id)
                return i;
        }
        return -1;
    }

    public List<ItemGroup> GetAllCards()
    {
        List<ItemGroup> result = new List<ItemGroup>();
        foreach (var group in itemGroups)
        {
            ItemSO item = UtilsItem.GetItemById(group.IdItem);
            if (item.ItemType == UtilsItem.ItemType.Card)
                result.Add(group);
        }
        return result;
    }

    #endregion
}
