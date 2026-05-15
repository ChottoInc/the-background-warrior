using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Blacksmith Data", fileName = "BlacksmithData")]
public class PlayerJobBlacksmithSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainCraftSpeed { get; private set; }
    [field: SerializeField] public float PerLevelGainEfficiency { get; private set; }
    [field: SerializeField] public float PerLevelGainLuck { get; private set; }
    [field: SerializeField] public float PerLevelGainMetallurgy { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public int MaxLevelCraftSpeed { get; private set; }
    [field: SerializeField] public int MaxLevelEfficiency { get; private set; }
    [field: SerializeField] public int MaxLevelLuck { get; private set; }
    [field: SerializeField] public int MaxLevelMetallurgy { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public float HelmetMaxHpLinearGrowth { get; private set; }
    [field: SerializeField] public float HelmetMaxHpQuadraticGrowth { get; private set; }

    [field: Space(10)]
    [field: SerializeField] public float ArmorDefLinearGrowth { get; private set; }
    [field: SerializeField] public float ArmorDefQuadraticGrowth { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public float GlovesAtkSpdLinearGrowth { get; private set; }
    [field: SerializeField] public float GlovesAtkSpdQuadraticGrowth { get; private set; }
    [field: SerializeField] public float GlovesCritDmgLinearGrowth { get; private set; }
    [field: SerializeField] public float GlovesCritDmgQuadraticGrowth { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public float BootsDefLinearGrowth { get; private set; }
    [field: SerializeField] public float BootsDefQuadraticGrowth { get; private set; }
    [field: SerializeField] public float BootsCritRateLinearGrowth { get; private set; }
    [field: SerializeField] public float BootsCritRateQuadraticGrowth { get; private set; }



    public void SetPerLevelGainCraftSpeed(float value)
    {
        PerLevelGainCraftSpeed = value;
    }

    public void SetPerLevelGainEfficiency(float value)
    {
        PerLevelGainEfficiency = value;
    }

    public void SetPerLevelGainLuck(float value)
    {
        PerLevelGainLuck = value;
    }

    public void SetPerLevelGainMetallurgy(float value)
    {
        PerLevelGainMetallurgy = value;
    }


    public void SetMaxLevelCraftSpeed(int value)
    {
        MaxLevelCraftSpeed = value;
    }

    public void SetMaxLevelEfficiency(int value)
    {
        MaxLevelEfficiency = value;
    }

    public void SetMaxLevelLuck(int value)
    {
        MaxLevelLuck = value;
    }

    public void SetMaxLevelMetallurgy(int value)
    {
        MaxLevelMetallurgy = value;
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



    public void SetHelmetMaxHpLinearGrowth(float value)
    {
        HelmetMaxHpLinearGrowth = value;
    }

    public void SetHelmetMaxHpQuadraticGrowth(float value)
    {
        HelmetMaxHpQuadraticGrowth = value;
    }

    public void SetArmorDefLinearGrowth(float value)
    {
        ArmorDefLinearGrowth = value;
    }

    public void SetArmorDefQuadraticGrowth(float value)
    {
        ArmorDefQuadraticGrowth = value;
    }



    public void SetGlovesAtkSpdLinearGrowth(float value)
    {
        GlovesAtkSpdLinearGrowth = value;
    }

    public void SetGlovesAtkSpdQuadraticGrowth(float value)
    {
        GlovesAtkSpdQuadraticGrowth = value;
    }

    public void SetGlovesCritDmgLinearGrowth(float value)
    {
        GlovesCritDmgLinearGrowth = value;
    }

    public void SetGlovesCritDmgQuadraticGrowth(float value)
    {
        GlovesCritDmgQuadraticGrowth = value;
    }




    public void SetBootsDefLinearGrowth(float value)
    {
        BootsDefLinearGrowth = value;
    }

    public void SetBootsDefQuadraticGrowth(float value)
    {
        BootsDefQuadraticGrowth = value;
    }

    public void SetBootsCritRateLinearGrowth(float value)
    {
        BootsCritRateLinearGrowth = value;
    }

    public void SetBootsCritRateQuadraticGrowth(float value)
    {
        BootsCritRateQuadraticGrowth = value;
    }
}
