using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    private const float PER_STAGE_GAIN_LEVEL = 1f;
    private const float PER_ENEMY_GAIN_LEVEL = 0.0202f;


    private const float MAXHP_GAIN_PER_LEVEL = 3.5f;
    private const float ATK_GAIN_PER_LEVEL = 0.1f;
    private const float DEF_GAIN_PER_LEVEL = 0.06f;




    private CombatMapSO mapSO;


    public EnemySO EnemySO { get; private set; }


    public int CurrentLevel { get; private set; }

    public float MaxHp { get; private set; }
    public float CurrentHp { get; private set; }

    public float CurrentAtk { get; private set; }
    public float CurrentDef { get; private set; }

    public float CurrentAtkSpd { get; private set; }
    public float CurrentCritRate { get; private set; }
    public float CurrentCritDmg { get; private set; }


    private List<UtilsEnemy.EnemyAffectedStatus> _affectedStatuses;


    public event Action<int> OnTakeDamage;




    public EnemyData(EnemySO enemySO)
    {
        EnemySO = enemySO;

        _affectedStatuses = new List<UtilsEnemy.EnemyAffectedStatus>();
    }


    public EnemyData(EnemySO enemySO, CombatMapSO mapSO)
    {
        EnemySO = enemySO;
        this.mapSO = mapSO;

        CurrentLevel = Mathf.FloorToInt(CalculateLevel());

        MaxHp = CalculateMaxHp();
        CurrentHp = MaxHp;

        CurrentAtk = CalculateAtk();
        CurrentDef = CalculateDef();
        
        CurrentAtkSpd = CalculateAtkSpd();
        
        CurrentCritRate = CalculateCritRate();
        CurrentCritDmg = CalculateCritDmg();

        _affectedStatuses = new List<UtilsEnemy.EnemyAffectedStatus>();
    }   

    private float CalculateLevel()
    {
        float result;

        result = mapSO.BaseEnemyLevel +
            (StageManager.Instance.CurrentStage - 1) * PER_STAGE_GAIN_LEVEL +
            (StageManager.Instance.CurrentEnemyIndex - 1) * PER_ENEMY_GAIN_LEVEL;

        return result;
    }

    private float CalculateMaxHp()
    {
        // exp growth
        float p = 1.2f;
        return EnemySO.BaseMaxHp + MAXHP_GAIN_PER_LEVEL * Mathf.Pow(CurrentLevel - 1, p);
    }

    private float CalculateAtk()
    {
        // exp growth
        float p = 1.3f;
        return EnemySO.BaseAtk + ATK_GAIN_PER_LEVEL * Mathf.Pow(CurrentLevel - 1, p);
    }

    private float CalculateDef()
    {
        // exp growth
        float p = 1.1f;
        return EnemySO.BaseDef + DEF_GAIN_PER_LEVEL * Mathf.Pow(CurrentLevel - 1, p);
    }

    private float CalculateAtkSpd()
    {
        float minDelay = 0.6f;
        float maxDelay = 1.0f;
        float k = 80f;

        float t = CurrentLevel / (CurrentLevel + k);
        return Mathf.Lerp(maxDelay, minDelay, t);
    }

    private float CalculateCritRate()
    {
        float maxCritRate = 0.5f;

        // controls how fast the crit rate goes, the bigger the slower
        float k = 100f;

        return Mathf.Min(maxCritRate, maxCritRate * (CurrentLevel / (CurrentLevel + k)));
    }

    private float CalculateCritDmg()
    {
        float maxCritDmg = 1.5f;

        // controls how fast the crit dmg goes, the bigger the slower
        float k = 200f;

        return EnemySO.BaseCritDmg + maxCritDmg * (CurrentLevel / (CurrentLevel + k));
    }

    #region COMABT SYSTEM

    private int GetDisplayDamage(float damage)
    {
        if(damage == 0f)
        {
            return 0;
        }
        else if(damage > 0f && damage < 1f)
        {
            return 1;
        }
        else
        {
            return Mathf.FloorToInt(damage);
        }
    }

    public void TakeDamage(PlayerFightData data)
    {
        if (CurrentHp <= 0) return;

        // can't take less than 0 or it will cure

        float baseDamage = data.CurrentAtk;
        float total;

        // base chance
        if(UnityEngine.Random.value <= data.CurrentCritRate)
        {
            baseDamage *= data.CurrentCritDmg;
        }
        else if (UnityEngine.Random.value <= data.CurrentLuck)
        {
            // if base crit rate chance doesn't go, check for luck for extra roll on crit rate
            if (UnityEngine.Random.value <= data.CurrentCritRate)
            {
                baseDamage *= data.CurrentCritDmg;
            }
        }

        total = Mathf.Max(0f, baseDamage - CurrentDef);

        // subtract total to hp
        CurrentHp -= total;

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        // heal warrior if poisoned
        if (HasStatus(UtilsEnemy.EnemyAffectedStatus.Poisoned))
        {
            var poisonSpell = PlayerManager.Instance.PlayerMageData.GetSpellByType(UtilsMage.MageSpellType.PoisonGas);
            float healBy = data.MaxHp * poisonSpell.PercentageLifesteal;
            //Debug.Log("player heals by" + poisonSpell.PercentageLifesteal.ToString() + " for: " + healBy);
            data.Heal(healBy);
        }

        CurrentHp = Mathf.Max(CurrentHp, 0f);
    }

    /// <summary>
    /// Used only for testing
    /// </summary>
    public void TakeDamageCheat(float damage)
    {
        float total = Mathf.Max(0f, damage);

        // subtract total to hp
        CurrentHp -= total;

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        CurrentHp = Mathf.Max(CurrentHp, 0f);
    }

    public void TakeDamage(float damage)
    {
        float total = Mathf.Max(0f, damage);

        // subtract total to hp
        CurrentHp -= total;

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        CurrentHp = Mathf.Max(CurrentHp, 0f);
    }

    public void TakeDamage(CompanionData data)
    {
        if (CurrentHp <= 0) return;

        // can't take less than 0 or it will cure
        float total = MaxHp * data.CurrentAtkPerc;

        total = Mathf.Max(0f, total);

        // subtract total to hp
        CurrentHp -= total;

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        // companions can't kill enemies, at most they reach 1 hp
        CurrentHp = Mathf.Max(CurrentHp, 1f);
    }

    public void TakeDamage(SummonData data)
    {
        if (CurrentHp <= 0) return;
        
        // can't take less than 0 or it will cure
        float total = MaxHp * data.CurrentAtkPerc;

        total = Mathf.Max(0f, total);
        //Debug.Log("Total dmg: " + total);

        // subtract total to hp
        CurrentHp -= total;

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        // companions can't kill enemies, at most they reach 1 hp
        //CurrentHp = Mathf.Max(CurrentHp, 1f);

        // set to 0 if lower
        CurrentHp = Mathf.Max(CurrentHp, 0f);
    }

    public void TakeDamageFromSpell(float damage)
    {
        if (CurrentHp <= 0) return;

        float total = damage;

        if (HasStatus(UtilsEnemy.EnemyAffectedStatus.Chilled))
        {
            var chillWindSpell = PlayerManager.Instance.PlayerMageData.GetSpellByType(UtilsMage.MageSpellType.ChillWind);
            total += total * chillWindSpell.PercentageMoreDamageFromSpells;
            //Debug.Log("enemy affcted by chilled");
            //Debug.Log("increase by: " + chillWindSpell.PercentageMoreDamageFromSpells);
        }

        total = Mathf.Max(0f, total);

        //Debug.Log("Enemy hit by " + spellData.SpellSO.SpellType.ToString() + " for: " + total);

        // subtract total to hp
        CurrentHp -= total;

        OnTakeDamage?.Invoke(GetDisplayDamage(total));

        // set to 0 if lower
        CurrentHp = Mathf.Max(CurrentHp, 0f);
    }

    public bool HasStatus(UtilsEnemy.EnemyAffectedStatus status)
    {
        return _affectedStatuses.Contains(status);
    }

    public void AddStatus(UtilsEnemy.EnemyAffectedStatus status)
    {
        _affectedStatuses.Add(status);
    }

    public void RemoveStatus(UtilsEnemy.EnemyAffectedStatus status)
    {
        _affectedStatuses.Remove(status);
    }

    public void SetDead()
    {
        CurrentHp = 0;
    }

    #endregion
}
