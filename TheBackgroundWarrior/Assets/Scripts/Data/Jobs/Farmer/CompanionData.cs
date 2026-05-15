
public class CompanionData
{
    private CompanionSO companionSO;

    private int currentLevel;

    private int currentSlot;


    public CompanionSO CompanionSO => companionSO;

    public int CurrentLevel => currentLevel;

    public int CurrentSlot => currentSlot;



    public float CurrentAtkPerc => companionSO.BaseAtkPerc;
    public float CurrentAtkSpd => companionSO.BaseAtkSpd;



    public CompanionData(CompanionSO companionSO)
    {
        this.companionSO = companionSO;

        currentLevel = 1;

        currentSlot = -1;
    }

    public CompanionData(CompanionSaveData saveData)
    {
        companionSO = UtilsGather.GetCompanionById(saveData.companionId);

        currentLevel = saveData.currentLevel;

        currentSlot = saveData.currentSlot;
    }

    public void SetSlot(int id)
    {
        currentSlot = id;
    }
}
