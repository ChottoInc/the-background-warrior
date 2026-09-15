using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerMinerData : BasePlayerData
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



    // ---- FINAL STAT VALUES

    public long ExpToNextLevel => UtilsMiner.RequiredExpForMinerLevel(CurrentLevel + 1);


    public float CurrentPower => basePower + UtilsMiner.PER_LEVEL_MINER_GAIN_POWER * (levelStatPower - 1) * _inspirationModifier;
    public float CurrentSmashSpeed => baseSmashSpeed + UtilsMiner.PER_LEVEL_MINER_GAIN_SMASHSPEED * (levelSmashSpeed - 1) * _inspirationModifier;
    public float CurrentShockwave => baseShockwave + UtilsMiner.PER_LEVEL_MINER_GAIN_SHOCKWAVE * (levelShockwave - 1) + (_inspirationModifier - 1f);
    public float CurrentLuck => baseLuck + UtilsMiner.PER_LEVEL_MINER_GAIN_LUCK * (levelStatLuck - 1) + (_inspirationModifier - 1f);

    public int WeaponLevel => levelWeaponMiner;



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


        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            levelStatPower + levelSmashSpeed + levelShockwave + levelStatLuck +
            //startLevelPower + startLevelSmashSpeed + startLevelShockwave + startLevelLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= UtilsMiner.MAX_LEVEL_MINER)
        {
            AvailableStatPoints = UtilsMiner.MAX_LEVEL_MINER - 1 -
               levelStatPower - levelSmashSpeed - levelShockwave - levelStatLuck;
            CurrentExp = 0;
        }

        // ---- WEAPON

        levelWeaponMiner = saveData.levelWeaponMiner;
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;

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

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= UtilsMiner.MAX_LEVEL_MINER,
            () => ExpToNextLevel
        );
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

        InvokeStatChange(id, amount);
    }

    public void AddMinerWeaponLevel(int level)
    {
        levelWeaponMiner += level;
    }
}
