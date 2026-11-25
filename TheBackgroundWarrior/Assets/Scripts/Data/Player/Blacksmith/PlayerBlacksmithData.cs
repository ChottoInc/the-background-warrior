using System;
using UnityEngine;

using static UtilsPlayer;
using static UtilsBlacksmith;

public class PlayerBlacksmithData : BasePlayerData
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




    // ---- FORGING VARIABLES

    private int currentForgingOre;
    private bool isInfiniteForging;
    private int currentForgingQuantity;


    public int CurrentForgingOre => currentForgingOre;
    public bool IsInfiniteForging => isInfiniteForging;
    public int CurrentForgingQuantity => currentForgingQuantity;


    public long ExpToNextLevel => RequiredExpForBlacksmithLevel(CurrentLevel + 1);


    public float CurrentCraftSpeed => baseCraftSpeed +
        PER_LEVEL_BLACKSMITH_GAIN_CRAFTSPEED * (LevelStatCraftSpeed >= PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED ? LevelStatCraftSpeed : LevelStatCraftSpeed - 1) * 
        _inspirationModifier;

    public float CurrentEfficiency => baseEfficiency +
        PER_LEVEL_BLACKSMITH_GAIN_EFFICIENCY * (LevelEfficiency >= PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY ? LevelEfficiency : LevelEfficiency - 1) +
        (_inspirationModifier - 1f);

    public float CurrentLuck => baseLuck +
        PER_LEVEL_BLACKSMITH_GAIN_LUCK * (LevelStatLuck >= PER_LEVEL_BLACKSMITH_MAX_LUCK ? LevelStatLuck : LevelStatLuck - 1) +
        (_inspirationModifier - 1f);

    public float CurrentMetallurgy => baseMetallurgy + PER_LEVEL_BLACKSMITH_GAIN_METALLURGY * (levelStatMetallurgy);

    public float CurrentCraftTime => 60f / CurrentCraftSpeed;



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

        levelStatCraftSpeed = Math.Min(levelStatCraftSpeed, PER_LEVEL_BLACKSMITH_MAX_CRAFTSPEED);
        levelEfficiency = Math.Min(levelEfficiency, PER_LEVEL_BLACKSMITH_MAX_EFFICIENCY);
        levelStatLuck = Math.Min(levelStatLuck, PER_LEVEL_BLACKSMITH_MAX_LUCK);
        levelStatMetallurgy = Math.Min(levelStatMetallurgy, PER_LEVEL_BLACKSMITH_MAX_METALLURGY);


        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels = 
            levelStatCraftSpeed + levelEfficiency + levelStatLuck + levelStatMetallurgy +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if(CurrentLevel >= MAX_LEVEL_BLACKSMITH)
        {
            AvailableStatPoints = MAX_LEVEL_BLACKSMITH - 1 -
                levelStatCraftSpeed - levelEfficiency - levelStatLuck - levelStatMetallurgy +
                START_LEVEL_CRAFT_SPEED +
                START_LEVEL_EFFICIENCY +
                START_LEVEL_LUCK +
                START_LEVEL_METALLURGY;
            CurrentExp = 0;
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
        CurrentLevel = 1;
        CurrentExp = 0;

        levelStatCraftSpeed = START_LEVEL_CRAFT_SPEED;
        levelEfficiency = START_LEVEL_EFFICIENCY;
        levelStatLuck = START_LEVEL_LUCK;
        levelStatMetallurgy = START_LEVEL_METALLURGY;

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

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= MAX_LEVEL_BLACKSMITH,
            () => ExpToNextLevel
        );
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

        InvokeStatChange(id, amount);
    }

    public void AddBlacksmithHelmetLevel(int level)
    {
        levelHelmetBlacksmith += level;
        CheckAllAchievements();
    }

    public void AddBlacksmithArmorLevel(int level)
    {
        levelArmorBlacksmith += level;
        CheckAllAchievements();
    }

    public void AddBlacksmithGlovesLevel(int level)
    {
        levelGlovesBlacksmith += level;
        CheckAllAchievements();
    }

    public void AddBlacksmithBootsLevel(int level)
    {
        levelBootsBlacksmith += level;
        CheckAllAchievements();
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


    public void CheckAllAchievements()
    {
        if (levelHelmetBlacksmith >= 10 &&
            levelArmorBlacksmith >= 10 &&
            levelGlovesBlacksmith >= 10 &&
            levelBootsBlacksmith >= 10)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UPGRADE_GEARS_10))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UPGRADE_GEARS_10);
            }
        }
        else if (levelHelmetBlacksmith >= 5 &&
            levelArmorBlacksmith >= 5 &&
            levelGlovesBlacksmith >= 5 &&
            levelBootsBlacksmith >= 5)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UPGRADE_GEARS_5))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UPGRADE_GEARS_5);
            }
        }
    }
}
