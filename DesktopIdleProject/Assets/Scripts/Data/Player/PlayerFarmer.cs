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


    private float timer5Mins;

    private PlayerFarmerData playerData;


    // --------- MOVEMENT VARS

    private bool canInitialMove;

    private bool isWalking;

    private bool isSowing;
    private bool canSow;


    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isIdling;
    private float timerIdle;

    private Rigidbody2D rb;

    private Queue<Transform> farmPlotsToSow;
    private Queue<CropSlotData> farmPlotsToSowData;
    private int currentFarmPlot;


    // ------ FARMER VARS

    public event Action<int, int> OnStatChange;




    public PlayerFarmerData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeFarmer;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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
    }


    private void FixedUpdate()
    {
        if (!isSowing)
        {
            HandleMovement();
        }
        else
        {
            if(canSow)
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
            // reset to stop the movement
            canSow = false;

            isWalking = false;
            animator.SetBool("isWalking", isWalking);

            // wait for sow animation and call next farm plot
            StartCoroutine(CoWaitSowAnimation());
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
        // reset index
        //currentFarmPlot = 0;

        farmPlotsToSow ??= new Queue<Transform>();
        farmPlotsToSowData ??= new Queue<CropSlotData>();

        foreach (var farmPlot in farmPlots)
        {
            farmPlotsToSow.Enqueue(farmPlot);
        }

        farmPlotsToSowData.Enqueue(cropSlotData);

        // handles if it's not already sowing, or else just add to queues
        if (!isSowing)
        {
            // set sowing so a new target isn't generated
            isSowing = true;

            // start sow plots
            NextPlot(farmPlotsToSow.Dequeue());
        }
    }

    private void NextPlot(Transform farmPlot)
    {
        // set next farm plot X
        currentTarget = farmPlot.position.x;

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

        // set already next farm plot index
        currentFarmPlot++;

        // start move
        canSow = true;
    }

    private IEnumerator CoWaitSowAnimation()
    {
        // tell animator to do the sowing animation
        animator.SetTrigger("Sow");

        yield return new WaitForSeconds(sowClip.length);

        // every 4 plots reset and dequeue datas to set sprite
        if (currentFarmPlot == 4)
        {
            currentFarmPlot = 0;
            CropSlotData slotData = farmPlotsToSowData.Dequeue();

            CropsPlantManager.Instance.SetCropSprite(slotData.slot, slotData.cropData);
        }

        if (farmPlotsToSow.Count > 0)
        {
            NextPlot(farmPlotsToSow.Dequeue());
        }
        else
        {
            isSowing = false;

            // set new target
            GenerateNewTarget();
        }
    }


    public void HandleSwitchScene()
    {

    }


    #region SAVE

    public void SaveFarmerData()
    {
        PlayerManager.Instance.UpdateFarmerData(playerData);
        PlayerManager.Instance.SaveFarmerData();
    }

    #endregion

    #region HANDLE EVENTS FROM FISHER DATA

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
