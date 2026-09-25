using System.Collections;
using System.Linq;
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
        long rewardedExp = UtilsMiner.GetRockExp(currentRock.RockData.RockSO.RockType);

        // kill rock
        currentRock.PlayDeath(false);
        //RockSpawnManager.Instance.AddSmash(1);

        // check nearby shockwave rocks
        HandleShockwave();

        // stop fight after setting death
        EnableSmash(false);

        // give exp to player
        player.PlayerData.AddExp(rewardedExp);

        GiveLoot(currentRock);

        PlayerManager.Instance.UpdateMinerData(player.PlayerData);

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
                        long rewardedExp = UtilsMiner.GetRockExp(rock.RockData.RockSO.RockType);

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

        // check if player has stoned buff
        if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Stoned))
        {
            thresholdLoot += 0.2f;
        }

        if (randPercLoot <= thresholdLoot)
        {
            //Debug.Log("Looted!");
            ItemSO randLoot = UtilsGeneral.GetRandomValueFromGeneralChanches(rock.RockData.RockSO.PossibleItems);

            if(randLoot != null)
            {
                player.AddItem(randLoot.Id, 1);


                // Check for Blacksmith job unlock
                if(randLoot.Id == UtilsItem.ID_GOLD_ORE)
                {
                    if (!PlayerManager.Instance.PlayerJobsData.IsBlacksmithUnlocked)
                    {
                        PlayerManager.Instance.PlayerJobsData.AddAvailableJob(UtilsPlayer.PlayerJob.Blacksmith);
                    }
                }
            }
        }

        // add a check for when the miner is maxed out, handles giving more metal to player
        if(player.PlayerData.CurrentLevel >= UtilsMiner.MAX_LEVEL_MINER)
        {
            float randVal = Random.value;
            float checkVal = float.MaxValue;
            switch (rock.RockData.RockSO.RockType)
            {
                case UtilsMiner.RockType.Copper: checkVal = 0.025f; break;
                case UtilsMiner.RockType.Iron: checkVal = 0.05f; break;
                case UtilsMiner.RockType.Bronze: checkVal = 0.1f; break;
                case UtilsMiner.RockType.Silver: checkVal = 0.2f; break;
                case UtilsMiner.RockType.Gold: checkVal = 0.4f; break;
            }

            if(checkVal <= randVal)
            {
                // gives correspondig metal to player
                var metal = UtilsItem.GetAllTypeItem<MetalSO>().FirstOrDefault(m => m.RockType == rock.RockData.RockSO.RockType);
                if (metal != null)
                {
                    PlayerManager.Instance.Inventory.AddItem(metal.Id, 1);
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
