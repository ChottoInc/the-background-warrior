using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightData : BasePlayerData
{
    // ---- VISITABLE MAPS

    private List<int> availableMaps;


    // ---- BASE STAT VALUES

    private const float ALCHEMIST_PERMA_ADD_MAXHP = 100f;
    private const float ALCHEMIST_PERMA_ADD_ATTACK = 2f;
    private const float ALCHEMIST_PERMA_ADD_DEFENSE = 1f;



    private const float BASE_MAXHP = 85f;
    private const float BASE_ATK = 10f;
    private const float BASE_DEF = 2.5f;
    private const float BASE_ATKSPD = 1.2f;
    private const float BASE_CRITRATE = 0.05f;
    private const float BASE_CRITDMG = 1.5f;

    private float baseMaxHp;

    private float baseAtk;
    private float baseDef;

    private float baseAtkSpd;

    private float baseCritRate;
    private float baseCritDmg;

    private float baseLuck;

    // ---- LEVEL STAT POINTS

    private int levelStatMaxHp = 1;
            
    private int levelStatAtk = 1;
    private int levelStatDef = 1;
             
    private int levelStatAtkSpd = 1;
             
    private int levelStatCritRate = 1;
    private int levelStatCritDmg = 1;
             
    private int levelStatLuck = 1;


    private int startLevelMaxHp = 1;
    private int startLevelAtk = 1;
    private int startLevelDef = 1;
    private int startLevelAtkSpd = 1;
    private int startLevelCritRate = 1;
    private int startLevelCritDmg = 1;
    private int startLevelLuck = 1;


    public List<int> AvailableMaps => availableMaps;



    public int LevelStatMaxHp => levelStatMaxHp;

    public int LevelStatAtk => levelStatAtk;
    public int LevelStatDef => levelStatDef;

    public int LevelStatAtkSpd => levelStatAtkSpd;

    public int LevelStatCritRate => levelStatCritRate;
    public int LevelStatCritDmg => levelStatCritDmg;

    public int LevelStatLuck => levelStatLuck;



    // ---- FINAL STAT VALUES

    private float maxHpModifier = 1f;
    private float atkModifier = 1f;
    private float defModifier = 1f;
    private float atkSpdModifier = 1f;
    private float critDmgModifier = 1f;


    public long ExpToNextLevel => UtilsWarrior.RequiredExpForWarriorLevel(CurrentLevel + 1);


    
    public float MaxHp => 
        (baseMaxHp + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_MAXHP * (levelStatMaxHp - 1)) *
        PlayerManager.Instance.HelmetMaxHpBlacksmithMultiplier *
        PlayerManager.Instance.FisherLifeSeriesMultiplier *
        maxHpModifier *
        _inspirationModifier;
        

    public float CurrentHp { get; private set; }

    public float CurrentAtk =>
        (baseAtk + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_ATK * (levelStatAtk - 1)) *
        PlayerManager.Instance.WeaponMinerMultiplier *
        PlayerManager.Instance.FisherPredatorSeriesMultiplier *
        atkModifier *
        _inspirationModifier;



    public float CurrentDef =>
        (baseDef + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_DEF * (levelStatDef - 1)) *
        PlayerManager.Instance.ArmorDefBlacksmithMultiplier *
        PlayerManager.Instance.BootsDefBlacksmithMultiplier *
        PlayerManager.Instance.FisherGuardianSeriesMultiplier *
        defModifier *
        _inspirationModifier;
        

    // todo: if more mehods will be available to increase atk spd and crit rate, then check if you want those stats to be past the max threshold
    public float CurrentAtkSpd => 
        (baseAtkSpd + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_ATK_SPEED * (levelStatAtkSpd - 1)) *
        PlayerManager.Instance.GlovesAtkSpdBlacksmithMultiplier *
        PlayerManager.Instance.FisherDartSeriesMultiplier *
        atkSpdModifier *
        _inspirationModifier;

    public float CurrentCritRate => 
        (baseCritRate + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_CRIT_RATE * (levelStatCritRate - 1)) *
        PlayerManager.Instance.BootsCritRateBlacksmithMultiplier *
        PlayerManager.Instance.FisherSharpSeriesMultiplier *
        _inspirationModifier;

    public float CurrentCritDmg => 
        (baseCritDmg + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_CRIT_DMG * (levelStatCritDmg - 1)) *
        PlayerManager.Instance.GlovesCritDmgBlacksmithMultiplier *
        PlayerManager.Instance.FisherPiercingSeriesMultiplier *
        critDmgModifier *
        _inspirationModifier;

    // affects card drop rates, and gives a one more chance to crit rate check
    public float CurrentLuck => 
        (baseLuck + UtilsWarrior.PER_LEVEL_WARRIOR_GAIN_LUCK * (levelStatLuck - 1)) *
        PlayerManager.Instance.FisherGoldenSeriesMultiplier +
        (_inspirationModifier - 1f); // so adds .1


    // ----- SHIELD 

    public float MaxShield { get; private set; }
    public float CurrentShield { get; private set; }


    public event Action OnHpChange;
    public event Action<int> OnTakeDamage;
    public event Action<int> OnHeal;

    public event Action OnShieldChange;


    public event Action<int> OnAddMap;




    public PlayerFightData()
    {
        GenerateBaseStats();
    }

    public PlayerFightData(PlayerFightSaveData saveData)
    {
        GenerateBaseStats();

        availableMaps = new List<int>();
        availableMaps.AddRange(saveData.availableMaps);


        levelStatMaxHp = saveData.levelStatMaxHp;

        levelStatAtk = saveData.levelStatAtk;
        levelStatDef = saveData.levelStatDef;

        levelStatAtkSpd = saveData.levelStatAtkSpd;

        levelStatCritRate = saveData.levelStatCritRate;
        levelStatCritDmg = saveData.levelStatCritDmg;

        levelStatLuck = saveData.levelStatLuck;


        levelStatMaxHp = Math.Min(levelStatMaxHp, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_MAXHP);
        levelStatAtk = Math.Min(levelStatAtk, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_ATK);
        levelStatDef = Math.Min(levelStatDef, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_DEF);
        levelStatAtkSpd = Math.Min(levelStatAtkSpd, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_ATK_SPEED);
        levelStatCritRate = Math.Min(levelStatCritRate, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_CRIT_RATE);
        levelStatCritDmg = Math.Min(levelStatCritDmg, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_CRIT_DMG);
        levelStatLuck = Math.Min(levelStatLuck, UtilsWarrior.PER_LEVEL_WARRIOR_MAX_LUCK);


        AvailableStatPoints = saveData.availableStatPoints;

        CurrentLevel = saveData.currentLevel;
        CurrentExp = saveData.currentExp;

        int sumLevels =
            levelStatMaxHp + levelStatAtk + levelStatDef + levelStatAtkSpd + levelStatCritRate + levelStatCritDmg + levelStatLuck +
            //startLevelMaxHp + startLevelAtk + startLevelDef + startLevelAtkSpd + startLevelCritRate + startLevelCritDmg + startLevelLuck +
            AvailableStatPoints +
            1;

        CurrentLevel = Math.Min(CurrentLevel, sumLevels);

        // reset available points to 0 if previous bugs occured, and set exp to 0
        if (CurrentLevel >= UtilsWarrior.MAX_LEVEL_WARRIOR)
        {
            AvailableStatPoints = UtilsWarrior.MAX_LEVEL_WARRIOR - 1 -
                levelStatMaxHp - levelStatAtk - levelStatDef - levelStatAtkSpd - levelStatCritRate - levelStatCritDmg - levelStatLuck;
            CurrentExp = 0;
        }

        CurrentHp = MaxHp;

        // at start check if has iron skin
        CheckShieldAtStart();
    }

    private void GenerateBaseStats()
    {
        availableMaps = new List<int>
        {
            0 // Woods map by default
        };


        CurrentLevel = 1;
        CurrentExp = 0;


        levelStatMaxHp = startLevelMaxHp;

        levelStatAtk = startLevelAtk;
        levelStatDef = startLevelDef;

        levelStatAtkSpd = startLevelAtkSpd;

        levelStatCritRate = startLevelCritRate;
        levelStatCritDmg = startLevelCritDmg;

        levelStatLuck = startLevelLuck;


        baseMaxHp = BASE_MAXHP + ((float)PlayerManager.Instance.PlayerAlchemistData.StatPermaMaxHpCounter * ALCHEMIST_PERMA_ADD_MAXHP);
        CurrentHp = MaxHp;

        baseAtk = BASE_ATK + ((float)PlayerManager.Instance.PlayerAlchemistData.StatPermaAttackCounter * ALCHEMIST_PERMA_ADD_ATTACK);
        baseDef = BASE_DEF + ((float)PlayerManager.Instance.PlayerAlchemistData.StatPermaDefenseCounter * ALCHEMIST_PERMA_ADD_DEFENSE);

        baseAtkSpd = BASE_ATKSPD;   // 1 attack per second
        //baseAtkSpd = 5f;   // 1 attack per second

        baseCritRate = BASE_CRITRATE;  // 5%
        baseCritDmg = BASE_CRITDMG;  // +50%

        baseLuck = 0f;
    }

    #region STATS

    public void AddExp(long amount)
    {
        base.AddExp(
            amount,
            level => level >= UtilsWarrior.MAX_LEVEL_WARRIOR,
            () => ExpToNextLevel
        );
    }

    public void IncreaseLevelStat(int id, int amount)
    {
        switch (id)
        {
            default: Debug.Log("Increased stat id not correct. " + id); break;
            case UtilsPlayer.ID_WARRIOR_MAXHP: levelStatMaxHp += amount; break;
            case UtilsPlayer.ID_WARRIOR_ATK: levelStatAtk += amount; break;
            case UtilsPlayer.ID_WARRIOR_DEF: levelStatDef += amount; break;
            case UtilsPlayer.ID_WARRIOR_ATKSPD: levelStatAtkSpd += amount; break;
            case UtilsPlayer.ID_WARRIOR_CRITRATE: levelStatCritRate += amount; break;
            case UtilsPlayer.ID_WARRIOR_CRITDMG: levelStatCritDmg += amount; break;
            case UtilsPlayer.ID_WARRIOR_LUCK: levelStatLuck += amount; break;
        }

        InvokeStatChange(id, amount);
    }

    public void SetHp(float value)
    {
        CurrentHp = value;
    }

    #endregion

    #region FIGHT

    private int GetDisplayDamage(float damage)
    {
        if (damage == 0f)
        {
            return 0;
        }
        else if (damage > 0f && damage < 1f)
        {
            return 1;
        }
        else
        {
            return Mathf.FloorToInt(damage);
        }
    }

    private void CheckShieldAtStart()
    {
        if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.IronSkin))
        {
            MaxShield = MaxHp * 0.2f;
            CurrentShield = MaxShield;
        }
    }

    public void TakeDamage(EnemyData data)
    {
        // can't take less than 0 or it will cure
        float baseDamage = data.CurrentAtk;
        float total;

        if (UnityEngine.Random.value <= data.CurrentCritRate)
        {
            baseDamage *= data.CurrentCritDmg;
        }

        total = Mathf.Max(0f, baseDamage - CurrentDef);

        // check for shield
        if(CurrentShield > 0)
        {
            float restFromShield = 0;

            // damage shield
            CurrentShield -= total;

            // if shield breaks, damage hp
            if(CurrentShield < 0)
            {
                restFromShield = Mathf.Abs(CurrentShield);
                CurrentHp -= restFromShield;
                CurrentShield = 0;
            }

            OnShieldChange?.Invoke();
        }
        else
        {
            // subtract total to hp
            CurrentHp -= total;
        }
            

        //Debug.Log("damage: " + total);
        //Debug.Log("current hp: " + currentHp);
        //Debug.Log("max hp: " + MaxHp);

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        if (CurrentHp <= 0f)
        {
            CurrentHp = 0;
        }

        OnHpChange?.Invoke();
    }

    /// <summary>
    /// Used only for testing
    /// </summary>
    public void TakeDamageCheat(float damage)
    {
        float total = Mathf.Max(0f, damage);

        // subtract total to hp
        CurrentHp -= total;

        OnHpChange?.Invoke();

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        if (CurrentHp <= 0f)
        {
            CurrentHp = 0;
        }
    }

    /// <summary>
    /// Heal Warrior
    /// </summary>
    public void Heal(float value)
    {
        float total = Mathf.Max(0f, value);

        // subtract total to hp
        CurrentHp += total;

        OnHpChange?.Invoke();

        OnHeal?.Invoke(Mathf.FloorToInt(total));

        CurrentHp = Mathf.Max(0, CurrentHp);
    }

    public void ResetAfterStage()
    {
        CurrentHp = MaxHp;
        CheckShieldAtStart();

        //Debug.Log("Max hp after death: " + currentHp);

        OnHpChange?.Invoke();
        OnShieldChange?.Invoke();
    }


    public void ChangeStatModifier(int id, float value, bool adding)
    {
        //Debug.Log("id stat: " + id + ", val: " + value + ", adding: " + adding.ToString());
        switch (id)
        {
            case UtilsPlayer.ID_WARRIOR_MAXHP: if (adding) maxHpModifier += value; else maxHpModifier -= value; break;
            case UtilsPlayer.ID_WARRIOR_ATK: if (adding) atkModifier += value; else atkModifier -= value; break;
            case UtilsPlayer.ID_WARRIOR_DEF: if (adding) defModifier += value; else defModifier -= value; break;
            case UtilsPlayer.ID_WARRIOR_ATKSPD: if (adding) atkSpdModifier += value; else atkSpdModifier -= value; break;
            case UtilsPlayer.ID_WARRIOR_CRITDMG: if (adding) critDmgModifier += value; else critDmgModifier -= value; break;
        }
    }

    public void ResetStatModifiers()
    {
        maxHpModifier = 1f;
        atkModifier = 1f;
        defModifier = 1f;
        atkSpdModifier = 1f;
        critDmgModifier = 1f;
    }

    #endregion


    public void AddAvailableMap(int id)
    {
        if (availableMaps.Contains(id)) return;

        availableMaps.Add(id);
        OnAddMap?.Invoke(id);
    }
}
