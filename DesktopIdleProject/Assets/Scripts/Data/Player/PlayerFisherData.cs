using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerFisherData
{
    // ---- BASE STAT VALUES

    private float baseCalmness;
    private float baseReflex;
    private float baseKnowledge;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatCalmness = 1;
    private int levelstatReflex = 1;
    private int levelstatKnowledge = 1;
    private int levelStatLuck = 1;

    private int startLevelCalmness = 1;
    private int startLevelReflex = 1;
    private int startLevelKnowledge = 1;
    private int startLevelLuck = 1;


    public int LevelStatCalmness => levelStatCalmness;
    public int LevelStatReflex => levelstatReflex;
    public int LevelStatKnowledge => levelstatKnowledge;
    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private long currentExp;


    // ---- FISH GROUPS CHECKS COMPLETION

    private bool isLifeSeriesCompleted;
    private bool isPredatorSeriesCompleted;
    private bool isGuardianSeriesCompleted;
    private bool isDartSeriesCompleted;
    private bool isSharpSeriesCompleted;
    private bool isPiercingSeriesCompleted;
    private bool isGoldenSeriesCompleted;
    private bool isElderSeriesCompleted;
    private bool isQuickSeriesCompleted;





    public int CurrentLevel => currentLevel;
    public long CurrentExp => currentExp;
    public long ExpToNextLevel => UtilsFisher.RequiredExpForFisherLevel(currentLevel + 1);

    public float CurrentCalmness => baseCalmness + UtilsFisher.PER_LEVEL_FISHER_GAIN_CALMNESS * (levelStatCalmness);
    public float CurrentReflex => baseReflex + UtilsFisher.PER_LEVEL_FISHER_GAIN_REFLEX * (levelstatReflex - 1);
    public float CurrentKnowledge => baseKnowledge + UtilsFisher.PER_LEVEL_FISHER_GAIN_KNOWLEDGE * (levelstatKnowledge - 1);
    public float CurrentLuck => baseLuck + UtilsFisher.PER_LEVEL_FISHER_GAIN_LUCK * (levelStatLuck - 1);



    public bool IsLifeSeriesCompleted => isLifeSeriesCompleted;
    public bool IsPredatorSeriesCompleted => isPredatorSeriesCompleted;
    public bool IsGuardianSeriesCompleted => isGuardianSeriesCompleted;
    public bool IsDartSeriesCompleted => isDartSeriesCompleted;
    public bool IsSharpSeriesCompleted => isSharpSeriesCompleted;
    public bool IsPiercingSeriesCompleted => isPiercingSeriesCompleted;
    public bool IsGoldenSeriesCompleted => isGoldenSeriesCompleted;
    public bool IsElderSeriesCompleted => isElderSeriesCompleted;
    public bool IsQuickSeriesCompleted => isQuickSeriesCompleted;



    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public PlayerFisherData()
    {
        GenerateBaseStats();

        FillFishGroupsSeriesCompletion();
    }

    public PlayerFisherData(PlayerFisherSaveData saveData)
    {
        GenerateBaseStats();

        levelStatCalmness = saveData.levelStatCalmness;
        levelstatReflex = saveData.levelReflex;
        levelstatKnowledge = saveData.levelKnowledge;
        levelStatLuck = saveData.levelStatLuck;


        levelStatCalmness = Math.Min(levelStatCalmness, UtilsFisher.PER_LEVEL_FISHER_MAX_CALMNESS);
        levelstatReflex = Math.Min(levelstatReflex, UtilsFisher.PER_LEVEL_FISHER_MAX_REFLEX);
        levelstatKnowledge = Math.Min(levelstatKnowledge, UtilsFisher.PER_LEVEL_FISHER_MAX_KNOWLEDGE);
        levelStatLuck = Math.Min(levelStatLuck, UtilsFisher.PER_LEVEL_FISHER_MAX_LUCK);


        availableStatPoints = saveData.availableStatPoints;
        
        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        int sumLevels =
            levelStatCalmness + levelstatReflex + levelstatKnowledge + levelStatLuck -
            startLevelCalmness - startLevelReflex - startLevelKnowledge - startLevelLuck +
            availableStatPoints +
            1;

        currentLevel = Math.Min(currentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (currentLevel > UtilsFisher.MAX_LEVEL_FISHER)
        {
            availableStatPoints = 0;
            currentExp = 0;
        }

        FillFishGroupsSeriesCompletion();
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;

        levelStatCalmness = startLevelCalmness;
        levelstatReflex = startLevelReflex;
        levelstatKnowledge = startLevelKnowledge;
        levelStatLuck = startLevelLuck;

        // multiplier
        baseCalmness = 0f; // reduced max time for spawn fish, up to 0.5f - 50%

        baseReflex = 0.5f; // stat contrlling if the fish is caught, up to 0.75 - 25%
        baseKnowledge = 0f; // reduce chances of same species, up to 0.3 - 30%

        baseLuck = 0f; // controls rarity of fish, up to 0.4 - 40%
    }

    public void FillFishGroupsSeriesCompletion()
    {
        FishGroupSO currentGroup = null;

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Life);
        isLifeSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Predator);
        isPredatorSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Guardian);
        isGuardianSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Dart);
        isDartSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Sharp);
        isSharpSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Piercing);
        isPiercingSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Golden);
        isGoldenSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Elder);
        isElderSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsGather.GetFishGroupByType(UtilsGather.FishGroupType.Quick);
        isQuickSeriesCompleted = IsGroupCaught(currentGroup);
    }

    private bool IsGroupCaught(FishGroupSO group)
    {
        bool result = true;

        foreach (var fish in group.Fishes)
        {
            // check for not caught fish
            if (!PlayerManager.Instance.Inventory.HasItem(fish.Id))
            {
                result = false;
                break;
            }
        }

        return result;
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
        if (currentLevel > UtilsFisher.MAX_LEVEL_FISHER)
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
            case ID_FISHER_CALMNESS: levelStatCalmness += amount; break;
            case ID_FISHER_REFLEX: levelstatReflex += amount; break;
            case ID_FISHER_KNOWLEDGE: levelstatKnowledge += amount; break;
            case ID_FISHER_LUCK: levelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }
}
