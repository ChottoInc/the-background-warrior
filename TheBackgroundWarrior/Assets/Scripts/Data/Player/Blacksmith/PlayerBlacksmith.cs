using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlacksmith : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;

    [Header("UI")]
    [SerializeField] GenericBar forgingBar;
    [SerializeField] Image imageOutOfOrder;

    [Header("Animation")]
    [SerializeField] AnimationClip forgeClip;
    [SerializeField] ParticleSystem forgeVFX;


    private PlayerBlacksmithData playerData;

    // ------ ATTACK VARS

    private bool isForging;
    private float CooldownSmash => 1f / playerData.CurrentCraftSpeed;
    private float timerForge;

    // handles forging progress
    private int currentOreId;
    private float currentForgingPoints;


    public int CurrentOreId => currentOreId;
    public bool IsForging => isForging;



    public event Action OnPerformSmash;

    public event Action<int, int> OnStatChange;




    public PlayerBlacksmithData PlayerData => playerData;


    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeBlacksmith;
        }

        OnPerformSmash -= CheckForgeProgress;
    }


    protected override void Awake()
    {
        base.Awake();
        OnPerformSmash += CheckForgeProgress;
    }

    private void Start()
    {
        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            UtilsBuffs.BuffType.Dwarf,
            UtilsBuffs.BuffType.Inspiration,
        };

        OnTryForge();
    }

    protected override void Update()
    {
        base.Update();

        if (isForging)
        {
            CheckForge();
        }
    }

    public void PlayForgeVFX()
    {
        forgeVFX.Play();
    }


    /// <summary>
    /// Check if ore is selected and if enough
    /// </summary>
    private bool CanForge()
    {
        int id = playerData.CurrentForgingOre;

        if(id != -1)
        {
            OreSO ore = UtilsItem.GetItemById(id) as OreSO;

            MetalSO metal = ore.RefinedMetal;

            int needAmount = metal.RequiredOres;

            // check has enough material
            if(PlayerManager.Instance.Inventory.HasEnough(ore.Id, needAmount))
            {
                imageOutOfOrder.gameObject.SetActive(false);

                // if infinite keep forging, else check for quantity
                if (playerData.IsInfiniteForging)
                {
                    currentOreId = id;
                    return true;
                }
                else
                {
                    if(playerData.CurrentForgingQuantity > 0)
                    {
                        currentOreId = id;
                        return true;
                    }
                }
            }
            else
            {
                imageOutOfOrder.gameObject.SetActive(true);
            }
        }

        return false;
    }

    private void CheckForge()
    {
        if (timerForge <= 0)
        {
            OnPerformSmash?.Invoke();
            timerForge = CooldownSmash;
        }
        else
        {
            timerForge -= Time.deltaTime;
        }
    }

    public void Setup(PlayerBlacksmithData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeBlacksmith;
        }
    }


    private void CheckForgeProgress()
    {
        // Add progress counter and update UI
        currentForgingPoints += 1f;

        UpdateForgingBarUI();

        // item has been forged
        if(currentForgingPoints >= playerData.CurrentCraftTime)
        {
            SetForging(false);
            
            // Get forging ore
            int id = playerData.CurrentForgingOre;

            OreSO ore = UtilsItem.GetItemById(id) as OreSO;

            // Get refined metal
            MetalSO metal = ore.RefinedMetal;

            // Calculate if sparing materials
            bool successSpared = UtilsGeneral.GetRandomSuccessFromValue(playerData.CurrentEfficiency);

            int amountMetalToAdd = 1;
            if(UnityEngine.Random.value <= playerData.CurrentLuck)
            {
                // if luck procs, check Metallurgy stats for amount
                int amountMultiplier = 1 + (Mathf.FloorToInt(playerData.CurrentMetallurgy) + 1);
                amountMetalToAdd *= amountMultiplier; // up to * 5
            }

            // check if player has dwarf buff
            if (PlayerManager.Instance.PlayerBuffsData.HasBuff(UtilsBuffs.BuffType.Dwarf))
            {
                amountMetalToAdd = Mathf.RoundToInt((float)amountMetalToAdd * 1.2f);
            }

            // remove quantity from blacksmith
            playerData.SetCurrentForgingQuantity(playerData.CurrentForgingQuantity - 1);

            // Update inventory
            if(!successSpared)
                PlayerManager.Instance.Inventory.RemoveItem(id, metal.RequiredOres);

            PlayerManager.Instance.Inventory.AddItem(metal.Id, amountMetalToAdd);

            PlayerManager.Instance.SaveInventoryData();

            // Give exp to blacksmith job
            long rewardedExp = UtilsItem.GetMetalExp(metal);
            playerData.AddExp(rewardedExp);

            PlayerManager.Instance.UpdateBlacksmithData(playerData);
            SaveBlacksmithData();

            // Recheck for next batch, or idle
            OnTryForge();
        }
    }


    private void SetForgingBarUI()
    {
        forgingBar.SetMaxValue(playerData.CurrentCraftTime);

        currentForgingPoints = 0;
        UpdateForgingBarUI();
    }

    private void UpdateForgingBarUI()
    {
        forgingBar.SetCurrentValue(currentForgingPoints);
    }

    public void SetForging(bool isForging)
    {
        this.isForging = isForging;

        animator.SetBool("isForging", isForging);

        if (isForging)
        {
            forgingBar.gameObject.SetActive(true);
            SetForgingBarUI();
        }
        else
        {
            forgeVFX.Stop();
            forgingBar.gameObject.SetActive(false);
        }
    }

    public void OnTryForge()
    {
        SetForging(CanForge());
    }

    public void HandleSwitchScene()
    {
        SetForging(false);
    }


    public void AddBlacksmithGearLevel(UtilsBlacksmith.BlacksmithGear gear, int level)
    {
        switch (gear)
        {
            case UtilsBlacksmith.BlacksmithGear.Helmet: playerData.AddBlacksmithHelmetLevel(1); break;
            case UtilsBlacksmith.BlacksmithGear.Armor: playerData.AddBlacksmithArmorLevel(1); break;
            case UtilsBlacksmith.BlacksmithGear.Gloves: playerData.AddBlacksmithGlovesLevel(1); break;
            case UtilsBlacksmith.BlacksmithGear.Boots: playerData.AddBlacksmithBootsLevel(1); break;
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


    #region SAVE

    public void SaveBlacksmithData()
    {
        PlayerManager.Instance.UpdateBlacksmithData(playerData);
        PlayerManager.Instance.SaveMinerData();
    }

    #endregion

    #region HANDLE EVENTS FROM BLACKSMITH DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveBlacksmithData();
    }

    private void OnStatChangeBlacksmith(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
