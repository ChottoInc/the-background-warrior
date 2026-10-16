using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsAlchemist
{
    public enum PermaStat { MaxHp, Attack, Defense }

    private static Dictionary<int, ListableGameDataSO> _dictRecipes;
    private static Dictionary<int, ListableGameDataSO> _dictResearch;

    // ---------------- START LEVELS --------------- //

    public const int START_LEVEL_ROUTINE = 1;
    public const int START_LEVEL_YIELD = 1;
    public const int START_LEVEL_RESEARCH = 0;
    public const int START_LEVEL_STABILITY = 0;



    public static float PER_LEVEL_ALCHEMIST_GAIN_ROUTINE = 0.01f;       // up to 15
    public static float PER_LEVEL_ALCHEMIST_GAIN_YIELD = 0.01f;         // up to 20
    public static float PER_LEVEL_ALCHEMIST_GAIN_RESEARCH = 0.2f;       // every 5 levels new recipe      
    public static float PER_LEVEL_ALCHEMIST_GAIN_STABILITY = 0.01f;     // up to 30, base 0.5

    public static int PER_LEVEL_ALCHEMIST_MAX_ROUTINE = 15;
    public static int PER_LEVEL_ALCHEMIST_MAX_YIELD = 20;
    public static int PER_LEVEL_ALCHEMIST_MAX_RESEARCH = 20;           // 
    public static int PER_LEVEL_ALCHEMIST_MAX_STABILITY = 30;          //


    public static int MAX_LEVEL_ALCHEMIST;



    private static float BASE_ALCHEMIST_EXP_GROWTH = 50f;
    private static float EXPO_ALCHEMIST_EXP_GROWTH = 1.08f;
    private static float FLAT_ALCHEMIST_EXP_GROWTH = 10f;



    private static PlayerJobAlchemistSO jobDataSO;






    public static void Initialize()
    {
        jobDataSO = UtilsPlayer.GetJobFromDatabase(UtilsPlayer.PlayerJob.Alchemist) as PlayerJobAlchemistSO;

        PER_LEVEL_ALCHEMIST_GAIN_ROUTINE = jobDataSO.PerLevelGainRoutine;
        PER_LEVEL_ALCHEMIST_GAIN_YIELD = jobDataSO.PerLevelGainYield;
        PER_LEVEL_ALCHEMIST_GAIN_RESEARCH = jobDataSO.PerLevelGainResearch;
        PER_LEVEL_ALCHEMIST_GAIN_STABILITY = jobDataSO.PerLevelGainStability;

        PER_LEVEL_ALCHEMIST_MAX_ROUTINE = jobDataSO.MaxLevelRoutine;
        PER_LEVEL_ALCHEMIST_MAX_YIELD = jobDataSO.MaxLevelYield;
        PER_LEVEL_ALCHEMIST_MAX_RESEARCH = jobDataSO.MaxLevelResearch;
        PER_LEVEL_ALCHEMIST_MAX_STABILITY = jobDataSO.MaxLevelStability;


        MAX_LEVEL_ALCHEMIST =
            1 +
            PER_LEVEL_ALCHEMIST_MAX_ROUTINE +
            PER_LEVEL_ALCHEMIST_MAX_YIELD +
            PER_LEVEL_ALCHEMIST_MAX_RESEARCH +
            PER_LEVEL_ALCHEMIST_MAX_STABILITY -
            (START_LEVEL_ROUTINE + START_LEVEL_YIELD + START_LEVEL_RESEARCH + START_LEVEL_STABILITY);


        BASE_ALCHEMIST_EXP_GROWTH = jobDataSO.BaseExpGrowth;
        EXPO_ALCHEMIST_EXP_GROWTH = jobDataSO.ExpoExpGrowth;
        FLAT_ALCHEMIST_EXP_GROWTH = jobDataSO.FlatExpGrowth;


        LoadDictRecipes();
        LoadDictResearch();
    }



    public static long RequiredExpForAlchemistLevel(int level)
    {
        // Level starts at 1
        if (level <= 1) return 0;

        // Formula: baseExp * (growthRate^(level-1) - 1)
        return (long)(BASE_ALCHEMIST_EXP_GROWTH * Mathf.Pow(level, EXPO_ALCHEMIST_EXP_GROWTH) + FLAT_ALCHEMIST_EXP_GROWTH * level);
    }


    private static void LoadDictRecipes()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Alchemist/ContainerGameData_Recipes");
        _dictRecipes = container.Entries.ToDictionary(e => e.Id);
    }

    public static List<RecipeSO> GetAllRecipes()
    {
        return _dictRecipes.Values.OfType<RecipeSO>().ToList();
    }

    public static RecipeSO GetRecipeById(int id)
    {
        return UtilsGeneral.GetGameDataSO<RecipeSO>(id, _dictRecipes);
    }



    private static void LoadDictResearch()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/Alchemist/ContainerGameData_Research");
        _dictResearch = container.Entries.ToDictionary(e => e.Id);
    }

    public static RecipeSO GetRecipeFromResearch(int id)
    {
        return UtilsGeneral.GetGameDataSO<ResearchItemSO>(id, _dictResearch).UnlocksRecipe;
    }



    public static int GetPossibleQuantity(RecipeSO recipe, Inventory inventory)
    {
        int maxCraftable = int.MaxValue;

        foreach (var ingredient in recipe.Ingredients)
        {
            int ownedAmount = inventory.GetItemQuantity(ingredient.item.Id);
            int possibleCrafts = ownedAmount / ingredient.quantity;

            maxCraftable = Mathf.Min(maxCraftable, possibleCrafts);
        }

        return maxCraftable == int.MaxValue ? 0 : maxCraftable;
    }
}
