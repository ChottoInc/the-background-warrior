using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Farmer/Companion Data", fileName = "CompanionData_")]
public class CompanionSO : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string companionName;

    [Space(10)]
    [SerializeField] Sprite iconCompanion;

    [Space(10)]
    [SerializeField] GameObject prefab;

    [Header("Combat")]
    [Range(0,1)]
    [SerializeField] float baseAtkPerc;
    [SerializeField] float baseAtkSpd;

    public int Id => id;
    public string CompanionName => companionName;

    public Sprite IconCompanion => iconCompanion;

    public GameObject Prefab => prefab;

    public float BaseAtkPerc => baseAtkPerc;
    public float BaseAtkSpd => baseAtkSpd;
}
