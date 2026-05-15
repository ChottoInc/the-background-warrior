using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Fisher Data", fileName = "FisherData")]
public class PlayerJobFisherSO : AbstractPlayerJobData
{
    [field: SerializeField] public float PerLevelGainCalmness { get; private set; }
    [field: SerializeField] public float PerLevelGainReflex { get; private set; }
    [field: SerializeField] public float PerLevelGainKnowledge { get; private set; }
    [field: SerializeField] public float PerLevelGainLuck { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public int MaxLevelCalmness { get; private set; }
    [field: SerializeField] public int MaxLevelReflex { get; private set; }
    [field: SerializeField] public int MaxLevelKnowledge { get; private set; }
    [field: SerializeField] public int MaxLevelLuck { get; private set; }


    [field: Space(10)]
    [field: SerializeField] public float BaseExpGrowth { get; private set; }
    [field: SerializeField] public float ExpoExpGrowth { get; private set; }
    [field: SerializeField] public float FlatExpGrowth { get; private set; }


    public void SetPerLevelGainCalmness(float value)
    {
        PerLevelGainCalmness = value;
    }

    public void SetPerLevelGainReflex(float value)
    {
        PerLevelGainReflex = value;
    }

    public void SetPerLevelGainKnowledge(float value)
    {
        PerLevelGainKnowledge = value;
    }

    public void SetPerLevelGainLuck(float value)
    {
        PerLevelGainLuck = value;
    }


    public void SetMaxLevelCalmness(int value)
    {
        MaxLevelCalmness = value;
    }

    public void SetMaxLevelReflex(int value)
    {
        MaxLevelReflex = value;
    }

    public void SetMaxLevelKnowledge(int value)
    {
        MaxLevelKnowledge = value;
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
