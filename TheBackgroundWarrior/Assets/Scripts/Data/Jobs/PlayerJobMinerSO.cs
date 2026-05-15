using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Miner Data", fileName = "MinerData")]
public class PlayerJobMinerSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainPower { get; private set; }
    [field: SerializeField] public float PerLevelGainSmashSpeed { get; private set; }
    [field: SerializeField] public float PerLevelGainShockwave { get; private set; }
    [field: SerializeField] public float PerLevelGainLuck { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public int MaxLevelPower { get; private set; }
    [field: SerializeField] public int MaxLevelSmashSpeed { get; private set; }
    [field: SerializeField] public int MaxLevelShockwave { get; private set; }
    [field: SerializeField] public int MaxLevelLuck { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public float WeaponLinearGrowth { get; private set; }
    [field: SerializeField] public float WeaponQuadraticGrowth { get; private set; }


    public void SetPerLevelGainPower(float value)
    {
        PerLevelGainPower = value;
    }

    public void SetPerLevelGainSmashSpeed(float value)
    {
        PerLevelGainSmashSpeed = value;
    }

    public void SetPerLevelGainShockwave(float value)
    {
        PerLevelGainShockwave = value;
    }

    public void SetPerLevelGainLuck(float value)
    {
        PerLevelGainLuck = value;
    }


    public void SetMaxLevelPower(int value)
    {
        MaxLevelPower = value;
    }

    public void SetMaxLevelSmashSpeed(int value)
    {
        MaxLevelSmashSpeed = value;
    }

    public void SetMaxLevelShockwave(int value)
    {
        MaxLevelShockwave = value;
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


    public void SetWeaponLinearGrowth(float value)
    {
        WeaponLinearGrowth = value;
    }

    public void SetWeaponQuadraticGrowth(float value)
    {
        WeaponQuadraticGrowth = value;
    }
}
