using System.Collections.Generic;

public class PlayerFarmerSaveData
{
    public int levelStatGreenthumb;
    public int levelAgronomy;
    public int levelKindness;
    public int levelStatLuck;

    public int availableStatPoints;

    public int currentLevel;
    public long currentExp;

    public CropSaveData slot1CropSaveData;
    public CropSaveData slot2CropSaveData;
    public CropSaveData slot3CropSaveData;
    public CropSaveData slot4CropSaveData;

    public List<CompanionSaveData> companionSaveDatas;

    public PlayerFarmerSaveData() { }

    public PlayerFarmerSaveData(PlayerFarmerData data)
    {
        levelStatGreenthumb = data.LevelStatGreenthumb;
        levelAgronomy = data.LevelStatAgronomy;
        levelKindness = data.LevelStatKindness;
        levelStatLuck = data.LevelStatLuck;

        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;


        if(data.Slot1CropData != null)
            slot1CropSaveData = new CropSaveData(data.Slot1CropData);

        if (data.Slot2CropData != null)
            slot2CropSaveData = new CropSaveData(data.Slot2CropData);

        if (data.Slot3CropData != null)
            slot3CropSaveData = new CropSaveData(data.Slot3CropData);

        if (data.Slot4CropData != null)
            slot4CropSaveData = new CropSaveData(data.Slot4CropData);


        companionSaveDatas = new List<CompanionSaveData>();
        foreach (var companion in data.Companions)
        {
            CompanionSaveData saveData = new CompanionSaveData(companion);
            companionSaveDatas.Add(saveData);
        }
    }
}
