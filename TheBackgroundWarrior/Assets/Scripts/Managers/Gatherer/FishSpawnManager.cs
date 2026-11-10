using System.Collections.Generic;
using UnityEngine;

public class FishSpawnManager : MonoBehaviour
{
    private const int MAX_FISHES_IN_POOL = 25;

    [SerializeField] float minHookTime = 30f;

    // default max spawn time
    [SerializeField] float maxHookTime = 60f;

    [Space(10)]
    [SerializeField] GenericBar fishBar;

    private float timer20Seconds;

    private float passedTimeHook;
    private float timerHook;

    private float CurrentMaxHookTime => Mathf.Max(maxHookTime - (maxHookTime * player.PlayerData.CurrentCalmness), minHookTime);

    public float AverageHookTime => (minHookTime + CurrentMaxHookTime) /2f;

    [Space(10)]
    [SerializeField] UtilsGeneral.GeneralChances<UtilsItem.FishRarity>[] rarityProbabilities;

    [Space(10)]
    [SerializeField] PlayerFisher player;


    [Header("Cheats")]
    [SerializeField] bool alwaysCatchFishCheat;
    [SerializeField] bool reducedHookCheat;

    public bool AlwaysCatchFishCheat => alwaysCatchFishCheat;



    private List<FishSO> currentPool;



    public List<FishSO> CaughtFishesSession { get; private set; }



    private bool isInitialized;


    public static FishSpawnManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        fishBar.gameObject.SetActive(!SettingsManager.Instance.IsHiddenFishingBar);

        SettingsManager.Instance.OnIsHiddenFishingBarChange += OnFishBarHiddenChange;
    }

    private void OnDestroy()
    {
        player.OnFishCaught -= AddFishToCaughtList;

        SettingsManager.Instance.OnIsHiddenFishingBarChange -= OnFishBarHiddenChange;
    }


    private void Start()
    {
        player.OnFishCaught += AddFishToCaughtList;


        currentPool = new List<FishSO>();
        CaughtFishesSession = new List<FishSO>();

        // set pool
        FillPool();

        timer20Seconds = UtilsGeneral.TIMER_20SECONDS;

        timerHook = GetRandomHookTime();
        passedTimeHook = 0;

        fishBar.Setup(timerHook, passedTimeHook);

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        HandleFishBarUI();

        HandleHook();
    }

    private void HandleFishBarUI()
    {
        if (timer20Seconds <= 0)
        {
            UpdateFishBarUI();
            timer20Seconds = UtilsGeneral.TIMER_20SECONDS;
        }
        else
        {
            timer20Seconds -= Time.deltaTime;
        }
    }

    private void HandleHook()
    {
        if (passedTimeHook >= timerHook)
        {
            //Debug.Log("Attempt catch fish");

            player.HandleHook();

            timerHook = GetRandomHookTime();
            passedTimeHook = 0;

            fishBar.Setup(timerHook, passedTimeHook);
        }
        else
        {
            passedTimeHook += Time.deltaTime;
        }
    }

    private void OnFishBarHiddenChange(bool isOn)
    {
        fishBar.gameObject.SetActive(!isOn);
    }
    

    public void FillPool()
    {
        while(currentPool.Count < MAX_FISHES_IN_POOL)
        {
            // Get day moment
            UtilsGeneral.DayMoment currentMoment = UtilsGeneral.GetDayMoment();

            SinglePoolAdd(currentMoment);
        }
    }

    public void FillPool(int loop1Count, int loop2Count, UtilsGeneral.DayMoment moment)
    {
        //Debug.Log("count 1: " + loop1Count);
        while (currentPool.Count < loop1Count)
        {
            SinglePoolAdd(moment);
            //Debug.Log("pool count: " + currentPool.Count);
        }

        //Debug.Log("count 2: " + loop2Count);
        while (currentPool.Count < loop2Count + loop1Count)
        {
            // Get day moment
            UtilsGeneral.DayMoment currentMoment = UtilsGeneral.GetDayMoment();

            SinglePoolAdd(currentMoment);
            //Debug.Log("pool count: " + currentPool.Count);
        }
    }

    private void SinglePoolAdd(UtilsGeneral.DayMoment moment)
    {
        // Get rand rarity 
        UtilsItem.FishRarity randRarity = UtilsGeneral.GetRandomValueFromGeneralChanches(rarityProbabilities);

        // Get luck and cycle until it fails boost rarity
        float baseLuckPlayer = player.PlayerData.CurrentLuck;
        while (UtilsGeneral.GetRandomSuccessFromValue(baseLuckPlayer))
        {
            randRarity = UpgradeRarity(randRarity);

            // interrupt check luck if max rarity reached
            if ((int)randRarity == System.Enum.GetValues(typeof(UtilsItem.FishRarity)).Length - 1)
            {
                break;
            }

            baseLuckPlayer *= 0.5f;
        }

        // Get random fish
        FishSO randFish = UtilsItem.GetRandomFishByDayMomentAndRarity(moment, randRarity);
        //Debug.Log(randFish.ToString());

        // Add to pool
        currentPool.Add(randFish);
    }

    public FishSO GetRandomFishFromPool(bool successKnowledge)
    {
        bool found;
        FishSO result;

        int tries = 0;
        int maxTries = 1000;

        do
        {
            found = false;

            int randIndex = Random.Range(0, currentPool.Count);
            result = currentPool[randIndex];

            if (!successKnowledge)
            {
                // always found if not knowledge success
                found = true;
            }
            else
            {
                // if success on knowledge, found only if fish hasn't already got caught
                found = !PlayerManager.Instance.Inventory.HasItem(result.Id);
            }

            tries++;
        }
        while (!found && tries < maxTries);

        return result;
    }

    private UtilsItem.FishRarity UpgradeRarity(UtilsItem.FishRarity current)
    {
        int nextValue = Mathf.Min(
            (int)current + 1,
            System.Enum.GetValues(typeof(UtilsItem.FishRarity)).Length - 1
        );

        return (UtilsItem.FishRarity)nextValue;
    }

    public void RemoveFishFromPool(FishSO fishSO)
    {
        currentPool.Remove(fishSO);
    }

    private float GetRandomHookTime()
    {
        if (reducedHookCheat && SettingsManager.Instance.AreCheatsEnabled)
            return 30f;
        else
            return Random.Range(minHookTime, CurrentMaxHookTime);

        // Test
        //int rand = Random.Range(10, 15);
        //Debug.Log("Hook time: " + rand);
        //return rand;
    }

    private void AddFishToCaughtList(FishSO fishSO)
    {
        CaughtFishesSession.Add(fishSO);
    }

    private void UpdateFishBarUI()
    {
        fishBar.SetCurrentValue(passedTimeHook);
    }
}
