public class PlayerMinerSaveData
{
    // ---- LEVEL STAT POINTS

    public int levelStatPower;
    public int levelStatSmashSpeed;
    public int levelStatShockwave;
    public int levelStatLuck;

    // ---- POINTS

    public int availableStatPoints;


    // ---- STAT VALUES

    public int currentLevel;
    public long currentExp;

    // ---- WEAPON

    public int levelWeaponMiner;



    public PlayerMinerSaveData() { }

    public PlayerMinerSaveData(PlayerMinerData data)
    {
        levelStatPower = data.LevelStatPower;
        levelStatSmashSpeed = data.LevelStatSmashSpeed;
        levelStatShockwave = data.LevelStatShockwave;
        levelStatLuck = data.LevelStatLuck;


        availableStatPoints = data.AvailableStatPoints;

        currentLevel = data.CurrentLevel;
        currentExp = data.CurrentExp;

        levelWeaponMiner = data.WeaponLevel;
    }
}
