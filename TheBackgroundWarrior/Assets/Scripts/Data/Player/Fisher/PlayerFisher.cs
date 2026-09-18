using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFisher : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;

    [Header("Focus Fishing")]
    [SerializeField] FocusFishingManager _focusFighingManager;


    private float timer5Mins;


    private PlayerFisherData playerData;


    // ------ FISHING VARS

    public event Action<int, int> OnStatChange;

    public event Action<FishSO> OnFishCaught;

    public event Action OnBaitChange;


    public PlayerFisherData PlayerData => playerData;


    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeFisher;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            UtilsBuffs.BuffType.Sailor,
            UtilsBuffs.BuffType.MorningAngler,
            UtilsBuffs.BuffType.AfternoonAngler,
            UtilsBuffs.BuffType.NightAngler,
            UtilsBuffs.BuffType.Inspiration,
        };
    }

    private void Start()
    {
        timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;
    }

    protected override void Update()
    {
        base.Update();

        // every 5 mins give some exp to the player
        if(timer5Mins <= 0)
        {
            playerData.AddExp(UtilsFisher.PASSIVE_EXP);
            timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;

            PlayerManager.Instance.UpdateFisherData(playerData);
            PlayerManager.Instance.SaveFisherData();
        }
        else
        {
            timer5Mins -= Time.deltaTime;
        }
    }

    public void Setup(PlayerFisherData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeFisher;
        }
    }

    public void HandleHook()
    {
        // Check knowledge stat success
        bool successKnowledge = UtilsGeneral.GetRandomSuccessFromValue(playerData.CurrentKnowledge);

        // Get fish from pool that has been hooked
        FishSO hookedFish = FishSpawnManager.Instance.GetRandomFishFromPool(successKnowledge);

        bool successReflex;

        if (FishSpawnManager.Instance.AlwaysCatchFishCheat && SettingsManager.Instance.AreCheatsEnabled)
        {
            successReflex = true;
        }
        else
        {
            // Check success hook
            float finalTotalCheck = playerData.CurrentReflex;

            // check if player has sailor buff
            if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Sailor))
            {
                finalTotalCheck += 0.2f;
            }
            successReflex = UtilsGeneral.GetRandomSuccessFromValue(finalTotalCheck);
        }

        //Debug.Log("Success: " + successReflex);

        if (successReflex)
        {
            HandleCaughtSuccess(hookedFish);
        }
        else
        {
            HandleCaughtUnsuccess(hookedFish);
        }

        // Save fisher data
        PlayerManager.Instance.UpdateFisherData(playerData);
        PlayerManager.Instance.SaveFisherData();
    }

    private void HandleCaughtSuccess(FishSO hookedFish)
    {
        // animation
        animator.SetTrigger("Caught");

        long rewardedExp;

        // Add fish to caught
        PlayerManager.Instance.Inventory.AddItem(hookedFish.Id, 1);

        // check for fishgroups
        playerData.FillFishGroupsSeriesCompletion();

        // Save ivnentory
        PlayerManager.Instance.SaveInventoryData();

        // Remove from pool if caught
        FishSpawnManager.Instance.RemoveFishFromPool(hookedFish);

        // refill pool
        FishSpawnManager.Instance.FillPool();

        // Give player full exp
        rewardedExp = UtilsItem.GetFishExp(hookedFish.FishRarity);
        playerData.AddExp(rewardedExp);

        OnFishCaught?.Invoke(hookedFish);
    }

    public void HandleCaughtFocusFishing(FishSO hookedFish)
    {
        long rewardedExp;

        // Add fish to caught
        PlayerManager.Instance.Inventory.AddItem(hookedFish.Id, 1);

        // check for fishgroups
        playerData.FillFishGroupsSeriesCompletion();

        // Save ivnentory
        PlayerManager.Instance.SaveInventoryData();

        // Remove from pool if caught
        //FishSpawnManager.Instance.RemoveFishFromPool(hookedFish);

        // refill pool
        //FishSpawnManager.Instance.FillPool();

        // Give player full exp - for now halves the amount of exp since you can spam that game
        rewardedExp = UtilsItem.GetFishExp(hookedFish.FishRarity) / 2;
        playerData.AddExp(rewardedExp);

        OnFishCaught?.Invoke(hookedFish);
    }

    private void HandleCaughtUnsuccess(FishSO hookedFish)
    {
        // animation
        animator.SetTrigger("Fled");

        // fish go back into pool, nothing happens

        // Give player some exp, half of the minimum rarity, might tweak it later
        long rewardedExp = UtilsFisher.UNCAUGHT_EXP;
        playerData.AddExp(rewardedExp);
    }


    public override IBasePlayerData GetPlayerData()
    {
        return PlayerData;
    }

    public override long GetCurrenExp()
    {
        return PlayerData.CurrentExp;
    }

    public override long GetExpToNextLevel()
    {
        return PlayerData.ExpToNextLevel;
    }

    protected override void OnBuffAdded(UtilsBuffs.BuffType buffType)
    {
        OnBuffChange(buffType);
    }

    protected override void OnBuffRemoved(UtilsBuffs.BuffType buffType)
    {
        OnBuffChange(buffType);
    }

    private void OnBuffChange(UtilsBuffs.BuffType buffType)
    {
        if (buffType == UtilsBuffs.BuffType.MorningAngler ||
            buffType == UtilsBuffs.BuffType.AfternoonAngler ||
            buffType == UtilsBuffs.BuffType.NightAngler)
        {
            FishSpawnManager.Instance.CheckBuffs();
            OnBaitChange?.Invoke();
        }
    }


    public void OnButtonFocusFishing()
    {
        _focusFighingManager.Open();
    }


    #region SAVE

    public void SaveFisherData()
    {
        PlayerManager.Instance.UpdateFisherData(playerData);
        PlayerManager.Instance.SaveFisherData();
    }

    #endregion

    #region HANDLE EVENTS FROM FISHER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveFisherData();
    }

    private void OnStatChangeFisher(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
