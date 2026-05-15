using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Farmer/Crop Data", fileName = "CropData_")]
public class CropSO : ScriptableObject
{
    [SerializeField] int id;

    [Space(10)]
    [SerializeField] float baseGrowthTime;

    [Space(10)]
    [SerializeField] string cropName;

    [Space(10)]
    [SerializeField] Sprite spriteSeed;
    [SerializeField] Sprite[] spriteCrop;

    [Space(10)]
    [SerializeField] CompanionSO[] attractedCompanions;

    [Space(10)]
    [SerializeField] long rewardedExp;

    public int Id => id;

    public float BaseGrowthTime => baseGrowthTime;

    public string CropName => cropName;

    public Sprite SpriteSeed => spriteSeed;
    public Sprite[] SpriteCrop => spriteCrop;

    public CompanionSO[] AttractedCompanions => attractedCompanions;

    public long RewardedExp => rewardedExp; 
}
