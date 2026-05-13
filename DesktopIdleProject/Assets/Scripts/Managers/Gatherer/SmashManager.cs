using System.Collections;
using UnityEngine;

public class SmashManager : MonoBehaviour
{
    [SerializeField] PlayerMiner player;

    [Header("Miner stats")]
    [SerializeField] LayerMask rockMask;
    [SerializeField] float radiusShockwave = 3f;

    [Header("Cheats")]
    [SerializeField] bool alwaysFindLootCheat;


    private Rock currentRock;


    private const int ID_GOLD_ORE = 4;



    public static SmashManager Instance { get; private set; }

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


    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPerformSmash -= OnPlayerSmash;
        }
    }


    public void Setup()
    {
        // setup stage
        RockSpawnManager.Instance.Setup();

        // initialize player
        PlayerMinerData playerData = PlayerManager.Instance.PlayerMinerData;
        player.Setup(playerData);


        if (player != null)
        {
            player.OnPerformSmash += OnPlayerSmash;
        }
    }

    public void StartSmash(Rock rock)
    {
        SetupRock(rock);

        StartCoroutine(CoDelaySmash(0f, 0f));
    }


    private void SetupRock(Rock rock)
    {
        // get rock
        currentRock = rock;
    }


    private IEnumerator CoDelaySmash(float timerIdle, float timerSmash)
    {
        yield return new WaitForSeconds(timerIdle);

        //currentEnemy.Show();

        yield return new WaitForSeconds(timerSmash);

        EnableSmash(true);
    }





    private void OnPlayerSmash()
    {
        currentRock.RockData.TakeDamage(player.PlayerData);

        if (currentRock.IsSmashed)
        {
            HandleRockSmash();
        }
    }

    private void HandleRockSmash()
    {
        //Debug.Log("Rock smash");

        // get exp before starting death for safety
        long rewardedExp = UtilsGather.GetRockExp(currentRock.RockData.RockSO.RockType);

        // kill rock
        currentRock.PlayDeath(false);
        //RockSpawnManager.Instance.AddSmash(1);

        // check nearby shockwave rocks
        HandleShockwave();

        // stop fight after setting death
        EnableSmash(false);

        // give exp to player
        player.PlayerData.AddExp(rewardedExp);
        PlayerManager.Instance.UpdateMinerData(player.PlayerData);

        GiveLoot(currentRock);

        // always spawn next rock
        RockSpawnManager.Instance.SpawnNextRock();
    }

    private void HandleShockwave()
    {
        if (player.PlayerData.CurrentShockwave > 0)
        {
            // get center current rock
            Vector2 currentCenter = currentRock.transform.position;

            // check hits
            var hits = Physics2D.OverlapCircleAll(currentCenter, radiusShockwave, rockMask);
            if (hits.Length > 0)
            {
                float shockwaveDamage = player.PlayerData.CurrentPower * player.PlayerData.CurrentShockwave;

                //int hitcounter = 0;
                // deal damage to every rock
                foreach (var hit in hits)
                {
                    Rock rock = hit.GetComponent<Rock>();
                    rock.RockData.TakeDamage(shockwaveDamage);

                    if (rock.IsSmashed)
                    {
                        // exp for each rock
                        long rewardedExp = UtilsGather.GetRockExp(rock.RockData.RockSO.RockType);

                        // destroy them
                        rock.PlayDeath(false);

                        // add exp
                        player.PlayerData.AddExp(rewardedExp);

                        // reward
                        GiveLoot(rock);

                        // spawn next rock
                        //Debug.Log("smashed with shockwave: " + hitcounter);
                        RockSpawnManager.Instance.SpawnNextRock();
                    }

                    //hitcounter++;
                }

                // save
                PlayerManager.Instance.UpdateMinerData(player.PlayerData);
            }
        }
    }

    private void GiveLoot(Rock rock)
    {
        float randPercLoot = Random.value;
        float thresholdLoot = (rock.RockData.RockSO.BaseLootChance / 100f) + player.PlayerData.CurrentLuck;

        if (alwaysFindLootCheat && SettingsManager.Instance.AreCheatsEnabled)
        {
            thresholdLoot = 1f;
        }

        //Debug.Log("Threshold: " + thresholdLoot);

        if (randPercLoot <= thresholdLoot)
        {
            //Debug.Log("Looted!");
            ItemSO randLoot = UtilsGeneral.GetRandomValueFromGeneralChanches(rock.RockData.RockSO.PossibleItems);

            if(randLoot != null)
            {
                player.AddItem(randLoot.Id, 1);


                // Check for Blacksmith job unlock
                if(randLoot.Id == ID_GOLD_ORE)
                {
                    if (!PlayerManager.Instance.PlayerJobsData.IsBlacksmithUnlocked)
                    {
                        PlayerManager.Instance.PlayerJobsData.AddAvailableJob(UtilsPlayer.PlayerJob.Blacksmith);
                    }
                }
            }
        }
    }


    private void EnableSmash(bool smash)
    {
        player.SetSmashing(smash);
    }


    public void HandleSwitchScene()
    {
        EnableSmash(false);

        RockSpawnManager.Instance.StopSpawns();
        RockSpawnManager.Instance.KillAllRocks();
    }
}
