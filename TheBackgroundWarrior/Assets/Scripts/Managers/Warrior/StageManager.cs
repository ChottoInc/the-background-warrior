using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private const int MAX_ENEMIES = 10;

    [Header("Enemies")]
    [SerializeField] MapToEnemiesSO mapEnemies;

    private bool finishedStartingEnemies;

    public bool FinishedStartingEnemies => finishedStartingEnemies;

    [Header("Drops")]
    [SerializeField] UtilsGeneral.GeneralChances<CardSO>[] possibleCards;

    [Header("UI")]
    [SerializeField] TMP_Text textStage;



    private int currentPrestige;
    private int currentStage;
    private int currentEnemyIndex;


    private CombatMapSaveData combatMapSaveData;


    private int currentStageKilled;



    private float offsetSpawn = 200f;
    private float ySpawn = 0f;
    private float minXSpawn, maxXSpawn;

    private List<Enemy> currentEnemies;

    // drop utils

    public UtilsGeneral.GeneralChances<CardSO>[] PossibleCards => possibleCards;



    // enemy utils

    public int CurrentPrestige => currentPrestige;
    public int CurrentStage => currentStage;
    public int CurrentEnemyIndex => currentEnemyIndex;


    // companions

    private List<GameObject> currentCompanionsObjs;


    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            PlayerManager.Instance.PlayerFarmerData.OnCompanionEquipped += SpawnCompanions;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.PlayerFarmerData.OnCompanionEquipped -= SpawnCompanions;
    }

    private void Start()
    {
        currentCompanionsObjs = new List<GameObject>();
    }


    public void Setup()
    {
        currentEnemies = new List<Enemy>();

        combatMapSaveData = SettingsManager.Instance.GetCombatMapSaveData(CombatManager.Instance.MapSO);

        if(combatMapSaveData != null)
        {
            currentPrestige = combatMapSaveData.reachedPrestige;
            currentStage = combatMapSaveData.currentStage;
        }
        else
        {
            currentPrestige = 0;
            currentStage = 1;
        }

        currentEnemyIndex = 1;

        minXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(0f + offsetSpawn, 0)).x; 
        maxXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(Screen.currentResolution.width - offsetSpawn, 0)).x;

        StartCoroutine(CoSpawnStartingEnemies());

        SpawnCompanions();

        UpdateStageUI();
    }

    #region ENEMY FUNCTIONS

    /// <summary>
    /// Called every time the game requires the next enemy
    /// </summary>
    private EnemyData GenerateEnemy()
    {
        //EnemySO randEnemySO = UtilsGeneral.GetRandomValueFromGeneralChanches(possibleEnemies);
        EnemySO randEnemySO = UtilsGeneral.GetRandomValueFromGeneralChanches(mapEnemies.PossibleEnemies);

        // generate data
        EnemyData result = new EnemyData(randEnemySO, CombatManager.Instance.MapSO);

        // increase index
        currentEnemyIndex++;

        return result;
    }

    private IEnumerator CoSpawnStartingEnemies(float timer = 0f)
    {
        finishedStartingEnemies = false;

        yield return new WaitForSeconds(timer);

        for (int i = 0; i < MAX_ENEMIES; i++)
        {
            float randX = Random.Range(minXSpawn, maxXSpawn);
            Vector2 spawnPos = new Vector2(randX, ySpawn);

            EnemyData data = GenerateEnemy();
            SpawnEnemy(data, spawnPos);

            yield return new WaitForSeconds(0.35f);
        }

        finishedStartingEnemies = true;
    }

    private void SpawnEnemy(EnemyData data, Vector2 spawnPos)
    {
        GameObject enemyObj = PoolManager.Instance.Pull(data.EnemySO.EnemyPoolName);

        if(enemyObj != null)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            currentEnemies.Add(enemy);

            enemyObj.transform.position = spawnPos;

            enemy.Setup(data, currentEnemyIndex - 1, SceneLoaderManager.SceneType.CombatMap);

            //Debug.Log("spawned enemy with id: " + (currentEnemyIndex - 1));
        }
        else
        {
            Debug.Log("Object not instatiated, probably the enemy so misses the pooling name");
        }
    }

    public void SpawnNextEnemy()
    {
        float randX = Random.Range(minXSpawn, maxXSpawn);
        Vector2 spawnPos = new Vector2(randX, ySpawn);

        EnemyData data = GenerateEnemy();
        SpawnEnemy(data, spawnPos);
    }

    public void RemoveFromCurrentEnemiesList(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
        //Debug.Log("removed enemy with id: " + enemy.EnemyIndex);
    }

    #endregion

    #region COMPANIONS

    private int GetCompanionSpawnIndex(CompanionData data)
    {
        for (int i = 0; i < currentCompanionsObjs.Count; i++)
        {
            Companion companion = currentCompanionsObjs[i].GetComponent<Companion>();
            if (companion.CurrentCompanionData.CompanionSO.Id == data.CompanionSO.Id)
                return i;
        }
        return -1;
    }

    private void SpawnCompanions()
    {
        StartCoroutine(CoSpawnCompanions(1f));
    }

    private IEnumerator CoSpawnCompanions(float timer = 0f)
    {
        yield return new WaitForSeconds(timer);

        List<int> companionIds = new List<int>();

        // get equipped companions
        var companions = PlayerManager.Instance.PlayerFarmerData.GetEquippedCompanions();


        // spawn whatever companion isn't spawned yet
        for (int i = 0; i < companions.Count; i++)
        {
            CompanionData data = companions[i].companionData;

            // get index
            int index = GetCompanionSpawnIndex(data);

            // if not spawned yet, can spawn it
            if(index == -1)
            {
                // random pos
                float randX = Random.Range(minXSpawn, maxXSpawn);
                Vector2 spawnPos = new Vector2(randX, ySpawn);

                // spawn
                SpawnCompanion(companions[i].companionData, spawnPos);
            }

            companionIds.Add(data.CompanionSO.Id);

            yield return new WaitForSeconds(1f);
        }

        // check the spawned and the current companions
        // if any of current isn't in the list it will be destroyed
        for (int i = currentCompanionsObjs.Count - 1; i >= 0; i--)
        {
            Companion companion = currentCompanionsObjs[i].GetComponent<Companion>();

            if (!companionIds.Contains(companion.CurrentCompanionData.CompanionSO.Id))
            {
                Destroy(currentCompanionsObjs[i]);
                currentCompanionsObjs.RemoveAt(i);
            }
        }

    }

    private void SpawnCompanion(CompanionData data, Vector2 spawnPos)
    {
        GameObject companionObj = Instantiate(data.CompanionSO.Prefab, spawnPos, Quaternion.identity);

        if (companionObj != null)
        {
            Companion companion = companionObj.GetComponent<Companion>();

            companion.SetupFight(data);

            currentCompanionsObjs.Add(companionObj);
        }
        else
        {
            Debug.Log("Object not instatiated, check the companion so");
        }
    }

    #endregion

    public void AddKill(int amount)
    {
        currentStageKilled += amount;
        //Debug.Log("current killed count: " + currentStageKilled);
    }

    

    /// <summary>
    /// If true resets player and move to next stage
    /// </summary>
    public bool NextStage()
    {
        if (currentStageKilled != currentEnemyIndex - 1) return false;

        //Debug.Log("------ NEXT STAGE -------------");
        //Debug.Log("current killed count: " + currentStageKilled);

        if (currentStage < CombatManager.Instance.MapSO.Stages && SettingsManager.Instance.IsAutoBattleOn)
        {
            // update stage
            currentStage++;

            // check for save
            HandleUpdateSaveMap();

            Resets();

            // restart wave
            StartCoroutine(CoSpawnStartingEnemies());

            // update ui
            UpdateStageUI();

            return true;
        }
        else
        {
            if (!SettingsManager.Instance.IsAutoBattleOn)
            {
                //Debug.Log("Is auto attle on: " + SettingsManager.Instance.IsAutoBattleOn);

                Resets();

                // restart wave
                StartCoroutine(CoSpawnStartingEnemies());

                // update ui
                UpdateStageUI();

                return true;
            }
            else
            {
                // I'm at the last stage of the map, so I need to go to the next map if auto battle is on
                currentStage++;

                return true;
            }
           
        }
    }

    /// <summary>
    /// Handle update of data and save file
    /// </summary>
    private void HandleUpdateSaveMap()
    {
        // update reached max stage
        if (currentStage > combatMapSaveData.reachedStage)
        {
            combatMapSaveData.reachedStage = currentStage;
        }

        // update stage counter
        combatMapSaveData.currentStage = currentStage;

        // update save file
        SettingsManager.Instance.SaveCombatMapData(CombatManager.Instance.MapSO, combatMapSaveData);
    }

    public void RestartCurrentStage()
    {
        KillAllEnemies();

        Resets();

        StartCoroutine(CoSpawnStartingEnemies(2f));
    }

    public void KillAllEnemies()
    {
        foreach (var enemy in currentEnemies)
        {
            enemy.PlayDeath(true);
        }

        // clear remaining rocks
        Enemy[] remains = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in remains)
        {
            enemy.PlayDeath(true);
        }
    }
    public void StopSpawns()
    {
        StopAllCoroutines();
    }

    private void Resets()
    {
        currentEnemyIndex = 1;
        currentStageKilled = 0;
    }

    #region UI

    private void UpdateStageUI()
    {
        textStage.text = $"{CombatManager.Instance.MapSO.MapName} - {CurrentStage}";
    }

    #endregion
}
