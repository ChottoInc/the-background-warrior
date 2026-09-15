using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : Player, IDamageable, IHealable
{
    [Header("Movement")]
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip walkClip;
    [SerializeField] AnimationClip attackClip;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool faceRight;
    [SerializeField] float speed = 5f;

    private float startingAttackSpeedAnimationDuration;



    [Header("Combat")]
    [SerializeField] Transform checkEnemyPoint;
    [SerializeField] LayerMask enemyLayer;

    [Space(10)]
    [SerializeField] GameObject swordHitVFXPrefab;

    [Space(10)]
    [SerializeField] GenericBar hpBar;
    [SerializeField] GenericBar _shieldBar;

    [Header("Death")]
    [SerializeField] float timerResetAfterDeath = 2f;


    private PlayerFightData playerData;

    // ------ MOVEMENT VARS

    private bool canInitialMove;

    private Vector3 startScale;

    private float currentTarget;
    private Vector2 currentDirection;

    private bool isIdling;

    private bool dirLeft;

    private Rigidbody2D rb;

    private float CurrentSpeed => speed *
        PlayerManager.Instance.FisherQuickSeriesMultiplier;

    // ------ ATTACK VARS


    private bool isEnemyDetected;

    private bool isAttacking;
    private float CooldownAttack => 1f / playerData.CurrentAtkSpd;



    public event Action OnPerformAttack;
    public event Action<int> OnTakeDamage;
    public event Action<int> OnHeal;


    public event Action<int, int> OnStatChange;
    public event Action<int> OnAddMap;


    public event Action OnResetAfterDeath;

    public PlayerFightData PlayerData => playerData;

    public bool IsDead => playerData.CurrentHp <= 0;





    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(playerData != null)
        {
            playerData.OnHpChange -= UpdateHpBarUI;
            playerData.OnShieldChange -= UpdateShieldBarUI;
            playerData.OnTakeDamage -= OnActionTakeDamage;
            playerData.OnHeal -= OnActionHeal;
            playerData.OnLevelUp -= LevelUp;

            playerData.OnStatChange -= OnStatChangeFight;
            playerData.OnAddMap -= OnAddMapFight;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();

        // Get default speed for animator walk
        startingAttackSpeedAnimationDuration = attackClip.length;
    }

    private void Start()
    {
        startScale = spriteRenderer.transform.localScale;

        StartCoroutine(CoSpawned());

        _buffsToCheckTypes = new List<UtilsBuffs.BuffType>()
        {
            UtilsBuffs.BuffType.Greed,
            UtilsBuffs.BuffType.Veteran,
            UtilsBuffs.BuffType.Storyteller,
            UtilsBuffs.BuffType.IronSkin,
            UtilsBuffs.BuffType.Inspiration,
        };
    }

    private IEnumerator CoSpawned()
    {
        yield return new WaitForSeconds(1f);

        // change rb type
        rb.bodyType = RigidbodyType2D.Kinematic;

        // enable movement
        canInitialMove = true;

        GenerateNewTarget();
    }

    private void FixedUpdate()
    {
        if (!isAttacking && !IsDead)
        {
            HandleMovement();

            CheckForEnemy();
        }
    }

    public void PerformAttack()
    {
        OnPerformAttack?.Invoke();
    }

    private void OnActionTakeDamage(int damage)
    {
        OnTakeDamage?.Invoke(damage);
    }

    private void OnActionHeal(int heal)
    {
        OnHeal?.Invoke(heal);
    }

    private void CheckForEnemy()
    {
        if (!canInitialMove) return;

        if (!StageManager.Instance.FinishedStartingEnemies) return;

        if (isEnemyDetected) return;

        if (CheckEnemyAtPoint(checkEnemyPoint.position, 0.5f, enemyLayer, out Collider2D hit))
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            //Debug.Log("Enemy found: " + enemy.IsDead);
            if (enemy.CanFight && !enemy.IsDead && !IsDead)
            {
                isEnemyDetected = true;
                CombatManager.Instance.StartFight(enemy);
            }
        }
    }

    private void HandleMovement()
    {
        if (!canInitialMove) return;

        float distance = Mathf.Abs(transform.position.x - currentTarget);

        if (distance > 0.1f && !isIdling)
        {
            // get target dir
            currentDirection = new Vector2(currentTarget - transform.position.x, 0).normalized;

            // move
            transform.position += CurrentSpeed * Time.fixedDeltaTime * (Vector3)currentDirection;

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



    public void Setup(PlayerFightData playerData)
    {
        this.playerData = playerData;

        if (playerData != null)
        {
            playerData.OnHpChange += UpdateHpBarUI;
            playerData.OnShieldChange += UpdateShieldBarUI;
            playerData.OnTakeDamage += OnActionTakeDamage;
            playerData.OnHeal += OnActionHeal;
            playerData.OnLevelUp += LevelUp;

            playerData.OnStatChange += OnStatChangeFight;
            playerData.OnAddMap += OnAddMapFight;
        }

        playerData.ResetAfterStage();
        hpBar.Setup(playerData.MaxHp, playerData.CurrentHp);
        _shieldBar.Setup(playerData.MaxShield, playerData.CurrentShield);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateHpBarUI()
    {
        hpBar.gameObject.SetActive(playerData.CurrentShield <= 0 ? true : false);
        hpBar.SetCurrentValue(playerData.CurrentHp);
        //Debug.Log("current: " + playerData.CurrentHp);
        //Debug.Log("max hp: " + playerData.MaxHp);
    }

    private void UpdateShieldBarUI()
    {
        _shieldBar.gameObject.SetActive(playerData.CurrentShield > 0 ? true : false);
        _shieldBar.SetCurrentValue(playerData.CurrentShield);
        //Debug.Log("current: " + playerData.CurrentHp);
        //Debug.Log("max hp: " + playerData.MaxHp);
    }

    public void SetAttacking(bool isAttacking)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        this.isAttacking = isAttacking;

        //reset detection rocks for update
        if (!isAttacking)
        {
            isEnemyDetected = false;
        }
        else
        {
            float attackSpeedMultiplier = startingAttackSpeedAnimationDuration / CooldownAttack;

            // Set the animator speed accordingly to Atk Spd
            animator.SetFloat("AttackSpeedMultiplier", attackSpeedMultiplier);
        }

        animator.SetBool("isAttacking", isAttacking);
    }

    public void PlaySwordHit(Vector2 enemyPos)
    {
        GameObject hitVFX = Instantiate(swordHitVFXPrefab, enemyPos, Quaternion.identity);
        hitVFX.transform.parent = null;
    }

    public bool CheckEnemyAtPoint(Vector2 point, float radius, LayerMask enemyMask, out Collider2D hitEnemy)
    {
        hitEnemy = Physics2D.OverlapCircle(point, radius, enemyMask);
        return hitEnemy != null;
    }

    public void SetDeath(bool isDead)
    {
        if(isDead)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);

            animator.SetBool("isDead", true);

            StartCoroutine(CoResetAfterDeath());
        }
        else
        {
            animator.SetBool("isDead", false);
        }
    }

    private IEnumerator CoResetAfterDeath()
    {
       // Debug.Log("start routine death");
        yield return new WaitForSeconds(timerResetAfterDeath);

        OnResetAfterDeath?.Invoke();
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

    public void SaveFightData()
    {
        PlayerManager.Instance.UpdateFightData(playerData);
        PlayerManager.Instance.SaveFightData();
    }

    #endregion

    #region HANDLE EVENTS FROM FIGHTER DATA

    protected override void LevelUp()
    {
        base.LevelUp();

        SaveFightData();
    }

    private void OnStatChangeFight(int id, int value)
    {
        // if max hp are updated during battle, the ui needs update
        if(id == UtilsPlayer.ID_WARRIOR_MAXHP)
        {
            float diff = hpBar.MaxValue - playerData.CurrentHp;
            playerData.SetHp(playerData.MaxHp - diff);
            hpBar.Setup(playerData.MaxHp, playerData.CurrentHp);
        }

        OnStatChange?.Invoke(id, value);
    }

    private void OnAddMapFight(int id)
    {
        OnAddMap?.Invoke(id);
    }

    #endregion
}
