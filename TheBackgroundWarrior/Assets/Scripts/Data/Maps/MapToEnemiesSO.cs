using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Map To Enemies Data", fileName = "MapToEnemiesData_")]
public class MapToEnemiesSO : ScriptableObject
{
    [SerializeField] CombatMapSO mapSO;
    [SerializeField] UtilsGeneral.GeneralChances<EnemySO>[] possibleEnemies;

    public CombatMapSO MapSO => mapSO;
    public UtilsGeneral.GeneralChances<EnemySO>[] PossibleEnemies => possibleEnemies;
}
