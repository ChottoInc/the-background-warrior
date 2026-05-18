using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerBlacksmithData : IBasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseCraftSpeed;
    private float baseEfficiency;
    private float baseLuck;
    private float baseMetallurgy;


    // ---- LEVEL STAT POINTS

    private int levelStatCraftSpeed;
    private int levelEfficiency;
    private int levelStatLuck;
    private int levelStatMetallurgy;

    private readonly int startLevelCraftSpeed = 1;
    private readonly int startLevelEfficiency = 1;
    private readonly int startLevelStatLuck = 1;
    private readonly int startLevelMetallurgy = 0;

    public int LevelStatCraftSpeed => levelStatCraftSpeed;
    public int LevelEfficiency => levelEfficiency;
    public int LevelStatLuck => levelStatLuck;
    public int LevelStatMetallurgy => levelStatMetallurgy;


    // ---- GEARS BLACKSMITH

    private int levelHelmetBlacksmith;

    private int levelArmorBlacksmith;

    private int levelGlovesBlacksmith;

    private int levelBootsBlacksmith;


    public int HelmetLevel => levelHelmetBlacksmith;
    public int ArmorLevel => levelArmorBlacksmith;
    public int GlovesLevel => levelGlovesBlacksmith;
    public int BootsLevel => levelBootsBlacksmith;




    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private long currentExp;


    // ---- FORGING VARIABLES

    private int currentForgingOre;
    private bool isInfiniteForging;
    private int currentForgingQuantity;


    public int CurrentForgingOre => currentForgingOre;
    public bool IsInfiniteForging => isInfiniteForging;
    public int CurrentForgingQuantity => currentForgingQuantity;





    public int CurrentLevel => currentLevel;
    public long CurrentExp => currentExp;
    public long ExpToNextLevel => UtilsBlacksmith.RequiredExpForBlacksmithLevel(currentLevel + 1);


    public float CurrentCraftSpeed => baseCraftSpeed + UtilsBlacksmith.PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED * (levelStatCraftSpeed - 1);
    public float CurrentEfficiency => baseEfficiency + UtilsBlacksmith.PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY * (levelEfficiency - 1);
    public float CurrentLuck => baseLuck + UtilsBlacksmith.PER_LEVEL_BLACKSMITH_GAIN_LUCK * (levelStatLuck - 1);
    public float CurrentMetallurgy => baseMetallurgy + UtilsBlacksmith.PER_LEVEL_BLACKSMITH_GAIN_METALLURGY * (levelStatMetallurgy);

    public float CurrentCraftTime => 60f / CurrentCraftSpeed;



    



    



    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public PlayerBlacksmithData()
    {
        GenerateBaseStats();
    }

    public PlayerBlacksmithData(PlayerBlacksmithSaveData saveData)
    {
        GenerateBaseStats();

        levelStatCraftSpeed = saveData.levelStatCraftSpeed;
        levelEfficiency = saveData.levelStatEfficiency;
        levelStatLuck = saveData.levelStatLuck;
        levelStatMetallurgy = saveData.levelStatMetallurgy;

        levelStatCraftSpeed = Math.Min(levelStatCraftSpeed, UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED);
        levelEfficiency = Math.Min(levelEfficiency, UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY);
        levelStatLuck = Math.Min(levelStatLuck, UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_LUCK);
        levelStatMetallurgy = Math.Min(levelStatMetallurgy, UtilsBlacksmith.PER_LEVEL_BLACKSMITH_MAX_METALLURGY);


        availableStatPoints = saveData.availableStatPoints;

        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        int sumLevels = 
            levelStatCraftSpeed + levelEfficiency + levelStatLuck + levelStatMetallurgy -
            startLevelCraftSpeed - startLevelEfficiency - startLevelStatLuck - startLevelMetallurgy +
            availableStatPoints +
            1;

        currentLevel = Math.Min(currentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if(currentLevel > UtilsBlacksmith.MAX_LEVEL_BLACKSMITH)
        {
            availableStatPoints = 0;
            currentExp = 0;
        }

        // ---- WEAPON

        levelHelmetBlacksmith = saveData.levelHelmetBlacksmith;
        levelArmorBlacksmith = saveData.levelArmorBlacksmith;
        levelGlovesBlacksmith = saveData.levelGlovesBlacksmith;
        levelBootsBlacksmith = saveData.levelBootsBlacksmith;


        currentForgingOre = saveData.currentForgingOre;
        isInfiniteForging = saveData.isInfiniteForging;
        currentForgingQuantity = saveData.currentForgingQuantity;
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        levelStatCraftSpeed = startLevelCraftSpeed;
        levelEfficiency = startLevelEfficiency;
        levelStatLuck = startLevelStatLuck;
        levelStatMetallurgy = startLevelMetallurgy;


        baseCraftSpeed = 1f;
        baseEfficiency = 0f;
        baseLuck = 0f;
        baseMetallurgy = 0f; // multiplier extra materials, check on whole values

        // ---- EQUIPMENT

        levelHelmetBlacksmith = 1;
        levelArmorBlacksmith = 1;
        levelGlovesBlacksmith = 1;
        levelBootsBlacksmith = 1;

        // ---- FORGING

        currentForgingOre = -1;
        isInfiniteForging = false;
        currentForgingQuantity = 0;
    }

    public void AddStatPoints(int amount)
    {
        availableStatPoints += amount;
    }

    public void RemoveStatPoints(int amount)
    {
        availableStatPoints -= amount;
    }

    public void AddLevel(int amount)
    {
        if (currentLevel + amount > UtilsBlacksmith.MAX_LEVEL_BLACKSMITH)
        {
            amount = UtilsBlacksmith.MAX_LEVEL_BLACKSMITH - currentLevel;
        }
        currentLevel += amount;
        availableStatPoints += amount;
    }

    public void AddExp(long amount)
    {
        // check max level
        if (currentLevel > UtilsBlacksmith.MAX_LEVEL_BLACKSMITH)
        {
            // set current exp to 0
            currentExp = 0;
            return;
        }

        currentExp += amount;

        // looping for every level gained
        while (currentExp >= ExpToNextLevel)
        {
            // recalculate current exp
            currentExp -= ExpToNextLevel;

            // give level and stat point
            currentLevel++;
            AddStatPoints(1);

            OnLevelUp?.Invoke();
        }

        OnAddedExp?.Invoke();
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case ID_BLACKSMITH_CRAFTSPEED: levelStatCraftSpeed += amount; break;
            case ID_BLACKSMITH_EFFICIENCY: levelEfficiency += amount; break;
            case ID_BLACKSMITH_LUCK: levelStatLuck += amount; break;
            case ID_BLACKSMITH_METALLURGY: levelStatMetallurgy += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }

    public void AddBlacksmithHelmetLevel(int level)
    {
        levelHelmetBlacksmith += level;
    }

    public void AddBlacksmithArmorLevel(int level)
    {
        levelArmorBlacksmith += level;
    }

    public void AddBlacksmithGlovesLevel(int level)
    {
        levelGlovesBlacksmith += level;
    }

    public void AddBlacksmithBootsLevel(int level)
    {
        levelBootsBlacksmith += level;
    }

    public void SetForgingOre(int id)
    {
        currentForgingOre = id;
    }

    public void SetInfiniteForging(bool infinite)
    {
        isInfiniteForging = infinite;
    }

    public void SetCurrentForgingQuantity(int max)
    {
        currentForgingQuantity = max;
    }
}
