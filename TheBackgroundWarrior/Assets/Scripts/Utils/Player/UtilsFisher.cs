using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsFisher
{
    // Max hp, Atk, Def, Atk Spd, Crit rate, Crit dmg, Luck, Exp gain, Move spd warrior
    public enum FishGroupType { Life, Predator, Guardian, Dart, Sharp, Piercing, Golden, Elder, Quick }

    public enum BaitEffectivness { Normal, Great, Max }

    // Fisher
    private static Dictionary<int, ListableGameDataSO> dictFishGroups;


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



    // -------------------- FISH GROUPS -----------------------
    public const float FISHER_LIFE_SERIES_COMPLETE_MULTIPLIER = 2f;         // max hp
    public const float FISHER_PREDATOR_SERIES_COMPLETE_MULTIPLIER = 1.5f;   // atk
    public const float FISHER_GUARDIAN_SERIES_COMPLETE_MULTIPLIER = 1.3f;   // def
    public const float FISHER_DART_SERIES_COMPLETE_MULTIPLIER = 1.2f;       // atk spd
    public const float FISHER_SHARP_SERIES_COMPLETE_MULTIPLIER = 1.2f;      // crit rate
    public const float FISHER_PIERCING_SERIES_COMPLETE_MULTIPLIER = 1.2f;   // crit dmg
    public const float FISHER_GOLDEN_SERIES_COMPLETE_MULTIPLIER = 1.1f;     // luck
    public const float FISHER_ELDER_SERIES_COMPLETE_MULTIPLIER = 1.2f;      // exp mult
    public const float FISHER_QUICK_SERIES_COMPLETE_MULTIPLIER = 1.2f;      // move spd



    private static PlayerJobFisherSO jobDataSO;




    public const long PASSIVE_EXP = 50;
    public const long UNCAUGHT_EXP = 400;



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
           PER_LEVEL_FISHER_MAX_LUCK +
           1;


        BASE_FISHER_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_FISHER_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_FISHER_EXP_GROWTH = jobDataSO.FlatExpGrowth;


        LoadDictFishGroups();
    }



    public static long RequiredExpForFisherLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_FISHER_EXP_GROWTH * Mathf.Pow(level, EXPO_FISHER_EXP_GROWTH) + FLAT_FISHER_EXP_GROWTH * level);
    }


    private static void LoadDictFishGroups()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Fisher/ContainerGameData_FishGroups");
        dictFishGroups = container.Entries.ToDictionary(e => e.Id);
    }


    public static FishGroupSO[] GetAllFishGroups()
    {
        return dictFishGroups.Values.OfType<FishGroupSO>().ToArray();
    }

    public static FishGroupSO GetFishGroupByType(FishGroupType type)
    {
        return UtilsGeneral.GetGameDataSO<FishGroupSO>((int)type, dictFishGroups);
    }

    public static FishGroupSO GetFishGroupByFish(FishSO fish)
    {
        return GetByPredicate<FishGroupSO>(group => group.Fishes.Contains(fish), dictFishGroups);
    }

    public static T GetByPredicate<T>(Func<T, bool> predicate, Dictionary<int, ListableGameDataSO> dict) where T : ListableGameDataSO
    {
        return dict.Values
            .OfType<T>()
            .FirstOrDefault(predicate);
    }
}
