using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    // base currency
    public int CurrentBits { get; private set; }

    public List<ItemGroup> ItemGroups { get; private set; }



    // Trigger used for quests
    public event Action<int> OnItemAdd;



    public Inventory()
    {
        CurrentBits = 0;
        ItemGroups = new List<ItemGroup>();
    }

    public Inventory(InventorySaveData saveData)
    {
        CurrentBits = saveData.currentBits;

        ItemGroups = new List<ItemGroup>();

        foreach (var group in saveData.groupSaves)
        {
            ItemGroups.Add(new ItemGroup(group));
        }
    }

    #region CURRENCIES

    public void AddBits(int amount)
    {
        // check if player has greed buff active, adds 20%
        if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Greed))
        {
            amount = Mathf.RoundToInt((float)amount * 1.2f);
        }

        CurrentBits += amount;
    }

    public bool RemoveBits(int amount)
    {
        if(CurrentBits < amount)
        {
            Debug.Log("Insufficient bits");
            return false;
        }

        CurrentBits -= amount;
        return true;
    }

    #endregion

    #region ITEMS

    public void AddItems(List<int> ids)
    {
        foreach (var id in ids)
        {
            AddItem(id, 1);
        }
    }

    public void AddItems(List<ItemGroup> groups)
    {
        foreach (var group in groups)
        {
            AddItem(group.IdItem, group.Quantity);
        }
    }

    public void AddItem(int id, int quantity)
    {
        //Debug.Log("id: " + id);
        OnItemAdd?.Invoke(id);

        if (!HasItem(id))
        {
            ItemGroup group = new ItemGroup(id, quantity);
            ItemGroups.Add(group);
        }
        else
        {
            //ItemSO itemSO = UtilsItem.GetItemById(id);

            int index = GetGroupIndex(id);

            if (ItemGroups[index].Quantity + quantity > UtilsItem.MAX_ITEM_IN_INVENTORY)
            {
                ItemGroups[index].AddQuantity(UtilsItem.MAX_ITEM_IN_INVENTORY - ItemGroups[index].Quantity);
            }
            else
            {
                ItemGroups[index].AddQuantity(quantity);
            }
        }

        ItemGroups.Sort();
    }

    public bool RemoveItem(int id, int quantity)
    {
        if (!HasItem(id)) return false;

        int index = GetGroupIndex(id);

        bool result = ItemGroups[index].RemoveQuantity(quantity);

        if(result)
        {
            if (ItemGroups[index].Quantity <= 0)
            {
                ItemGroups.RemoveAt(index);
            }
        }

        return result;
    }

    public bool HasItem(int id)
    {
        foreach (var group in ItemGroups)
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
            if (ItemGroups[index].Quantity >= amount)
            {
                return true;
            }
        }

        return false;
    }

    public int GetGroupIndex(int id)
    {
        for (int i = 0; i < ItemGroups.Count; i++)
        {
            if (ItemGroups[i].IdItem == id)
                return i;
        }
        return -1;
    }

    public int GetItemQuantity(int id)
    {
        int index = GetGroupIndex(id);

        if (index == -1) return -1;
        return ItemGroups[index].Quantity;
    }

    public List<ItemGroup> GetGroupsOfType(UtilsItem.ItemType itemType)
    {
        return ItemGroups.Where(group => UtilsItem.GetItemById(group.IdItem).ItemType == itemType).ToList();
    }

    public List<ItemGroup> GetAllCards()
    {
        List<ItemGroup> result = new List<ItemGroup>();
        foreach (var group in ItemGroups)
        {
            ItemSO item = UtilsItem.GetItemById(group.IdItem);
            if (item.ItemType == UtilsItem.ItemType.Card)
                result.Add(group);
        }
        return result;
    }

    #endregion
}
