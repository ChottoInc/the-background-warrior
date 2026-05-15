using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Fish Data", fileName = "FishData_")]
public class FishSO : ItemSO
{
    [SerializeField] UtilsItem.FishRarity fishRarity;
    [SerializeField] UtilsGeneral.DayMoment spawnDayMoment;

    public UtilsItem.FishRarity FishRarity => fishRarity;
    public UtilsGeneral.DayMoment SpawnDayMoment => spawnDayMoment;
}
