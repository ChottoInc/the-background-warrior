using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Warrior Data", fileName = "WarriorData")]
public class PlayerJobWarriorSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainMaxHp { get; private set; }
    [field: SerializeField] public float PerLevelGainAtk { get; private set; }
    [field: SerializeField] public float PerLevelGainDef { get; private set; }
    [field: SerializeField] public float PerLevelGainAtkSpd { get; private set; }
    [field: SerializeField] public float PerLevelGainCritRate { get; private set; }
    [field: SerializeField] public float PerLevelGainCritDmg { get; private set; }
    [field: SerializeField] public float PerLevelGainLuck { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public int MaxLevelMaxHp { get; private set; }
    [field: SerializeField] public int MaxLevelAtk { get; private set; }
    [field: SerializeField] public int MaxLevelDef { get; private set; }
    [field: SerializeField] public int MaxLevelAtkSpd { get; private set; }
    [field: SerializeField] public int MaxLevelCritRate { get; private set; }
    [field: SerializeField] public int MaxLevelCritDmg { get; private set; }
    [field: SerializeField] public int MaxLevelLuck { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }


    public void SetPerLevelGainMaxHp(float value)
    {
        PerLevelGainMaxHp = value;
    }

    public void SetPerLevelGainAtk(float value)
    {
        PerLevelGainAtk = value;
    }

    public void SetPerLevelGainDef(float value)
    {
        PerLevelGainDef = value;
    }

    public void SetPerLevelGainAtkSpd(float value)
    {
        PerLevelGainAtkSpd = value;
    }

    public void SetPerLevelGainCritRate(float value)
    {
        PerLevelGainCritRate = value;
    }

    public void SetPerLevelGainCritDmg(float value)
    {
        PerLevelGainCritDmg = value;
    }

    public void SetPerLevelGainLuck(float value)
    {
        PerLevelGainLuck = value;
    }


    public void SetMaxLevelMaxHp(int value)
    {
        MaxLevelMaxHp = value;
    }

    public void SetMaxLevelAtk(int value)
    {
        MaxLevelAtk = value;
    }

    public void SetMaxLevelDef(int value)
    {
        MaxLevelDef = value;
    }

    public void SetMaxLevelAtkSpd(int value)
    {
        MaxLevelAtkSpd = value;
    }

    public void SetMaxLevelCritRate(int value)
    {
        MaxLevelCritRate = value;
    }

    public void SetMaxLevelCritDmg(int value)
    {
        MaxLevelCritDmg = value;
    }

    public void SetMaxLevelLuck(int value)
    {
        MaxLevelLuck = value;
    }




    public void SetBaseExpGrowth(float value)
    {
        BaseExpGrowth = value;
    }

    public void SetExpoExpGrowth(float value)
    {
        ExpoExpGrowth = value;
    }

    public void SetFlatExpGrowth(float value)
    {
        FlatExpGrowth = value;
    }
}
