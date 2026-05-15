using UnityEngine;

public static class UtilsMiner
{
    public static float PER_LEVEL_MINER_GAIN_POWER = 2;
    public static float PER_LEVEL_MINER_GAIN_SMASHSPEED = 0.02f;
    public static float PER_LEVEL_MINER_GAIN_SHOCKWAVE = 0.01f;
    public static float PER_LEVEL_MINER_GAIN_LUCK = 0.01f;
           
    public static int PER_LEVEL_MINER_MAX_POWER = 50;
    public static int PER_LEVEL_MINER_MAX_SMASHSPEED = 40;
    public static int PER_LEVEL_MINER_MAX_SHOCKWAVE = 25;
    public static int PER_LEVEL_MINER_MAX_LUCK = 40;


    public static int MAX_LEVEL_MINER;


    private static float BASE_MINER_EXP_GROWTH = 50f;
    private static float EXPO_MINER_EXP_GROWTH = 1.08f;
    private static float FLAT_MINER_EXP_GROWTH = 10f;
            

    private static float MINER_WEAPON_LINEAR_GROWTH = 0.35f;
    private static float MINER_WEAPON_QUADRATIC_GROWTH = 0.05f;

    public const int MINER_WEAPON_MAX_LEVEL = 10;


    private static PlayerJobMinerSO jobDataSO;

    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Miner) as PlayerJobMinerSO;

        PER_LEVEL_MINER_GAIN_POWER = jobDataSO.PerLevelGainPower;
        PER_LEVEL_MINER_GAIN_SMASHSPEED = jobDataSO.PerLevelGainSmashSpeed;
        PER_LEVEL_MINER_GAIN_SHOCKWAVE = jobDataSO.PerLevelGainShockwave;
        PER_LEVEL_MINER_GAIN_LUCK = jobDataSO.PerLevelGainLuck;


        PER_LEVEL_MINER_MAX_POWER = jobDataSO.MaxLevelPower;
        PER_LEVEL_MINER_MAX_SMASHSPEED = jobDataSO.MaxLevelSmashSpeed;
        PER_LEVEL_MINER_MAX_SHOCKWAVE = jobDataSO.MaxLevelShockwave;
        PER_LEVEL_MINER_MAX_LUCK = jobDataSO.MaxLevelLuck;


        MAX_LEVEL_MINER =
            PER_LEVEL_MINER_MAX_POWER +
            PER_LEVEL_MINER_MAX_SMASHSPEED +
            PER_LEVEL_MINER_MAX_SHOCKWAVE +
            PER_LEVEL_MINER_MAX_LUCK;


        BASE_MINER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_MINER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_MINER_EXP_GROWTH = jobDataSO.FlatExpGrowth;

        MINER_WEAPON_LINEAR_GROWTH = jobDataSO.WeaponLinearGrowth;
        MINER_WEAPON_QUADRATIC_GROWTH = jobDataSO.WeaponQuadraticGrowth;

        //Debug.Log("base miner exp growth: " + BASE_MINER_EXP_GROWTH);
        //Debug.Log("expo miner exp growth: " + EXPO_MINER_EXP_GROWTH);
    }


    public static long RequiredExpForMinerLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;
        
        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_MINER_EXP_GROWTH * Mathf.Pow(level, EXPO_MINER_EXP_GROWTH) + FLAT_MINER_EXP_GROWTH * level);
    }

    public static float GetMinerWeaponMultiplier(int weaponLevel)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = weaponLevel - 1;

        return baseMultiplier
               + MINER_WEAPON_LINEAR_GROWTH * lv
               + MINER_WEAPON_QUADRATIC_GROWTH * lv * lv;
    }
}
