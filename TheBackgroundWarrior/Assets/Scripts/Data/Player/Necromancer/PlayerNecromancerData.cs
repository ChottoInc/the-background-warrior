using System;
using UnityEngine;

using static UtilsNecromancer;

public class PlayerNecromancerData : BasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseAptitude;
    private float baseSummon;
    private float baseMight;
    private float baseLifespan;
    private float baseHorde;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    public int LevelStatAptitude { get; private set; }
    public int LevelStatSummon { get; private set; }
    public int LevelStatMight { get; private set; }
    public int LevelStatLifespan { get; private set; }
    public int LevelStatHorde { get; private set; }
    public int LevelStatLuck { get; private set; }



    // ---- FINAL STAT VALUES
    public long ExpToNextLevel => RequiredExpForNecromancerLevel(CurrentLevel + 1);


    public float CurrentAptitude => baseAptitude + PER_LEVEL_NECROMANCER_GAIN_APTITUDE * LevelStatAptitude;
    public float CurrentSummon => baseSummon + PER_LEVEL_NECROMANCER_GAIN_SUMMON * (LevelStatSummon >= PER_LEVEL_NECROMANCER_MAX_SUMMON ? LevelStatSummon : LevelStatSummon - 1);
    public float CurrentMight => baseMight +
        PER_LEVEL_NECROMANCER_GAIN_MIGHT * (LevelStatMight >= PER_LEVEL_NECROMANCER_MAX_MIGHT ? LevelStatMight : LevelStatMight - 1) +
        (_inspirationModifier - 1f);

    public float CurrentLifespan => baseLifespan +
        PER_LEVEL_NECROMANCER_GAIN_LIFESPAN * (LevelStatLifespan >= PER_LEVEL_NECROMANCER_MAX_LIFESPAN ? LevelStatLifespan : LevelStatLifespan - 1) +
        (_inspirationModifier - 1f);

    public float CurrentHorde => baseHorde + PER_LEVEL_NECROMANCER_GAIN_HORDE * LevelStatHorde;
    public float CurrentLuck => baseLuck +
        PER_LEVEL_NECROMANCER_GAIN_LUCK * (LevelStatLuck >= PER_LEVEL_NECROMANCER_MAX_LUCK ? LevelStatLuck : LevelStatLuck - 1) +
        (_inspirationModifier - 1f);


    public bool IsAriseRitualUnlocked =>
        LevelStatSummon >= PER_LEVEL_NECROMANCER_MAX_SUMMON &&
        LevelStatHorde >= PER_LEVEL_NECROMANCER_MAX_HORDE;

    public bool IsAfterlifeRitualUnlocked =>
        LevelStatAptitude >= PER_LEVEL_NECROMANCER_MAX_APTITUDE &&
        LevelStatMight >= PER_LEVEL_NECROMANCER_MAX_MIGHT;

    public bool IsAdeRitualUnlocked =>
        LevelStatLifespan >= PER_LEVEL_NECROMANCER_MAX_LIFESPAN &&
        LevelStatLuck >= PER_LEVEL_NECROMANCER_MAX_LUCK;


    public PlayerNecromancerData()
    {
        GenerateBaseStats();
    }

    public PlayerNecromancerData(PlayerNecromancerSaveData saveData)
    {
        GenerateBaseStats();

        LevelStatAptitude = saveData.levelStatAptitude;
        LevelStatSummon = saveData.levelStatSummon;
        LevelStatMight = saveData.levelStatMight;
        LevelStatLifespan = saveData.levelStatLifespan;
        LevelStatHorde = saveData.levelStatHorde;
        LevelStatLuck = saveData.levelStatLuck;

        LevelStatAptitude = Math.Min(LevelStatAptitude, PER_LEVEL_NECROMANCER_MAX_APTITUDE);
        LevelStatSummon = Math.Min(LevelStatSummon, PER_LEVEL_NECROMANCER_MAX_SUMMON);
        LevelStatMight = Math.Min(LevelStatMight, PER_LEVEL_NECROMANCER_MAX_MIGHT);
        LevelStatLifespan = Math.Min(LevelStatLifespan, PER_LEVEL_NECROMANCER_MAX_LIFESPAN);
        LevelStatHorde = Math.Min(LevelStatHorde, PER_LEVEL_NECROMANCER_MAX_HORDE);
        LevelStatLuck = Math.Min(LevelStatLuck, PER_LEVEL_NECROMANCER_MAX_LUCK);

        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatAptitude + LevelStatSummon + LevelStatMight + LevelStatLifespan + LevelStatHorde + LevelStatLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= MAX_LEVEL_NECROMANCER)
        {
            AvailableStatPoints = MAX_LEVEL_NECROMANCER - 1 -
               LevelStatAptitude - LevelStatSummon - LevelStatMight - LevelStatLifespan - LevelStatHorde - LevelStatLuck +
               START_LEVEL_APTITUDE +
               START_LEVEL_SUMMON +
               START_LEVEL_MIGHT +
               START_LEVEL_LIFESPAN +
               START_LEVEL_HORDE +
               START_LEVEL_LUCK;
            CurrentExp = 0;
        }
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;


        LevelStatAptitude = START_LEVEL_APTITUDE;
        LevelStatSummon = START_LEVEL_SUMMON;
        LevelStatMight = START_LEVEL_MIGHT;
        LevelStatLifespan = START_LEVEL_LIFESPAN;
        LevelStatHorde = START_LEVEL_HORDE;
        LevelStatLuck = START_LEVEL_LUCK;


        // multiplier
        baseAptitude = 0f; // unlocks new couple in scene to fight

        baseSummon = 0f; // reduce ritual speed
        baseMight = 0.02f; // increase minions strength

        baseLifespan = 0f; // increase life duration
        baseHorde = 0f; // unlocks new size limit horde
        baseLuck = 0f; // increase chance of big minion - increase experience gain necromancer
    }

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= MAX_LEVEL_NECROMANCER,
            () => ExpToNextLevel
        );
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_NECROMANCER_APTITUDE: LevelStatAptitude += amount; break;
            case UtilsPlayer.ID_NECROMANCER_SUMMON: LevelStatSummon += amount; break;
            case UtilsPlayer.ID_NECROMANCER_MIGHT: LevelStatMight += amount; break;
            case UtilsPlayer.ID_NECROMANCER_LIFESPAN: LevelStatLifespan += amount; break;
            case UtilsPlayer.ID_NECROMANCER_HORDE: LevelStatHorde += amount; break;
            case UtilsPlayer.ID_NECROMANCER_LUCK: LevelStatLuck += amount; break;
        }

        CheckAllAchievements();

        InvokeStatChange(id, amount);
    }


    public void CheckAllAchievements()
    {
        if (SteamManager.Initialized)
        {
            CheckUnlockCouplesAchievement();
            CheckNecromancerAriseAchievement();
            CheckNecromancerAfterlifeAchievement();
            CheckNecromancerAdeAchievement();
        }
    }

    private void CheckUnlockCouplesAchievement()
    {
        if (CurrentAptitude >= PER_LEVEL_NECROMANCER_MAX_APTITUDE)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UNLOCK_NECROMANCER_ALL_COUPLES))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UNLOCK_NECROMANCER_ALL_COUPLES);
            }
        }
    }

    private void CheckNecromancerAriseAchievement()
    {
        if (IsAriseRitualUnlocked)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UNLOCK_NECROMANCER_ARISE_RITUAL))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UNLOCK_NECROMANCER_ARISE_RITUAL);
            }
        }
    }

    private void CheckNecromancerAfterlifeAchievement()
    {
        if (IsAfterlifeRitualUnlocked)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UNLOCK_NECROMANCER_AFTERLIFE_RITUAL))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UNLOCK_NECROMANCER_AFTERLIFE_RITUAL);
            }
        }
    }

    private void CheckNecromancerAdeAchievement()
    {
        if (IsAdeRitualUnlocked)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UNLOCK_NECROMANCER_ADE_RITUAL))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UNLOCK_NECROMANCER_ADE_RITUAL);
            }
        }
    }
}
