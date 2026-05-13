using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RockSpawnManager : MonoBehaviour
{
    public const int MAX_ROCK_INDEX = 50;

    [SerializeField] int startingRocks = 20;
    [SerializeField] UtilsGeneral.GeneralChances<RockSO>[] possibleRocks;

    [Header("UI")]
    [SerializeField] TMP_Text textStage;

    private int currentRockIndex;


    //private int currentStageSmashed;



    [SerializeField] float ySpawn = 2f;
    private float offsetSpawn = 200f;
    private float minXSpawn, maxXSpawn;

    private List<Rock> currentRocks;

    public int CurrentRockIndex => currentRockIndex;


    public static RockSpawnManager Instance { get; private set; }

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


    public void Setup()
    {
        currentRocks = new List<Rock>();

        currentRockIndex = 1;

        minXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(0f + offsetSpawn, 0)).x;
        maxXSpawn = Camera.main.ScreenToWorldPoint(new Vector2(Screen.currentResolution.width - offsetSpawn, 0)).x;

        StartCoroutine(CoSpawnStartingRocks());

        UpdateStageUI();
    }

    #region ROCKS FUNCTIONS

    /// <summary>
    /// Called every time the game requires the next rock
    /// </summary>
    private RockData GenerateRock()
    {
        // generate data

        //RockSO randRock = UtilsGather.GetRandomRock();
        RockSO randRock = UtilsGeneral.GetRandomValueFromGeneralChanches(possibleRocks);
        RockData result = new RockData(randRock);

        // increase index
        currentRockIndex++;

        // reset index (for sorting purposes on scene)
        if (currentRockIndex > MAX_ROCK_INDEX)
            currentRockIndex = 1;

        return result;
    }

    private IEnumerator CoSpawnStartingRocks(float timer = 0f)
    {
        yield return new WaitForSeconds(timer);

        for (int i = 0; i < startingRocks; i++)
        {
            float randX = Random.Range(minXSpawn, maxXSpawn);
            Vector2 spawnPos = new Vector2(randX, ySpawn);

            RockData data = GenerateRock();
            SpawnRock(data, spawnPos);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnRock(RockData data, Vector2 spawnPos)
    {
        GameObject rockObj = PoolManager.Instance.Pull("rock");
        Rock rock = rockObj.GetComponent<Rock>();
        currentRocks.Add(rock);

        rock.Setup(data, currentRockIndex - 1);

        rockObj.transform.position = spawnPos;
    }

    public void SpawnNextRock()
    {
        if (currentRocks.Count >= MAX_ROCK_INDEX)
            return;

        float randX = Random.Range(minXSpawn, maxXSpawn);
        Vector2 spawnPos = new Vector2(randX, ySpawn);

        RockData data = GenerateRock();
        SpawnRock(data, spawnPos);
    }

    public void RemoveFromCurrentRocksList(Rock rock)
    {
        currentRocks.Remove(rock);
    }

    #endregion

    /*
    public void AddSmash(int amount)
    {
        currentStageSmashed += amount;
    }*/

    public void KillAllRocks()
    {
        foreach (var rock in currentRocks)
        {
            rock.PlayDeath(true);
        }

        // clear remaining rocks
        Rock[] remains = FindObjectsByType<Rock>(FindObjectsSortMode.None);
        foreach (var rock in remains)
        {
            rock.PlayDeath(true);
        }
    }

    public void StopSpawns()
    {
        StopAllCoroutines();
    }

    private void Resets()
    {
        currentRockIndex = 1;
    }

    #region UI

    private void UpdateStageUI()
    {
        textStage.text = $"Rocks";
    }

    #endregion
}
