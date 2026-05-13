using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerMinerData
{
    // ---- BASE STAT VALUES

    private float basePower;
    private float baseSmashSpeed;
    private float baseShockwave;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatPower = 1;
    private int levelSmashSpeed = 1;
    private int levelShockwave = 1;
    private int levelStatLuck = 1;

    private int startLevelPower = 1;
    private int startLevelSmashSpeed = 1;
    private int startLevelShockwave = 1;
    private int startLevelLuck = 1;

    // ---- WEAPON MINER

    private int levelWeaponMiner;



    public int LevelStatPower => levelStatPower;
    public int LevelStatSmashSpeed => levelSmashSpeed;
    public int LevelStatShockwave => levelShockwave;
    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private long currentExp;





    public int CurrentLevel => currentLevel;
    public long CurrentExp => currentExp;
    public long ExpToNextLevel => UtilsMiner.RequiredExpForMinerLevel(currentLevel + 1);


    public float CurrentPower => basePower + UtilsMiner.PER_LEVEL_MINER_GAIN_POWER * (levelStatPower - 1);
    public float CurrentSmashSpeed => baseSmashSpeed + UtilsMiner.PER_LEVEL_MINER_GAIN_SMASHSPEED * (levelSmashSpeed - 1);
    public float CurrentShockwave => baseShockwave + UtilsMiner.PER_LEVEL_MINER_GAIN_SHOCKWAVE * (levelShockwave - 1);
    public float CurrentLuck => baseLuck + UtilsMiner.PER_LEVEL_MINER_GAIN_LUCK * (levelStatLuck - 1);

    public int WeaponLevel => levelWeaponMiner;



    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public PlayerMinerData()
    {
        GenerateBaseStats();
    }
    
    public PlayerMinerData(PlayerMinerSaveData saveData)
    {
        GenerateBaseStats();

        levelStatPower = saveData.levelStatPower;
        levelSmashSpeed = saveData.levelStatSmashSpeed;
        levelShockwave = saveData.levelStatShockwave;
        levelStatLuck = saveData.levelStatLuck;


        levelStatPower = Math.Min(levelStatPower, UtilsMiner.PER_LEVEL_MINER_MAX_POWER);
        levelSmashSpeed = Math.Min(levelSmashSpeed, UtilsMiner.PER_LEVEL_MINER_MAX_SMASHSPEED);
        levelShockwave = Math.Min(levelShockwave, UtilsMiner.PER_LEVEL_MINER_MAX_SHOCKWAVE);
        levelStatLuck = Math.Min(levelStatLuck, UtilsMiner.PER_LEVEL_MINER_MAX_LUCK);


        availableStatPoints = saveData.availableStatPoints;

        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        int sumLevels =
            levelStatPower + levelSmashSpeed + levelShockwave + levelStatLuck -
            startLevelPower - startLevelSmashSpeed - startLevelShockwave - startLevelLuck +
            availableStatPoints +
            1;

        currentLevel = Math.Min(currentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (currentLevel > UtilsMiner.MAX_LEVEL_MINER)
        {
            availableStatPoints = 0;
            currentExp = 0;
        }

        // ---- WEAPON

        levelWeaponMiner = saveData.levelWeaponMiner;
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        levelStatPower = startLevelPower;
        levelSmashSpeed = startLevelSmashSpeed;
        levelShockwave = startLevelShockwave;
        levelStatLuck = startLevelLuck;


        basePower = 10;

        baseSmashSpeed = 1f;
        baseShockwave = 0f;

        baseLuck = 0f;

        // ---- WEAPON

        levelWeaponMiner = 1;
    }

    public void AddStatPoints(int amount)
    {
        availableStatPoints += amount;
    }

    public void RemoveStatPoints(int amount)
    {
        availableStatPoints -= amount;
    }

    public void AddExp(long amount)
    {
        // check max level
        if (currentLevel > UtilsMiner.MAX_LEVEL_MINER)
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

        //Debug.Log("current exp: " + currentExp);
        //Debug.Log("next level exp: " + ExpToNextLevel);

        OnAddedExp?.Invoke();
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case ID_MINER_POWER: levelStatPower += amount; break;
            case ID_MINER_SMASHSPEED: levelSmashSpeed += amount; break;
            case ID_MINER_SHOCKWAVE: levelShockwave += amount; break;
            case ID_MINER_LUCK: levelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }

    public void AddMinerWeaponLevel(int level)
    {
        levelWeaponMiner += level;
    }
}
