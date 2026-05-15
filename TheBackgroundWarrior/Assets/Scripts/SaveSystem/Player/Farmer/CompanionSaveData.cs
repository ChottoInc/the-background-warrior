
public class CompanionSaveData
{
    public int companionId;

    public int currentLevel;

    public int currentSlot;


    public CompanionSaveData() { }

    public CompanionSaveData(CompanionData data)
    {
        companionId = data.CompanionSO.Id;

        currentLevel = data.CurrentLevel;

        currentSlot = data.CurrentSlot;
    }
}
