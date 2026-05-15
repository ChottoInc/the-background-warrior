using System;
using UnityEngine;

public class PlayerFisher : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;


    private float timer5Mins;


    private PlayerFisherData playerData;


    // ------ FISHING VARS

    public event Action<int, int> OnStatChange;

    public event Action<FishSO> OnFishCaught;




    public PlayerFisherData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeFisher;
        }
    }

    private void Start()
    {
        timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;
    }

    private void Update()
    {
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
            successReflex = UtilsGeneral.GetRandomSuccessFromValue(playerData.CurrentReflex);
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

        // Fish caught
        bool hasAlreadyFish = PlayerManager.Instance.Inventory.HasItem(hookedFish.Id);

        // add to inventort even if already hasve, to trigger quest progress
        if (hasAlreadyFish)
        {
            // Dismantle fish into bits? for now
            // TODO: remake this option?
            int bitsToAdd = UtilsItem.DismantleFish(hookedFish.FishRarity);
            PlayerManager.Instance.Inventory.AddBits(bitsToAdd);
        }

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

    private void HandleCaughtUnsuccess(FishSO hookedFish)
    {
        // animation
        animator.SetTrigger("Fled");

        // fish go back into pool, nothing happens

        // Give player some exp, half of the minimum rarity, might tweak it later
        long rewardedExp = UtilsFisher.UNCAUGHT_EXP;
        playerData.AddExp(rewardedExp);
    }

    public void HandleSwitchScene()
    {

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
