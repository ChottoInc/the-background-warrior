
public class PlayerBlacksmithSaveData
{
    // ---- LEVEL STAT POINTS

    public int levelStatCraftSpeed;
    public int levelStatEfficiency;
    public int levelStatLuck;
    public int levelStatMetallurgy;

    // ---- POINTS

    public int availableStatPoints;


    // ---- STAT VALUES

    public int currentLevel;

    public long currentExp;

    // ---- WEAPON

    public int levelHelmetBlacksmith;
    public int levelArmorBlacksmith;
    public int levelGlovesBlacksmith;
    public int levelBootsBlacksmith;

    // ---- FORGING

    public int currentForgingOre;
    public bool isInfiniteForging;
    public int currentForgingQuantity;



    public PlayerBlacksmithSaveData() { }

    public PlayerBlacksmithSaveData(PlayerBlacksmithData data)
    {
        levelStatCraftSpeed = data.LevelStatCraftSpeed;
        levelStatEfficiency = data.LevelEfficiency;
        levelStatLuck = data.LevelStatLuck;
        levelStatMetallurgy = data.LevelStatMetallurgy;


        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        levelHelmetBlacksmith = data.HelmetLevel;
        levelArmorBlacksmith = data.ArmorLevel;
        levelGlovesBlacksmith = data.GlovesLevel;
        levelBootsBlacksmith = data.BootsLevel;

        currentForgingOre = data.CurrentForgingOre;
        isInfiniteForging = data.IsInfiniteForging;
        currentForgingQuantity = data.CurrentForgingQuantity;
    }
}
