using UnityEngine;

public class UtilsNecromancer : MonoBehaviour
{
    public enum RitualType { Summon, Arise, Afterlife, Ade }

    public const long BASE_EXP_GAIN_FROM_FIGHTERS = 400;



    public static float PER_LEVEL_NECROMANCER_GAIN_APTITUDE = 0.2f;         // every 5 levels new couple, up to 15
    public static float PER_LEVEL_NECROMANCER_GAIN_SUMMON = 0.02f;          // up to 35
    public static float PER_LEVEL_NECROMANCER_GAIN_MIGHT = 0.002f;          // up to 40, gain total 8% on enemy hp
    public static float PER_LEVEL_NECROMANCER_GAIN_LIFESPAN = 0.02f;        // up to 50, increase to 100% lifepan
    public static float PER_LEVEL_NECROMANCER_GAIN_HORDE = 0.2f;            // every 5 levels +1 horde limit, up to 20
    public static float PER_LEVEL_NECROMANCER_GAIN_LUCK = 0.005f;           // up to 10, spawn different mob

    public static int PER_LEVEL_NECROMANCER_MAX_APTITUDE = 15;              // 4 total couples, 3 unlockable
    public static int PER_LEVEL_NECROMANCER_MAX_SUMMON = 35;                // max 20% spawn rate
    public static int PER_LEVEL_NECROMANCER_MAX_MIGHT = 40;                 // 40 -> gain total 8% on enemy hp
    public static int PER_LEVEL_NECROMANCER_MAX_LIFESPAN = 50;              // 25 -> 50% lifespan
    public static int PER_LEVEL_NECROMANCER_MAX_HORDE = 20;                 // 5 total horde, 4 unlockable
    public static int PER_LEVEL_NECROMANCER_MAX_LUCK = 10;                  // max 5% luck - double odds for more exp gain necromancer


    public static int MAX_LEVEL_NECROMANCER;



    private static float BASE_NECROMANCER_EXP_GROWTH = 50f;
    private static float EXPO_NECROMANCER_EXP_GROWTH = 1.08f;
    private static float FLAT_NECROMANCER_EXP_GROWTH = 10f;


    private static PlayerJobNecromancerSO jobDataSO;

    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Necromancer) as PlayerJobNecromancerSO;

        PER_LEVEL_NECROMANCER_GAIN_APTITUDE = jobDataSO.PerLevelGainAptitude;
        PER_LEVEL_NECROMANCER_GAIN_SUMMON = jobDataSO.PerLevelGainSummon;
        PER_LEVEL_NECROMANCER_GAIN_MIGHT = jobDataSO.PerLevelGainMight;
        PER_LEVEL_NECROMANCER_GAIN_LIFESPAN = jobDataSO.PerLevelGainLifespan;
        PER_LEVEL_NECROMANCER_GAIN_HORDE = jobDataSO.PerLevelGainHorde;
        PER_LEVEL_NECROMANCER_GAIN_LUCK = jobDataSO.PerLevelGainLuck;

        PER_LEVEL_NECROMANCER_MAX_APTITUDE = jobDataSO.MaxLevelAptitude;
        PER_LEVEL_NECROMANCER_MAX_SUMMON = jobDataSO.MaxLevelSummon;
        PER_LEVEL_NECROMANCER_MAX_MIGHT = jobDataSO.MaxLevelMight;
        PER_LEVEL_NECROMANCER_MAX_LIFESPAN = jobDataSO.MaxLevelLifespan;
        PER_LEVEL_NECROMANCER_MAX_HORDE = jobDataSO.MaxLevelHorde;
        PER_LEVEL_NECROMANCER_MAX_LUCK = jobDataSO.MaxLevelLuck;


        MAX_LEVEL_NECROMANCER =
           PER_LEVEL_NECROMANCER_MAX_APTITUDE +
           PER_LEVEL_NECROMANCER_MAX_SUMMON +
           PER_LEVEL_NECROMANCER_MAX_MIGHT +
           PER_LEVEL_NECROMANCER_MAX_LIFESPAN +
           PER_LEVEL_NECROMANCER_MAX_HORDE +
           PER_LEVEL_NECROMANCER_MAX_LUCK +
           1;


        BASE_NECROMANCER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_NECROMANCER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_NECROMANCER_EXP_GROWTH = jobDataSO.FlatExpGrowth;


    }

    public static long RequiredExpForNecromancerLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_NECROMANCER_EXP_GROWTH * Mathf.Pow(level, EXPO_NECROMANCER_EXP_GROWTH) + FLAT_NECROMANCER_EXP_GROWTH * level);
    }
}
