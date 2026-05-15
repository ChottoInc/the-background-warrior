using UnityEngine;

public static class UtilsWarrior
{
    public static float PER_LEVEL_WARRIOR_GAIN_MAXHP;
    public static float PER_LEVEL_WARRIOR_GAIN_ATK;
    public static float PER_LEVEL_WARRIOR_GAIN_DEF;
    public static float PER_LEVEL_WARRIOR_GAIN_ATK_SPEED;
    public static float PER_LEVEL_WARRIOR_GAIN_CRIT_RATE;
    public static float PER_LEVEL_WARRIOR_GAIN_CRIT_DMG;
    public static float PER_LEVEL_WARRIOR_GAIN_LUCK;
                  
    public static int PER_LEVEL_WARRIOR_MAX_MAXHP;
    public static int PER_LEVEL_WARRIOR_MAX_ATK;
    public static int PER_LEVEL_WARRIOR_MAX_DEF;
    public static int PER_LEVEL_WARRIOR_MAX_ATK_SPEED;
    public static int PER_LEVEL_WARRIOR_MAX_CRIT_RATE;
    public static int PER_LEVEL_WARRIOR_MAX_CRIT_DMG;
    public static int PER_LEVEL_WARRIOR_MAX_LUCK;


    public static int MAX_LEVEL_WARRIOR;



    private static float BASE_FIGHT_EXP_GROWTH;
    private static float EXPO_FIGHT_EXP_GROWTH;
    private static float FLAT_FIGHT_EXP_GROWTH;


    private static PlayerJobWarriorSO jobDataSO;



    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Warrior) as PlayerJobWarriorSO;

        PER_LEVEL_WARRIOR_GAIN_MAXHP = jobDataSO.PerLevelGainMaxHp;
        PER_LEVEL_WARRIOR_GAIN_ATK = jobDataSO.PerLevelGainAtk;
        PER_LEVEL_WARRIOR_GAIN_DEF = jobDataSO.PerLevelGainDef;
        PER_LEVEL_WARRIOR_GAIN_ATK_SPEED = jobDataSO.PerLevelGainAtkSpd;
        PER_LEVEL_WARRIOR_GAIN_CRIT_RATE = jobDataSO.PerLevelGainCritRate;
        PER_LEVEL_WARRIOR_GAIN_CRIT_DMG = jobDataSO.PerLevelGainCritDmg;
        PER_LEVEL_WARRIOR_GAIN_LUCK = jobDataSO.PerLevelGainLuck;


        PER_LEVEL_WARRIOR_MAX_MAXHP = jobDataSO.MaxLevelMaxHp;
        PER_LEVEL_WARRIOR_MAX_ATK = jobDataSO.MaxLevelAtk;
        PER_LEVEL_WARRIOR_MAX_DEF = jobDataSO.MaxLevelDef;
        PER_LEVEL_WARRIOR_MAX_ATK_SPEED = jobDataSO.MaxLevelAtkSpd;
        PER_LEVEL_WARRIOR_MAX_CRIT_RATE = jobDataSO.MaxLevelCritRate;
        PER_LEVEL_WARRIOR_MAX_CRIT_DMG = jobDataSO.MaxLevelCritDmg;
        PER_LEVEL_WARRIOR_MAX_LUCK = jobDataSO.MaxLevelLuck;


        MAX_LEVEL_WARRIOR =
            PER_LEVEL_WARRIOR_MAX_MAXHP +
            PER_LEVEL_WARRIOR_MAX_ATK +
            PER_LEVEL_WARRIOR_MAX_DEF +
            PER_LEVEL_WARRIOR_MAX_ATK_SPEED +
            PER_LEVEL_WARRIOR_MAX_CRIT_RATE +
            PER_LEVEL_WARRIOR_MAX_CRIT_DMG +
            PER_LEVEL_WARRIOR_MAX_LUCK;


        BASE_FIGHT_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_FIGHT_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_FIGHT_EXP_GROWTH = jobDataSO.FlatExpGrowth;



        //Debug.Log("gain max hp: " + PER_LEVEL_WARRIOR_GAIN_MAXHP);
        //Debug.Log("max level max hp: " + PER_LEVEL_WARRIOR_MAX_MAXHP);
    }

    public static long RequiredExpForWarriorLevel(int level)
    {
        return (long)(BASE_FIGHT_EXP_GROWTH * Mathf.Pow(level, EXPO_FIGHT_EXP_GROWTH) + FLAT_FIGHT_EXP_GROWTH * level);
    }
}
