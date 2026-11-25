using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static UtilsMage;

public class PlayerMageData : BasePlayerData
{
    // ---- BASE STAT VALUES

    private float baseInsight;
    private float baseCastSpeed;
    private float baseScholar;
    private float baseProficiency;

    // ---- LEVEL STAT POINTS



    public int LevelStatInsight { get; private set; }
    public int LevelStatCastSpeed { get; private set; }
    public int LevelStatScholar { get; private set; }
    public int LevelStatProficiency { get; private set; }



    // ---- FINAL STAT VALUES
    public long ExpToNextLevel => RequiredExpForMageLevel(CurrentLevel + 1);


    public float CurrentInsight => baseInsight +
        PER_LEVEL_MAGE_GAIN_INSIGHT * (LevelStatInsight >= PER_LEVEL_MAGE_MAX_INSIGHT ? LevelStatInsight : LevelStatInsight - 1) +
        (_inspirationModifier - 1f);

    public float CurrentCastSpeed => baseCastSpeed +
        PER_LEVEL_MAGE_GAIN_CASTSPEED * (LevelStatCastSpeed >= PER_LEVEL_MAGE_MAX_CASTSPEED ? LevelStatCastSpeed : LevelStatCastSpeed - 1) +
        (_inspirationModifier - 1f);

    public float CurrentScholar => baseScholar + PER_LEVEL_MAGE_GAIN_SCHOLAR * LevelStatScholar;
    public float CurrentProficiency => baseProficiency + PER_LEVEL_MAGE_GAIN_PROFICIENCY * LevelStatProficiency;


    // ---- SPELLS

    public List<SpellData> Spells { get; private set; }

    public MageSpellType CurrentLearningSpell { get; private set; }

    public bool IsSlot1Unlocked => 0 <= PlayerManager.Instance.PlayerMageData.CurrentProficiency;
    public bool IsSlot2Unlocked => 1 <= PlayerManager.Instance.PlayerMageData.CurrentProficiency;
    public bool IsSlot3Unlocked => 2 <= PlayerManager.Instance.PlayerMageData.CurrentProficiency;
    public bool IsSlot4Unlocked => 3 <= PlayerManager.Instance.PlayerMageData.CurrentProficiency;

    public MageSpellType EquippedSlot1Spell { get; private set; }
    public MageSpellType EquippedSlot2Spell { get; private set; }
    public MageSpellType EquippedSlot3Spell { get; private set; }
    public MageSpellType EquippedSlot4Spell { get; private set; }



    public event Action OnEquippedSpellUpdate;


    public PlayerMageData()
    {
        GenerateBaseStats();
    }

    public PlayerMageData(PlayerMageSaveData saveData)
    {
        GenerateBaseStats();

        LevelStatInsight = saveData.levelStatInsight;
        LevelStatCastSpeed = saveData.levelStatCastSpeed;
        LevelStatScholar = saveData.levelStatScholar;
        LevelStatProficiency = saveData.levelStatProficiency;

        LevelStatInsight = Math.Min(LevelStatInsight, PER_LEVEL_MAGE_MAX_INSIGHT);
        LevelStatCastSpeed = Math.Min(LevelStatCastSpeed, PER_LEVEL_MAGE_MAX_CASTSPEED);
        LevelStatScholar = Math.Min(LevelStatScholar, PER_LEVEL_MAGE_MAX_SCHOLAR);
        LevelStatProficiency = Math.Min(LevelStatProficiency, PER_LEVEL_MAGE_MAX_PROFICIENCY);

        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            LevelStatInsight + LevelStatCastSpeed + LevelStatScholar + LevelStatProficiency +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= MAX_LEVEL_MAGE)
        {
            AvailableStatPoints = MAX_LEVEL_MAGE - 1 -
               LevelStatInsight - LevelStatCastSpeed - LevelStatScholar - LevelStatProficiency +
               START_LEVEL_INSIGHT +
               START_LEVEL_CAST_SPEED +
               START_LEVEL_SCHOLAR +
               START_LEVEL_PROFICIENCY;
            CurrentExp = 0;
        }

        // load spells
        Spells = saveData.spells.Select(spell => new SpellData(spell)).ToList();

        CurrentLearningSpell = (MageSpellType)saveData.currentLearningSpell;

        EquippedSlot1Spell = (MageSpellType)saveData.equippedSlot1Spell;
        EquippedSlot2Spell = (MageSpellType)saveData.equippedSlot2Spell;
        EquippedSlot3Spell = (MageSpellType)saveData.equippedSlot3Spell;
        EquippedSlot4Spell = (MageSpellType)saveData.equippedSlot4Spell;
    }

    private void GenerateBaseStats()
    {
        CurrentLevel = 1;
        CurrentExp = 0;


        LevelStatInsight = START_LEVEL_INSIGHT;
        LevelStatCastSpeed = START_LEVEL_CAST_SPEED;
        LevelStatScholar = START_LEVEL_SCHOLAR;
        LevelStatProficiency = START_LEVEL_PROFICIENCY;


        // multiplier
        baseInsight = 0f; // reduced learn time spells, up to 25%

        baseCastSpeed = 0f; // reduce cast speed, up to 20%
        baseScholar = 0f; // unlocks new spell, check on whole values

        baseProficiency = 0f; // unlocks new slots, check on whole values

        // creat default spells
        Spells = GetAllSpells().Select(spell => new SpellData(spell)).ToList();

        // unlock fireball as default
        if (Spells.Count > 0)
        {
            Spells[0].SetUnlocked();
            CurrentLearningSpell = MageSpellType.None;
        }

        EquippedSlot1Spell = EquippedSlot2Spell = EquippedSlot3Spell = EquippedSlot4Spell = MageSpellType.None;
    }

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= MAX_LEVEL_MAGE,
            () => ExpToNextLevel
        );
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_MAGE_INSIGHT: LevelStatInsight += amount; break;
            case UtilsPlayer.ID_MAGE_CASTSPEED: LevelStatCastSpeed += amount; break;
            case UtilsPlayer.ID_MAGE_SCHOLAR: 
                LevelStatScholar += amount;

                // since it's a float, cast to int to get which spell you are currently allow to learn
                int maxIndex = (int)CurrentScholar + 1;
                for (int i = 0; i < maxIndex; i++)
                {
                    // unlock if they are not unlocked yet
                    if (!Spells[i].IsUnlocked)
                        Spells[i].SetUnlocked();
                }

                // check for necromancer job once the scholar stat is maxed out
                if(LevelStatScholar >= PER_LEVEL_MAGE_MAX_SCHOLAR)
                {
                    PlayerManager.Instance.PlayerJobsData.AddAvailableJob(UtilsPlayer.PlayerJob.Necromancer);
                }

                break;
            case UtilsPlayer.ID_MAGE_PROFICIENCY: LevelStatProficiency += amount; break;
        }

        CheckUnlockAllSpellsAchievement();
        CheckUnlockAllSlotsAchievement();

        InvokeStatChange(id, amount);
    }


    public void SetLearningSpell(MageSpellType spellType)
    {
        CurrentLearningSpell = spellType;
    }

    public SpellData GetSpellByType(MageSpellType spellType)
    {
        return Spells.Where(spell => spell.SpellSO.SpellType == spellType).FirstOrDefault();
    }

    public void UpdateSpellData(SpellData data)
    {
        int index = Spells.FindIndex(spell => spell.SpellSO.SpellType == data.SpellSO.SpellType);
        if(index >= 0)
            Spells[index] = data;
    }




    public void EquipToSlot(int idSlot, SpellData spell)
    {
        switch(idSlot)
        {
            default: Debug.Log("Wrong slot id, probably wrong in the inspector"); break;
            case 1: EquippedSlot1Spell = spell.SpellSO.SpellType; break;
            case 2: EquippedSlot2Spell = spell.SpellSO.SpellType; break;
            case 3: EquippedSlot3Spell = spell.SpellSO.SpellType; break;
            case 4: EquippedSlot4Spell = spell.SpellSO.SpellType; break;
        }

        OnEquippedSpellUpdate?.Invoke();
    }

    public void UnequipFromSlot(int idSlot)
    {
        switch (idSlot)
        {
            default: Debug.Log("Wrong slot id, probably wrong in the inspector"); break;
            case 1: EquippedSlot1Spell = MageSpellType.None; break;
            case 2: EquippedSlot2Spell = MageSpellType.None; break;
            case 3: EquippedSlot3Spell = MageSpellType.None; break;
            case 4: EquippedSlot4Spell = MageSpellType.None; break;
        }

        OnEquippedSpellUpdate?.Invoke();
    }

    public bool IsSpellEquipped(SpellData spell)
    {
        if(spell.SpellSO.SpellType == EquippedSlot1Spell ||
            spell.SpellSO.SpellType == EquippedSlot2Spell ||
            spell.SpellSO.SpellType == EquippedSlot3Spell ||
            spell.SpellSO.SpellType == EquippedSlot4Spell )
        {
            return true;
        }

        return false;
    }

    public int GetEquippedSlot(SpellData spell)
    {
        if (spell.SpellSO.SpellType == EquippedSlot1Spell) return 1;
        else if (spell.SpellSO.SpellType == EquippedSlot2Spell) return 2;
        else if (spell.SpellSO.SpellType == EquippedSlot3Spell) return 3;
        else if (spell.SpellSO.SpellType == EquippedSlot4Spell) return 4;

        return -1;
    }

    public int GetFirstEmptySlot()
    {
        if (EquippedSlot1Spell == MageSpellType.None && IsSlot1Unlocked) return 1;
        else if (EquippedSlot2Spell == MageSpellType.None && IsSlot2Unlocked) return 2;
        else if (EquippedSlot3Spell == MageSpellType.None && IsSlot3Unlocked) return 3;
        else if (EquippedSlot4Spell == MageSpellType.None && IsSlot4Unlocked) return 4;

        return -1;
    }

    public bool IsSlotUnlocked(int idSlot)
    {
        switch (idSlot)
        {
            default: Debug.Log("Wrong slot id, probably wrong in the inspector"); return false;
            case 1: return IsSlot1Unlocked;
            case 2: return IsSlot2Unlocked;
            case 3: return IsSlot3Unlocked;
            case 4: return IsSlot4Unlocked;
        }
    }


    public void CheckAllAchievements()
    {
        if (SteamManager.Initialized)
        {
            CheckMaxSpellsAchievements();
            CheckUnlockAllSpellsAchievement();
            CheckUnlockAllSlotsAchievement();
        }
    }

    public void CheckMaxSpellsAchievements()
    {
        int countMaxLevel = 0;
        foreach (var spell in Spells)
        {
            if(spell.CurrentRank >= spell.SpellSO.MaxRank)
            {
                countMaxLevel++;
            }
        }

        if (countMaxLevel >= 5)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.MAX_UPGRADE_MAGE_SPELLS_5))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.MAX_UPGRADE_MAGE_SPELLS_5);
            }
        }
        else if (countMaxLevel >= 1)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.MAX_UPGRADE_MAGE_SPELLS_1))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.MAX_UPGRADE_MAGE_SPELLS_1);
            }
        }
    }

    private void CheckUnlockAllSpellsAchievement()
    {
        if(CurrentScholar >= PER_LEVEL_MAGE_MAX_SCHOLAR)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UNLOCK_ALL_MAGE_SPELLS))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UNLOCK_ALL_MAGE_SPELLS);
            }
        }
    }

    private void CheckUnlockAllSlotsAchievement()
    {
        if (CurrentProficiency >= PER_LEVEL_MAGE_MAX_PROFICIENCY)
        {
            if (!UtilsAchievement.IsAchievementUnlocked(UtilsAchievement.UNLOCK_ALL_MAGE_SLOTS))
            {
                UtilsAchievement.UnlockAchievement(UtilsAchievement.UNLOCK_ALL_MAGE_SLOTS);
            }
        }
    }
}
