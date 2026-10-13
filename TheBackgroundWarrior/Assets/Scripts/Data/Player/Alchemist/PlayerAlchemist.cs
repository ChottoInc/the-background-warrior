using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAlchemist : Player
{
    [Header("Movement")]
    [SerializeField] Animator _animator;


    [Space(10)]
    [SerializeField] GenericBar _barCrafting;
    [SerializeField] Image _imageOutOfOrder;


    private float CooldownCraft
    {
        get
        {
            if (CurrentRecipe != null)
            {
                return CurrentRecipe.CraftTime - CurrentRecipe.CraftTime * PlayerData.CurrentRoutine;
            }
            else return 0;
        }
    }

    private float _timer1Sec;

    // handles crafting progress
    private float _currentCraftingPoints;


    public RecipeSO CurrentRecipe { get; private set; }
    public bool IsCrafting { get; private set; }



    public event Action<int, int> OnStatChange;




    public PlayerAlchemistData PlayerData { get; private set; }



    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (PlayerData != null)
        {
            PlayerData.OnLevelUp -= LevelUp;

            PlayerData.OnStatChange -= OnStatChangeMage;
        }
    }

    private void Start()
    {
        timer1Second = UtilsGeneral.TIMER_1SECONDS;

        OnTryCraft();
    }

    public void Setup(PlayerAlchemistData playerData)
    {
        PlayerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeMage;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (IsCrafting)
        {
            CheckCraft();
        }
    }

    private void CheckCraft()
    {
        if (_timer1Sec <= 0)
        {
            CheckCraftProgress();
            _timer1Sec = UtilsGeneral.TIMER_1SECONDS;
        }
        else
        {
            _timer1Sec -= Time.deltaTime;
        }
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

    public void HandleSwitchScene()
    {
        SetCrafting(false);
    }

    private void CheckCraftProgress()
    {
        // Add progress counter and update UI
        _currentCraftingPoints += 1f;

        UpdateForgingBarUI();

        // item has been craft
        if (_currentCraftingPoints >= CooldownCraft)
        {
            SetCrafting(false);

            // Calculate if successful craft
            bool success = UtilsGeneral.GetRandomSuccessFromValue(PlayerData.CurrentStability);

            // add materials in case of success
            if (success)
            {
                // make little animation for success?

                int amountToAdd = UtilsGeneral.GetRandomSuccessFromValue(PlayerData.CurrentYield) ? 2 : 1;

                // remove quantity from alchemist
                PlayerData.SetCurrentCraftingQuantity(PlayerData.CurrentCraftingQuantity - 1);

                // Update inventory
                foreach (var item in CurrentRecipe.Ingredients)
                {
                    PlayerManager.Instance.Inventory.RemoveItem(item.item.Id, item.quantity);
                }

                PlayerManager.Instance.Inventory.AddItem(CurrentRecipe.Id, amountToAdd);

                PlayerManager.Instance.SaveInventoryData();

                UtilsAchievement.AddStat(UtilsAchievement.STAT_CRAFT_CONCOCTIONS_COUNT, amountToAdd, UtilsAchievement.STAT_CRAFT_CONCOCTIONS_COUNT_MAX);

                // Give exp to alchemist job
                long rewardedExp = CurrentRecipe.RewardedExp;
                PlayerData.AddExp(rewardedExp);

                PlayerManager.Instance.UpdateAlchemistData(PlayerData);
                SaveAlchemistData();
            }
            else
            {
                // make little animation for failed?
            }

            // Recheck for next batch, or idle
            OnTryCraft();
        }
    }

    public void OnTryCraft()
    {
        SetCrafting(CanCraft());
    }


    /// <summary>
    /// Check if ore is selected and if enough
    /// </summary>
    private bool CanCraft()
    {
        if (PlayerData.CurrentCraftingRecipe != null)
        {
            // check has enough material
            if (UtilsAlchemist.GetPossibleQuantity(PlayerData.CurrentCraftingRecipe, PlayerManager.Instance.Inventory) > 0)
            {
                _imageOutOfOrder.gameObject.SetActive(false);

                // if infinite keep forging, else check for quantity
                if (PlayerData.IsInfiniteCrafting)
                {
                    CurrentRecipe = PlayerData.CurrentCraftingRecipe;
                    return true;
                }
                else
                {
                    if (PlayerData.CurrentCraftingQuantity > 0)
                    {
                        CurrentRecipe = PlayerData.CurrentCraftingRecipe;
                        return true;
                    }
                }
            }
            else
            {
                _imageOutOfOrder.gameObject.SetActive(true);
            }
        }

        return false;
    }


    public void SetCrafting(bool isCrafting)
    {
        IsCrafting = isCrafting;

        _animator.SetBool("isCrafting", isCrafting);

        if (isCrafting)
        {
            _currentCraftingPoints = 0;
            _barCrafting.gameObject.SetActive(true);
            SetCraftingBarUI();
        }
        else
        {
            //forgeVFX.Stop();
            _barCrafting.gameObject.SetActive(false);
        }
    }

    private void SetCraftingBarUI()
    {
        _barCrafting.SetMaxValue(CooldownCraft);

        _currentCraftingPoints = 0;
        UpdateForgingBarUI();
    }

    private void UpdateForgingBarUI()
    {
        _barCrafting.SetCurrentValue(_currentCraftingPoints);
    }


    #region SAVE

    public void SaveAlchemistData()
    {
        PlayerManager.Instance.UpdateAlchemistData(PlayerData);
        PlayerManager.Instance.SaveAlchemistData();
    }

    #endregion

    #region HANDLE EVENTS FROM MAGE DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveAlchemistData();
    }

    private void OnStatChangeMage(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
