using System;
using UnityEngine;

using static UtilsPlayer;
using static UtilsMiner;

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

    

    // ---- WEAPON MINER

    private int levelWeaponMiner;



    public int LevelStatPower => levelStatPower;
    public int LevelStatSmashSpeed => levelSmashSpeed;
    public int LevelStatShockwave => levelShockwave;
    public int LevelStatLuck => levelStatLuck;



    // ---- FINAL STAT VALUES

    public long ExpToNextLevel => RequiredExpForMinerLevel(CurrentLevel + 1);


    public float CurrentPower => basePower +
        PER_LEVEL_MINER_GAIN_POWER * (LevelStatPower >= PER_LEVEL_MINER_MAX_POWER ? LevelStatPower : LevelStatPower - 1) * 
        _inspirationModifier;

    public float CurrentSmashSpeed => baseSmashSpeed +
        PER_LEVEL_MINER_GAIN_SMASHSPEED * (LevelStatSmashSpeed >= PER_LEVEL_MINER_MAX_SMASHSPEED ? LevelStatSmashSpeed : LevelStatSmashSpeed - 1) * 
        _inspirationModifier;
    public float CurrentShockwave => baseShockwave +
        PER_LEVEL_MINER_GAIN_SHOCKWAVE * (LevelStatShockwave >= PER_LEVEL_MINER_MAX_SHOCKWAVE ? LevelStatShockwave : LevelStatShockwave - 1) +
        (_inspirationModifier - 1f);
    public float CurrentLuck => baseLuck +
        PER_LEVEL_MINER_GAIN_LUCK * (LevelStatLuck >= PER_LEVEL_MINER_MAX_LUCK ? LevelStatLuck : LevelStatLuck - 1) +
        (_inspirationModifier - 1f);

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


        levelStatPower = Math.Min(levelStatPower, PER_LEVEL_MINER_MAX_POWER);
        levelSmashSpeed = Math.Min(levelSmashSpeed, PER_LEVEL_MINER_MAX_SMASHSPEED);
        levelShockwave = Math.Min(levelShockwave, PER_LEVEL_MINER_MAX_SHOCKWAVE);
        levelStatLuck = Math.Min(levelStatLuck, PER_LEVEL_MINER_MAX_LUCK);


        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            levelStatPower + levelSmashSpeed + levelShockwave + levelStatLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= MAX_LEVEL_MINER)
        {
            AvailableStatPoints = MAX_LEVEL_MINER - 1 -
               levelStatPower - levelSmashSpeed - levelShockwave - levelStatLuck +
               START_LEVEL_POWER +
               START_LEVEL_SMASH_SPEED +
               START_LEVEL_SHOCKWAVE +
               START_LEVEL_LUCK;
            CurrentExp = 0;
        }

        // ---- WEAPON

        levelWeaponMiner = saveData.levelWeaponMiner;
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;

        levelStatPower = START_LEVEL_POWER;
        levelSmashSpeed = START_LEVEL_SMASH_SPEED;
        levelShockwave = START_LEVEL_SHOCKWAVE;
        levelStatLuck = START_LEVEL_LUCK;


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
            level => level >= MAX_LEVEL_MINER,
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

        CheckAllAchievements();
    }

    public void CheckAllAchievements()
    {
        if (levelWeaponMiner >= 10)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UPGRADE_SWORD_10))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UPGRADE_SWORD_10);
            }
        }
        else if (levelWeaponMiner >= 5)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UPGRADE_SWORD_5))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UPGRADE_SWORD_5);
            }
        }
    }
}
