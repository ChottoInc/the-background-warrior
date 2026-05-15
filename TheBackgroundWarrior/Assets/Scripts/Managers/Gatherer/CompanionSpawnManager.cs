using System;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawnManager : MonoBehaviour
{
    [Header("Rand Spawn")]
    [SerializeField] float ySpawn = -4f;
    [SerializeField] float minSpawnCooldown = 30f;
    [SerializeField] float maxSpawnCooldown = 150f;

    [Header("Rand Despawn")]
    [SerializeField] float minDespawnCooldown = 20f;
    [SerializeField] float maxDespawnCooldown = 120;

    private float timerSpawnRandom;

    private List<CompanionSpawn> currentSpawns;
    private List<CompanionSpawn> spawnsToRemove;

    // variable ensure multiple spawns at once
    private bool isPullingRandomCompanion;


    private void Start()
    {
        currentSpawns = new List<CompanionSpawn>();
        spawnsToRemove = new List<CompanionSpawn>();

        timerSpawnRandom = GenerateRandTimer(minSpawnCooldown, maxSpawnCooldown);
    }

    private void Update()
    {
        // decrease every spawn timer
        HandleSpawns();

        // new random spawn
        if(timerSpawnRandom <= 0 && !isPullingRandomCompanion)
        {
            isPullingRandomCompanion = true;

            GenerateCompanion();

            timerSpawnRandom = GenerateRandTimer(minSpawnCooldown, maxSpawnCooldown);

            isPullingRandomCompanion = false;
        }
        else
        {
            timerSpawnRandom -= Time.deltaTime;
        }
    }

    private void HandleSpawns()
    {
        foreach(var companion in currentSpawns)
        {
            companion.DecreaseTimer(Time.deltaTime);
        }

        if(spawnsToRemove.Count > 0)
        {
            foreach (var companion in spawnsToRemove)
            {
                currentSpawns.Remove(companion);
            }

            spawnsToRemove.Clear();
        }
    }

    private void GenerateCompanion()
    {
        bool valid;
        int tries = 0;
        int maxTries = 100;

        CompanionData randCompanion;

        do
        {
            valid = true;

            randCompanion = GetRandomCompanionSpawn();

            // if null the farmes has no companions yet
            if(randCompanion == null)
            {
                valid = false;
                tries = maxTries;
            }
            else
            {
                if (IsCompanionSpawned(randCompanion))
                {
                    valid = false;
                }
            }

            tries++;
        } while (tries < maxTries && !valid);

        // if the companions hasn't been spawned
        if (valid)
        {
            float offset = 300f;
            Vector2 spawnPos = Vector2.zero;

            // random spawn right or left
            if (UnityEngine.Random.value < 0.5f)
            {
                spawnPos = new Vector2(InitializerManager.Instance.GetScreenOffsetBound() - offset, ySpawn);
            }
            else
            {
                spawnPos = new Vector2(InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound() + offset, ySpawn);
            }

            spawnPos = Camera.main.ScreenToWorldPoint(spawnPos);

            GameObject prefab = Instantiate(randCompanion.CompanionSO.Prefab, spawnPos, Quaternion.identity);
            prefab.GetComponent<Companion>().SetupRandomWalk();

            CompanionSpawn spawn = new CompanionSpawn(prefab, randCompanion, GenerateRandTimer(minDespawnCooldown, maxDespawnCooldown));

            // attach event
            spawn.OnTimerEnded += DespawnCompanion;

            // add to list
            currentSpawns.Add(spawn);
        }
    }

    private float GenerateRandTimer(float val1, float val2)
    {
        return UtilsGeneral.GetRandomValueBtwValues(val1, val2);
    }

    private CompanionData GetRandomCompanionSpawn()
    {
        CompanionData result = null;

        var companions = PlayerManager.Instance.PlayerFarmerData.Companions;

        if (companions.Count > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, companions.Count);
            result = companions[randIndex];
        }

        return result;
    }

    private bool IsCompanionSpawned(CompanionData data)
    {
        foreach (var companion in currentSpawns)
        {
            if(companion.data.CompanionSO.Id == data.CompanionSO.Id)
                return true;
        }
        return false;
    }

    private void DespawnCompanion(CompanionSpawn spawn)
    {
        // make companion walk out
        Companion prefab = spawn.prefab.GetComponent<Companion>();
        prefab.SetTargetOutsideScreen();

        // detach event
        spawn.OnTimerEnded -= DespawnCompanion;

        // remove from list
        spawnsToRemove.Add(spawn);
    }
}

public class CompanionSpawn
{
    public GameObject prefab;
    public CompanionData data;
    public float timerDespawn;

    public event Action<CompanionSpawn> OnTimerEnded;

    private bool isTimerEnded;



    public CompanionSpawn(GameObject prefab, CompanionData data, float timerDespawn)
    {
        this.prefab = prefab;
        this.data = data;
        this.timerDespawn = timerDespawn;
    }


    public void DecreaseTimer(float time)
    {
        if (isTimerEnded) return;

        timerDespawn -= time;

        if(timerDespawn <= 0)
        {
            isTimerEnded = true;
            OnTimerEnded?.Invoke(this);
        }
    }




    public override bool Equals(object obj)
    {
        CompanionSpawn other = obj as CompanionSpawn;
        return data.CompanionSO.Id == other.data.CompanionSO.Id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}