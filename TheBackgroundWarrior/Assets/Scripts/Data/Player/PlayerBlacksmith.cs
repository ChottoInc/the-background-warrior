using System;
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


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeBlacksmith;
        }

        OnPerformSmash -= CheckForgeProgress;
    }


    private void Awake()
    {
        OnPerformSmash += CheckForgeProgress;
    }

    private void Start()
    {
        OnTryForge();
    }

    private void Update()
    {
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
            if( PlayerManager.Instance.Inventory.HasEnough(ore.Id, needAmount))
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


    public void AddBlacksmithGearLevel(int idGear, int level)
    {
        switch (idGear)
        {
            case UtilsBlacksmith.ID_BLACKSMITH_HELMET: playerData.AddBlacksmithHelmetLevel(1); break;
            case UtilsBlacksmith.ID_BLACKSMITH_ARMOR: playerData.AddBlacksmithArmorLevel(1); break;
            case UtilsBlacksmith.ID_BLACKSMITH_GLOVES: playerData.AddBlacksmithGlovesLevel(1); break;
            case UtilsBlacksmith.ID_BLACKSMITH_BOOTS: playerData.AddBlacksmithBootsLevel(1); break;
        }
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
