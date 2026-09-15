using System;
using UnityEngine;

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


    private int startLevelAptitude = 0;
    private int startLevelSummon = 1;
    private int startLevelMight = 1;
    private int startLevelLifespan = 1;
    private int startLevelHorde = 0;
    private int startLevelLuck = 1;


    public int LevelStatAptitude { get; private set; }
    public int LevelStatSummon { get; private set; }
    public int LevelStatMight { get; private set; }
    public int LevelStatLifespan { get; private set; }
    public int LevelStatHorde { get; private set; }
    public int LevelStatLuck { get; private set; }



    // ---- FINAL STAT VALUES
    public long ExpToNextLevel => UtilsNecromancer.RequiredExpForNecromancerLevel(CurrentLevel + 1);


    public float CurrentAptitude => baseAptitude + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_APTITUDE * LevelStatAptitude;
    public float CurrentSummon => baseSummon + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_SUMMON * (LevelStatSummon - 1);
    public float CurrentMight => baseMight + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_MIGHT * (LevelStatMight - 1) + (_inspirationModifier - 1f);
    public float CurrentLifespan => baseLifespan + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_LIFESPAN * (LevelStatLifespan - 1) + (_inspirationModifier - 1f);
    public float CurrentHorde => baseHorde + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_HORDE * LevelStatHorde;
    public float CurrentLuck => baseLuck + UtilsNecromancer.PER_LEVEL_NECROMANCER_GAIN_LUCK * (LevelStatLuck - 1) + (_inspirationModifier - 1f);


    public bool IsAriseRitualUnlocked =>
        LevelStatSummon >= UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_SUMMON &&
        LevelStatHorde >= UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_HORDE;

    public bool IsAfterlifeRitualUnlocked =>
        LevelStatAptitude >= UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_APTITUDE &&
        LevelStatMight >= UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_MIGHT;

    public bool IsAdeRitualUnlocked =>
        LevelStatLifespan >= UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_LIFESPAN &&
        LevelStatLuck >= UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_LUCK;


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

        LevelStatAptitude = Math.Min(LevelStatAptitude, UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_APTITUDE);
        LevelStatSummon = Math.Min(LevelStatSummon, UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_SUMMON);
        LevelStatMight = Math.Min(LevelStatMight, UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_MIGHT);
        LevelStatLifespan = Math.Min(LevelStatLifespan, UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_LIFESPAN);
        LevelStatHorde = Math.Min(LevelStatHorde, UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_HORDE);
        LevelStatLuck = Math.Min(LevelStatLuck, UtilsNecromancer.PER_LEVEL_NECROMANCER_MAX_LUCK);

        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatAptitude + LevelStatSummon + LevelStatMight + LevelStatLifespan + LevelStatHorde + LevelStatLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= UtilsNecromancer.MAX_LEVEL_NECROMANCER)
        {
            AvailableStatPoints = UtilsNecromancer.MAX_LEVEL_NECROMANCER - 1 -
               LevelStatAptitude - LevelStatSummon - LevelStatMight - LevelStatLifespan - LevelStatHorde - LevelStatLuck;
            CurrentExp = 0;
        }
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;


        LevelStatAptitude = startLevelAptitude;
        LevelStatSummon = startLevelSummon;
        LevelStatMight = startLevelMight;
        LevelStatLifespan = startLevelLifespan;
        LevelStatHorde = startLevelHorde;
        LevelStatLuck = startLevelLuck;


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
            level => level >= UtilsMage.MAX_LEVEL_MAGE,
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

        InvokeStatChange(id, amount);
    }
}
