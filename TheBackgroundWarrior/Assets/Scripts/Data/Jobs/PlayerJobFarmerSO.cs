using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Farmer Data", fileName = "FarmerData")]
public class PlayerJobFarmerSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainGreenthumb { get; private set; }
    [field: SerializeField] public float PerLevelGainAgronomy { get; private set; }
    [field: SerializeField] public float PerLevelGainKindness { get; private set; }
    [field: SerializeField] public float PerLevelGainLuck { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public int MaxLevelGreenthumb { get; private set; }
    [field: SerializeField] public int MaxLevelAgronomy { get; private set; }
    [field: SerializeField] public int MaxLevelKindness { get; private set; }
    [field: SerializeField] public int MaxLevelLuck { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }


    public void SetPerLevelGainGreenthumb(float value)
    {
        PerLevelGainGreenthumb = value;
    }

    public void SetPerLevelGainAgronomy(float value)
    {
        PerLevelGainAgronomy = value;
    }

    public void SetPerLevelGainKindness(float value)
    {
        PerLevelGainKindness = value;
    }

    public void SetPerLevelGainLuck(float value)
    {
        PerLevelGainLuck = value;
    }



    public void SetMaxLevelGreenthumb(int value)
    {
        MaxLevelGreenthumb = value;
    }

    public void SetMaxLevelAgronomy(int value)
    {
        MaxLevelAgronomy = value;
    }

    public void SetMaxLevelKindness(int value)
    {
        MaxLevelKindness = value;
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
