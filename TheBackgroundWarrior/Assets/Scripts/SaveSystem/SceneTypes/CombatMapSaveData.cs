
public class CombatMapSaveData
{
    public int mapId;
    public int currentStage;
    public int reachedStage;
    public int reachedPrestige;

    public CombatMapSaveData() { }

    public CombatMapSaveData(int mapId, int currentStage, int reachedStage, int reachedPrestige)
    {
        this.mapId = mapId;
        this.currentStage = currentStage;
        this.reachedStage = reachedStage;
        this.reachedPrestige = reachedPrestige;
    }
}
