using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFarmer : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;

    [Space(10)]
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 3f;

    [Space(10)]
    [SerializeField] AnimationClip sowClip;
    [SerializeField] AnimationClip mowClip;

    [Header("Watering")]
    [SerializeField] float minCooldownWatering;
    [SerializeField] float maxCooldownWatering;
    [SerializeField] Transform[] cropsPositions;


    private float timer5Mins;

    private PlayerFarmerData playerData;


    // --------- MOVEMENT VARS

    private bool canInitialMove;

    private bool isWalking;
    private float timerWatering;

    private bool _needResetCrop;

    private enum FarmerAction { None, Sowing, Mowing, Watering }

    private Queue<FarmerAction> nextActions;
    private FarmerAction currentAction;
    private bool _isPerformingAction;


    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isIdling;
    private float timerIdle;

    private Rigidbody2D rb;

    // ------ FARMER VARS

    private Queue<Transform> farmPlotsToMow;
    private Queue<CropSlotData> farmPlotsToMowData;
    private int currentFarmPlotToMow;

    private Queue<Transform> farmPlotsToSow;
    private Queue<CropSlotData> farmPlotsToSowData;
    private int currentFarmPlotToSow;

    public event Action<int, int> OnStatChange;




    public PlayerFarmerData PlayerData => playerData;


    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeFarmer;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();

        nextActions = new Queue<FarmerAction>();

        timerWatering = GetRandomWateringTime();
    }

    protected override void Update()
    {
        base.Update();

        // get next action in queue
        if((currentAction == FarmerAction.Watering || currentAction == FarmerAction.None) && nextActions.Count > 0)
        {
            if(currentAction == FarmerAction.Watering)
            {
                // stop watering animation
                StopCoroutine(nameof(CoWaitWateringAnimation));
                animator.SetTrigger("Idle");
                _isPerformingAction = false;
            }

            currentAction = nextActions.Dequeue();
            CheckAction();
        }

        if(timerWatering <= 0)
        {
            if(currentAction == FarmerAction.None)
            {
                currentAction = FarmerAction.Watering;
            }

            timerWatering = GetRandomWateringTime();
        }
        else
        {
            timerWatering -= Time.deltaTime;
        }

        // every 5 mins give some exp to the player
        if (timer5Mins <= 0)
        {
            playerData.AddExp(UtilsFarmer.PASSIVE_EXP);
            timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;

            PlayerManager.Instance.UpdateFarmerData(playerData);
            PlayerManager.Instance.SaveFarmerData();
        }
        else
        {
            timer5Mins -= Time.deltaTime;
        }
    }

    public void Setup(PlayerFarmerData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeFarmer;
        }
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        StartCoroutine(CoSpawned());

        timer5Mins = UtilsGeneral.TIMER_5MIN_IN_SECONDS;

        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            UtilsBuffs.BuffType.Tamer,
            UtilsBuffs.BuffType.Inspiration,
        };
    }


    private void FixedUpdate()
    {
        if(currentAction == FarmerAction.None && nextActions.Count == 0)
        {
            HandleMovement();
        }
        else
        {
            GoToFarmPlot();
        }
    }

    private IEnumerator CoSpawned()
    {
        yield return new WaitForSeconds(2f);

        // change rb type
        rb.bodyType = RigidbodyType2D.Kinematic;

        // enable movement
        canInitialMove = true;

        GenerateNewTarget();

        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }

    /// <summary>
    /// Handles walking, if distant from target walk, else idle
    /// </summary>
    private void HandleMovement()
    {
        if (!canInitialMove) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isWalking", isWalking);
            }

            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            // handles idling timer before move again
            if (!isIdling)
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                timerIdle = cooldownIdle;
                isIdling = true;
            }
            else
            {
                if (timerIdle <= 0)
                {
                    GenerateNewTarget();
                    isIdling = false;
                }
                else
                {
                    timerIdle -= Time.fixedDeltaTime;
                }
            }
        }
    }

    /// <summary>
    /// Handles movement to first farm plot when sowing, starts a coroutine for next ones
    /// </summary>
    private void GoToFarmPlot()
    {
        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 1f && !isIdling)
        {
            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            if (_isPerformingAction) return;

            if(currentAction == FarmerAction.Sowing)
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                // wait for sow animation and call next farm plot
                StartCoroutine(CoWaitSowAnimation());
            }
            else if(currentAction == FarmerAction.Mowing)
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                // wait for sow animation and call next farm plot
                StartCoroutine(CoWaitMowAnimation());
            }
            else if (currentAction == FarmerAction.Watering)
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);

                // wait for mow animation and call next farm plot
                StartCoroutine(CoWaitWateringAnimation());
            }
        }
    }

    private void CheckFlip()
    {
        // check sprite flip
        float vx = currentDirection.x;
        if (vx > 0.01f && faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
        else if (vx > 0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (vx < -0.01f && !faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        currentTarget = UnityEngine.Random.Range(InitializerManager.Instance.GetScreenOffsetBound(), InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound());
        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }

    public void AddSow(CropSlotData cropSlotData, Transform[] farmPlots)
    {
        farmPlotsToSow ??= new Queue<Transform>();
        farmPlotsToSowData ??= new Queue<CropSlotData>();

        foreach (var farmPlot in farmPlots)
        {
            farmPlotsToSow.Enqueue(farmPlot);
        }

        farmPlotsToSowData.Enqueue(cropSlotData);

        // enqueue the next action
        nextActions.Enqueue(FarmerAction.Sowing);
    }

    public void AddMow(CropSlotData cropSlotData, Transform[] farmPlots, bool needResetCrop)
    {
        _needResetCrop = needResetCrop;

        farmPlotsToMow ??= new Queue<Transform>();
        farmPlotsToMowData ??= new Queue<CropSlotData>();

        foreach (var farmPlot in farmPlots)
        {
            farmPlotsToMow.Enqueue(farmPlot);
        }

        farmPlotsToMowData.Enqueue(cropSlotData);

        // enqueue the next action
        nextActions.Enqueue(FarmerAction.Mowing);
    }

    private void CheckAction()
    {
        if(currentAction == FarmerAction.Sowing)
        {
            HandleSowingAction();
        }
        else if(currentAction == FarmerAction.Mowing)
        {
            HandleMowingAction();
        }
        else if (currentAction == FarmerAction.Watering)
        {
            HandleWateringAction();
        }
    }

    private void HandleSowingAction()
    {
        Transform farmPlot = farmPlotsToSow.Dequeue();

        // set next farm plot X
        currentTarget = farmPlot.position.x;

        SetToWalking();

        // set already next farm plot index
        currentFarmPlotToSow++;
    }

    private void HandleMowingAction()
    {
        Transform farmPlot = farmPlotsToMow.Dequeue();

        // set next farm plot X
        currentTarget = farmPlot.position.x;

        SetToWalking();

        // set already next farm plot index
        currentFarmPlotToMow++;
    }

    private void HandleWateringAction()
    {
        Transform farmPlot = cropsPositions[UnityEngine.Random.Range(0, cropsPositions.Length)];

        // set next farm plot X
        currentTarget = farmPlot.position.x;

        SetToWalking();
    }

    private void SetToWalking()
    {
        // disable isIdling
        if (isIdling)
        {
            isIdling = false;
        }

        // set to walking before reaching farm plot
        if (!isWalking)
        {
            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }
    }

    private IEnumerator CoWaitSowAnimation()
    {
        _isPerformingAction = true;

        // tell animator to do the sowing animation
        animator.SetTrigger("Sow");

        yield return new WaitForSeconds(sowClip.length);

        // every 4 plots reset and dequeue datas to set sprite
        if (currentFarmPlotToSow == 4)
        {
            currentFarmPlotToSow = 0;
            CropSlotData slotData = farmPlotsToSowData.Dequeue();

            // set sprites
            CropsPlantManager.Instance.SetCropSprite(slotData.slot, slotData.cropData);

            // reset action
            currentAction = FarmerAction.None;
        }

        if (farmPlotsToSow.Count == 0)
        {
            GenerateNewTarget();
        }
        else
        {
            HandleSowingAction();
        }

        _isPerformingAction = false;
    }

    private IEnumerator CoWaitMowAnimation()
    {
        _isPerformingAction = true;

        // tell animator to do the mowing animation
        animator.SetTrigger("Mow");

        yield return new WaitForSeconds(mowClip.length);

        // every 4 plots reset and dequeue datas to set sprite
        if (currentFarmPlotToMow == 4)
        {
            currentFarmPlotToMow = 0;
            CropSlotData slotData = farmPlotsToMowData.Dequeue();

            // harvest
            Harvest(slotData);

            // reset crop
            if(_needResetCrop)
                ResetCrop(slotData);

            // reset action
            currentAction = FarmerAction.None;
        }

        if (farmPlotsToMow.Count == 0)
        {
            GenerateNewTarget();
        }
        else
        {
            HandleMowingAction();
        }

        _isPerformingAction = false;
    }

    private IEnumerator CoWaitWateringAnimation()
    {
        _isPerformingAction = true;

        // tell animator to do the watering animation
        animator.SetTrigger("Watering");

        yield return new WaitForSeconds(cooldownIdle);

        // reset animation
        animator.SetTrigger("Idle");
        currentAction = FarmerAction.None;

        GenerateNewTarget();

        _isPerformingAction = false;
    }

    private void Harvest(CropSlotData slotData)
    {
        if (_needResetCrop)
        {
            // Add only 4 if has been befriended companion
            PlayerManager.Instance.Inventory.AddItem(slotData.cropData.CropSO.Id, 4);
        }
        else
        {
            // scale with greenthumb ability
            PlayerManager.Instance.Inventory.AddItem(slotData.cropData.CropSO.Id, Mathf.FloorToInt(4f * playerData.CurrentGreenthumb));
        }
        //Debug.Log("Adding: " + slotData.cropData.CropSO.Id);
        PlayerManager.Instance.SaveInventoryData();
    }

    private void ResetCrop(CropSlotData slotData)
    {
        CropData currentCrop = null;

        switch (slotData.slot)
        {
            case 0: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot1CropData; break;
            case 1: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot2CropData; break;
            case 2: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot3CropData; break;
            case 3: currentCrop = PlayerManager.Instance.PlayerFarmerData.Slot4CropData; break;
        }

        if (currentCrop != null)
        {
            currentCrop.ResetGrowth();
            CropsPlantManager.Instance.SetCrop(slotData.slot, currentCrop, false);
        }
    }

    private float GetRandomWateringTime()
    {
        return UtilsGeneral.GetRandomValueBtwValues(minCooldownWatering, maxCooldownWatering);
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

    public void SaveFarmerData()
    {
        PlayerManager.Instance.UpdateFarmerData(playerData);
        PlayerManager.Instance.SaveFarmerData();
    }

    #endregion

    #region HANDLE EVENTS FROM FARMER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveFarmerData();
    }

    private void OnStatChangeFarmer(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
