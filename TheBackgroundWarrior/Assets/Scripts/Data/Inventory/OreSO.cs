using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Ore Data", fileName = "OreData_")]
public class OreSO : ItemSO
{
    [Space(10)]
    [SerializeField] MetalSO refinedMetal;

    public MetalSO RefinedMetal => refinedMetal;
}
