using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsBlacksmith
{
    // ---------------- GEAR IDS --------------- //

    public enum BlacksmithGear { Helmet, Armor, Gloves, Boots }


    // ---------------- STATS SCALING --------------- //

    public static float PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED = 0.02f;
    public static float PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY = 0.01f;
    public static float PER_LEVEL_BLACKSMITH_GAIN_LUCK = 0.01f;
    public static float PER_LEVEL_BLACKSMITH_GAIN_METALLURGY = 0.1f;
           
    public static int PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED = 50;
    public static int PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY = 30;
    public static int PER_LEVEL_BLACKSMITH_MAX_LUCK = 70;
    public static int PER_LEVEL_BLACKSMITH_MAX_METALLURGY = 30;


    public static int MAX_LEVEL_BLACKSMITH;



    private static float BASE_BLACKSMITH_EXP_GROWTH = 50f;
    private static float EXPO_BLACKSMITH_EXP_GROWTH = 1.08f;
    private static float FLAT_BLACKSMITH_EXP_GROWTH = 10f;


    // ---------------- GEAR STATS SCALING --------------- //
            
    //Helmet
    private static float BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH = 0.20f;
    private static float BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH = 0.05f;
            
    // Armor
    private static float BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH = 0.04f;
            
    // Gloves
    private static float BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH = 0.04f;
            
    private static float BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH = 0.05f;
            
    // Boots
    private static float BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH = 0.15f;
    private static float BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH = 0.038f;
            
    private static float BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH = 0.2f;
    private static float BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH = 0.05f;



    // ---------------- GEAR MATERIALS SCALING --------------- //


    private const int MAX_MANUAL_GEAR_REQUIREMENT = 5;

    private const int MAX_ITEM_REQUIREMENTS_FOR_BLACKSMITH_GEARS = 5;



    // sprites
    private static Dictionary<int, ListableGameDataSO> dictHelmetLevelToSprite;
    private static Dictionary<int, ListableGameDataSO> dictArmorLevelToSprite;
    private static Dictionary<int, ListableGameDataSO> dictGlovesLevelToSprite;
    private static Dictionary<int, ListableGameDataSO> dictBootsLevelToSprite;


    

    // materials scaling
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER = 1200;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON = 650;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE = 350;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER = 200;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD = 80;

    private static readonly float[] BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL =
    {
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER,
        BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD
    };

    private const float GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER = 2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON = 1.9f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE = 1.8f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER = 1.7f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD = 1.6f;

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD
    };

    public static int BLACKSMITH_HELMET_MAX_LEVEL = 10;
    public static int BLACKSMITH_ARMOR_MAX_LEVEL = 10;
    public static int BLACKSMITH_GLOVES_MAX_LEVEL = 10;
    public static int BLACKSMITH_BOOTS_MAX_LEVEL = 10;


    private static PlayerJobBlacksmithSO jobDataSO;


    public static void Initialize()
    {
        // load blacksmith abilities stats
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Blacksmith) as PlayerJobBlacksmithSO;

        PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED = jobDataSO.PerLevelGainCraftSpeed;
        PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY = jobDataSO.PerLevelGainEfficiency;
        PER_LEVEL_BLACKSMITH_GAIN_LUCK = jobDataSO.PerLevelGainLuck;
        PER_LEVEL_BLACKSMITH_GAIN_METALLURGY = jobDataSO.PerLevelGainMetallurgy;


        PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED = jobDataSO.MaxLevelCraftSpeed;
        PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY = jobDataSO.MaxLevelEfficiency;
        PER_LEVEL_BLACKSMITH_MAX_LUCK = jobDataSO.MaxLevelLuck;
        PER_LEVEL_BLACKSMITH_MAX_METALLURGY = jobDataSO.MaxLevelMetallurgy;


        MAX_LEVEL_BLACKSMITH =
            PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED +
            PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY +
            PER_LEVEL_BLACKSMITH_MAX_LUCK +
            PER_LEVEL_BLACKSMITH_MAX_METALLURGY +
            1;



        BASE_BLACKSMITH_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_BLACKSMITH_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_BLACKSMITH_EXP_GROWTH = jobDataSO.FlatExpGrowth;


        BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH = jobDataSO.HelmetMaxHpLinearGrowth;
        BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH = jobDataSO.HelmetMaxHpQuadraticGrowth;

        BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH = jobDataSO.ArmorDefLinearGrowth;
        BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH = jobDataSO.ArmorDefQuadraticGrowth;


        BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH = jobDataSO.GlovesAtkSpdLinearGrowth;
        BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH = jobDataSO.GlovesAtkSpdQuadraticGrowth;

        BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH = jobDataSO.GlovesCritDmgLinearGrowth;
        BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH = jobDataSO.GlovesCritDmgQuadraticGrowth;



        BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH = jobDataSO.BootsDefLinearGrowth;
        BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH = jobDataSO.BootsDefQuadraticGrowth;

        BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH = jobDataSO.BootsCritRateLinearGrowth;
        BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH = jobDataSO.BootsCritRateQuadraticGrowth;


        // load sprites

        dictHelmetLevelToSprite = LoadDictGearLevelToSprites("Data/Player/Blacksmith/ContainerGameData_HelmetToSprites");
        dictArmorLevelToSprite = LoadDictGearLevelToSprites("Data/Player/Blacksmith/ContainerGameData_ArmorToSprites");
        dictGlovesLevelToSprite = LoadDictGearLevelToSprites("Data/Player/Blacksmith/ContainerGameData_GlovesToSprites");
        dictBootsLevelToSprite = LoadDictGearLevelToSprites("Data/Player/Blacksmith/ContainerGameData_BootsToSprites");
    }



    // ---------------- BLACKSMITH STATS --------------- //

    public static long RequiredExpForBlacksmithLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_BLACKSMITH_EXP_GROWTH * Mathf.Pow(level, EXPO_BLACKSMITH_EXP_GROWTH) + FLAT_BLACKSMITH_EXP_GROWTH * level);
    }

    // ---------------- GEAR STATS --------------- //

    public static float GetBlacksmithHelmetMaxHpMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_HELMET_MAXHP_LINEAR_GROWTH * lv
               + BLACKSMITH_HELMET_MAXHP_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithArmorDefMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_ARMOR_DEF_LINEAR_GROWTH * lv
               + BLACKSMITH_ARMOR_DEF_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithGlovesAtkSpdMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_GLOVES_ATKSPD_LINEAR_GROWTH * lv
               + BLACKSMITH_GLOVES_ATKSPD_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithGlovesCritDmgMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_GLOVES_CRITDGM_LINEAR_GROWTH * lv
               + BLACKSMITH_GLOVES_CRITDGM_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithBootsDefMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_BOOTS_DEF_LINEAR_GROWTH * lv
               + BLACKSMITH_BOOTS_DEF_QUADRATIC_GROWTH * lv * lv;
    }

    public static float GetBlacksmithBootsCritRateMultiplier(int level)
    {
        float baseMultiplier = 1f; // 1x damage at level 1

        int lv = level - 1;

        return baseMultiplier
               + BLACKSMITH_BOOTS_CRITRATE_LINEAR_GROWTH * lv
               + BLACKSMITH_BOOTS_CRITRATE_QUADRATIC_GROWTH * lv * lv;
    }


    // ---------------- GEAR MATERIALS --------------- //

    private static Dictionary<int, ListableGameDataSO> LoadDictGearLevelToSprites(string path)
    {
        var container = Resources.Load<ContainerGameDataSO>(path);
        return container.Entries.ToDictionary(e => e.Id);
    }

    public static Sprite GetGearSpriteByLevel(int id, BlacksmithGear gear)
    {
        switch (gear)
        {
            default: return null;
            case BlacksmithGear.Helmet: return UtilsGeneral.GetGameDataSO<GearToSpriteSO>(id, dictHelmetLevelToSprite).Sprite;
            case BlacksmithGear.Armor: return UtilsGeneral.GetGameDataSO<GearToSpriteSO>(id, dictArmorLevelToSprite).Sprite;
            case BlacksmithGear.Gloves: return UtilsGeneral.GetGameDataSO<GearToSpriteSO>(id, dictGlovesLevelToSprite).Sprite;
            case BlacksmithGear.Boots: return UtilsGeneral.GetGameDataSO<GearToSpriteSO>(id, dictBootsLevelToSprite).Sprite;
        }
    }

    public static List<ItemGroup> GetRequirementsForBlacksmithGearLevel(BlacksmithGear gear, int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        // For simplicity, the first 5 levels are shared between gears
        if (level <= MAX_MANUAL_GEAR_REQUIREMENT)
        {
            switch (level)
            {
                default:
                case 2:
                    result.Add(new ItemGroup(150, 50));
                    result.Add(new ItemGroup(151, 20));
                    break;

                case 3:
                    result.Add(new ItemGroup(150, 150));
                    result.Add(new ItemGroup(151, 100));
                    result.Add(new ItemGroup(152, 50));
                    break;

                case 4:
                    result.Add(new ItemGroup(150, 450));
                    result.Add(new ItemGroup(151, 300));
                    result.Add(new ItemGroup(152, 150));
                    result.Add(new ItemGroup(153, 60));
                    break;

                case 5:
                    result.Add(new ItemGroup(150, 1200));
                    result.Add(new ItemGroup(151, 650));
                    result.Add(new ItemGroup(152, 350));
                    result.Add(new ItemGroup(153, 200));
                    result.Add(new ItemGroup(154, 80));
                    break;
            }
        }
        else
        {

            List<int> ids = new List<int> { 150, 151, 152, 153, 154 };
            // automatically get items amount after all of them are used manually
            for (int i = 0; i < MAX_ITEM_REQUIREMENTS_FOR_BLACKSMITH_GEARS; i++)
            {
                int amount = RequiredBlacksmithItemAmount(gear, level, i);

                if (amount != -1)
                    result.Add(new ItemGroup(ids[i], amount));
            }
        }

        return result;
    }

    private static int RequiredBlacksmithItemAmount(BlacksmithGear gear, int level, int itemIndex)
    {
        return
            Mathf.RoundToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] *
            Mathf.Pow(GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex], level - MAX_MANUAL_GEAR_REQUIREMENT));
    }
}
