using UnityEngine;

public static class UtilsFisher
{
    public static float PER_LEVEL_FISHER_GAIN_CALMNESS = 0.01f;
    public static float PER_LEVEL_FISHER_GAIN_REFLEX = 0.01f;
    public static float PER_LEVEL_FISHER_GAIN_KNOWLEDGE = 0.01f;
    public static float PER_LEVEL_FISHER_GAIN_LUCK = 0.01f;
            
    public static int PER_LEVEL_FISHER_MAX_CALMNESS = 50;
    public static int PER_LEVEL_FISHER_MAX_REFLEX = 25;
    public static int PER_LEVEL_FISHER_MAX_KNOWLEDGE = 30;
    public static int PER_LEVEL_FISHER_MAX_LUCK = 40;


    public static int MAX_LEVEL_FISHER;
           
           
           
    private static float BASE_FISHER_EXP_GROWTH = 50f;
    private static float EXPO_FISHER_EXP_GROWTH = 1.08f;
    private static float FLAT_FISHER_EXP_GROWTH = 10f;


    private static PlayerJobFisherSO jobDataSO;




    public const long PASSIVE_EXP = 50;
    public const long UNCAUGHT_EXP = 300;



    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Fisher) as PlayerJobFisherSO;

        PER_LEVEL_FISHER_GAIN_CALMNESS = jobDataSO.PerLevelGainCalmness;
        PER_LEVEL_FISHER_GAIN_REFLEX = jobDataSO.PerLevelGainReflex;
        PER_LEVEL_FISHER_GAIN_KNOWLEDGE = jobDataSO.PerLevelGainKnowledge;
        PER_LEVEL_FISHER_GAIN_LUCK = jobDataSO.PerLevelGainLuck;

        PER_LEVEL_FISHER_MAX_CALMNESS = jobDataSO.MaxLevelCalmness;
        PER_LEVEL_FISHER_MAX_REFLEX = jobDataSO.MaxLevelReflex;
        PER_LEVEL_FISHER_MAX_KNOWLEDGE = jobDataSO.MaxLevelKnowledge;
        PER_LEVEL_FISHER_MAX_LUCK = jobDataSO.MaxLevelLuck;


        MAX_LEVEL_FISHER =
           PER_LEVEL_FISHER_MAX_CALMNESS +
           PER_LEVEL_FISHER_MAX_REFLEX +
           PER_LEVEL_FISHER_MAX_KNOWLEDGE + 
           PER_LEVEL_FISHER_MAX_LUCK;


        BASE_FISHER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_FISHER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_FISHER_EXP_GROWTH = jobDataSO.FlatExpGrowth;
    }



    public static long RequiredExpForFisherLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_FISHER_EXP_GROWTH * Mathf.Pow(EXPO_FISHER_EXP_GROWTH, level) + FLAT_FISHER_EXP_GROWTH * level);
    }
}
