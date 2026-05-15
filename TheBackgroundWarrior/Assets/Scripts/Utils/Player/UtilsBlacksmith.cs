using System.Collections.Generic;
using UnityEngine;

public static class UtilsBlacksmith
{
    // ---------------- GEAR IDS --------------- //

    public const int ID_BLACKSMITH_HELMET = 0;
    public const int ID_BLACKSMITH_ARMOR = 1;
    public const int ID_BLACKSMITH_GLOVES = 2;
    public const int ID_BLACKSMITH_BOOTS = 3;


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


    public const int CHANGE_BLACKSMITH_GEARS_EVERY = 5;

    private const int MAX_MANUAL_WEAPON_REQUIREMENT = 5;

    private const int MAX_ITEM_REQUIREMENTS_FOR_BLACKSMITH_GEARS = 5;



    // sprites
    private static Sprite[] blacksmithHelmetSprites;
    private static Sprite[] blacksmithArmorSprites;
    private static Sprite[] blacksmithGlovesSprites;
    private static Sprite[] blacksmithBootsSprites;


    

    // materials scaling
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER = 360;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON = 200;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE = 120;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER = 70;
    private const int BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD = 20;

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

    /*
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_COPPER = 2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_IRON = 1.9f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_BRONZE = 1.8f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_SILVER = 1.7f;
    private const float GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_GOLD = 1.6f;

    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_COPPER = 2.4f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_IRON = 2.3f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_BRONZE = 2.2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_SILVER = 2.1f;
    private const float GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_GOLD = 2f;

    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_COPPER = 2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_IRON = 1.9f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_BRONZE = 1.8f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_SILVER = 1.8f;
    private const float GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_GOLD = 1.7f;

    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_COPPER = 2.2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_IRON = 2.1f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_BRONZE = 2f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_SILVER = 1.9f;
    private const float GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_GOLD = 1.8f;
    */

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_GOLD
    };

    /*
    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_GOLD
    };

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_GOLD
    };

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_GOLD
    };

    private static readonly float[] GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_METAL =
    {
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_COPPER,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_IRON,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_BRONZE,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_SILVER,
        GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_GOLD
    };
    */

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
            PER_LEVEL_BLACKSMITH_MAX_METALLURGY;



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

        blacksmithHelmetSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Helmets");
        blacksmithArmorSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Armors");
        blacksmithGlovesSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Gloves");
        blacksmithBootsSprites = LoadBlacksmithGearSprites("Sprites/Blacksmith/Boots");
    }



    // ---------------- BLACKSMITH STATS --------------- //

    public static long RequiredExpForBlacksmithLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_BLACKSMITH_EXP_GROWTH * Mathf.Pow(EXPO_BLACKSMITH_EXP_GROWTH, level) + FLAT_BLACKSMITH_EXP_GROWTH * level);
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

    private static Sprite[] LoadBlacksmithGearSprites(string path)
    {
        return Resources.LoadAll<Sprite>(path);
    }


    public static Sprite[] GetAllBlacksmithGearSprites(int idGear)
    {
        switch (idGear)
        {
            case ID_BLACKSMITH_HELMET: return blacksmithHelmetSprites;
            case ID_BLACKSMITH_ARMOR: return blacksmithArmorSprites;
            case ID_BLACKSMITH_GLOVES: return blacksmithGlovesSprites;
            case ID_BLACKSMITH_BOOTS: return blacksmithBootsSprites;
        }
        return null;

    }

    public static Sprite GetBlacksmithGearSpriteByIndex(int idGear, int index)
    {
        Sprite[] sprites = null;

        switch (idGear)
        {
            case ID_BLACKSMITH_HELMET: sprites = blacksmithHelmetSprites; break;
            case ID_BLACKSMITH_ARMOR: sprites = blacksmithArmorSprites; break;
            case ID_BLACKSMITH_GLOVES: sprites = blacksmithGlovesSprites; break;
            case ID_BLACKSMITH_BOOTS: sprites = blacksmithBootsSprites; break;
        }

        if (sprites != null)
        {
            if (index < sprites.Length)
                return sprites[index];
        }

        return null;
    }

    public static List<ItemGroup> GetRequirementsForBlacksmithGearLevel(int idGear, int level)
    {
        List<ItemGroup> result = new List<ItemGroup>();

        // For simplicity, the first 5 levels are shared between gears
        if (level <= MAX_MANUAL_WEAPON_REQUIREMENT)
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
                int amount = RequiredBlacksmithItemAmount(idGear, level, i);

                if (amount != -1)
                    result.Add(new ItemGroup(ids[i], amount));
            }
        }

        return result;
    }

    private static int RequiredBlacksmithItemAmount(int idGear, int level, int itemIndex)
    {
        /*
        switch (idGear)
        {
            case ID_BLACKSMITH_HELMET:
                //return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_METAL[itemIndex]));
                return
                    Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] *
                    Mathf.Pow(GROWTH_AMOUNT_BLACKSMITH_HELMET_PER_LEVEL_METAL[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));

            case ID_BLACKSMITH_ARMOR:
                //return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_METAL[itemIndex]));
                return
                    Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] *
                    Mathf.Pow(GROWTH_AMOUNT_BLACKSMITH_ARMOR_PER_LEVEL_METAL[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));

            case ID_BLACKSMITH_GLOVES:
                //return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_METAL[itemIndex]));
                return
                    Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] *
                    Mathf.Pow(GROWTH_AMOUNT_BLACKSMITH_GLOVES_PER_LEVEL_METAL[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));

            case ID_BLACKSMITH_BOOTS:
                //return Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] * Mathf.Pow(level, GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_METAL[itemIndex]));
                return
                    Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] *
                    Mathf.Pow(GROWTH_AMOUNT_BLACKSMITH_BOOTS_PER_LEVEL_METAL[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));
        }
        
        return -1;
        */

        return
                    Mathf.FloorToInt(BASE_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex] *
                    Mathf.Pow(GROWTH_AMOUNT_BLACKSMITH_GEAR_PER_LEVEL_METAL[itemIndex], level - MAX_MANUAL_WEAPON_REQUIREMENT));
    }
}
