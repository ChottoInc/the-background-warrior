using System;
using System.Collections;
using UnityEngine;

public class PlayerMiner : Player
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackClip;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;


    private float startingAttackSpeedAnimationDuration;


    [Header("Combat")]
    [SerializeField] Transform checkRockPoint;
    [SerializeField] LayerMask rockLayer;


    private PlayerMinerData playerData;

    // ------ MOVEMENT VARS

    private bool canInitialMove;

    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool dirLeft;

    private Rigidbody2D rb;

    // ------ ATTACK VARS

    private bool isRockDetected;

    private bool isSmashing;
    private float CooldownSmash => 1f / playerData.CurrentSmashSpeed;



    public event Action OnPerformSmash;

    public event Action<int, int> OnStatChange;




    public PlayerMinerData PlayerData => playerData;


    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeMiner;
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        startingAttackSpeedAnimationDuration = attackClip.length;
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        StartCoroutine(CoSpawned());
    }

    private void FixedUpdate()
    {
        if (!isSmashing)
        {
            HandleMovement();

            CheckForRock();
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
    }


    public void PerformSmash()
    {
        OnPerformSmash?.Invoke();
    }


    private void CheckForRock()
    {
        if (isRockDetected) return;

        if (CheckRockAtPoint(checkRockPoint.position, 0.3f, rockLayer, out Collider2D hit))
        {
            Rock rock = hit.GetComponent<Rock>();

            if (!rock.IsSmashed && rock.RockIndex != RockSpawnManager.Instance.CurrentRockIndex - 1)
            {
                isRockDetected = true;
                SmashManager.Instance.StartSmash(rock);
            }
                
        }
    }

    private void HandleMovement()
    {
        if (!canInitialMove) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f)
        {
            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            CheckFlip();
        }
        else
        {
            // reverse direction if destination reached
            GenerateNewTarget();
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
        dirLeft = !dirLeft;

        if (dirLeft)
        {
            currentTarget = InitializerManager.Instance.GetScreenOffsetBound();
        }
        else
        {
            currentTarget = InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound();
        }

        currentTarget = Camera.main.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }



    public void Setup(PlayerMinerData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeMiner;
        }
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetSmashing(bool isSmashing)
    {
        this.isSmashing = isSmashing;

        //reset detection rocks for update
        if (!isSmashing)
        {
            isRockDetected = false;
        }
        else
        {
            float attackSpeedMultiplier = startingAttackSpeedAnimationDuration / CooldownSmash;

            // Set the animator speed accordingly to Atk Spd
            animator.SetFloat("AttackSpeedMultiplier", attackSpeedMultiplier);
        }

        animator.SetBool("isSmashing", isSmashing);
    }

    public bool CheckRockAtPoint(Vector2 point, float radius, LayerMask rockMask, out Collider2D hitRock)
    {
        hitRock = Physics2D.OverlapCircle(point, radius, rockMask);
        return hitRock != null;
    }


    public void AddMinerWeaponLevel(int level)
    {
        playerData.AddMinerWeaponLevel(level);
    }



    #region SAVE

    public void SaveMinerData()
    {
        PlayerManager.Instance.UpdateMinerData(playerData);
        PlayerManager.Instance.SaveMinerData();
    }

    #endregion

    #region HANDLE EVENTS FROM MINER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveMinerData();
    }

    private void OnStatChangeMiner(int id, int value)
    {
        OnStatChange?.Invoke(id, value);
    }

    #endregion
}
