
public class CropSaveData
{
    public int cropId;

    public float currentGrowth;

    public int plantedSlot;

    public CropSaveData() { }

    public CropSaveData(CropData data)
    {
        cropId = data.CropSO.Id;

        currentGrowth = data.CurrentGrowth;

        plantedSlot = data.PlantedSlot;
    }
}
