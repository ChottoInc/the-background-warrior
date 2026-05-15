using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map/Combat/Combat Map Data", fileName = "CombatMapData_")]
public class CombatMapSO : ScriptableObject
{
    [SerializeField] int idMap;
    [SerializeField] string mapName;
    [SerializeField] string mapSceneName;

    [Space(10)]
    [SerializeField] UtilsCombatMap.MapDifficulty mapDifficulty;
    [SerializeField] int baseEnemyLevel;
    [SerializeField] int enemiesPerStage;
    [SerializeField] int stages;

    [Space(10)]
    [SerializeField] CombatMapSO nextMap;

    public int IdMap => idMap;
    public string MapName => mapName;
    public string MapSceneName => mapSceneName;

    public UtilsCombatMap.MapDifficulty MapDifficuty => mapDifficulty;
    public int BaseEnemyLevel => baseEnemyLevel;
    public int EnemiesPerStage => enemiesPerStage;
    public int Stages => stages;

    public CombatMapSO NextMap => nextMap;
}
