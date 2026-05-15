using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Fisher/Fish Group Data", fileName = "FishGroupData_")]
public class FishGroupSO : ScriptableObject
{
    [SerializeField] UtilsGather.FishGroupType groupType;
    [SerializeField] string groupName;

    [TextArea]
    [SerializeField] string groupDesc;

    [Space(10)]
    [SerializeField] FishSO[] fishes;


    public UtilsGather.FishGroupType GroupType => groupType;
    public string GroupName => groupName;
    public string GroupDesc => groupDesc;


    public FishSO[] Fishes => fishes;
}
