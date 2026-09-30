using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsMage
{
    public enum MageSpellType { None, Fireball, Explosion, ChillWind, PoisonGas, Zap }

    public enum SpellTargetType { Single, Aoe }

    private static Dictionary<int, ListableGameDataSO> dictSpells;



    public static float PER_LEVEL_MAGE_GAIN_INSIGHT = 0.01f;        // up to 25
    public static float PER_LEVEL_MAGE_GAIN_CASTSPEED = 0.01f;      // up to 20
    public static float PER_LEVEL_MAGE_GAIN_SCHOLAR = 0.2f;         // every 5 levels new spell      
    public static float PER_LEVEL_MAGE_GAIN_PROFICIENCY = 0.2f;     // every 5 levels new slot FOR NOW

    public static int PER_LEVEL_MAGE_MAX_INSIGHT = 25;
    public static int PER_LEVEL_MAGE_MAX_CASTSPEED = 20;
    public static int PER_LEVEL_MAGE_MAX_SCHOLAR = 20;              // 5 total spell, 4 unlcoked
    public static int PER_LEVEL_MAGE_MAX_PROFICIENCY = 15;          // 4 total slots, 3 unlocked


    public static int MAX_LEVEL_MAGE;



    private static float BASE_MAGE_EXP_GROWTH = 50f;
    private static float EXPO_MAGE_EXP_GROWTH = 1.08f;
    private static float FLAT_MAGE_EXP_GROWTH = 10f;



    private static PlayerJobMageSO jobDataSO;



    private const long FIREBALL_EXP = 50;
    private const long EXPLOSION_EXP = 75;
    private const long CHILLWIND_EXP = 100;
    private const long POISONGAS_EXP = 125;
    private const long ZAP_EXP = 150;





    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Mage) as PlayerJobMageSO;

        PER_LEVEL_MAGE_GAIN_INSIGHT = jobDataSO.PerLevelGainInsight;
        PER_LEVEL_MAGE_GAIN_CASTSPEED = jobDataSO.PerLevelGainCastSpeed;
        PER_LEVEL_MAGE_GAIN_SCHOLAR = jobDataSO.PerLevelGainScholar;
        PER_LEVEL_MAGE_GAIN_PROFICIENCY = jobDataSO.PerLevelGainProficiency;

        PER_LEVEL_MAGE_MAX_INSIGHT = jobDataSO.MaxLevelInsight;
        PER_LEVEL_MAGE_MAX_CASTSPEED = jobDataSO.MaxLevelCastSpeed;
        PER_LEVEL_MAGE_MAX_SCHOLAR = jobDataSO.MaxLevelScholar;
        PER_LEVEL_MAGE_MAX_PROFICIENCY = jobDataSO.MaxLevelProficiency;


        MAX_LEVEL_MAGE =
           PER_LEVEL_MAGE_MAX_INSIGHT +
           PER_LEVEL_MAGE_MAX_CASTSPEED +
           PER_LEVEL_MAGE_MAX_SCHOLAR +
           PER_LEVEL_MAGE_MAX_PROFICIENCY +
           1;


        BASE_MAGE_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_MAGE_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_MAGE_EXP_GROWTH = jobDataSO.FlatExpGrowth;


        LoadDictSpells();
    }



    public static long RequiredExpForMageLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_MAGE_EXP_GROWTH * Mathf.Pow(level, EXPO_MAGE_EXP_GROWTH) + FLAT_MAGE_EXP_GROWTH * level);
    }

    public static int RequiredPointsForNextRank(SpellData data)
    {
        if (data == null) return -1;

        float mult = 1f;

        switch (data.CurrentRank)
        {
            default: break;
            case 1: mult = 1.5f; break;
            case 2: mult = 2.5f; break;
            case 3: mult = 4f; break;
            case 4: mult = 6f; break;
        }

        return Mathf.FloorToInt(data.SpellSO.BaseLearningPoints * mult);
    }

    private static void LoadDictSpells()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Mage/ContainerGameData_Spells");
        dictSpells = container.Entries.ToDictionary(e => e.Id);
    }
    
    public static List<SpellSO> GetAllSpells()
    {
        return dictSpells.Values.OfType<SpellSO>().ToList();
    }

    public static SpellSO GetSpellById(int id)
    {
        return UtilsGeneral.GetGameDataSO<SpellSO>(id, dictSpells);
    }

    public static SpellSO GetSpellByType(MageSpellType spellType)
    {
        return dictSpells.Cast<SpellSO>().Where(spell => spell.SpellType == spellType).FirstOrDefault();
    }

    /// <summary>
    /// Exp given by the caught fishes
    /// </summary>
    public static long GetSpellCastExp(MageSpellType spellType)
    {
        switch (spellType)
        {
            default: return 0;
            case MageSpellType.Fireball: return FIREBALL_EXP;
            case MageSpellType.Explosion: return EXPLOSION_EXP;
            case MageSpellType.ChillWind: return CHILLWIND_EXP;
            case MageSpellType.PoisonGas: return POISONGAS_EXP;
            case MageSpellType.Zap: return ZAP_EXP;
        }
    }


    public static string GetSpellDescription(SpellData data)
    {
        float percentageDamage = data.PercDamage * 100f;
        float radius = data.Radius;
        float percentageMoreDamageFromSpells = data.PercentageMoreDamageFromSpells * 100f;
        float percentageLifesteal = data.PercentageLifesteal * 100f;
        int bounces = data.Bounces;

        switch (data.SpellSO.SpellType)
        {
            default: return string.Empty;

            case MageSpellType.Fireball:
                return string.Format(data.SpellSO.SpellDesc, percentageDamage);

            case MageSpellType.Explosion:
                return string.Format(data.SpellSO.SpellDesc, percentageDamage, radius);

            case MageSpellType.ChillWind:
                return string.Format(data.SpellSO.SpellDesc, percentageDamage, radius, percentageMoreDamageFromSpells);

            case MageSpellType.PoisonGas:
                return string.Format(data.SpellSO.SpellDesc, percentageDamage, radius, percentageLifesteal);

            case MageSpellType.Zap:
                return string.Format(data.SpellSO.SpellDesc, percentageDamage, bounces);
        }
    }


    [System.Serializable]
    public struct SpellCombatData
    {
        // damage based on max hp enemy
        public float percDamage;
        public float percAddDamagePerLevel;

        // radius
        public float radius;
        public float percAddRadiusPerLevel;

        // chill wind
        public float percMoreDamageFromSpells;
        public float percAddMoreDamageFromSpellsPerLevel;

        // poison gas
        public float percLifesteal;
        public float percAddLifestealPerLevel;

        // zap
        public int maxBounces;
        public int addBouncesPerLevel;
    }
}
