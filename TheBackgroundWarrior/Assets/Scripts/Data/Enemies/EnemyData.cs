using System;
using UnityEngine;

public class EnemyData
{
    private const float PER_STAGE_GAIN_LEVEL = 1f;
    private const float PER_ENEMY_GAIN_LEVEL = 0.0202f;


    private const float MAXHP_GAIN_PER_LEVEL = 3.5f;
    private const float ATK_GAIN_PER_LEVEL = 0.1f;
    private const float DEF_GAIN_PER_LEVEL = 0.06f;


    private EnemySO enemySO;


    private int currentLevel;

    private float maxHp;
    private float currentHp;

    private float currentAtk;
    private float currentDef;

    private float currentAtkSpd;

    private float currentCritRate;
    private float currentCritDmg;


    private CombatMapSO mapSO;


    public EnemySO EnemySO => enemySO;


    public int CurrentLevel => currentLevel;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    public float CurrentAtk => currentAtk;
    public float CurrentDef => currentDef;

    public float CurrentAtkSpd => currentAtkSpd;
    public float CurrentCritRate => currentCritRate;
    public float CurrentCritDmg => currentCritDmg;


    public event Action<int> OnTakeDamage;




    public EnemyData(EnemySO enemySO)
    {
        this.enemySO = enemySO;
    }


    public EnemyData(EnemySO enemySO, CombatMapSO mapSO)
    {
        this.enemySO = enemySO;
        this.mapSO = mapSO;

        currentLevel = Mathf.FloorToInt(CalculateLevel());

        maxHp = CalculateMaxHp();
        currentHp = maxHp;

        currentAtk = CalculateAtk();
        currentDef = CalculateDef();

        currentAtkSpd = CalculateAtkSpd();

        currentCritRate = CalculateCritRate();
        currentCritDmg = CalculateCritDmg();
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
        float p = 1.25f;
        return enemySO.BaseMaxHp + MAXHP_GAIN_PER_LEVEL * Mathf.Pow(currentLevel - 1, p);
    }

    private float CalculateAtk()
    {
        // exp growth
        float p = 1.35f;
        return enemySO.BaseAtk + ATK_GAIN_PER_LEVEL * Mathf.Pow(currentLevel - 1, p);
    }

    private float CalculateDef()
    {
        // exp growth
        float p = 1.12f;
        return enemySO.BaseDef + DEF_GAIN_PER_LEVEL * Mathf.Pow(currentLevel - 1, p);
    }

    private float CalculateAtkSpd()
    {
        float minDelay = 0.6f;
        float maxDelay = 1.0f;
        float k = 80f;

        float t = currentLevel / (currentLevel + k);
        return Mathf.Lerp(maxDelay, minDelay, t);
    }

    private float CalculateCritRate()
    {
        float maxCritRate = 0.5f;

        // controls how fast the crit rate goes, the bigger the slower
        float k = 100f;

        return Mathf.Min(maxCritRate, maxCritRate * (currentLevel / (currentLevel + k)));
    }

    private float CalculateCritDmg()
    {
        float maxCritDmg = 1.5f;

        // controls how fast the crit dmg goes, the bigger the slower
        float k = 200f;

        return enemySO.BaseCritDmg + maxCritDmg * (currentLevel / (currentLevel + k));
    }

    /*
    private float CalculateMaxHp()
    {
        float result;

        result =
            baseMaxHp *
            Mathf.Pow(PER_STAGE_GAIN_MAXHP, StageManager.Instance.CurrentStage - 1) *
            Mathf.Pow(PER_SUBSTAGE_MULTIPLIER_MAXHP, StageManager.Instance.CurrentEnemyIndex - 1) *
            StageManager.Instance.CurrentPrestige;

        return result;
    }

    private float CalculateAtk()
    {
        float result;

        result =
            baseAtk *
            Mathf.Pow(PER_STAGE_GAIN_ATK, StageManager.Instance.CurrentStage - 1) *
            Mathf.Pow(PER_SUBSTAGE_MULTIPLIER_ATK, StageManager.Instance.CurrentEnemyIndex - 1) *
            StageManager.Instance.CurrentPrestige;

        return result;
    }

    private float CalculateDef()
    {
        float result;

        result =
            baseDef *
            Mathf.Pow(PER_STAGE_GAIN_DEF, StageManager.Instance.CurrentStage - 1) *
            Mathf.Pow(PER_SUBSTAGE_MULTIPLIER_DEF, StageManager.Instance.CurrentEnemyIndex - 1) *
            StageManager.Instance.CurrentPrestige;

        return result;
    }

    */

    #region COMABT SYSTEM

    public void TakeDamage(PlayerFightData data)
    {
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

        total = Mathf.Max(0f, baseDamage - currentDef);

        // subtract total to hp
        currentHp -= total;

        OnTakeDamage?.Invoke(Mathf.FloorToInt(total));

        if(currentHp <= 0f)
        {
            currentHp = 0;
        }
    }

    /// <summary>
    /// Used only for testing
    /// </summary>
    public void TakeDamageCheat(float damage)
    {
        float total = Mathf.Max(0f, damage);

        // subtract total to hp
        currentHp -= total;

        OnTakeDamage?.Invoke(Mathf.FloorToInt(total));

        if (currentHp <= 0f)
        {
            currentHp = 0;
        }
    }

    public void TakeDamage(CompanionData data)
    {
        // can't take less than 0 or it will cure
        float total = maxHp * data.CurrentAtkPerc;

        total = Mathf.Max(0f, total);

        // subtract total to hp
        currentHp -= total;

        OnTakeDamage?.Invoke(Mathf.FloorToInt(total));

        // companions can't kill enemies, at most they reach 1 hp
        currentHp = MathF.Max(currentHp, 1f);
    }

    public void SetDead()
    {
        currentHp = 0;
    }

    #endregion
}
