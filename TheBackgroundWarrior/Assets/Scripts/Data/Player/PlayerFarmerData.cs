using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFarmerData
{
    // ---- BASE STAT VALUES

    private float baseGreenthumb;
    private float baseAgronomy;
    private float baseKindness;
    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatGreenthumb = 1;
    private int levelstatAgronomy = 0;
    private int levelstatKindness = 1;
    private int levelStatLuck = 1;

    private int startLevelGreenthumb = 1;
    private int startLevelAgronomy = 0;
    private int startLevelKindness = 1;
    private int startLevelLuck = 1;


    public int LevelStatGreenthumb => levelStatGreenthumb;
    public int LevelStatAgronomy => levelstatAgronomy;
    public int LevelStatKindness => levelstatKindness;
    public int LevelStatLuck => levelStatLuck;


    // ---- POINTS

    private int availableStatPoints;

    public int AvailableStatPoints => availableStatPoints;


    // ---- FINAL STAT VALUES

    private int currentLevel;
    private long currentExp;



    public int CurrentLevel => currentLevel;
    public long CurrentExp => currentExp;
    public long ExpToNextLevel => UtilsFarmer.RequiredExpForFarmerLevel(currentLevel + 1);


    public float CurrentGreenthumb => baseGreenthumb + UtilsFarmer.PER_LEVEL_FARMER_GAIN_GREENTHUMB * (levelStatGreenthumb - 1);
    public float CurrentAgronomy => baseAgronomy + UtilsFarmer.PER_LEVEL_FARMER_GAIN_AGRONOMY * (levelstatAgronomy);
    public float CurrentKindness => baseKindness + UtilsFarmer.PER_LEVEL_FARMER_GAIN_KINDNESS * (levelstatKindness - 1);
    public float CurrentLuck => baseLuck + UtilsFarmer.PER_LEVEL_FARMER_GAIN_LUCK * (levelStatLuck - 1);


    // ---- PLANTED CROPS

    private CropData slot1CropData;
    private CropData slot2CropData;
    private CropData slot3CropData;
    private CropData slot4CropData;


    public CropData Slot1CropData => slot1CropData;
    public CropData Slot2CropData => slot2CropData;
    public CropData Slot3CropData => slot3CropData;
    public CropData Slot4CropData => slot4CropData;


    // ---- EQUIPPED COMPANIONS

    private List<CompanionData> companions;

    public List<CompanionData> Companions => companions;




    public event Action OnAddedExp;
    public event Action OnLevelUp;
    public event Action<int, int> OnStatChange;

    public event Action OnCompanionEquipped;

    public PlayerFarmerData()
    {
        GenerateBaseStats();
    }

    public PlayerFarmerData(PlayerFarmerSaveData saveData)
    {
        GenerateBaseStats();

        levelStatGreenthumb = saveData.levelStatGreenthumb;
        levelstatAgronomy = saveData.levelAgronomy;
        levelstatKindness = saveData.levelKindness;
        levelStatLuck = saveData.levelStatLuck;

        levelStatGreenthumb = Math.Min(levelStatGreenthumb, UtilsFarmer.PER_LEVEL_FARMER_MAX_GREENTHUMB);
        levelstatAgronomy = Math.Min(levelstatAgronomy, UtilsFarmer.PER_LEVEL_FARMER_MAX_AGRONOMY);
        levelstatKindness = Math.Min(levelstatKindness, UtilsFarmer.PER_LEVEL_FARMER_MAX_KINDNESS);
        levelStatLuck = Math.Min(levelStatLuck, UtilsFarmer.PER_LEVEL_FARMER_MAX_LUCK);

        availableStatPoints = saveData.availableStatPoints;

        currentLevel = saveData.currentLevel;
        currentExp = saveData.currentExp;

        int sumLevels =
            levelStatGreenthumb + levelstatAgronomy + levelstatKindness + levelStatLuck -
            startLevelGreenthumb - startLevelAgronomy - startLevelKindness - startLevelLuck +
            availableStatPoints +
            1;

        currentLevel = Math.Min(currentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (currentLevel > UtilsFarmer.MAX_LEVEL_FARMER)
        {
            availableStatPoints = 0;
            currentExp = 0;
        }


        if (saveData.slot1CropSaveData != null)
            slot1CropData = new CropData(saveData.slot1CropSaveData);

        if (saveData.slot2CropSaveData != null)
            slot2CropData = new CropData(saveData.slot2CropSaveData);

        if (saveData.slot3CropSaveData != null)
            slot3CropData = new CropData(saveData.slot3CropSaveData);

        if (saveData.slot4CropSaveData != null)
            slot4CropData = new CropData(saveData.slot4CropSaveData);


        foreach (var companion in saveData.companionSaveDatas)
        {
            CompanionData data = new CompanionData(companion);
            companions.Add(data);
        }
    }

    private void GenerateBaseStats()
    {
        currentLevel = 1;
        currentExp = 0;


        levelStatGreenthumb = startLevelGreenthumb;
        levelstatAgronomy = startLevelAgronomy;
        levelstatKindness = startLevelKindness;
        levelStatLuck = startLevelLuck;


        // multiplier
        baseGreenthumb = 0f; // reduced growth time crops, up to 25%

        baseAgronomy = 0f; // unlocks new seeds, check on whole values
        baseKindness = 0f; // increase prob to encounter companions, base 10% up to 35%

        baseLuck = 0.1f; // increase befriend companions, base 10% up to 25%

        companions = new List<CompanionData>();
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
        if (currentLevel > UtilsFarmer.MAX_LEVEL_FARMER)
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
            case UtilsPlayer.ID_FARMER_GREENTHUMB: levelStatGreenthumb += amount; break;
            case UtilsPlayer.ID_FARMER_AGRONOMY: levelstatAgronomy += amount; break;
            case UtilsPlayer.ID_FARMER_KINDNESS: levelstatKindness += amount; break;
            case UtilsPlayer.ID_FARMER_LUCK: levelStatLuck += amount; break;
        }

        OnStatChange?.Invoke(id, amount);
    }

    public CropData SetCropToSlot(CropSO crop, int slot)
    {
        if(crop != null)
        {
            CropData data = new CropData(crop, slot);

            switch (slot)
            {
                case 0: slot1CropData = data; break;
                case 1: slot2CropData = data; break;
                case 2: slot3CropData = data; break;
                case 3: slot4CropData = data; break;
            }

            return data;
        }
        return null;
    }

    public void UpdateCropToSlot(CropData data, int slot)
    {
        if (data != null)
        {
            switch (slot)
            {
                case 0: slot1CropData = data; break;
                case 1: slot2CropData = data; break;
                case 2: slot3CropData = data; break;
                case 3: slot4CropData = data; break;
            }
        }
    }

    public void AddCompanion(CompanionSO companionSO)
    {
        CompanionData data = new CompanionData(companionSO);
        companions.Add(data);
    }

    public bool HasCompanion(CompanionSO companionSO)
    {
        foreach(var companion in companions)
        {
            if (companion.CompanionSO.Id == companionSO.Id)
                return true;
        }
        return false;
    }

    public int GetCompanionIndexList(CompanionSO companionSO)
    {
        for (int i = 0; i < companions.Count; i++)
        {
            if (companions[i].CompanionSO.Id == companionSO.Id)
                return i;
        }
        return -1;
    }

    public bool SetCompanionToSlot(CompanionData companion, int slot)
    {
        if (HasCompanion(companion.CompanionSO))
        {
            int index = GetCompanionIndexList(companion.CompanionSO);
            companions[index].SetSlot(slot);

            OnCompanionEquipped?.Invoke();

            return true;
        }
        return false;
    }

    public List<CompanionSlotData> GetEquippedCompanions()
    {
        List<CompanionSlotData> result = new List<CompanionSlotData>();

        foreach (var companion in companions)
        {
            if(companion.CurrentSlot != -1)
            {
                CompanionSlotData slotdata = new CompanionSlotData(companion, companion.CurrentSlot);
                result.Add(slotdata);
            }
        }

        return result;
    }

    public bool AreEquippedCompanionsFull()
    {
        return GetEquippedCompanions().Count >= 3 ? true : false;
    }

    public int GetFirstEmptyCompanionSlot()
    {
        var equippedCompanions = GetEquippedCompanions();
        bool empty1 = true, empty2 = true, empty3 = true;

        foreach (var equipped in equippedCompanions)
        {
            switch (equipped.slot)
            {
                case 0: empty1 = false; break;
                case 1: empty2 = false; break;
                case 2: empty3 = false; break;
            }
        }

        if (empty1) return 0;
        if (empty2) return 1;
        if (empty3) return 2;

        return -1;
    }
}


[Serializable]
public struct CompanionSlotData
{
    public CompanionData companionData;
    public int slot;

    public CompanionSlotData(CompanionData companionData, int slot)
    {
        this.companionData = companionData;
        this.slot = slot;
    }
}

[Serializable]
public struct CropSlotData
{
    public CropData cropData;
    public int slot;

    public CropSlotData(CropData cropData, int slot)
    {
        this.cropData = cropData;
        this.slot = slot;
    }
}