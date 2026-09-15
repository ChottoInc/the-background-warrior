using System;
using UnityEngine;
using static UtilsPlayer;

public class PlayerFisherData : BasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseCalmness;
    private float baseReflex;
    private float baseKnowledge;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int startLevelCalmness = 1;
    private int startLevelReflex = 1;
    private int startLevelKnowledge = 1;
    private int startLevelLuck = 1;


    public int LevelStatCalmness { get; private set; }
    public int LevelStatReflex { get; private set; }
    public int LevelStatKnowledge { get; private set; }
    public int LevelStatLuck { get; private set; }


    // ---- POINTS

    


    // ---- FINAL STAT VALUES



    
    public long ExpToNextLevel => UtilsFisher.RequiredExpForFisherLevel(CurrentLevel + 1);

    public float CurrentCalmness => baseCalmness + UtilsFisher.PER_LEVEL_FISHER_GAIN_CALMNESS * (LevelStatCalmness);
    public float CurrentReflex => baseReflex + UtilsFisher.PER_LEVEL_FISHER_GAIN_REFLEX * (LevelStatReflex - 1) + (_inspirationModifier - 1f);
    public float CurrentKnowledge => baseKnowledge + UtilsFisher.PER_LEVEL_FISHER_GAIN_KNOWLEDGE * (LevelStatKnowledge - 1) + (_inspirationModifier - 1f);
    public float CurrentLuck => baseLuck + UtilsFisher.PER_LEVEL_FISHER_GAIN_LUCK * (LevelStatLuck - 1) + (_inspirationModifier - 1f);


    // ---- FISH GROUPS CHECKS COMPLETION

    public bool IsLifeSeriesCompleted { get; private set; }
    public bool IsPredatorSeriesCompleted { get; private set; }
    public bool IsGuardianSeriesCompleted { get; private set; }
    public bool IsDartSeriesCompleted { get; private set; }
    public bool IsSharpSeriesCompleted { get; private set; }
    public bool IsPiercingSeriesCompleted { get; private set; }
    public bool IsGoldenSeriesCompleted { get; private set; }
    public bool IsElderSeriesCompleted { get; private set; }
    public bool IsQuickSeriesCompleted { get; private set; }


    // ---- BAITS VARS

    public BaitSO ActiveBait { get; private set; }



    

    public event Action OnBaitChange;

    public PlayerFisherData()
    {
        GenerateBaseStats();

        FillFishGroupsSeriesCompletion();
    }

    public PlayerFisherData(PlayerFisherSaveData saveData)
    {
        GenerateBaseStats();

        LevelStatCalmness = saveData.levelStatCalmness;
        LevelStatReflex = saveData.levelReflex;
        LevelStatKnowledge = saveData.levelKnowledge;
        LevelStatLuck = saveData.levelStatLuck;


        LevelStatCalmness = Math.Min(LevelStatCalmness, UtilsFisher.PER_LEVEL_FISHER_MAX_CALMNESS);
        LevelStatReflex = Math.Min(LevelStatReflex, UtilsFisher.PER_LEVEL_FISHER_MAX_REFLEX);
        LevelStatKnowledge = Math.Min(LevelStatKnowledge, UtilsFisher.PER_LEVEL_FISHER_MAX_KNOWLEDGE);
        LevelStatLuck = Math.Min(LevelStatLuck, UtilsFisher.PER_LEVEL_FISHER_MAX_LUCK);


        AvailableStatPoints = saveData.availableStatPoints;
        
        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatCalmness + LevelStatReflex + LevelStatKnowledge + LevelStatLuck +
            //startLevelCalmness + startLevelReflex + startLevelKnowledge + startLevelLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= UtilsFisher.MAX_LEVEL_FISHER)
        {
            AvailableStatPoints = UtilsFisher.MAX_LEVEL_FISHER - 1 -
               LevelStatCalmness - LevelStatReflex - LevelStatKnowledge - LevelStatLuck;
            CurrentExp = 0;
        }

        FillFishGroupsSeriesCompletion();

        ActiveBait = UtilsItem.GetItemById(saveData.activeBaitId) as BaitSO;
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;

        LevelStatCalmness = startLevelCalmness;
        LevelStatReflex = startLevelReflex;
        LevelStatKnowledge = startLevelKnowledge;
        LevelStatLuck = startLevelLuck;

        // multiplier
        baseCalmness = 0f; // reduced max time for spawn fish, up to 0.5f - 50%

        baseReflex = 0.5f; // stat contrlling if the fish is caught, up to 0.75 - 25%
        baseKnowledge = 0f; // reduce chances of same species, up to 0.3 - 30%

        baseLuck = 0f; // controls rarity of fish, up to 0.4 - 40%

        ActiveBait = null;
    }

    public void FillFishGroupsSeriesCompletion()
    {
        FishGroupSO currentGroup = null;

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Life);
        IsLifeSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Predator);
        IsPredatorSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Guardian);
        IsGuardianSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Dart);
        IsDartSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Sharp);
        IsSharpSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Piercing);
        IsPiercingSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Golden);
        IsGoldenSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Elder);
        IsElderSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = UtilsFisher.GetFishGroupByType(UtilsFisher.FishGroupType.Quick);
        IsQuickSeriesCompleted = IsGroupCaught(currentGroup);
    }

    private bool IsGroupCaught(FishGroupSO group)
    {
        if (group == null) return false;

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

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= UtilsFisher.MAX_LEVEL_FISHER,
            () => ExpToNextLevel
        );
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case ID_FISHER_CALMNESS: LevelStatCalmness += amount; break;
            case ID_FISHER_REFLEX: LevelStatReflex += amount; break;
            case ID_FISHER_KNOWLEDGE: LevelStatKnowledge += amount; break;
            case ID_FISHER_LUCK: LevelStatLuck += amount; break;
        }

        InvokeStatChange(id, amount);
    }


    public void SetActiveBait(BaitSO bait)
    {
        ActiveBait = bait;

        OnBaitChange?.Invoke();
    }

    public void DisableBait()
    {
        ActiveBait = null;

        OnBaitChange?.Invoke();
    }
}
