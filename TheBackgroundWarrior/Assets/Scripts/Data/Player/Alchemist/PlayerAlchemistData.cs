using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAlchemistData : BasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseRoutine;
    private float baseYield;
    private float baseResearch;
    private float baseStability;

    // ---- LEVEL STAT POINTS


    private int startLevelRoutine = 1;
    private int startLevelYield = 1;
    private int startLevelResearch = 0;
    private int startLevelStability = 0;


    public int LevelStatRoutine { get; private set; }
    public int LevelStatYield { get; private set; }
    public int LevelStatResearch { get; private set; }
    public int LevelStatStability { get; private set; }


    // ---- POINTS


    // ---- FINAL STAT VALUES
    public long ExpToNextLevel => UtilsAlchemist.RequiredExpForAlchemistLevel(CurrentLevel + 1);


    public float CurrentRoutine => baseRoutine + UtilsAlchemist.PER_LEVEL_ALCHEMIST_GAIN_ROUTINE * (LevelStatRoutine - 1) + (_inspirationModifier - 1f);
    public float CurrentYield => baseYield + UtilsAlchemist.PER_LEVEL_ALCHEMIST_GAIN_YIELD * (LevelStatYield - 1);
    public float CurrentResearch => baseResearch + UtilsAlchemist.PER_LEVEL_ALCHEMIST_GAIN_RESEARCH * LevelStatResearch;
    public float CurrentStability => baseStability + UtilsAlchemist.PER_LEVEL_ALCHEMIST_GAIN_STABILITY * (LevelStatStability - 1) + (_inspirationModifier - 1f);



    // ---- RECIPES

    public RecipeSO CurrentCraftingRecipe { get; private set; }
    public bool IsInfiniteCrafting { get; private set; }
    public int CurrentCraftingQuantity { get; private set; }


    public List<RecipeSO> AvailableRecipes { get; private set; }


    // ---- PERMA STATS USED

    public int StatPermaMaxHpCounter { get; private set; }
    public int StatPermaAttackCounter { get; private set; }
    public int StatPermaDefenseCounter { get; private set; }


    public PlayerAlchemistData()
    {
        GenerateBaseStats();
    }

    public PlayerAlchemistData(PlayerAlchemistSaveData saveData)
    {
        GenerateBaseStats();

        LevelStatRoutine = saveData.levelStatRoutine;
        LevelStatYield = saveData.levelStatYield;
        LevelStatResearch = saveData.levelStatResearch;
        LevelStatStability = saveData.levelStatStability;

        LevelStatRoutine = Math.Min(LevelStatRoutine, UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_ROUTINE);
        LevelStatYield = Math.Min(LevelStatYield, UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_YIELD);
        LevelStatResearch = Math.Min(LevelStatResearch, UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_RESEARCH);
        LevelStatStability = Math.Min(LevelStatStability, UtilsAlchemist.PER_LEVEL_ALCHEMIST_MAX_STABILITY);

        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatRoutine + LevelStatYield + LevelStatResearch + LevelStatStability +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= UtilsAlchemist.MAX_LEVEL_ALCHEMIST)
        {
            AvailableStatPoints = UtilsAlchemist.MAX_LEVEL_ALCHEMIST - 1 -
               LevelStatRoutine - LevelStatYield - LevelStatResearch - LevelStatStability;
            CurrentExp = 0;
        }

        // get active recipe
        if (saveData.currentCraftingRecipe >= 0)
            CurrentCraftingRecipe = UtilsAlchemist.GetRecipeById(saveData.currentCraftingRecipe);
        else
            CurrentCraftingRecipe = null;

        IsInfiniteCrafting = saveData.isInfiniteCrafting;
        CurrentCraftingQuantity = saveData.currentCraftingQuantity;

        // load recipes
        AvailableRecipes = saveData.recipes.Select(recipe => UtilsAlchemist.GetRecipeById(recipe)).ToList();


        StatPermaMaxHpCounter = saveData.statPermaMaxHpCounter;
        StatPermaAttackCounter = saveData.statPermaAttackCounter;
        StatPermaDefenseCounter = saveData.statPermaDefenseCounter;
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;


        LevelStatRoutine = startLevelRoutine;
        LevelStatYield = startLevelYield;
        LevelStatResearch = startLevelResearch;
        LevelStatStability = startLevelStability;


        // multiplier
        baseRoutine = 0f; // increase craft speed, up to 15%
        baseYield = 0f; // craft extra materials chance, up to 20%
        baseResearch = 0f; // unlocks new recipe, check on whole values
        baseStability = 0.5f; // reduce failed crafts

        CurrentCraftingRecipe = null;
        IsInfiniteCrafting = false;
        CurrentCraftingQuantity = 0;

        // by default has first recipe when unlocked
        AvailableRecipes = new List<RecipeSO>
        {
            UtilsAlchemist.GetRecipeById(0)
        };


        StatPermaMaxHpCounter = 0;
        StatPermaAttackCounter = 0;
        StatPermaDefenseCounter = 0;
    }

    public void AddExp(long amount)
    {
        Debug.Log("current level:" + CurrentLevel + ", required exp: " + ExpToNextLevel);
        base.AddExp(
            amount,
            level => level >= UtilsAlchemist.MAX_LEVEL_ALCHEMIST,
            () => ExpToNextLevel
        );

        Debug.Log("added exp: " + amount + ", required after added exp: " + ExpToNextLevel);
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_ALCHEMIST_ROUTINE: LevelStatRoutine += amount; break;
            case UtilsPlayer.ID_ALCHEMIST_YIELD: LevelStatYield += amount; break;
            case UtilsPlayer.ID_ALCHEMIST_RESEARCH:
                LevelStatResearch += amount;
                int maxIndex = (int)CurrentResearch + 1;
                for (int i = 0; i < maxIndex; i++)
                {
                    // check list of research and add recipe if not available yet
                    var recipe = UtilsAlchemist.GetRecipeFromResearch(i);
                    if (!IsRecipeAvailable(recipe))
                        AddAvailableRecipe(recipe);
                }
                break;
            case UtilsPlayer.ID_ALCHEMIST_STABILITY: LevelStatStability += amount; break;
        }

        InvokeStatChange(id, amount);
    }

    public void AddAvailableRecipe(int id)
    {
        AvailableRecipes.Add(UtilsAlchemist.GetRecipeById(id));
    }

    public void AddAvailableRecipe(RecipeSO recipe)
    {
        AvailableRecipes.Add(recipe);
    }

    public bool IsRecipeAvailable(RecipeSO recipe)
    {
        return AvailableRecipes.Where(r => r.Id == recipe.Id).Any();
    }

    public void AddPermaStatCounter(UtilsAlchemist.PermaStat stat)
    {
        // TODO: add some kind of event to trigger increase stats while in warrior scene, and also for quests maybe
        switch (stat)
        {
            default: Debug.Log("Wrong type of perma stat tried to add"); break;
            case UtilsAlchemist.PermaStat.MaxHp: StatPermaMaxHpCounter++; break;
            case UtilsAlchemist.PermaStat.Attack: StatPermaMaxHpCounter++; break;
            case UtilsAlchemist.PermaStat.Defense: StatPermaMaxHpCounter++; break;
        }
    }

    // ------------ CRAFTING --------------

    public void SetCraftingRecipe(RecipeSO recipe)
    {
        CurrentCraftingRecipe = recipe;
    }

    public void SetInfiniteCrafting(bool infinite)
    {
        IsInfiniteCrafting = infinite;
    }

    public void SetCurrentCraftingQuantity(int max)
    {
        CurrentCraftingQuantity = max;
    }
}
