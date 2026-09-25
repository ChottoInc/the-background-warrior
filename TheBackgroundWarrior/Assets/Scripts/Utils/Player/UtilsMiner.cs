using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsMiner
{
    public enum RockType { Copper, Iron, Bronze, Silver, Gold }


    private static Dictionary<int, ListableGameDataSO> dictWeaponLevelToSprite;

    // ----------- JOB STATS ------------- //

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

    // ----------- ROCKS ------------- //

    private const float BASE_ROCK_DURABILITY = 45f;
    private const float ROCK_DURABILITY_SCALE = 2.8f;


    // ----------- WEAPON ------------- //

    public const int ID_MINER_WEAPON = 0;

    public const int MINER_WEAPON_MAX_LEVEL = 10;

    /*
     * The first requirements for level up the miner weapon are manually set, 
     * but after 5 levels, the weapon needs at least every item on the list, so can cycle throught them and use const values to determine
     * how many you should use to level up.
     * 
     * if expanding the game you need more than 5 items to level up, make manual checks. and add const values to arrays
     * 
     * */

    private const int MAX_MANUAL_WEAPON_REQUIREMENT = 5;

    private const int MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON = 5;

    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 900;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE = 500;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE = 200;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE = 50;
    private const int BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE = 20;

    private static readonly float[] BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE =
    {
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE,
        BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE
    };


    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE = 1.8f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE = 1.75f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE = 1.7f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE = 1.65f;
    private const float GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE = 1.6f;


    private static readonly float[] GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE =
    {
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_COPPER_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_IRON_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_BRONZE_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_SILVER_ORE,
        GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_GOLD_ORE
    };




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
            PER_LEVEL_MINER_MAX_LUCK +
            1;


        BASE_MINER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_MINER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_MINER_EXP_GROWTH = jobDataSO.FlatExpGrowth;

        MINER_WEAPON_LINEAR_GROWTH = jobDataSO.WeaponLinearGrowth;
        MINER_WEAPON_QUADRATIC_GROWTH = jobDataSO.WeaponQuadraticGrowth;


        LoadDictWeaponLevelToSprites();

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




    public static float GetRockDurabilityByType(RockType rockType)
    {
        return BASE_ROCK_DURABILITY * Mathf.Pow(ROCK_DURABILITY_SCALE, (int)rockType);
    }


    /// <summary>
    /// Exp given by the smashed rocks
    /// </summary>
    public static long GetRockExp(RockType rockType)
    {
        switch (rockType)
        {
            default:
            case RockType.Copper: return 2;
            case RockType.Iron: return 6;
            case RockType.Bronze: return 12;
            case RockType.Silver: return 20;
            case RockType.Gold: return 40;
        }
    }




    private static void LoadDictWeaponLevelToSprites()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Miner/ContainerGameData_WeaponToSprites");
        dictWeaponLevelToSprite = container.Entries.ToDictionary(e => e.Id);
    }

    public static Sprite GetWeaponSpriteByLevel(int id)
    {
        return UtilsGeneral.GetGameDataSO<GearToSpriteSO>(id, dictWeaponLevelToSprite).Sprite;
    }

    public static List<ItemGroup> GetRequirementsForMinerWeaponLevel(int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        if (level <= MAX_MANUAL_WEAPON_REQUIREMENT)
        {
            switch (level)
            {
                default:
                case 2:
                    result.Add(new ItemGroup(0, 30));
                    result.Add(new ItemGroup(1, 15));
                    break;

                case 3:
                    result.Add(new ItemGroup(0, 90));
                    result.Add(new ItemGroup(1, 50));
                    result.Add(new ItemGroup(2, 15));
                    break;

                case 4:
                    result.Add(new ItemGroup(0, 350));
                    result.Add(new ItemGroup(1, 200));
                    result.Add(new ItemGroup(2, 50));
                    result.Add(new ItemGroup(3, 15));
                    break;

                case 5:
                    result.Add(new ItemGroup(0, 900));
                    result.Add(new ItemGroup(1, 500));
                    result.Add(new ItemGroup(2, 200));
                    result.Add(new ItemGroup(3, 50));
                    result.Add(new ItemGroup(4, 20));
                    break;
            }
        }
        else
        {
            List<int> ids = new List<int> { 0, 1, 2, 3, 4 };
            // automatically get items amount after all of them are used manually
            for (int i = 0; i < MAX_ITEM_REQUIREMENTS_FOR_MINER_WEAPON; i++)
            {
                result.Add(new ItemGroup(ids[i], RequiredMinerItemAmount(level, i)));
            }
        }

        return result;
    }

    private static int RequiredMinerItemAmount(int level, int itemIndex)
    {
        //return Mathf.FloorToInt(BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex]));
        return
            Mathf.FloorToInt(BASE_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex] *
            Mathf.Pow(GROWTH_AMOUNT_MINER_WEAPON_PER_LEVEL_ORE[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));
    }
}
