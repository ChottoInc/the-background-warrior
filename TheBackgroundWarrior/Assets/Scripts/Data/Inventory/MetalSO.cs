using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Metal Data", fileName = "MetalData_")]
public class MetalSO : ItemSO
{
    [Space(10)]
    [SerializeField] int requiredOres;
    [SerializeField] UtilsGather.RockType rockType;

    public int RequiredOres => requiredOres;
    public UtilsGather.RockType RockType => rockType;
}
