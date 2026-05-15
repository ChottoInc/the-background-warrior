
using System;

public class ItemGroup : IComparable<ItemGroup>
{
    private int idItem;
    private int quantity;

    public int IdItem => idItem;
    public int Quantity => quantity;

    public ItemGroup(int idItem, int quantity)
    {
        this.idItem = idItem;
        this.quantity = quantity;
    }

    public ItemGroup(ItemGroupSaveData save)
    {
        idItem = save.idItem;
        quantity = save.quantity;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public bool RemoveQuantity(int amount)
    {
        if (amount > quantity) return false;

        quantity -= amount;
        return true;
    }




    public override bool Equals(object obj)
    {
        ItemGroup otherGroup = obj as ItemGroup;
        return otherGroup.idItem == idItem;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public int CompareTo(ItemGroup other)
    {
        if (idItem < other.idItem)
            return -1;
        else if (idItem > other.idItem)
            return 1;
        return 0;
    }
}
