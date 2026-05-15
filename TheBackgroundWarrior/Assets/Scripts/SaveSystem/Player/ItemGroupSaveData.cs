
public class ItemGroupSaveData
{
    public int idItem;
    public int quantity;

    public ItemGroupSaveData() { }

    public ItemGroupSaveData(ItemGroup group)
    {
        idItem = group.IdItem;
        quantity = group.Quantity;
    }
}
