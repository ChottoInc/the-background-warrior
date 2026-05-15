using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWorldManager : MonoBehaviour
{
    //public const int MAX_ENEMY_INDEX = 100;
    public const int MAX_ENEMY_INDEX = 10;

    [Header("Enemies")]
    [SerializeField] EnemySO enemySO;
    [SerializeField] private int startingEnemies = 10;


    private int currentEnemyIndex;



    private float offsetSpawn = 200f;
    private float ySpawn = 2f;
    private float minXSpawn, maxXSpawn;

    private List<Enemy> currentEnemies;



    // enemy utils

    public int CurrentEnemyIndex => currentEnemyIndex;

    public static HomeWorldManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoaderManager.Instance.LoadHome();
        }
    }


    public void Setup()
    {
        currentEnemies = new List<Enemy>();

        currentEnemyIndex = 1;

        minXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(0f + offsetSpawn, 0)).x;
        maxXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(Screen.currentResolution.width - offsetSpawn, 0)).x;

        StartCoroutine(CoSpawnStartingEnemies());
    }

    #region ENEMY FUNCTIONS

    /// <summary>
    /// Called every time the game requires the next enemy
    /// </summary>
    private EnemyData GenerateEnemy()
    {
        // generate data
        EnemyData result = new EnemyData(enemySO);

        // increase index
        currentEnemyIndex++;

        return result;
    }

    private IEnumerator CoSpawnStartingEnemies(float timer = 0f)
    {
        yield return new WaitForSeconds(timer);

        for (int i = 0; i < startingEnemies; i++)
        {
            float randX = Random.Range(minXSpawn, maxXSpawn);
            Vector2 spawnPos = new Vector2(randX, ySpawn);

            EnemyData data = GenerateEnemy();
            SpawnEnemy(data, spawnPos);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnEnemy(EnemyData data, Vector2 spawnPos)
    {
        GameObject enemyObj = PoolManager.Instance.Pull(data.EnemySO.EnemyPoolName);
        //GameObject enemyObj = Instantiate(enemyPrefab, transform);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        currentEnemies.Add(enemy);

        enemy.Setup(data, currentEnemyIndex - 1, SceneLoaderManager.SceneType.Home);

        enemyObj.transform.position = spawnPos;
    }

    public void RemoveFromCurrentEnemiesList(Enemy enemy)
    {
        currentEnemies.Remove(enemy);
    }

    #endregion

    public void KillAllEnemies()
    {
        foreach (var enemy in currentEnemies)
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
    }

    public void HandleSwitchScene()
    {
        StopSpawns();
        KillAllEnemies();
    }
}
