using System.Collections.Generic;

public class InventorySaveData
{
    public int currentBits;

    public List<ItemGroupSaveData> groupSaves;


    public InventorySaveData() { }

    public InventorySaveData(Inventory inventory)
    {
        currentBits = inventory.CurrentBits;

        groupSaves = new List<ItemGroupSaveData>();

        foreach (var group in inventory.ItemGroups)
        {
            groupSaves.Add(new ItemGroupSaveData(group));
        }
    }
}
