using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject, IDamageable
{
    [Header("Sprite")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldownIdle = 1.5f;

    [Space(10)]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip walkClip;
    [SerializeField] AnimationClip attackClip;

    private float startingAttackSpeedAnimationDuration;

    private RuntimeAnimatorController animController;

    [Header("Check Player")]
    [SerializeField] float hitRadius;
    [SerializeField] LayerMask playerMask;

    [Header("Rigidbody")]
    [SerializeField] LayerMask excludeLayersRb;

    [Header("VFXs")]
    [SerializeField] ParticleSystem deathVFX;

    [Header("UI")]
    [SerializeField] UIPanelDamage panelDamage;


    private float deathVFXDuration;
    private float timerDeathVFX;



    private EnemyData enemyData;

    private int enemyIndex;

    public int EnemyIndex => enemyIndex;


    // --------- MOVEMENT VARS

    private Camera mainCam;

    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isIdling;
    private float timerIdle;

    private bool canMove;

    private Rigidbody2D rb;


    public bool CanMove => canMove;

    // --------- ATTACK VARS

    private bool isAttacking;
    private float CooldownAttack => 1f / enemyData.CurrentAtkSpd;
    private float timerAttack;

    private bool canFight;

    public event Action OnPerformAttack;
    public event Action<int> OnTakeDamage;

    public bool IsAttacking => isAttacking;

    public bool CanFight => canFight;


    // ------- VFX

    private bool isDeathVFXPlaying;

    private SceneLoaderManager.SceneType sceneType;




    public EnemyData EnemyData => enemyData;

    public bool IsDead => enemyData.CurrentHp <= 0;


    private void OnEnable()
    {
        mainCam = Camera.main;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get default speed for animator walk
        startingAttackSpeedAnimationDuration = attackClip.length;
    }

    private void Start()
    {
        canMove = true;

        deathVFXDuration = deathVFX.main.duration;

        startScale = spriteRenderer.transform.localScale;

        GenerateNewTarget();
    }


    private void Update()
    {
        if (isAttacking)
        {
            CheckAttack();
        }

        if (isDeathVFXPlaying)
        {
            CheckDeathVFX();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (!canMove) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += speed * Time.fixedDeltaTime * (Vector3)currentDirection;

            animator.SetFloat("Velocity", Mathf.Abs(currentDirection.x));

            CheckFlip();
        }
        else
        {
            // handles idling timer before move again
            if (!isIdling)
            {
                animator.SetFloat("Velocity", 0);

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

    private void CheckFlipOnEnemy(Vector2 dir)
    {
        // check sprite flip
        if (dir.x > 0 && faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
        else if (dir.x > 0f && !faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (dir.x < 0 && faceRight)
        {
            spriteRenderer.transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        else if (dir.x < 0 && !faceRight)
        {
            spriteRenderer.transform.localScale = startScale;
        }
    }

    private void GenerateNewTarget()
    {
        currentTarget = UnityEngine.Random.Range(InitializerManager.Instance.GetScreenOffsetBound(), InitializerManager.GetScreenWidth() - InitializerManager.Instance.GetScreenOffsetBound());
        currentTarget = mainCam.ScreenToWorldPoint(new Vector2(currentTarget, 0)).x;
    }

    private void CheckAttack()
    {
        if (timerAttack <= 0)
        {
            OnPerformAttack?.Invoke();
            timerAttack = CooldownAttack;
        }
        else
        {
            timerAttack -= Time.deltaTime;
        }
    }

    private void CheckDeathVFX()
    {
        if (timerDeathVFX <= 0)
        {
            isDeathVFXPlaying = false;
            HideAfterDeath();
        }
        else
        {
            timerDeathVFX -= Time.deltaTime;
        }
    }

    public void Setup(EnemyData enemyData, int index, SceneLoaderManager.SceneType sceneType)
    {
        this.enemyData = enemyData;

        enemyData.OnTakeDamage += OnActionTakeDamage;

        //Debug.Log($"Enemytakedamage subscribers: {enemyData.OnTakeDamage?.GetInvocationList().Length ?? 0}");

        enemyIndex = index;

        this.sceneType = sceneType;

        spriteRenderer.sortingOrder = enemyIndex;
    }

    private void OnActionTakeDamage(int damage)
    {
        OnTakeDamage?.Invoke(damage);
    }

    private IEnumerator CoRespawned()
    {
        // enable rigidbody
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(2f);

        // destroy rb
        RemoveRb();

        // can move
        canMove = true;

        // set the enemy can fight
        canFight = true;
    }

    private void RemoveRb()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void HideSprite(bool hide)
    {
        // save initial color
        Color spriteColor = spriteRenderer.color;

        if(hide)
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
        else
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
    }

    public void SetMove(bool move)
    {
        canMove = move;
    }


    public void SetAttacking(bool isAttacking, Vector2 playerDir)
    {
        this.isAttacking = isAttacking;

        CheckFlipOnEnemy(playerDir);

        if (isAttacking)
        {
            float attackSpeedMultiplier = startingAttackSpeedAnimationDuration / CooldownAttack;

            // Set the animator speed accordingly to Atk Spd
            animator.SetFloat("AttackSpeedMultiplier", attackSpeedMultiplier);

            timerAttack = 0;
        }

        animator.SetBool("IsAttacking", isAttacking);
    }

    private void CheckPlayerPos(EnemySO so)
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, hitRadius, playerMask);
        if (hit)
        {
            // set new direction towards the player
            currentTarget = hit.transform.position.x;
        }
    }

    public void PlayDeath(bool setDead)
    {
        if (setDead)
            enemyData.SetDead();

        HideSprite(true);

        // play vfx
        deathVFX.Play();
        timerDeathVFX = deathVFXDuration;
        isDeathVFXPlaying = true;
    }

    private void HideAfterDeath()
    {
        deathVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Die();
    }

    public void OnSpawn()
    {
        HideSprite(false);

        // call coroutine to act after respawn
        StartCoroutine(CoRespawned());

        // attach combat manager OnEnemyKill event, to check nearby player
        if (CombatManager.Instance != null)
            CombatManager.Instance.OnEnemyKill += CheckPlayerPos;
    }

    public void OnDespawn()
    {
        canFight = false;

        Resets();
    }

    private void Resets()
    {
        StopAllCoroutines();

        RemoveRb();

        // disable move
        canMove = false;

        // disable fight
        canFight = false;

        // detach event and reset data
        if (enemyData != null)
        {
            enemyData.OnTakeDamage -= OnActionTakeDamage;
            enemyData = null;
        }
        else
        {
            Debug.Log("enemy data not found");
        }

        // detach combat manager OnEnemyKill event
        if (CombatManager.Instance != null)
            CombatManager.Instance.OnEnemyKill -= CheckPlayerPos;
    }

    public void Die() 
    {
        switch (sceneType)
        {
            case SceneLoaderManager.SceneType.CombatMap:
                StageManager.Instance.RemoveFromCurrentEnemiesList(this);
                PoolManager.Instance.Return(gameObject, enemyData.EnemySO.EnemyPoolName);
                break;

            case SceneLoaderManager.SceneType.Home:
                PoolManager.Instance.Return(gameObject, enemyData.EnemySO.EnemyPoolName);
                break;
        }
    }


    public void UpdateDamageUI()
    {
        // check if floating damage is enabled
        if (SettingsManager.Instance.IsDamageOn)
        {
            //panelDamage.ShowDamage();
        }
    }



    public override bool Equals(object other)
    {
        Enemy otherEnemy = other as Enemy;
        return enemyIndex == otherEnemy.enemyIndex;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
