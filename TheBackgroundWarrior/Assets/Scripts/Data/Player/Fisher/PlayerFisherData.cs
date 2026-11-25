using System;
using System.Collections.Generic;
using UnityEngine;

using static UtilsPlayer;
using static UtilsFisher;

public class PlayerFisherData : BasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseCalmness;
    private float baseReflex;
    private float baseKnowledge;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    


    public int LevelStatCalmness { get; private set; }
    public int LevelStatReflex { get; private set; }
    public int LevelStatKnowledge { get; private set; }
    public int LevelStatLuck { get; private set; }


    // ---- POINTS

    


    // ---- FINAL STAT VALUES



    
    public long ExpToNextLevel => RequiredExpForFisherLevel(CurrentLevel + 1);

    public float CurrentCalmness => baseCalmness + PER_LEVEL_FISHER_GAIN_CALMNESS * (LevelStatCalmness);
    public float CurrentReflex => baseReflex +
        PER_LEVEL_FISHER_GAIN_REFLEX * (LevelStatReflex >= PER_LEVEL_FISHER_MAX_REFLEX ? LevelStatReflex : LevelStatReflex - 1) + 
        (_inspirationModifier - 1f);

    public float CurrentKnowledge => baseKnowledge +
        PER_LEVEL_FISHER_GAIN_KNOWLEDGE * (LevelStatKnowledge >= PER_LEVEL_FISHER_MAX_KNOWLEDGE ? LevelStatKnowledge : LevelStatKnowledge - 1) +
        (_inspirationModifier - 1f);
    public float CurrentLuck => baseLuck +
        PER_LEVEL_FISHER_GAIN_LUCK * (LevelStatLuck >= PER_LEVEL_FISHER_MAX_LUCK ? LevelStatLuck : LevelStatLuck - 1) +
        (_inspirationModifier - 1f);


    // ---- FISH GROUPS CHECKS COMPLETION

    public HashSet<int> CaughtFishesOnce { get; private set; }


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


        LevelStatCalmness = Math.Min(LevelStatCalmness, PER_LEVEL_FISHER_MAX_CALMNESS);
        LevelStatReflex = Math.Min(LevelStatReflex, PER_LEVEL_FISHER_MAX_REFLEX);
        LevelStatKnowledge = Math.Min(LevelStatKnowledge, PER_LEVEL_FISHER_MAX_KNOWLEDGE);
        LevelStatLuck = Math.Min(LevelStatLuck, PER_LEVEL_FISHER_MAX_LUCK);


        AvailableStatPoints = saveData.availableStatPoints;
        
        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatCalmness + LevelStatReflex + LevelStatKnowledge + LevelStatLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= MAX_LEVEL_FISHER)
        {
            AvailableStatPoints = MAX_LEVEL_FISHER - 1 -
               LevelStatCalmness - LevelStatReflex - LevelStatKnowledge - LevelStatLuck +
               START_LEVEL_CALMNESS +
               START_LEVEL_REFLEX +
               START_LEVEL_KNOWLEDGE +
               START_LEVEL_LUCK;
            CurrentExp = 0;
        }

        ActiveBait = UtilsItem.GetItemById(saveData.activeBaitId) as BaitSO;

        // just to be sure the list isn't null
        if(saveData.caughtFishesOnce != null)
            CaughtFishesOnce = new HashSet<int>(saveData.caughtFishesOnce);

        // from previous saves, add all fishe you have for now
        FillCaughtOnceFromInventory();

        FillFishGroupsSeriesCompletion();
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;

        LevelStatCalmness = START_LEVEL_CALMNESS;
        LevelStatReflex = START_LEVEL_REFLEX;
        LevelStatKnowledge = START_LEVEL_KNOWLEDGE;
        LevelStatLuck = START_LEVEL_LUCK;

        // multiplier
        baseCalmness = 0f; // reduced max time for spawn fish, up to 0.5f - 50%

        baseReflex = 0.5f; // stat contrlling if the fish is caught, up to 0.75 - 25%
        baseKnowledge = 0f; // reduce chances of same species, up to 0.3 - 30%

        baseLuck = 0f; // controls rarity of fish, up to 0.4 - 40%

        ActiveBait = null;

        CaughtFishesOnce = new HashSet<int>();
    }

    private void FillCaughtOnceFromInventory()
    {
        foreach (var item in PlayerManager.Instance.Inventory.GetGroupsOfType(UtilsItem.ItemType.Fish))
        {
            CaughtFishesOnce.Add(item.IdItem);
        }
    }

    public void FillFishGroupsSeriesCompletion()
    {
        FishGroupSO currentGroup = null;

        currentGroup = GetFishGroupByType(FishGroupType.Life);
        IsLifeSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Predator);
        IsPredatorSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Guardian);
        IsGuardianSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Dart);
        IsDartSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Sharp);
        IsSharpSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Piercing);
        IsPiercingSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Golden);
        IsGoldenSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Elder);
        IsElderSeriesCompleted = IsGroupCaught(currentGroup);

        currentGroup = GetFishGroupByType(FishGroupType.Quick);
        IsQuickSeriesCompleted = IsGroupCaught(currentGroup);
    }

    private bool IsGroupCaught(FishGroupSO group)
    {
        if (group == null) return false;

        foreach (var fish in group.Fishes)
        {
            // check for not caught fish
            if (!CaughtFishesOnce.Contains(fish.Id))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsGroupCaughtCheckVar(FishGroupSO fishGroupSO)
    {
        switch (fishGroupSO.GroupType)
        {
            default: return false;
            case FishGroupType.Life: return IsLifeSeriesCompleted;
            case FishGroupType.Predator: return IsPredatorSeriesCompleted;
            case FishGroupType.Guardian: return IsGuardianSeriesCompleted;
            case FishGroupType.Dart: return IsDartSeriesCompleted;
            case FishGroupType.Sharp: return IsSharpSeriesCompleted;
            case FishGroupType.Piercing: return IsPiercingSeriesCompleted;
            case FishGroupType.Golden: return IsGoldenSeriesCompleted;
            case FishGroupType.Elder: return IsElderSeriesCompleted;
            case FishGroupType.Quick: return IsQuickSeriesCompleted;
        }
    }

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= MAX_LEVEL_FISHER,
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
    }

    private bool AreAllGroupsCompleted()
    {
        return 
            IsLifeSeriesCompleted &&
            IsPredatorSeriesCompleted &&
            IsGuardianSeriesCompleted &&
            IsDartSeriesCompleted &&
            IsSharpSeriesCompleted &&
            IsPiercingSeriesCompleted &&
            IsGoldenSeriesCompleted &&
            IsElderSeriesCompleted &&
            IsQuickSeriesCompleted;
    }

    public void AddToCaughtOnce(int fishId)
    {
        CaughtFishesOnce.Add(fishId);
    }


    public void CheckAllAchievements()
    {
        if (IsGoldenSeriesCompleted)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.OBTAIN_FISH_GOLDEN_GROUP))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.OBTAIN_FISH_GOLDEN_GROUP);
            }
        }

        if(AreAllGroupsCompleted())
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.OBTAIN_FISH_ALL_GROUP))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.OBTAIN_FISH_ALL_GROUP);
            }
        }
    }
}
